using AgendaContatos.Data.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaContatos.Reports
{
    /// <summary>
    /// Classe para geração de relatório de contatos em formato Excel
    /// </summary>
    public class ContatosReportExcel : IReport<Contato>
    {
        public byte[] Create(List<Contato> data)
        {
            //definindo o uso 'free' da biblioteca
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //abrindo uma planilha
            using (var excelPackage = new ExcelPackage())
            {
                var planilha = excelPackage.Workbook.Worksheets.Add("Contatos");

                #region Título da planilha

                planilha.Cells["A1"].Value = "Relatório de Contatos";
                var titulo = planilha.Cells["A1:F1"];
                titulo.Merge = true;
                titulo.Style.Font.Size = 18;
                titulo.Style.Font.Bold = true;

                #endregion

                planilha.Cells["A2"].Value = "Gerado em:";
                planilha.Cells["B2"].Value = DateTime.Now.ToString("dd/MM/yyyy");

                #region Cabeçalho das colunas de dados

                planilha.Cells["A4"].Value = "Id";
                planilha.Cells["B4"].Value = "Nome";
                planilha.Cells["C4"].Value = "Telefone";
                planilha.Cells["D4"].Value = "Email";
                planilha.Cells["E4"].Value = "CPF";
                planilha.Cells["F4"].Value = "Data de Nascimento";

                var cabecalho = planilha.Cells["A4:F4"];
                cabecalho.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#ffffff"));
                cabecalho.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cabecalho.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#000000"));

                #endregion

                #region Linhas de dados

                var linha = 5;
                foreach (var item in data)
                {
                    planilha.Cells[$"A{linha}"].Value = item.IdContato;
                    planilha.Cells[$"B{linha}"].Value = item.Nome;
                    planilha.Cells[$"C{linha}"].Value = item.Telefone;
                    planilha.Cells[$"D{linha}"].Value = item.Email;
                    planilha.Cells[$"E{linha}"].Value = item.Cpf;
                    planilha.Cells[$"F{linha}"].Value = Convert.ToDateTime(item.DataNascimento).ToString("dd/MM/yyyy");

                    if (linha % 2 == 0)
                    {
                        var conteudo = planilha.Cells[$"A{linha}:F{linha}"];
                        conteudo.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        conteudo.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#eeeeee"));
                    }

                    linha++;
                }

                #endregion

                //ajustar a largura das colunas
                planilha.Cells["A:F"].AutoFitColumns();

                //borda na área de dados
                var dados = planilha.Cells[$"A4:F{linha - 1}"];
                dados.Style.Border.BorderAround(ExcelBorderStyle.Medium);

                //retornando o conteudo do arquivo
                return excelPackage.GetAsByteArray();
            }
        }
    }
}



