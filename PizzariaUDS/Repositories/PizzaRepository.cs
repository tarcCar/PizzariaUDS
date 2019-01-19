using Dapper;
using Dapper.Contrib.Extensions;
using PizzariaUDS.Models;
using PizzariaUDS.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Repositories
{
    public class PizzaRepository : IPizzaRepository
    {
        private readonly IRepository repository;
        private readonly IDbConnection Database;
        private readonly IPizzaAdicionalRepository pizzaAdicionalRepository;
        private readonly IAdicionalRepository adicionalRepository;
        public PizzaRepository(IRepository repository, IPizzaAdicionalRepository pizzaAdicionalRepository, IAdicionalRepository adicionalRepository)
        {
            this.repository = repository;
            Database = repository.Database;
            this.pizzaAdicionalRepository = pizzaAdicionalRepository;
            this.adicionalRepository = adicionalRepository;
        }

        public async Task<Pizza> AdicionarAdicionalAsync(Pizza pizza, Adicional adicional)
        {


            var pizzaAdicional = new PizzaAdicional
            {
                Pizza = pizza,
                Adicional = adicional,
            };

            //Abre a conexao com base de dados caso não esteja aberta
            //por que para criar uma transaction precisa da conexao aberta;
            if(Database.State == ConnectionState.Closed) Database.Open();

            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    await pizzaAdicionalRepository.SalvarAsync(pizzaAdicional, transaction);
                    await AlterarAsync(pizza, transaction);
                    transaction.Commit();
                    return pizza;
                }
                catch (System.Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }

            }
        }

        public async Task AlterarAsync(Pizza pizza)
        {
            await Database.UpdateAsync(pizza);
        }
        public async Task AlterarAsync(Pizza pizza, IDbTransaction transaction)
        {
            var connection = transaction.Connection;
            await connection.UpdateAsync(pizza, transaction);
        }
        public async Task ExcluirAsync(Pizza pizza)
        {
            await Database.DeleteAsync(pizza);
        }

        public async Task<IEnumerable<Pizza>> ListarAsync()
        {
            const string sql = @"SELECT p.id,p.saborId,p.tamanhoId,p.tempoPreparo,p.valor,
                        s.id as idSabor,s.descricao,s.tempoAdicional,
                        t.id as idTamanho, t.descricao,t.tempoPreparo,t.valor
                        FROM pizza p
                        inner join sabor s on s.id = p.saborid
                        inner join tamanho t on t.id = p.tamanhoId";

            var pizzas = await Database.QueryAsync<Pizza, Sabor, Tamanho, Pizza>(sql,
           (pizza, sabor, tamanho) =>
           {
               tamanho.Id = pizza.TamanhoId;
               sabor.Id = pizza.SaborId;
               pizza.Tamanho = tamanho;
               pizza.Sabor = sabor;
               return pizza;
           }, splitOn: "id,idSabor,idTamanho");

            var idsPizzas = pizzas.Select(p => p.Id).Distinct();
            var pizzasAdicionais = await adicionalRepository.ListarAdicionaisPizzaAsync(idsPizzas);

            foreach (var pizza in pizzas)
            {
                pizza.Adicionais = new HashSet<Adicional>(pizzasAdicionais.Where(pa => pa.PizzaId == pizza.Id).Select(pa => pa.Adicional));
            }
            return pizzas;
        }

        public async Task<Pizza> RecuperarPorIdAsync(int id)
        {
            const string sql = @"SELECT p.id,p.saborId,p.tamanhoId,p.tempoPreparo,p.valor,
                        s.id as idSabor,s.descricao,s.tempoAdicional,
                        t.id as idTamanho, t.descricao,t.tempoPreparo,t.valor
                        FROM pizza p
                        inner join sabor s on s.id = p.saborid
                        inner join tamanho t on t.id = p.tamanhoId
                        WHERE P.id = @id";

            var parametros = new DynamicParameters();
            parametros.Add("@id", id);
            var pizzas = await Database.QueryAsync<Pizza, Sabor, Tamanho, Pizza>(sql,
            (pizza, sabor, tamanho) =>
            {
                tamanho.Id = pizza.TamanhoId;
                sabor.Id = pizza.SaborId;
                pizza.Tamanho = tamanho;
                pizza.Sabor = sabor;
                return pizza;
            }, parametros, splitOn: "id,idSabor,idTamanho");

            if (pizzas.Count() == 0)
                return null;
            else
            {
                var pizza = pizzas.ElementAt(0);
                pizza.Adicionais = new HashSet<Adicional>(await adicionalRepository.ListarAdicionaisPizzaAsync(pizza.Id));
                return pizza;
            }
        }

        public async Task<Pizza> SalvarAsync(Pizza pizza)
        {
            var id = await Database.InsertAsync(pizza);
            pizza.Id = id;
            return pizza;
        }
    }
}
