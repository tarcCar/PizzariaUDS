using PizzariaUDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Services.Interfaces
{
    public interface ITamanhoService
    {
        Task<IEnumerable<Tamanho>> ListarAsync();
        Task<Tamanho> SalvarAsync(Tamanho tamanho);
        Task AlterarAsync(Tamanho tamanho);
        Task<Tamanho> RecuperarPorIdAsync(int id);
        Task ExcluirAsync(Tamanho tamanho);
    }
}
