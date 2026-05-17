namespace MyFinan.Models.Dtos
{
    public class TransacaoResponse
    {
        public DateTime? Data { get; set; }
        public string Beneficiario { get; set; }
        public decimal Valor { get; set; }
    }
}