using PizzariaUDS.Models;
using PizzariaUDS.Repositories.Interfaces;
using PizzariaUDS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Services
{
    public class AdicionalService : IAdicionalService
    {
        private readonly IAdicionalRepository adicionalRepository;

        public AdicionalService(IAdicionalRepository adicionalRepository)
        {
            this.adicionalRepository = adicionalRepository;
        }

        public async Task<bool> AdicionalTemPizzasAsync(Adicional adicional) => await adicionalRepository.TemPizzasAsync(adicional);

        public async Task AlterarAsync(int id, Adicional adicional)
        {
            adicional.Id = id;
            await adicionalRepository.AlterarAsync(adicional);
        }

        public async Task ExcluirAsync(Adicional adicional) => await adicionalRepository.ExcluirAsync(adicional);

        public async Task<IEnumerable<Adicional>> ListarAsync() => await adicionalRepository.ListarAsync();

        public async Task<Adicional> RecuperarPorIdAsync(int id) => await adicionalRepository.RecuperarPorIdAsync(id);

        public async Task<Adicional> SalvarAsync(Adicional adicional) => await adicionalRepository.SalvarAsync(adicional);
       
    }
}
