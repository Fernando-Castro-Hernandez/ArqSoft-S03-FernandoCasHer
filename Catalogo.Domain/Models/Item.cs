namespace CatalogoApp.Domain.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Elixir { get; set; }
        public string Calidad { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Arena { get; set; }
    }
}