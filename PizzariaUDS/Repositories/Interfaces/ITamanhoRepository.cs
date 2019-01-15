using PizzariaUDS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzariaUDS.Repositories.Interfaces
{
    public interface ITamanhoRepository
    {
        Task<IEnumerable<Tamanho>> ListarAsync();
        Task<Tamanho> SalvarAsync(Tamanho tamanhoPizza);
        Task AlterarAsync(Tamanho tamanhoPizza);
        Task<Tamanho> RecuperarPorIdAsync(int id);
        Task ExcluirAsync(Tamanho tamanhoPizza);
    }
}
