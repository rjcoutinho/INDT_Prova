namespace INDT.Viagens.Domain
{
    public class Rota
    {
        public string Origem { get; set; }
        public string Destino { get; set; }
        public decimal Valor { get; set; }
    }
    public class ResultadoMelhorRota
    {
        public List<string> Caminho { get; set; }
        public decimal Custo { get; set; }
    }
}
