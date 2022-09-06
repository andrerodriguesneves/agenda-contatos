namespace AgendaContatos.Mvc.Models
{
    /// <summary>
    /// Modelo de dados para as informações que serão gravadas
    /// no Cookie de autenticação do AspNet MVC
    /// </summary>
    public class AuthenticationModel
    {
        public Guid IdUsuario { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public DateTime DataHoraAcesso { get; set; }
    }
}
