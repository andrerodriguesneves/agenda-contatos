using AgendaContatos.Data.Entities;
using AgendaContatos.Data.Helpers;
using AgendaContatos.Data.Repositories;
using AgendaContatos.Messages;
using AgendaContatos.Mvc.Models;
using Bogus;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AgendaContatos.Mvc.Controllers
{
    public class AccountController : Controller
    {
        //Account/Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost] //Recebe o SUBMIT do formulário
        public IActionResult Login(AccountLoginModel model)
        {
            //verificando se todos os campos passaram nas validações
            if(ModelState.IsValid)
            {
                try
                {
                    //pesquisar o usuário no banco de dados através do email e da senha
                    var usuarioRepository = new UsuarioRepository();
                    var usuario = usuarioRepository.GetByEmailAndSenha(model.Email, MD5Helper.Encrypt(model.Senha));

                    //verificar se o usuário foi encontrado
                    if(usuario != null)
                    {
                        //autenticando o usuário da aplicação
                        RealizarLoginDoUsuario(usuario);

                        //redirecionando o usuário para a página de consulta de contatos
                        return RedirectToAction("Consulta", "Contatos");
                    }
                    else
                    {
                        TempData["Mensagem"] = "Acesso negado, usuário inválido.";
                    }
                }
                catch(Exception e)
                {
                    TempData["Mensagem"] = $"Falha ao autenticar o usuário: {e.Message}.";
                }
            }

            return View();
        }

        //Account/Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost] //Recebe o SUBMIT do formulário
        public IActionResult Register(AccountRegisterModel model)
        {
            //verificar se todos os campos da model
            //passaram nas regras de validação
            if(ModelState.IsValid)
            {
                try
                {
                    //criando um objeto da classe UsuarioRepository
                    var usuarioRepository = new UsuarioRepository();
                    //verificar se o email informado já está cadastrado no banco de dados
                    if(usuarioRepository.GetByEmail(model.Email) != null)
                    {
                        TempData["Mensagem"] = "O email informado já está cadastrado para outro o usuário, tente novamente.";
                    }
                    else
                    {
                        //Realizar o cadastro do usuário
                        var usuario = new Usuario();
                        usuario.IdUsuario = Guid.NewGuid();
                        usuario.Nome = model.Nome;
                        usuario.Email = model.Email;
                        usuario.Senha = MD5Helper.Encrypt(model.Senha);
                        usuario.DataCadastro = DateTime.Now;

                        //gravando no banco de dados
                        usuarioRepository.Create(usuario);

                        TempData["Mensagem"] = $"Parabéns {usuario.Nome}, sua conta foi cadastrada com sucesso!";
                        ModelState.Clear(); //limpar o conteudo dos campos do formulário
                    }
                }
                catch (Exception e)
                {
                    TempData["Mensagem"] = $"Falha ao cadastrar usuário: {e.Message}";
                }
            }

            return View();
        }

        //Account/Password
        public IActionResult Password()
        {
            return View();
        }

        [HttpPost] //Recebe o SUBMIT do formulário
        public IActionResult Password(AccountPasswordModel model)
        {
            //verificar se os dados da model passaram nas validações
            if(ModelState.IsValid)
            {
                try
                {
                    //consultar o usuário no banco de dados
                    var usuarioRepository = new UsuarioRepository();
                    var usuario = usuarioRepository.GetByEmail(model.Email);

                    //verificar se o usuário foi encontrado..
                    if(usuario != null)
                    {
                        //redefinindo a senha do usuário
                        RedefinirSenhaDeAcesso(usuario);

                        ModelState.Clear(); //limpar os campos do formulário
                        TempData["Mensagem"] = $"Sucesso! Uma nova senha foi enviada para o seu email.";
                    }
                    else
                    {
                        TempData["Mensagem"] = "Nenhum usuário foi encontrado com o endereço de email informado.";
                    }
                }
                catch(Exception e)
                {
                    TempData["Mensagem"] = $"Falha ao recuperar senha: {e.Message}";
                }
            }

            return View();
        }

        //Account/Logout
        public IActionResult Logout()
        {
            //fazendo o logout do usuário
            RealizarLogoutUsuario();

            //redirecionando de volta para /Account/Login
            return RedirectToAction("Login", "Account");
        }

        private void RealizarLoginDoUsuario(Usuario usuario)
        {
            //criando um objeto da classe de modelo de dados
            var model = new AuthenticationModel
            {
                IdUsuario = usuario.IdUsuario,
                Nome = usuario.Nome,
                Email = usuario.Email,
                DataHoraAcesso = DateTime.Now
            };

            //serializando os dados da model para JSON
            var json = JsonConvert.SerializeObject(model);

            //criar o conteudo que será gravado no Cookie de autenticação
            var claimsIdentity = new ClaimsIdentity
                (new[] { new Claim(ClaimTypes.Name, json) }, 
                CookieAuthenticationDefaults.AuthenticationScheme);

            //gravando o Cookie de autenticação
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }

        private void RealizarLogoutUsuario()
        {
            //remover o cookie de autenticação gravado no navegador
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private void RedefinirSenhaDeAcesso(Usuario usuario)
        {
            #region Gerando uma nova senha para o usuário

            var faker = new Faker();
            var novaSenha = faker.Internet.Password(8);

            #endregion

            #region Enviando a nova senha por email para o usuário

            var mailTo = usuario.Email;
            var subject = "Recuperação de Senha - Sistema Agenda de Contatos.";
            var body = $@"
                <div>
                    <p>Olá {usuario.Nome}, sua senha foi redefinida com sucesso.</p>
                    <p>Utilize a senha <strong>{novaSenha}</strong> para acessar sua conta na Agenda de Contatos.</p>
                    <p>Você poderá, acessando o sistema, alterar esta senha para outra de sua preferência.</p>
                    <br/>
                    <p>Att,</p>
                    <p>Equipe Agenda de Contatos</p>
                    <p><small>Este é um email automático, por favor não responda.</small></p>
                </div>
            ";

            var emailMessage = new EmailMessage();
            emailMessage.SendMail(mailTo, subject, body);

            #endregion

            #region Atualizando a senha do usuário no banco de dados

            var usuarioRepository = new UsuarioRepository();
            usuarioRepository.Update(usuario.IdUsuario, MD5Helper.Encrypt(novaSenha));

            #endregion
        }
    }
}
