using AgendaContatos.Data.Helpers;
using AgendaContatos.Data.Repositories;
using AgendaContatos.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendaContatos.Mvc.Controllers
{
    [Authorize]
    public class UsuariosController : Controller
    {
        public IActionResult MinhaConta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MinhaConta(AlterarSenhaModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var usuarioRepository = new UsuarioRepository();

                    //capturar o usuário que está autenticado no sistema
                    var authenticationModel = JsonConvert
                        .DeserializeObject<AuthenticationModel>(User.Identity.Name);

                    //verificar se a senha atual informada está correta
                    if(usuarioRepository.GetByEmailAndSenha
                        (authenticationModel.Email, MD5Helper.Encrypt(model.SenhaAtual)) != null)
                    {
                        //atualizar a senha do usuário no banco de dados
                        usuarioRepository.Update(authenticationModel.IdUsuario, MD5Helper.Encrypt(model.NovaSenha));
                        TempData["Mensagem"] = "Senha atualizada com sucesso. Acesse o sistema novamente utilizando sua nova senha.";
                    }
                    else
                    {
                        TempData["Mensagem"] = "Senha atual inválida.";
                    }
                }
                catch(Exception e)
                {
                    TempData["Mensagem"] = e.Message;
                }
            }

            return View();
        }
    }
}
