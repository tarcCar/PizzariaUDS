using PizzariaUDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Repositories.Interfaces
{
    public interface IAdicionalRepository
    {
        Task<IEnumerable<Adicional>> ListarAsync();
        Task<Adicional> SalvarAsync(Adicional adicional);
        Task<Adicional> RecuperarPorIdAsync(int id);
        Task ExcluirAsync(Adicional adicional);
        Task AlterarAsync(Adicional adicional);
        Task<IEnumerable<Adicional>> ListarAdicionaisPizzaAsync(int pizzaId);
        Task<IEnumerable<PizzaAdicional>> ListarAdicionaisPizzaAsync(IEnumerable<int> pizzasIds);
        Task<bool> TemPizzasAsync(Adicional adicional);
    }
}
