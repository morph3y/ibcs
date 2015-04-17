using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Contracts.Business;
using Entities;
using Framework.Session;
using Web.Infrastructure;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IObjectService _objectService;
        public ProfileController(IObjectService objectService)
        {
            _objectService = objectService;
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

                Response.Cookies.Add(CookieManager.CreateCookie(new PlayerPrincipal(player.UserName) { Id = player.Id }));
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
            return View(_objectService.GetFirst<Player>(x => x.Id == Framework.Session.Session.Current.Id));
        }

        [HttpPost]
        public ActionResult Profile(Player player)
        {
            player = _objectService.GetFirst<Player>(x => x.Id == Framework.Session.Session.Current.Id);
            TryUpdateModel(player);
            _objectService.Save(player);
            return RedirectToAction("Profile");
        }

        [AllowAnonymous]
        public void CreateAdmin()
        {
            _objectService.Save(new Player
            {
                FirstName = "Alex",
                LastName = "Denysenko",
                IsAdmin = true,
                Name = "Alex Denysenko",
                Passsword = "admin",
                UserName = "admin"
            });
        }
    }
}
