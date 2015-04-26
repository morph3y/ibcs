using System.Web.Mvc;
using System.Web.Security;
using Contracts.Business;
using Entities;
using Framework.Session;
using Web.Infrastructure;
using Web.Models;
using System.Collections.Generic;

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
        public ActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var player = _objectService.GetFirst<Player>(x => x.UserName == model.UserEmail);
                if (player != null)
                {
                    ModelState.AddModelError("", "Such email already in use");
                    return View(model);
                }

                if (!model.ConfirmPassword.Equals(model.UserPassword)) 
                {
                    ModelState.AddModelError("", "Passwords must match");
                    return View(model);
                }
            }
            else
            {
                
                return View(model);
            }

            _objectService.Save(new Player 
            {
                FirstName = "",
                LastName = "",
                IsAdmin = false,
                Name = "",
                Passsword = model.UserPassword,
                UserName = model.UserEmail
            });

            return Logon(new LoginViewModel {UserName = model.UserEmail, Password = model.UserPassword },"", true);
        }

        [AllowAnonymous]
        public ActionResult Logon()
        {
            return View(new LoginViewModel());
        }




        [HttpPost]
        [AllowAnonymous]
        public ActionResult Logon(LoginViewModel model, string returnUrl, bool fromSignUp)
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

                if (fromSignUp) 
                {
                    return RedirectToAction("Profile", "Profile");
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
        public new ActionResult Profile(Player player)
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
