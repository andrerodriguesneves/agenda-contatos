using AgendaContatos.Data.Configurations;
using AgendaContatos.Data.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaContatos.Data.Repositories
{
    public class UsuarioRepository
    {
        public void Create(Usuario usuario)
        {
            var sql = @"
                    INSERT INTO USUARIO(IDUSUARIO, NOME, EMAIL, SENHA, DATACADASTRO)
                    VALUES(@IdUsuario, @Nome, @Email, @Senha, @DataCadastro)
                ";

            using (var connection = new SqlConnection(ConnectionConfiguration.GetConnectionString()))
            {
                connection.Execute(sql, usuario);
            }
        }

        public void Update(Guid idUsuario, string novaSenha)
        {
            var sql = @"
                    UPDATE USUARIO SET SENHA = @novaSenha
                    WHERE IDUSUARIO = @idUsuario
                ";

            using (var connection = new SqlConnection(ConnectionConfiguration.GetConnectionString()))
            {
                connection.Execute(sql, new { idUsuario, novaSenha });
            }
        }

        public Usuario? GetByEmail(string email)
        {
            var sql = @"
                    SELECT * FROM USUARIO WHERE EMAIL = @email
                ";

            using (var connection = new SqlConnection(ConnectionConfiguration.GetConnectionString()))
            {
                return connection.Query<Usuario>(sql, new { email }).FirstOrDefault();
            }
        }

        public Usuario? GetByEmailAndSenha(string email, string senha)
        {
            var sql = @"
                    SELECT * FROM USUARIO WHERE EMAIL = @email AND SENHA = @senha
                ";

            using (var connection = new SqlConnection(ConnectionConfiguration.GetConnectionString()))
            {
                return connection.Query<Usuario>(sql, new { email, senha }).FirstOrDefault();
            }
        }
    }
}
