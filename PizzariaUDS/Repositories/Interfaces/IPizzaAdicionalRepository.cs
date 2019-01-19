using PizzariaUDS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Repositories.Interfaces
{
    public interface IPizzaAdicionalRepository
    {
        Task<IEnumerable<PizzaAdicional>> ListarAsync();
        Task<PizzaAdicional> SalvarAsync(PizzaAdicional pizzaAdicional, IDbTransaction transaction);
        Task<PizzaAdicional> SalvarAsync(PizzaAdicional pizzaAdicional);
        Task<bool> TemPizzaComAdicional(int idAdicional);
        Task ExcluirAsync(PizzaAdicional pizzaAdicional,IDbTransaction transaction);
        Task ExcluirAsync(PizzaAdicional pizzaAdicional);
        Task ExcluirAsync(Pizza pizza, IDbTransaction transaction);
    }
}
