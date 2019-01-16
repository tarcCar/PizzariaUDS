using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PizzariaUDS.Models;
using PizzariaUDS.Services.Interfaces;

namespace PizzariaUDS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TamanhoController : ControllerBase
    {
        private readonly ITamanhoService tamanhoService;

        public TamanhoController(ITamanhoService tamanhoService)
        {
            this.tamanhoService = tamanhoService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RecuperarPorIdAsync([FromRoute] int id,[FromServices] IMemoryCache memoryCache)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id não pode ser menor ou igual a 0.");

                var tamanho = await memoryCache.GetOrCreateAsync($"tamanho{id}", async context =>
                {
                    return await tamanhoService.RecuperarPorIdAsync(id);
                });

                if (tamanho == null)
                    return NotFound($"Não foi encontrado o tamanho com id: {id}");

                return Ok(tamanho);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro na hora de recuperar o tamanho de pizza do id: {id}");
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
                //verifica se tem a lista de tamanhos em cache, se sim se não lista novamente os tamanhos
                //Coloquei o cache no tamanhos de pizza pq dificilmente vai ter mais um tamanho de pizza
                var tamanhos = await cache.GetOrCreate(
                       $"listaTamanhoPizza",
                       async context =>
                       {
                           //define quanto tempo o cache vai ficar em memoria
                           context.SetAbsoluteExpiration(TimeSpan.FromSeconds(60));
                           //prioridade quando o sistema for limpar a memoria
                           context.SetPriority(CacheItemPriority.Normal);
                           return await tamanhoService.ListarAsync(); 
                       }
                   );
                if (tamanhos.Count() == 0)
                    return NotFound("Não foi encontrado nenhum tamanho");

                return Ok(tamanhos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro na hora de listar os tamanhos");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SalvarAsync([FromBody] Tamanho tamanhoPizza,[FromServices] IMemoryCache memoryCache)
        {
            try
            {
                if (tamanhoPizza.TempoPreparo <= 0)
                    return BadRequest("Tempo de preparo do tamanho não pode ser menor ou igual a 0!");

                if (tamanhoPizza.Valor <= 0)
                    return BadRequest("Valor do tamanho não pode ser menor ou igual a 0!");

                if(string.IsNullOrWhiteSpace(tamanhoPizza.Descricao))
                    return BadRequest("Descrição do tamanho não de ser vazia!");

                tamanhoPizza = await tamanhoService.SalvarAsync(tamanhoPizza);
                //remove o cache da lista de tamanhos por que agora tem um novo tamanho;
                memoryCache.Remove("listaTamanhoPizza");
                string urlCriado = Url.Action("RecuperarPorIdAsync", "Tamanho", new { id = tamanhoPizza.Id });
                return Created(urlCriado,tamanhoPizza);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro na hora de salvar o tamanho de pizza");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExcluirAsync([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id não pode ser menor ou igual a 0.");

                var tamanho = await tamanhoService.RecuperarPorIdAsync(id);

                if (tamanho == null)
                    return NotFound($"Não foi encontrado o tamanho com id: {id}");

                await tamanhoService.ExcluirAsync(tamanho);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro na hora de excluir o tamanho de pizza do id: {id}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AlterarAsync([FromRoute] int id, [FromBody] Tamanho tamanhoPizzaAlterar)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id não pode ser menor ou igual a 0.");

                var tamanhoPizza = await tamanhoService.RecuperarPorIdAsync(id);

                if (tamanhoPizza == null)
                    return NotFound($"Não foi encontrado o tamanho com id: {id}");

                if (tamanhoPizza.TempoPreparo <= 0)
                    return BadRequest("Tempo de preparo do tamanho não pode ser menor ou igual a 0!");

                if (tamanhoPizza.Valor <= 0)
                    return BadRequest("Valor do tamanho não pode ser menor ou igual a 0!");

                if (string.IsNullOrWhiteSpace(tamanhoPizza.Descricao))
                    return BadRequest("Descrição do tamanho não de ser vazia!");

                await tamanhoService.AlterarAsync(tamanhoPizzaAlterar);
                return Ok(tamanhoPizzaAlterar);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro na hora de excluir o tamanho de pizza do id: {id}");
            }
        }
    }
}