namespace INDT.Viagens.Application.CommandHandler.ResultadoMelhorRotaHandler;

using INDT.Viagens.Application.CommandHandler.ResultadoMelhorRotaCommand;
using INDT.Viagens.Application.Interfaces;
using INDT.Viagens.Domain;
using MediatR;


public class ResultadoMelhorRotaHandler(IRotaRepository rotaRepository) : IRequestHandler<ResultadoMelhorRotaCommand, ResultadoMelhorRota>
{
    private readonly IRotaRepository _rotaRepository = rotaRepository;

    public async Task<ResultadoMelhorRota> Handle(ResultadoMelhorRotaCommand request, CancellationToken cancellationToken)
    {
        var rotas = _rotaRepository.LoadRotas();
        var resultado = _rotaRepository.EncontrarMelhorRota(rotas, request.Origem, request.Destino);
        return await Task.FromResult(resultado);
    }
}
