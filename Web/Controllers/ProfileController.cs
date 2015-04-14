using System.Web.Mvc;
using System.Web.Security;
using Contracts.Business;
using Contracts.Framework;
using Entities;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IObjectService _objectService;
        private readonly ISessionManager _sessionManager;
        public ProfileController(IObjectService objectService, ISessionManager sessionManager)
        {
            _objectService = objectService;
            _sessionManager = sessionManager;
        }

        [AllowAnonymous]
        public ActionResult Logon()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Logon(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var player = _objectService.GetFirst<Player>(x => x.UserName == model.UserName);
                if (player == null)
                {
                    ModelState.AddModelError("", "Such user does not exist!");
                    return View(model);
                }

                if (player.Passsword != model.Password)
                {
                    ModelState.AddModelError("", "Password incorrect");
                    return View(model);
                }

                _sessionManager.CreateOrValidate(player);
                FormsAuthentication.SetAuthCookie(model.UserName, true);
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Incorrect user name or password.");
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public new ActionResult Profile()
        {
            return View(Framework.Session.Session.Current.Player);
        }
    }
}
