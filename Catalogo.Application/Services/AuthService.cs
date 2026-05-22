using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _repo;

        public AuthService(IUserRepository repo)
        {
            _repo = repo;
        }

        // Resultado del registro: exito o el motivo del fallo.
        public (bool Exito, string? Error) Registrar(string nombre, string email, string password)
        {
            // Validaciones de negocio (esto es lo que faltaba en ItemService)
            if (string.IsNullOrWhiteSpace(nombre))
                return (false, "El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(email))
                return (false, "El email es obligatorio.");

            if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
                return (false, "La contraseña debe tener al menos 4 caracteres.");

            // ¿Ya existe alguien con ese email?
            if (_repo.ObtenerPorEmail(email) != null)
                return (false, "Ya existe una cuenta con ese email.");

            // Aquí ocurre la magia: hasheamos la contraseña.
            // Nunca guardamos el texto plano.
            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Nombre = nombre,
                Email = email,
                PasswordHash = hash
            };

            _repo.Agregar(user);
            return (true, null);
        }

        // Devuelve el User si las credenciales son correctas, o null si no.
        public User? Login(string email, string password)
        {
            var user = _repo.ObtenerPorEmail(email);

            // Si no existe el usuario, no hay login.
            if (user == null)
                return null;

            // BCrypt compara la contraseña escrita contra el hash guardado.
            bool coincide = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            return coincide ? user : null;
        }
    }
}