using PizzariaUDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Services.Interfaces
{
    public interface IPizzaService
    {
        Task<IEnumerable<Pizza>> ListarAsync();
        Task<Pizza> SalvarAsync(Pizza pizza);
        Task AlterarAsync(int id, Pizza pizza);
        Task<Pizza> RecuperarPorIdAsync(int id);
        Task ExcluirAsync(Pizza pizza);
        Task<Pizza> AdicionarAdicionalAsync(Pizza pizza, Adicional adicional);
        Task<bool> TemPedidoAsync(Pizza pizza);
    }
}
