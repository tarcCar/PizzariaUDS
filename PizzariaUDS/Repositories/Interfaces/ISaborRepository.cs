using PizzariaUDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Repositories.Interfaces
{
    public interface ISaborRepository
    {
        Task<IEnumerable<Sabor>> ListarAsync();
        Task<Sabor> SalvarAsync(Sabor sabor);
        Task<Sabor> RecuperarPorIdAsync(int id);
        Task ExcluirAsync(Sabor sabor);
        Task AlterarAsync(Sabor sabor);
    }
}
