using CatalogoApp.Domain.Models;

namespace CatalogoApp.Domain.Interfaces
{
    public interface IReviewRepository
    {
        // Todas las reviews de una carta específica
        List<Review> ObtenerPorItem(int itemId);

        // Agregar una nueva review
        void Agregar(Review review);
    }
}