using System.Text.Json;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Infrastructure.Repositories
{
    public class JsonReviewRepository : IReviewRepository
    {
        private readonly string _filePath;

        public JsonReviewRepository(string filePath)
        {
            _filePath = filePath;

            var carpeta = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta))
                Directory.CreateDirectory(carpeta);
        }

        // Método privado: lee TODAS las reviews del archivo.
        // Es privado porque la interfaz no expone "obtener todas":
        // por fuera solo se piden las de un item.
        private List<Review> LeerTodas()
        {
            if (!File.Exists(_filePath))
                return new List<Review>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Review>>(json)
                   ?? new List<Review>();
        }

        // Solo las reviews de la carta indicada
        public List<Review> ObtenerPorItem(int itemId)
        {
            return LeerTodas()
                   .Where(r => r.ItemId == itemId)
                   .ToList();
        }

        public void Agregar(Review review)
        {
            var reviews = LeerTodas();

            // Auto-incrementar el Id, igual que en los otros repositorios
            review.Id = reviews.Count > 0
                        ? reviews.Max(r => r.Id) + 1
                        : 1;

            reviews.Add(review);
            Guardar(reviews);
        }

        private void Guardar(List<Review> reviews)
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(reviews, opciones);
            File.WriteAllText(_filePath, json);
        }
    }
}