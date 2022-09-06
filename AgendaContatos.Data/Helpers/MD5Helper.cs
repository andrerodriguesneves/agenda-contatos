using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AgendaContatos.Data.Helpers
{
    public class MD5Helper
    {
        public static string Encrypt(string value)
        {
            var md5 = new MD5CryptoServiceProvider();

            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));

            var result = string.Empty;

            foreach (var item in hash)
            {
                result += item.ToString("X2");
            }

            return result;
        }
    }
}
