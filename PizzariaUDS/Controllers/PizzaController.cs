using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PizzariaUDS.Models;
using PizzariaUDS.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaUDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly IPizzaService pizzaService;
        private readonly ITamanhoService tamanhoService;
        private readonly ISaborService saborService;

        public PizzaController(IPizzaService pizzaService, ITamanhoService tamanhoService, ISaborService saborService)
        {
            this.pizzaService = pizzaService;
            this.tamanhoService = tamanhoService;
            this.saborService = saborService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RecuperarPorIdAsync([FromRoute] int id, [FromServices] IMemoryCache memoryCache)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id não pode ser menor ou igual a 0.");

                //Colocar o pizza com id em cache, pq raramente a pizza vai mudar
                var pizza = await memoryCache.GetOrCreateAsync($"pizza{id}", async context =>
                {
                    //define quanto tempo o cache vai ficar em memoria
                    context.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                    //prioridade quando o sistema for limpar a memoria
                    context.SetPriority(CacheItemPriority.Normal);
                    return await pizzaService.RecuperarPorIdAsync(id);
                });

                if (pizza == null)
                    return NotFound($"Não foi encontrado o pizza com id: {id}");

                return Ok(pizza);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro na hora de recuperar a pizza do id: {id}");
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarAsync([FromServices] IMemoryCache memoryCache)
        {
            try
            {
                var pizzas = await memoryCache.GetOrCreateAsync("listaPizza", async context =>
                {
                    //define quanto tempo o cache vai ficar em memoria
                    context.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                    //prioridade quando o sistema for limpar a memoria
                    context.SetPriority(CacheItemPriority.Normal);
                    return await pizzaService.ListarAsync();
                });

                if (pizzas.Count() == 0)
                    return NotFound("Não foi encontrado nenhuma pizza");
                return Ok(pizzas);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro na hora de listar de pizzas");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SalvarAsync([FromBody] Pizza pizza, [FromServices] IMemoryCache memoryCache)
        {
            try
            {
                if (pizza.SaborId <= 0)
                    return BadRequest("Id do sabor não pode ser menor ou igual a zero!");

                if (pizza.TamanhoId <= 0)
                    return BadRequest("Id do tamanho não pode ser menor ou igual a zero!");

                var tamanho = await tamanhoService.RecuperarPorIdAsync(pizza.TamanhoId);

                if (tamanho == null)
                    return BadRequest("O tamanho informado não existe!");

                var sabor = await saborService.RecuperarPorIdAsync(pizza.SaborId);

                if (sabor == null)
                    return BadRequest("O sabor informado não existe!");

                pizza = await pizzaService.SalvarAsync(pizza);
                //Remove o cache de lista das pizza, por que tem um sabor novo
                memoryCache.Remove("listaPizza");
                string urlCriado = Url.Action("RecuperarPorIdAsync", "Pizza", new { id = pizza.Id });
                return Created(urlCriado, pizza);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro na hora de salvar a pizza");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExcluirAsync([FromRoute] int id, [FromServices] IMemoryCache memoryCache)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id não pode ser menor ou igual a 0.");

                var pizza = await pizzaService.RecuperarPorIdAsync(id);

                if (pizza == null)
                    return NotFound($"Não foi encontrado a pizza com id: {id}");

                var temPedido = await pizzaService.TemPedidoAsync(pizza);

                if (temPedido)
                    return BadRequest("Não é possivel apagar uma pizza que já tenha pedido!");

                await pizzaService.ExcluirAsync(pizza);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro na hora de excluir a pizza do id: {id}");
            }
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarAsync([FromRoute] int id, [FromBody] Pizza pizza, [FromServices] IMemoryCache memoryCache)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id não pode ser menor ou igual a 0.");

                var pizzaExistente = await pizzaService.RecuperarPorIdAsync(id);

                if (pizzaExistente == null)
                    return NotFound($"Não foi encontrada a pizza com id: {id}");

                if (pizza.SaborId <= 0)
                    return BadRequest("Id do sabor não pode ser menor ou igual a zero!");

                if (pizza.TamanhoId <= 0)
                    return BadRequest("Id do tamanho não pode ser menor ou igual a zero!");

                var tamanho = await tamanhoService.RecuperarPorIdAsync(pizza.TamanhoId);

                if (tamanho == null)
                    return BadRequest("O tamanho informado não existe!");

                var sabor = await saborService.RecuperarPorIdAsync(pizza.SaborId);

                if (sabor == null)
                    return BadRequest("O sabor informado não existe!");

                await pizzaService.AlterarAsync(id, pizza);

                //remove o cache da pizza pelo id, para que o recuperar por id retorna com valor correto
                memoryCache.Remove($"pizza{id}");
                return Ok(pizza);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro na hora de excluir a pizza do id: {id}");
            }
        }

        [HttpPost("{id}/Adicional")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AdiconarAdicionalAsync([FromRoute] int id, [FromBody] Adicional adicional)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Id do sabor não pode ser menor ou igual a zero!");

                if (adicional == null)
                    return BadRequest("Adicional não pode ser nulo!");

                var pizza = await pizzaService.RecuperarPorIdAsync(id);

                if (pizza == null)
                    return BadRequest("Pizza escolhida não existe");

                
                if (pizza.TemAdicional(adicional))
                    return BadRequest($"Essa pizza ja tem o adicional {adicional.Descricao}!");

                await pizzaService.AdicionarAdicionalAsync(pizza, adicional);

                return Ok(pizza);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro na hora de salvar a pizza");
            }
        }
    }
}