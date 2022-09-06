namespace AgendaContatos.Mvc.Models
{
    /// <summary>
    /// Modelo de dados para a página de consulta
    /// </summary>
    public class ContatosConsultaModel
    {
        public Guid IdContato { get; set; }
        public string? Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? DataNascimento { get; set; }
    }
}
