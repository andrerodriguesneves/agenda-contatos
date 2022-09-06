using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaContatos.Data.Entities
{
    public class Usuario
    {
        #region Propriedades

        public Guid IdUsuario { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public DateTime DataCadastro { get; set; }

        #endregion

        #region Relacionamentos

        public List<Contato>? Contatos { get; set; }

        #endregion
    }
}
