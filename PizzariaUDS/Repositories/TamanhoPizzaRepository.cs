using Dapper.Contrib.Extensions;
using PizzariaUDS.Models;
using PizzariaUDS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzariaUDS.Repositories
{
    public class TamanhoPizzaRepository : MySQLRepository, ITamanhoPizzaRepository
    {

        public async Task AlterarAsync(Tamanho tamanhoPizza)
        {
            await Database.UpdateAsync(tamanhoPizza);
        }

        public async Task ExcluirAsync(Tamanho tamanhoPizza)
        {
            await Database.DeleteAsync(tamanhoPizza);
        }

        public async Task<IEnumerable<Tamanho>> ListarAsync()
        {
            return await Database.GetAllAsync<Tamanho>();
        }

        public async Task<Tamanho> RecuperarPorIdAsync(int id)
        {
            return await Database.GetAsync<Tamanho>(id);
        }

        public async Task<Tamanho> SalvarAsync(Tamanho tamanhoPizza)
        {
            var id = await Database.InsertAsync(tamanhoPizza);
            tamanhoPizza.Id = Convert.ToInt16(id);
            return tamanhoPizza;
        }
    }
}
