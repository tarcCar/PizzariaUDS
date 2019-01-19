using Dapper;
using Dapper.Contrib.Extensions;
using PizzariaUDS.Models;
using PizzariaUDS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public async Task<IEnumerable<PizzaAdicional>> ListarAsync()
        {
            return await Database.GetAllAsync<PizzaAdicional>();
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

            var resultado = await Database.QueryFirstOrDefaultAsync<PizzaAdicional>(sql,parametros);

            return resultado != null;

        }
    }
}
