using System.ComponentModel.DataAnnotations;

namespace AgendaContatos.Mvc.Models
{
    /// <summary>
    /// Modelo de dados para a página de consulta de relatórios
    /// </summary>
    public class RelatoriosConsultaModel
    {
        [Required(ErrorMessage = "Por favor, selecione o formato do relatório.")]
        public string? Formato { get; set; }
    }
}


