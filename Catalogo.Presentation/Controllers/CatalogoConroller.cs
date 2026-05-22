using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoApp.Presentation.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ItemService _service;
        private readonly ReviewService _reviewService;



        // El servicio llega por inyección de dependencias
        public CatalogoController(ItemService service, ReviewService reviewService)
        {
            _service = service;
            _reviewService = reviewService;
        }

        // Lista con filtro opcional por tipo
        public IActionResult Index(string? tipo)
        {
            var items = string.IsNullOrEmpty(tipo)
                ? _service.ObtenerTodos()
                : _service.ObtenerPorTipo(tipo);

            ViewBag.Tipos = _service.ObtenerTipos();
            ViewBag.TipoActual = tipo;

            return View(items);
        }

        // Detalle de un item (ahora también carga reviews y promedio)
        public IActionResult Detalle(int id)
        {
            var item = _service.ObtenerPorId(id);
            if (item == null)
                return NotFound();

            ViewBag.Reviews = _reviewService.ObtenerPorItem(id);
            ViewBag.Promedio = _reviewService.ObtenerPromedio(id);

            return View(item);
        }

        // Formulario — GET
        public IActionResult Agregar()
        {
            return View();
        }

        // Formulario — POST
        [HttpPost]
        public IActionResult Agregar(Item item)
        {
            _service.Agregar(item);
            return RedirectToAction("Index");
        }

        // Eliminar
        public IActionResult Eliminar(int id)
        {
            _service.Eliminar(id);
            return RedirectToAction("Index");
        }

        // Publicar una review (POST)
        [HttpPost]
        public IActionResult AgregarReview(int itemId, string comentario, int rating)
        {
            // ¿Hay sesión activa? Si no, no puede comentar.
            var userId = HttpContext.Session.GetInt32("UserId");
            var nombreUser = HttpContext.Session.GetString("UserNombre");

            if (userId == null || nombreUser == null)
            {
                // No logueado → lo mandamos a iniciar sesión
                return RedirectToAction("Login", "Account");
            }

            var (exito, error) = _reviewService.Agregar(
                itemId, userId.Value, nombreUser, comentario, rating);

            if (!exito)
            {
                TempData["ReviewError"] = error;
            }

            // Volvemos al detalle de la misma carta
            return RedirectToAction("Detalle", new { id = itemId });
        }
    }
}