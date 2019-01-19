using PizzariaUDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Repositories.Interfaces
{
    public interface IPizzaRepository
    {
        Task<IEnumerable<Pizza>> ListarAsync();
        Task<Pizza> SalvarAsync(Pizza pizza);
        Task<Pizza> RecuperarPorIdAsync(int id);
        Task ExcluirAsync(Pizza pizza);
        Task AlterarAsync(Pizza pizza);
    }
}
