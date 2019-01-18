using PizzariaUDS.Models;
using PizzariaUDS.Repositories.Interfaces;
using PizzariaUDS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Services
{
    /// <summary>
    /// Classe da camanda de regra de negocios do sabor
    /// </summary>
    public class SaborService : ISaborService
    {
        private readonly ISaborRepository saborRepository;

        public SaborService(ISaborRepository saborRepository)
        {
            this.saborRepository = saborRepository;
        }
        public async Task AlterarAsync(int id, Sabor sabor)
        {
            sabor.Id = Convert.ToInt16(id);
            await saborRepository.AlterarAsync(sabor);
        }

        public async Task ExcluirAsync(Sabor sabor) => await saborRepository.ExcluirAsync(sabor);

        public async Task<IEnumerable<Sabor>> ListarAsync() => await saborRepository.ListarAsync();

        public async Task<Sabor> RecuperarPorIdAsync(int id) => await saborRepository.RecuperarPorIdAsync(id);

        public async Task<Sabor> SalvarAsync(Sabor sabor) => await saborRepository.SalvarAsync(sabor);
    }
}
