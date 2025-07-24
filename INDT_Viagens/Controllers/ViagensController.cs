namespace INDT_Viagens.Controllers;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using INDT.Viagens.Domain;
using INDT.Viagens.Application.CommandHandler.ViagemCommand;
using INDT.Viagens.Application.CommandHandler.ResultadoMelhorRotaCommand;
using INDT.Viagens.WebAPI.DTO;

[ApiController]
[Route("[controller]")]
public class ViagensController : ControllerBase
{
    private readonly IMediator _mediator;

    public ViagensController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("CriarNovaRotas")]
    public async Task<ActionResult<RotaDTO>> CriarRota([FromBody] RotaDTO rota)
    {
        var command = new ViagemCommand(
             rota.Origem,
             rota.Destino,
             rota.Valor,
             TipoOperacao.CriarRota);

        var createdRota = await _mediator.Send(command);
        return CreatedAtAction(nameof(CriarRota), createdRota);
    }

    [HttpPut("AlterarRotas")]
    public async Task<ActionResult<RotaDTO>> AlterarRota([FromBody] RotaDTO rota)
    {
        var command = new ViagemCommand(
            rota.Origem,
            rota.Destino,
            rota.Valor,
            TipoOperacao.AlterarRota);

        var updatedRota = await _mediator.Send(command);
        if (updatedRota == null)
        {
            return NotFound("Não foi possível alterar a rota, pois ela não existe");
        }

        return CreatedAtAction(nameof(AlterarRota), updatedRota);
    }

    [HttpDelete("detetarRota")]
    public async Task<ActionResult> DeletarRota([FromQuery] string origem, [FromQuery] string destino)
    {
        var command = new ViagemCommand(
                 origem,
                 destino,
                 0,
                 TipoOperacao.DeletarRota);

        var result = await _mediator.Send(command);
        if (result == null)
        {
            return NotFound("Não foi possível deletar a rota, pois ela não existe");
        }

        return NoContent();
    }

    [HttpGet("BuscarMelhorRota")]
    public async Task<ActionResult<ResultadoMelhorRota>> BuscarMelhorRota([FromQuery] string origem, [FromQuery] string destino)
    {
        var command = new ResultadoMelhorRotaCommand(origem, destino);

        var melhorRota = await _mediator.Send(command);

        return Ok(melhorRota);
    }

}
