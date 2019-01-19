using Dapper.Contrib.Extensions;
using PizzariaUDS.Models;
using PizzariaUDS.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PizzariaUDS.Repositories
{
    public class PizzaRepository : IPizzaRepository
    {
        private readonly IRepository repository;
        private readonly IDbConnection Database;
        private readonly IPizzaAdicionalRepository pizzaAdicionalRepository;
        public PizzaRepository(IRepository repository, IPizzaAdicionalRepository pizzaAdicionalRepository)
        {
            this.repository = repository;
            Database = repository.Database;
            this.pizzaAdicionalRepository = pizzaAdicionalRepository;
        }

        public async Task<Pizza> AdicionarAdicionalAsync(Pizza pizza, Adicional adicional)
        {
           

            var pizzaAdicional = new PizzaAdicional
            {
                Pizza = pizza,
                Adicional = adicional,
            };

            Database.Open();
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
            return await Database.GetAllAsync<Pizza>();
        }

        public async Task<Pizza> RecuperarPorIdAsync(int id)
        {
            return await Database.GetAsync<Pizza>(id);
        }

        public async Task<Pizza> SalvarAsync(Pizza pizza)
        {
            var id = await Database.InsertAsync(pizza);
            pizza.Id = id;
            return pizza;
        }
    }
}
