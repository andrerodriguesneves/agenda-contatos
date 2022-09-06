using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaContatos.Data.Entities
{
    public class Contato
    {
        #region Propriedades

        public Guid IdContato { get; set; }
        public string? Nome { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? Cpf { get; set; }
        public DateTime? DataNascimento { get; set; }
        public Guid IdUsuario { get; set; }

        #endregion

        #region Relacionamentos

        public Usuario? Usuario { get; set; }

        #endregion
    }
}
