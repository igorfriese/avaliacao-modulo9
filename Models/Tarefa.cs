using System.ComponentModel.DataAnnotations;

namespace avaliacao_modulo9.Models
{
    public class Tarefa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título não pode ter mais de 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatório.")]
        [StringLength(500, ErrorMessage = "A descrição não pode ter mais de 500 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A data é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }
        public bool StatusConcluida { get; set; }


        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
