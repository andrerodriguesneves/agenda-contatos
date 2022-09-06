using System.ComponentModel.DataAnnotations;

namespace AgendaContatos.Mvc.Models
{
    /// <summary>
    /// Modelo de dados para edição do contato
    /// </summary>
    public class ContatosEdicaoModel
    {
        public Guid IdContato { get; set; }

        [MinLength(6, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome do contato.")]
        public string? Nome { get; set; }


        [Required(ErrorMessage = "Por favor, informe o telefone do contato.")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Por favor, informe um endereço de email válido.")]
        [Required(ErrorMessage = "Por favor, informe o email do contato.")]
        public string? Email { get; set; }

        [RegularExpression("^[0-9]{11}$", ErrorMessage = "Informe 11 dígitos numéricos.")]
        [Required(ErrorMessage = "Por favor, informe o cpf do contato.")]
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data de nascimento do contato.")]
        public string? DataNascimento { get; set; }
    }
}
