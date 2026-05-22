using System.Text.Json;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Infrastructure.Repositories
{
    public class JsonUserRepository : IUserRepository
    {
        private readonly string _filePath;

        public JsonUserRepository(string filePath)
        {
            _filePath = filePath;

            var carpeta = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta))
                Directory.CreateDirectory(carpeta);
        }

        public List<User> ObtenerTodos()
        {
            if (!File.Exists(_filePath))
                return new List<User>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<User>>(json)
                   ?? new List<User>();
        }

        public User? ObtenerPorId(int id)
        {
            return ObtenerTodos().FirstOrDefault(u => u.Id == id);
        }

        public User? ObtenerPorEmail(string email)
        {
            // Comparacion sin distinguir mayusculas:
            // "Jorge@correo.com" y "jorge@correo.com" son el mismo usuario.
            return ObtenerTodos()
                   .FirstOrDefault(u =>
                       u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public void Agregar(User user)
        {
            var users = ObtenerTodos();

            // Auto-incrementar el Id, igual que en JsonItemRepository
            user.Id = users.Count > 0
                      ? users.Max(u => u.Id) + 1
                      : 1;

            users.Add(user);
            Guardar(users);
        }

        private void Guardar(List<User> users)
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(users, opciones);
            File.WriteAllText(_filePath, json);
        }
    }
}