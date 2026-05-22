namespace CatalogoApp.Domain.Models
{
    public class Review
    {
        public int Id { get; set; }

        // A qué carta pertenece este comentario
        public int ItemId { get; set; }

        // Qué usuario lo escribió
        public int UserId { get; set; }

        // Guardamos el nombre para mostrarlo sin tener que buscar al usuario cada vez
        public string NombreUser { get; set; } = string.Empty;

        public string Comentario { get; set; } = string.Empty;

        public int Rating { get; set; }
    }
}