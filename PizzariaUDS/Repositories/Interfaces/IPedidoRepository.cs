using PizzariaUDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<Pedido>> ListarAsync();
        Task<Pedido> SalvarAsync(Pedido pedido);
        Task<Pedido> RecuperarPorIdAsync(int id);
        Task ExcluirAsync(Pedido pedido);
        Task AlterarAsync(Pedido pedido);
    }
}
