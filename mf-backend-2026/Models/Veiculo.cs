using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mf_backend_2026.Models
{
    [Table("Veiculos")]
    public class Veiculo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Marca é obrigatório.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "O campo Placa é obrigatório.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "O campo Ano de Fabricação é obrigatório.")]
        [Display(Name = "Ano de Fabricação")]
        public int? AnoFabricacao { get; set; }
        
        [Required(ErrorMessage = "O campo Ano do Modelo é obrigatório.")]
        [Display (Name ="Ano do Modelo")]
        public int? AnoModelo { get; set; }

      

    }
}
