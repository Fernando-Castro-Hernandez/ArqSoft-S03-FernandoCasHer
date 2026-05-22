namespace CatalogoApp.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Aquí NO se guarda la contraseña en texto plano:
        // guardamos el hash que genera BCrypt.
        public string PasswordHash { get; set; } = string.Empty;
    }
}