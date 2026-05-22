using CatalogoApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoApp.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _auth;

        public AccountController(AuthService auth)
        {
            _auth = auth;
        }

        // ===== REGISTRO =====

        // GET: muestra el formulario
        public IActionResult Register()
        {
            return View();
        }

        // POST: procesa el registro
        [HttpPost]
        public IActionResult Register(string nombre, string email, string password)
        {
            var (exito, error) = _auth.Registrar(nombre, email, password);

            if (!exito)
            {
                ViewBag.Error = error;
                return View();
            }

            // Registro exitoso → lo mandamos a iniciar sesión
            return RedirectToAction("Login");
        }

        // ===== LOGIN =====

        // GET: muestra el formulario
        public IActionResult Login()
        {
            return View();
        }

        // POST: procesa el login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _auth.Login(email, password);

            if (user == null)
            {
                // Mensaje genérico a propósito (no revelamos si el email existe o no)
                ViewBag.Error = "Email o contraseña incorrectos.";
                return View();
            }

            // Login correcto → guardamos al usuario en la SESIÓN
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserNombre", user.Nombre);

            return RedirectToAction("Index", "Catalogo");
        }

        // ===== CERRAR SESIÓN =====

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}