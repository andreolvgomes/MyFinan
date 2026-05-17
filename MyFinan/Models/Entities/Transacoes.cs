using DapperExtensions;

namespace MyFinan.Models.Entities
{
    [Table("myfinan.transacoes")]
    public class Transacoes
    {
        [Key]
        public long Id { get; set; }
        public int Categoria_Id { get; set; }

        public DateTime? Data { get; set; }
        public string Beneficiario { get; set; }
        public decimal Valor { get; set; }
    }

    [Table("myfinan.categorias")]
    public class Categorias
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Nome_Contains { get; set; }
    }
}