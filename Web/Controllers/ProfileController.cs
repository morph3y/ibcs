using System.Web.Mvc;
using System.Web.Security;
using Contracts.Business;
using Contracts.Session;
using Entities;
using Web.Infrastructure;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IPlayerService _playerService;
        private readonly IRankingService _rankingService;
        public ProfileController(IPlayerService playerService, IRankingService rankingService)
        {
            _playerService = playerService;
            _rankingService = rankingService;
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
                var player = _playerService.Get(x => x.UserName == model.UserEmail);
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

            var newPlayer = new Player
            {
                FirstName = "",
                LastName = "",
                IsAdmin = false,
                Name = "",
                Passsword = model.UserPassword,
                UserName = model.UserEmail
            };

            _playerService.Save(newPlayer);
            _rankingService.InitRank(newPlayer);

            return RedirectToAction("Logon");
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
                var player = _playerService.Get(x => x.UserName == model.UserName);
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

                Response.Cookies.Add(CookieManager.CreateCookie(new PlayerPrincipal(player.UserName) { Id = player.Id, IsAdmin = player.IsAdmin }));
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
            var user = _playerService.Get(x => x.Id == Contracts.Session.Session.Current.Id);
            return View(new PlayerViewModel
            {
                DisplayName = user.Name,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }

        [HttpPost]
        public ActionResult Profile(PlayerViewModel player)
        {
            var user = _playerService.Get(x => x.Id == Contracts.Session.Session.Current.Id);
            if (ModelState.IsValid)
            {
                user.FirstName = player.FirstName;
                user.LastName = player.LastName;
                user.Name = player.DisplayName;

                _playerService.Save(user);
            }
            else
            {
                return View(player);
            }

            return RedirectToAction("Profile");
        }
    }
}
