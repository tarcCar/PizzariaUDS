using Dapper.Contrib.Extensions;
using Microsoft.EntityFrameworkCore;
using PizzariaUDS.Models;
using PizzariaUDS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PizzariaUDS.Repositories
{
    /// <summary>
    /// Classe de acesso a base da dados do sabor
    /// </summary>
    public class SaborRepository : ISaborRepository
    {
        private readonly IRepository repository;
        private readonly IDbConnection Database;

        public SaborRepository(IRepository repository)
        {
            this.repository = repository;
            Database = repository.Database;
        }

        public async Task AlterarAsync(Sabor sabor)
        {
            await Database.UpdateAsync(sabor);
        }

        public async Task ExcluirAsync(Sabor sabor)
        {
            await Database.DeleteAsync(sabor);
        }

        public async Task<IEnumerable<Sabor>> ListarAsync()
        {
            return await Database.GetAllAsync<Sabor>();
        }

        public async Task<Sabor> RecuperarPorIdAsync(int id)
        {
            return await Database.GetAsync<Sabor>(id);
        }

        public async Task<Sabor> SalvarAsync(Sabor sabor)
        {
            var id = await Database.InsertAsync(sabor);
            sabor.Id = Convert.ToInt16(id);
            return sabor;
        }
    }
}
