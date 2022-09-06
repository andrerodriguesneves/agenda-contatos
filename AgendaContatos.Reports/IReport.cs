using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaContatos.Reports
{
    /// <summary>
    /// Interface padrão para geração de relatórios
    /// </summary>
    /// <typeparam name="T">Tipo da entidade</typeparam>
    public interface IReport<T>
    {
        /// <summary>
        /// Método para geração dos relatórios
        /// </summary>
        /// <param name="data">Lista de objetos para preencher o relatório</param>
        /// <returns>Relatório em formato bytes (arquivo em memória)</returns>
        byte[] Create(List<T> data);
    }
}



