using PizzariaUDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Services.Interfaces
{
    public interface IAdicionalService
    {
        Task<IEnumerable<Adicional>> ListarAsync();
        Task<Adicional> SalvarAsync(Adicional adicional);
        Task AlterarAsync(int id, Adicional adicional);
        Task<Adicional> RecuperarPorIdAsync(int id);
        Task ExcluirAsync(Adicional adicional);
    }
}
