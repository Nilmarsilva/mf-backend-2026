using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mf_backend_2026.Models
{
    [Table("consumo")]
    public class Consumo
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "O campo 'Descricao' é obrigatório.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo 'Data' é obrigatório.")]
        [Display(Name = "Data")]    
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "O campo 'Valor' é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O campo 'Valor' deve ser maior que zero.")]
        [Display(Name = "Valor")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo 'Quilometragem' é obrigatório.")]
        [Display(Name = "Quilometragem")]
        public int KM { get; set; }

        [Required(ErrorMessage = "O campo 'Tipo Combustivel' é obrigatório.")]
        [Display(Name = "Tipo de Combustível")]
        public TipoCombustivel Tipo { get; set; }

        [Required(ErrorMessage = "O campo 'Veículo' é obrigatório.")]
        [Display(Name = "Veículo")]
        public int VeiculoId { get; set; }

        [ForeignKey("VeiculoId")]
        public Veiculo Veiculo { get; set; }

    }
    public enum TipoCombustivel
    {
        Gasolina,
        Etanol,
        Diesel,
        Outros
    }

}
