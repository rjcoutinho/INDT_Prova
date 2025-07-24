namespace INDT.Viagens.Application.CommandHandler.ViagemHandler;

using INDT.Viagens.Application.CommandHandler.ViagemCommand;
using INDT.Viagens.Application.Interfaces;
using INDT.Viagens.Domain;
using MediatR;
using System.Threading;

public class ViagemHandler : IRequestHandler<ViagemCommand, Rota>
{
    private readonly IRotaRepository _rotaRepository;

    public ViagemHandler(IRotaRepository rotaRepository)
    {
        _rotaRepository = rotaRepository;
    }

    public async Task<Rota> Handle(ViagemCommand command, CancellationToken cancellationToken)
    {
        var rotas = _rotaRepository.LoadRotas();

        switch (command.TipoOperacao)
        {
            case TipoOperacao.CriarRota:
                var novaRota = new Rota
                {
                    Origem = command.Origem,
                    Destino = command.Destino,
                    Valor = command.Valor
                };
                rotas.Add(novaRota);
                _rotaRepository.SaveRotas(rotas);
                return novaRota;

            case TipoOperacao.AlterarRota:
                var rotaToUpdate = rotas.FirstOrDefault(r => r.Origem == command.Origem && r.Destino == command.Destino);
                if (rotaToUpdate != null)
                {
                    rotaToUpdate.Valor = command.Valor;
                    _rotaRepository.SaveRotas(rotas);
                    return rotaToUpdate;
                }
                break;

            case TipoOperacao.DeletarRota:
                var rotaToRemove = rotas.FirstOrDefault(r => r.Origem == command.Origem && r.Destino == command.Destino);
                if (rotaToRemove != null)
                {
                    rotas.Remove(rotaToRemove);
                    _rotaRepository.SaveRotas(rotas);
                    return rotaToRemove;
                }
                break;

            case TipoOperacao.ConsultarRotas:
                var rotasEncontradas = rotas
                    .Where(r => r.Origem == command.Origem && r.Destino == command.Destino)
                    .ToList();
                if (rotasEncontradas.Any())
                {
                    return rotasEncontradas.First();
                }
                break;
        }

        return null;
    }

}
