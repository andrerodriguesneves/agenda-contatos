using AgendaContatos.Data.Entities;
using AgendaContatos.Data.Repositories;
using AgendaContatos.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AgendaContatos.Reports;

namespace AgendaContatos.Mvc.Controllers
{
    [Authorize]
    public class RelatoriosController : Controller
    {
        public IActionResult Consulta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Consulta(RelatoriosConsultaModel model)
        {
            try
            {
                //capturar os dados do usuário autenticado..
                var auth = JsonConvert.DeserializeObject<AuthenticationModel>(User.Identity.Name);

                //consultar os contatos no banco de dados do usuário autenticado.
                var contatoRepository = new ContatoRepository();
                var contatos = contatoRepository.GetAllByUsuario(auth.IdUsuario);

                //polimorfismo
                IReport<Contato> report;
                var nomeArquivo = string.Empty;
                var tipoArquivo = string.Empty;

                switch (model.Formato)
                {
                    case "excel":
                        report = new ContatosReportExcel();
                        nomeArquivo = $"contatos_{Guid.NewGuid()}.xlsx";
                        tipoArquivo = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;

                    case "pdf":
                        report = new ContatosReportPdf();
                        nomeArquivo = $"contatos_{Guid.NewGuid()}.pdf";
                        tipoArquivo = "application/pdf";
                        break;

                    default:
                        throw new Exception("Formato inválido.");
                }

                //download do relatório
                return File(report.Create(contatos), tipoArquivo, nomeArquivo);
            }
            catch (Exception e)
            {
                TempData["Mensagem"] = $"Falha ao gerar relatório: {e.Message}";
            }

            return View();
        }
    }
}



