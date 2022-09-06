using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaContatos.Data.Configurations
{
    public class ConnectionConfiguration
    {
        public static string GetConnectionString()
        {
            return @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DB_AGENDACONTATOS;Integrated Security=True;
                    Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;
                    ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }
    }
}
