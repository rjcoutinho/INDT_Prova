using INDT.Viagens.Domain;
using MediatR;
namespace INDT.Viagens.Application.CommandHandler.ViagemCommand;

public class ViagemCommand : IRequest<Rota>
{
    public string Origem { get; set; }
    public string Destino { get; set; }
    public decimal Valor { get; set; }
    public TipoOperacao TipoOperacao { get; set; }

    public ViagemCommand(string origem, string destino, decimal valor, TipoOperacao tipoOperacao)
    {
        Origem = origem;
        Destino = destino;
        Valor = valor;
        TipoOperacao = tipoOperacao;
    }
    public ViagemCommand(TipoOperacao tipoOperacao)
    {
        TipoOperacao = tipoOperacao;
    }
}