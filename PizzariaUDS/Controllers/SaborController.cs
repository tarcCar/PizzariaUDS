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
    public class SaborController : ControllerBase
    {
        private readonly ISaborService saborService;

        public SaborController(ISaborService saborService)
        {
            this.saborService = saborService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RecuperarPorIdAsync([FromRoute] int id, [FromServices] IMemoryCache memoryCache)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id não pode ser menor ou igual a 0.");

                //Colocar o sabor com id em cache, pq raramente o sabor vai mudar
                var sabor = await memoryCache.GetOrCreateAsync($"sabor{id}", async context =>
                {
                    //define quanto tempo o cache vai ficar em memoria
                    context.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                    //prioridade quando o sistema for limpar a memoria
                    context.SetPriority(CacheItemPriority.Normal);
                    return await saborService.RecuperarPorIdAsync(id);
                });

                if (sabor == null)
                    return NotFound($"Não foi encontrado o sabor com id: {id}");

                return Ok(sabor);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro na hora de recuperar o sabor de pizza do id: {id}");
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListarAsync([FromServices] IMemoryCache cache)
        {
            try
            {
                //verifica se tem a lista de sabores em cache, se sim se não lista novamente os sabores
                //Coloquei o cache no sabores de pizza pq dificilmente vai ter mais um sabor de pizza
                var sabores = await cache.GetOrCreate(
                       "listaSaboresPizza",
                       async context =>
                       {
                           //define quanto tempo o cache vai ficar em memoria
                           context.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                           //prioridade quando o sistema for limpar a memoria
                           context.SetPriority(CacheItemPriority.Normal);
                           return await saborService.ListarAsync();
                       }
                   );
                if (sabores.Count() == 0)
                    return NotFound();

                return Ok(sabores);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro na hora de listar os sabores");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SalvarAsync([FromBody] Sabor sabor, [FromServices] IMemoryCache memoryCache)
        {
            try
            {
                sabor = await saborService.SalvarAsync(sabor);
                //Remove o cache de lista das pizza, por que tem um sabor novo
                memoryCache.Remove("listaSaboresPizza");
                string urlCriado = Url.Action("RecuperarPorIdAsync", "Sabor", new { id = sabor.Id });
                return Created(urlCriado, sabor);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro na hora de salvar o sabor de pizza");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExcluirAsync([FromRoute] int id,[FromServices] IMemoryCache memoryCache)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id não pode ser menor ou igual a 0.");

                var sabor = await saborService.RecuperarPorIdAsync(id);

                if (sabor == null)
                    return NotFound($"Não foi encontrado o sabor com id: {id}");

                await saborService.ExcluirAsync(sabor);
                //Remove o cache de lista das pizza, por que o sabor pode estár o cache da lista
                memoryCache.Remove("listaSaboresPizza");
                //remove o cache da sabor pelo id, por que o sabor pode estár o cache do id
                memoryCache.Remove($"sabor{id}");
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro na hora de excluir o sabor de pizza do id: {id}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarAsync([FromRoute] int id, [FromBody] Sabor sabor, [FromServices] IMemoryCache memoryCache)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id não pode ser menor ou igual a 0.");

                var saborPizza = await saborService.RecuperarPorIdAsync(id);

                if (saborPizza == null)
                    return NotFound($"Não foi encontrado o sabor com id: {id}");

                await saborService.AlterarAsync(id, sabor);

                //remove o cache da sabor pelo id, para que o recuperar por id retorna com valor correto
                memoryCache.Remove($"sabor{id}");
                return Ok(sabor);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro na hora de excluir o sabor de pizza do id: {id}");
            }
        }
    }
}