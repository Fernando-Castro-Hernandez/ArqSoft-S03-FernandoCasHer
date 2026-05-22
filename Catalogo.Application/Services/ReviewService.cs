using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Application.Services
{
    public class ReviewService
    {
        private readonly IReviewRepository _repo;

        public ReviewService(IReviewRepository repo)
        {
            _repo = repo;
        }

        public List<Review> ObtenerPorItem(int itemId)
        {
            return _repo.ObtenerPorItem(itemId);
        }

        // Crea una review ya validada. Devuelve (exito, error) como AuthService.
        public (bool Exito, string? Error) Agregar(
            int itemId, int userId, string nombreUser, string comentario, int rating)
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(comentario))
                return (false, "El comentario no puede estar vacío.");

            if (rating < 1 || rating > 5)
                return (false, "El rating debe estar entre 1 y 5.");

            var review = new Review
            {
                ItemId = itemId,
                UserId = userId,
                NombreUser = nombreUser,
                Comentario = comentario,
                Rating = rating
            };

            _repo.Agregar(review);
            return (true, null);
        }

        // Calcula el promedio de rating de una carta (para el "Rating promedio")
        public double ObtenerPromedio(int itemId)
        {
            var reviews = _repo.ObtenerPorItem(itemId);

            if (reviews.Count == 0)
                return 0;

            return Math.Round(reviews.Average(r => r.Rating), 1);
        }
    }
}