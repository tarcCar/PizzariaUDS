using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.EntityFrameworkCore;
using PizzariaUDS.Models;
using PizzariaUDS.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Repositories
{
    public class AdicionalRepository : IAdicionalRepository
    {
        private readonly IRepository repository;
        private readonly IDbConnection Database;

        public AdicionalRepository(IRepository repository)
        {
            this.repository = repository;
            Database = repository.Database;
        }
        public async Task AlterarAsync(Adicional adicional)
        {
            await Database.UpdateAsync(adicional);
        }

        public async Task ExcluirAsync(Adicional adicional)
        {
            await Database.DeleteAsync(adicional);
        }

        public async Task<IEnumerable<Adicional>> ListarAsync()
        {
            return await Database.GetAllAsync<Adicional>();
        }

        public async Task<IEnumerable<Adicional>> ListarAdicionaisPizzaAsync(int pizzaId)
        {
            const string sql = @"select a.id,a.descricao,a.tempoPreparo,a.valor from pizza_adicional pa
                                inner join adicional a on a.id = pa.adicionalId
                                where pizzaId  = @pizzaId";
            var parametros = new DynamicParameters();
            parametros.Add("@pizzaId", pizzaId);
            return await Database.QueryAsync<Adicional>(sql,parametros);
        }

        public async Task<IEnumerable<PizzaAdicional>> ListarAdicionaisPizzaAsync(IEnumerable<int> pizzasIds)
        {
             string sql = $@"select pa.pizzaId,a.id,a.descricao,a.tempoPreparo,a.valor from pizza_adicional pa
                                inner join adicional a on a.id = pa.adicionalId
                                where pizzaId  in( {string.Join(',',pizzasIds)})";

            var parametros = new DynamicParameters();
            parametros.Add("@pizzasIds", pizzasIds.ToArray());
            return await Database.QueryAsync<PizzaAdicional,Adicional,PizzaAdicional>(sql,
                (pizzaAdicional,adicional) => 
                {
                    pizzaAdicional.Adicional = adicional;
                    return pizzaAdicional;
                },
                new { pizzasIds=pizzasIds.ToArray() },splitOn: "pizzaId,id");
        }

        public async Task<Adicional> RecuperarPorIdAsync(int id)
        {
            return await Database.GetAsync<Adicional>(id);
        }

        public async Task<Adicional> SalvarAsync(Adicional adicional)
        {
            var id = await Database.InsertAsync(adicional);
            adicional.Id = id;
            return adicional;
        }
    }
}
