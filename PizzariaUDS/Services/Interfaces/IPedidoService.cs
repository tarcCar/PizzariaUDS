using PizzariaUDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Services.Interfaces
{
    public interface IPedidoService
    {

        Task<IEnumerable<Pedido>> ListarAsync();
        Task<Pedido> SalvarAsync(Pedido pedido);
        Task AlterarAsync(int id, Pedido pedido);
        Task<Pedido> RecuperarPorIdAsync(int id);
        Task ExcluirAsync(Pedido pedido);
    }
}
