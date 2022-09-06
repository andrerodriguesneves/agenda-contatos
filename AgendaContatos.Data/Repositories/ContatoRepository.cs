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
    public class ContatoRepository
    {
        public void Create(Contato contato)
        {
            var sql = @"
                INSERT INTO CONTATO(IDCONTATO, NOME, TELEFONE, EMAIL, CPF, DATANASCIMENTO, IDUSUARIO)
                VALUES(@IdContato, @Nome, @Telefone, @Email, @Cpf, @DataNascimento, @IdUsuario)
            ";

            using (var connection = new SqlConnection(ConnectionConfiguration.GetConnectionString()))
            {
                connection.Execute(sql, contato);
            }
        }

        public void Update(Contato contato)
        {
            var sql = @"
                UPDATE CONTATO 
                SET 
                    NOME = @Nome, TELEFONE = @Telefone, EMAIL = @Email, 
                    CPF = @Cpf, DATANASCIMENTO = @DataNascimento
                WHERE
                    IDCONTATO = @IdContato
                AND
                    IDUSUARIO = @IdUsuario
            ";

            using (var connection = new SqlConnection(ConnectionConfiguration.GetConnectionString()))
            {
                connection.Execute(sql, contato);
            }
        }

        public void Delete(Contato contato)
        {
            var sql = @"
                DELETE FROM CONTATO 
                WHERE
                    IDCONTATO = @IdContato
                AND
                    IDUSUARIO = @IdUsuario
            ";

            using (var connection = new SqlConnection(ConnectionConfiguration.GetConnectionString()))
            {
                connection.Execute(sql, contato);
            }
        }

        public List<Contato> GetAllByUsuario(Guid idUsuario)
        {
            var sql = @"
                SELECT * FROM CONTATO
                WHERE IDUSUARIO = @idUsuario
            ";

            using (var connection = new SqlConnection(ConnectionConfiguration.GetConnectionString()))
            {
                return connection.Query<Contato>(sql, new { idUsuario }).ToList();
            }
        }

        public Contato? GetById(Guid idContato, Guid idUsuario)
        {
            var sql = @"
                SELECT * FROM CONTATO
                WHERE
                    IDCONTATO = @IdContato
                AND
                    IDUSUARIO = @idUsuario
            ";

            using (var connection = new SqlConnection(ConnectionConfiguration.GetConnectionString()))
            {
                return connection.Query<Contato>(sql, new { idContato, idUsuario }).FirstOrDefault();
            }
        }
    }
}
