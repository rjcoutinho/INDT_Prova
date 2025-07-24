using INDT.Viagens.Domain;
using MediatR;

namespace INDT.Viagens.Application.CommandHandler.ResultadoMelhorRotaCommand;

public class ResultadoMelhorRotaCommand : IRequest<ResultadoMelhorRota>
{
    public string Origem { get; set; }
    public string Destino { get; set; }

    public ResultadoMelhorRotaCommand(string origem, string destino)
    {
        Origem = origem;
        Destino = destino;
    }
}
