using Dapper.Contrib.Extensions;
using Microsoft.EntityFrameworkCore;
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

        public PizzaRepository(IRepository repository)
        {
            this.repository = repository;
            Database = repository.Database;
        }

        public async Task AlterarAsync(Pizza pizza)
        {
            await Database.UpdateAsync(pizza);
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
