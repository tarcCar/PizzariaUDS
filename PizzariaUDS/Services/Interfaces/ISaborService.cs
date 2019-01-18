using PizzariaUDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Services.Interfaces
{
    public interface ISaborService
    {
        Task<IEnumerable<Sabor>> ListarAsync();
        Task<Sabor> SalvarAsync(Sabor sabor);
        Task AlterarAsync(int id, Sabor sabor);
        Task<Sabor> RecuperarPorIdAsync(int id);
        Task ExcluirAsync(Sabor sabor);
    }
}
