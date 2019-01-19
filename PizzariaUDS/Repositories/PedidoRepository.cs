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
    public class PedidoRepository: IPedidoRepository
    {
        private readonly IRepository repository;
        private readonly IDbConnection Database;
        private readonly IPizzaRepository pizzaRepository;
        public PedidoRepository(IRepository repository, IPizzaRepository pizzaRepository)
        {
            this.repository = repository;
            Database = repository.Database;
            this.pizzaRepository = pizzaRepository;
        }

        public async Task AlterarAsync(Pedido pedido)
        {
            await Database.UpdateAsync(pedido);
        }

        public async Task ExcluirAsync(Pedido pedido)
        {
            await Database.DeleteAsync(pedido);
        }

        public async Task<IEnumerable<Pedido>> ListarAsync()
        {
            return await Database.GetAllAsync<Pedido>();
        }

        public async Task<Pedido> RecuperarPorIdAsync(int id)
        {
            var pedido = await Database.GetAsync<Pedido>(id);
            if(pedido != null)
            {
                pedido.Pizza = await pizzaRepository.RecuperarPorIdAsync(pedido.PizzaId);
            }
            return pedido;
        }

        public async Task<Pedido> SalvarAsync(Pedido pedido)
        {
            var id = await Database.InsertAsync(pedido);
            pedido.Id = id;
            return pedido;
        }

    }
}
