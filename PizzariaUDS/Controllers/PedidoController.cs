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
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService pedidoService;

        public PedidoController(IPedidoService pedidoService)
        {
            this.pedidoService = pedidoService;
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

                var pedido = await pedidoService.RecuperarPorIdAsync(id);

                if (pedido == null)
                    return NotFound($"Não foi encontrado o pedido com id: {id}");

                return Ok(pedido);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro na hora de recuperar o pedido  do id: {id}");
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
          
                var pedidos = await pedidoService.ListarAsync();
                       
                if (pedidos.Count() == 0)
                    return NotFound();

                return Ok(pedidos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro na hora de listar os pedidos");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SalvarAsync([FromBody] Pedido pedido)
        {
            try
            {
                pedido = await pedidoService.SalvarAsync(pedido);
                string urlCriado = Url.Action("RecuperarPorIdAsync", "Pedido", new { id = pedido.Id });
                return Created(urlCriado, pedido);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro na hora de salvar o pedido!");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExcluirAsync([FromRoute] int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id não pode ser menor ou igual a 0.");

                var pedido = await pedidoService.RecuperarPorIdAsync(id);

                if (pedido == null)
                    return NotFound($"Não foi encontrado o pedido com id: {id}");

                await pedidoService.ExcluirAsync(pedido);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro na hora de excluir o pedido do id: {id}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarAsync([FromRoute] int id, [FromBody] Pedido pedido)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("id não pode ser menor ou igual a 0.");

                var pedidoExiste = await pedidoService.RecuperarPorIdAsync(id);

                if (pedidoExiste == null)
                    return NotFound($"Não foi encontrado o pedido com id: {id}");

                await pedidoService.AlterarAsync(id, pedido);

                return Ok(pedido);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro na hora de excluir o pedido id: {id}");
            }
        }
    }
}