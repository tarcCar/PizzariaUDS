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
    public class AdicionalController : ControllerBase
    {
        private readonly IAdicionalService adicionalService;

        public AdicionalController(IAdicionalService adicionalService)
        {
            this.adicionalService = adicionalService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RecuperarPorIdAsync([FromRoute] int id, [FromServices] IMemoryCache memoryCache)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id não pode ser menor ou igual a 0.");

                var adicionalPizza = await memoryCache.GetOrCreateAsync($"adicional{id}", async context =>
                 {
                     //define quanto tempo o cache vai ficar em memoria
                     context.SetAbsoluteExpiration(TimeSpan.FromSeconds(120));
                     //prioridade quando o sistema for limpar a memoria
                     context.SetPriority(CacheItemPriority.Normal);
                     return await adicionalService.RecuperarPorIdAsync(id);
                 });


                if (adicionalPizza == null)
                    return NotFound($"Não foi encontrado o adicional de pizza com id: {id}");

                return Ok(adicionalPizza);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro na hora de recuperar o adicional da pizza do id: {id}");
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListarAsync([FromServices] IMemoryCache cache)
        {
            try
            {
                //verifica se tem a lista de adicionais em cache, se sim se não lista novamente os adicionais
                //Coloquei o cache no adicionais de pizza pq dificilmente vai ter mais um adicional de pizza
                var adicionais = await cache.GetOrCreate(
                       $"listaAdicionalPizza",
                       async context =>
                       {
                           //define quanto tempo o cache vai ficar em memoria
                           context.SetAbsoluteExpiration(TimeSpan.FromSeconds(120));
                           //prioridade quando o sistema for limpar a memoria
                           context.SetPriority(CacheItemPriority.Normal);
                           return await adicionalService.ListarAsync();
                       }
                   );
                if(adicionais.Count() == 0)
                {
                    return NotFound("Não foi encontrado nenhum adicional!");
                }

                return Ok(adicionais);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro na hora de listar os adicionais da pizza");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SalvarAsync([FromBody] Adicional adicional,[FromServices] IMemoryCache memoryCache)
        {
            try
            {

                if (adicional == null)
                    return BadRequest("O adicional da pizza não pode ser nulo");

                adicional = await adicionalService.SalvarAsync(adicional);

                //Remove o cache de lista de adicionais, por que tem um adicional novo
                memoryCache.Remove("listaAdicionalPizza");

                string urlCriado = Url.Action("RecuperarPorIdAsync", "Adicional", new { id = adicional.Id });
                return Created(urlCriado, adicional);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro na hora de salvar o adicional da pizza");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AlterarAsync([FromRoute]int id, [FromBody] Adicional adicional,[FromServices] IMemoryCache memoryCache)
        {
            try
            {

                if (adicional == null)
                    return BadRequest("O adicional da pizza não pode ser nulo");

                await adicionalService.AlterarAsync(id,adicional);
                //remove o cache da sabor pelo id, para que o recuperar por id retorna com valor correto
                memoryCache.Remove($"adicional{id}");
                //Remove o cache de lista dos adicionais, por que o adicional pode estár o cache da lista
                memoryCache.Remove("listaAdicionalPizza");
                return Ok(adicional);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro na hora de salvar o adicional da pizza");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExcluirAsync([FromRoute] int id, [FromServices] IMemoryCache memoryCache)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id não pode ser menor ou igual a 0.");

                var adicional = await adicionalService.RecuperarPorIdAsync(id);

                if (adicional == null)
                    return NotFound($"Não foi encontrado o adicional com id: {id}");

                var temPizza = await adicionalService.AdicionalTemPizzasAsync(adicional);
                if (temPizza)
                    return BadRequest("Não é possivel excluir um adicional que já foi incluso em uma pizza!");

                await adicionalService.ExcluirAsync(adicional);
                //Remove o cache de lista dos adicionais, por que o adicional pode estár o cache da lista
                memoryCache.Remove("listaAdicionalPizza");
                //remove o cache da adicional pelo id, por que o adicional pode estár o cache do id
                memoryCache.Remove($"adicional{id}");


                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro na hora de recuperar o adicional da pizza do id: {id}");
            }
        }
    }
}