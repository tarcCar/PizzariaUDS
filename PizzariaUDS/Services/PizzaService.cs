using PizzariaUDS.Models;
using PizzariaUDS.Repositories.Interfaces;
using PizzariaUDS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Services
{
    public class PizzaService : IPizzaService
    {
        private readonly IPizzaRepository pizzaRepository;

        public PizzaService(IPizzaRepository pizzaRepository)
        {
            this.pizzaRepository = pizzaRepository;
        }

        public async Task<Pizza> AdicionarAdicionalAsync(Pizza pizza, Adicional adicional)
        {
            pizza.AdicionalAdicional(adicional);
            return await pizzaRepository.AdicionarAdicionalAsync(pizza, adicional);
        }

        public async Task AlterarAsync(int id, Pizza pizza)
        {
            pizza.Id = id;
            await pizzaRepository.AlterarAsync(pizza);
        }

        public async Task ExcluirAsync(Pizza pizza) => await pizzaRepository.ExcluirAsync(pizza);

        public async Task<IEnumerable<Pizza>> ListarAsync() => await pizzaRepository.ListarAsync();

        public async Task<Pizza> RecuperarPorIdAsync(int id) => await pizzaRepository.RecuperarPorIdAsync(id);

        public async Task<Pizza> SalvarAsync(Pizza pizza) => await pizzaRepository.SalvarAsync(pizza);
    }
}
