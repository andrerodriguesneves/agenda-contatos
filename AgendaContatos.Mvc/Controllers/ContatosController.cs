using AgendaContatos.Data.Entities;
using AgendaContatos.Data.Repositories;
using AgendaContatos.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendaContatos.Mvc.Controllers
{
    [Authorize]
    public class ContatosController : Controller
    {
        //Contatos/Cadastro
        public IActionResult Cadastro()
        {
            return View();
        }

        //Contatos/Cadastro
        [HttpPost]
        public IActionResult Cadastro(ContatosCadastroModel model)
        {
            //verificando os campos passaram nas regras de validação
            if(ModelState.IsValid)
            {
                try
                {
                    var contato = new Contato
                    {
                        IdContato = Guid.NewGuid(),
                        Nome = model.Nome,
                        Email = model.Email,
                        Cpf = model.Cpf,
                        Telefone = model.Telefone,
                        DataNascimento = Convert.ToDateTime(model.DataNascimento),
                        IdUsuario = GetIdUsuarioAutenticado()
                    };

                    var contatoRepository = new ContatoRepository();
                    contatoRepository.Create(contato);

                    TempData["Mensagem"] = $"Contato {contato.Nome}, cadastrado com sucesso.";
                    ModelState.Clear(); //limpar os campos do formulário
                }
                catch(Exception e)
                {
                    TempData["Mensagem"] = e.Message;
                }
            }

            return View();
        }

        //Contatos/Consulta
        public IActionResult Consulta()
        {
            //declarando uma lista de objetos da classe de modelo de dados
            var lista = new List<ContatosConsultaModel>();

            try
            {
                var contatoRepository = new ContatoRepository();
                foreach (var item in contatoRepository.GetAllByUsuario(GetIdUsuarioAutenticado()))
                {
                    var model = new ContatosConsultaModel
                    {
                        IdContato = item.IdContato,
                        Nome = item.Nome,
                        Cpf = item.Cpf,
                        Email = item.Email,
                        Telefone = item.Telefone,
                        DataNascimento = Convert.ToDateTime(item.DataNascimento).ToString("dd/MM/yyyy")
                    };

                    lista.Add(model);
                }
            }
            catch(Exception e)
            {
                TempData["Mensagem"] = e.Message;
            }

            //enviando a lista para a página
            return View(lista);
        }

        //Contatos/Exclusao/{id}
        public IActionResult Exclusao(Guid id)
        {
            try
            {
                var contato = new Contato
                {
                    IdContato = id,
                    IdUsuario = GetIdUsuarioAutenticado()
                };

                var contatoRepository = new ContatoRepository();
                contatoRepository.Delete(contato);

                TempData["Mensagem"] = $"Contato excluído com sucesso.";
            }
            catch(Exception e)
            {
                TempData["Mensagem"] = e.Message;
            }

            //redirecionar de volta para a página de consulta
            return RedirectToAction("Consulta");
        }

        //Contatos/Edicao/{id}
        public IActionResult Edicao(Guid id)
        {
            var model = new ContatosEdicaoModel();

            try
            {
                //buscando o contato no banco de dados
                var contatoRepository = new ContatoRepository();
                var contato = contatoRepository.GetById(id, GetIdUsuarioAutenticado());

                //preenchendo a classe model
                model.IdContato = contato.IdContato;
                model.Nome = contato.Nome;
                model.Cpf = contato.Cpf;
                model.Telefone = contato.Telefone;
                model.Email = contato.Email;
                model.DataNascimento = Convert.ToDateTime(contato.DataNascimento).ToString("yyyy-MM-dd");
            }
            catch(Exception e)
            {
                TempData["Mensagem"] = e.Message;
            }

            return View(model);
        }

        //Contatos/Edicao
        [HttpPost]
        public IActionResult Edicao(ContatosEdicaoModel model)
        {
            //verificando os campos passaram nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    var contato = new Contato
                    {
                        IdContato = model.IdContato,
                        Nome = model.Nome,
                        Email = model.Email,
                        Cpf = model.Cpf,
                        Telefone = model.Telefone,
                        DataNascimento = Convert.ToDateTime(model.DataNascimento),
                        IdUsuario = GetIdUsuarioAutenticado()
                    };

                    var contatoRepository = new ContatoRepository();
                    contatoRepository.Update(contato);

                    TempData["Mensagem"] = $"Contato {contato.Nome}, atualizado com sucesso.";
                }
                catch (Exception e)
                {
                    TempData["Mensagem"] = e.Message;
                }
            }

            return View(model);
        }

        //método para retornar o id do usuário autenticado
        private Guid GetIdUsuarioAutenticado()
        {
            var auth = JsonConvert.DeserializeObject<AuthenticationModel>(User.Identity.Name);
            return auth.IdUsuario;
        }
    }
}
