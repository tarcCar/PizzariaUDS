using PizzariaUDS.Models;
using PizzariaUDS.Repositories.Interfaces;
using PizzariaUDS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzariaUDS.Services
{
    /// <summary>
    /// Classe da camanda de regra de negocios do tamanho
    /// </summary>
    public class TamanhoService : ITamanhoService
    {
        private readonly ITamanhoRepository tamanhoRepository;

        public TamanhoService(ITamanhoRepository tamanhoRepository)
        {
            this.tamanhoRepository = tamanhoRepository;
        }

        public async Task AlterarAsync(int id, Tamanho tamanho)
        {
            tamanho.Id = Convert.ToInt16(id);
            await tamanhoRepository.AlterarAsync(tamanho);
        }

        public async Task ExcluirAsync(Tamanho tamanho) => await tamanhoRepository.ExcluirAsync(tamanho);

        public async Task<IEnumerable<Tamanho>> ListarAsync() => await tamanhoRepository.ListarAsync();

        public async Task<Tamanho> RecuperarPorIdAsync(int id) => await tamanhoRepository.RecuperarPorIdAsync(id);

        public async Task<Tamanho> SalvarAsync(Tamanho tamanho) => await tamanhoRepository.SalvarAsync(tamanho);
    }
}
