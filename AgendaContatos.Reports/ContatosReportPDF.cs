using AgendaContatos.Data.Entities;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaContatos.Reports
{
    /// <summary>
    /// Classe para geração de relatório de contatos em formato PDF
    /// </summary>
    public class ContatosReportPdf : IReport<Contato>
    {
        public byte[] Create(List<Contato> data)
        {
            var memoryStream = new MemoryStream();
            var pdf = new PdfDocument(new PdfWriter(memoryStream));

            //abrindo o arquivo PDF
            using (var document = new Document(pdf))
            {
                document.Add(new Paragraph("Relatorio de Contatos").AddStyle(new Style().SetFontSize(24)));
                document.Add(new Paragraph($"Gerado em: {DateTime.Now.ToString("dd/MM/yyyy")}")
                    .AddStyle(new Style().SetFontSize(14)));
                document.Add(new Paragraph("\n"));

                var table = new Table(5);
                table.SetWidth(UnitValue.CreatePercentValue(100));

                table.AddHeaderCell(new Paragraph("Nome").AddStyle(new Style().SetFontSize(12)));
                table.AddHeaderCell(new Paragraph("Telefone").AddStyle(new Style().SetFontSize(12)));
                table.AddHeaderCell(new Paragraph("Email").AddStyle(new Style().SetFontSize(12)));
                table.AddHeaderCell(new Paragraph("CPF").AddStyle(new Style().SetFontSize(12)));
                table.AddHeaderCell(new Paragraph("Data Nasc").AddStyle(new Style().SetFontSize(12)));

                foreach (var item in data)
                {
                    table.AddCell(new Paragraph(item.Nome).AddStyle(new Style().SetFontSize(11)));
                    table.AddCell(new Paragraph(item.Telefone).AddStyle(new Style().SetFontSize(11)));
                    table.AddCell(new Paragraph(item.Email).AddStyle(new Style().SetFontSize(11)));
                    table.AddCell(new Paragraph(item.Cpf).AddStyle(new Style().SetFontSize(11)));
                    table.AddCell(new Paragraph(Convert.ToDateTime(item.DataNascimento).ToString("dd/MM/yyyy"))
                        .AddStyle(new Style().SetFontSize(11)));
                }

                document.Add(table);

                document.Add(new Paragraph($"Quantidade de contatos: {data.Count}")
                    .AddStyle(new Style().SetFontSize(12)));
            }

            //retornar o arquivo PDF em memória
            return memoryStream.ToArray();
        }
    }
}



