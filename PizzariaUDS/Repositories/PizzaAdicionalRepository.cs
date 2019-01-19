using Dapper;
using Dapper.Contrib.Extensions;
using PizzariaUDS.Models;
using PizzariaUDS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PizzariaUDS.Repositories
{
    public class PizzaAdicionalRepository : IPizzaAdicionalRepository
    {
        private readonly IRepository repository;
        private readonly IDbConnection Database;

        public PizzaAdicionalRepository(IRepository repository)
        {
            this.repository = repository;
            Database = repository.Database;
        }

        public async Task ExcluirAsync(PizzaAdicional pizzaAdicional, IDbTransaction transaction)
        {
            var connection = transaction.Connection;
            await connection.DeleteAsync(pizzaAdicional, transaction);
        }

        public async Task ExcluirAsync(PizzaAdicional pizzaAdicional)
        {
            await Database.DeleteAsync(pizzaAdicional);
        }

        public async Task ExcluirAsync(Pizza pizza, IDbTransaction transaction)
        {
            var pizzasAdicionais = await ListarAsync(pizza, transaction);
            var connection = transaction.Connection;
            foreach (var pizzaAdicional in pizzasAdicionais)
            {
                await connection.DeleteAsync(pizzaAdicional, transaction);
            }
        }

        public async Task<IEnumerable<PizzaAdicional>> ListarAsync()
        {
            return await Database.GetAllAsync<PizzaAdicional>();
        }

        private async Task<IEnumerable<PizzaAdicional>> ListarAsync(Pizza pizza, IDbTransaction transaction)
        {
            var sql = "SELECT id,pizzaId,adicionalId FROM pizzaria.pizza_adicional where pizzaId = @pizzaId";
            var parametros = new DynamicParameters();
            parametros.Add("@pizzaId", pizza.Id);

            var connection = transaction.Connection;
            return await connection.QueryAsync<PizzaAdicional>(sql, parametros, transaction);
        }



        public async Task<PizzaAdicional> SalvarAsync(PizzaAdicional pizzaAdicional, IDbTransaction transaction)
        {
            var connection = transaction.Connection;
            var id = await connection.InsertAsync(pizzaAdicional, transaction);
            pizzaAdicional.Id = id;
            return pizzaAdicional;
        }

        public async Task<PizzaAdicional> SalvarAsync(PizzaAdicional pizzaAdicional)
        {
            var id = await Database.InsertAsync(pizzaAdicional);
            pizzaAdicional.Id = id;
            return pizzaAdicional;
        }

        public async Task<bool> TemPizzaComAdicional(int idAdicional)
        {
            var sql = "SELECT id,pizzaId,adicionalId FROM pizzaria.pizza_adicional where adicionalId = @idAdicional";
            var parametros = new DynamicParameters();
            parametros.Add("@idAdicional", idAdicional);

            var resultado = await Database.QueryFirstOrDefaultAsync<PizzaAdicional>(sql, parametros);

            return resultado != null;

        }
    }
}
