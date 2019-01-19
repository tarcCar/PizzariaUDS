using PizzariaUDS.Models;
using PizzariaUDS.Repositories.Interfaces;
using PizzariaUDS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Services
{
    public class PedidoService: IPedidoService
    {
        private readonly IPedidoRepository pedidoRepository;

        public PedidoService(IPedidoRepository pedidoRepository)
        {
            this.pedidoRepository = pedidoRepository;
        }
        public async Task AlterarAsync(int id, Pedido pedido)
        {
            pedido.Id = Convert.ToInt16(id);
            await pedidoRepository.AlterarAsync(pedido);
        }

        public async Task ExcluirAsync(Pedido pedido) => await pedidoRepository.ExcluirAsync(pedido);

        public async Task<IEnumerable<Pedido>> ListarAsync() => await pedidoRepository.ListarAsync();

        public async Task<Pedido> RecuperarPorIdAsync(int id) => await pedidoRepository.RecuperarPorIdAsync(id);

        public async Task<Pedido> SalvarAsync(Pedido pedido) => await pedidoRepository.SalvarAsync(pedido);
    }
}
