using INDT.Viagens.Application.Interfaces;
using INDT.Viagens.Domain;
using System.Text.Json;

namespace INDT.Viagens.Infra
{
    public class Service: IRotaRepository
    {
        private static readonly string FilePath = "rotas.json";
        public List<Rota> LoadRotas()
        {
            if (!System.IO.File.Exists(FilePath))
                return new List<Rota>();
            var json = System.IO.File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Rota>>(json) ?? new List<Rota>();
        }

        public void SaveRotas(List<Rota> rotas)
        {
            var json = JsonSerializer.Serialize(rotas);
            System.IO.File.WriteAllText(FilePath, json);
        }      

        public ResultadoMelhorRota EncontrarMelhorRota(List<Rota> rotas, string origem, string destino)
        {
            var melhor = new ResultadoMelhorRota { Caminho = null, Custo = decimal.MaxValue };
            void Buscar(string atual, List<string> caminho, decimal custo)
            {
                if (caminho.Contains(atual)) return;
                caminho.Add(atual);
                if (atual == destino)
                {
                    if (custo < melhor.Custo)
                    {
                        melhor.Caminho = new List<string>(caminho);
                        melhor.Custo = custo;
                    }
                }
                else
                {
                    foreach (var rota in rotas.Where(r => r.Origem == atual))
                    {
                        Buscar(rota.Destino, caminho, custo + rota.Valor);
                    }
                }
                caminho.RemoveAt(caminho.Count - 1);
            }
            Buscar(origem, new List<string>(), 0);
            return melhor.Caminho == null ? null : melhor;
        }
    }
}