using INDT.Viagens.Domain;

namespace INDT.Viagens.Application.Interfaces;


public interface IRotaRepository
{
    List<Rota> LoadRotas();
    void SaveRotas(List<Rota> rotas);
    ResultadoMelhorRota EncontrarMelhorRota(List<Rota> rotas, string origem, string destino);
}

