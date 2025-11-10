using System.ComponentModel.DataAnnotations;
using GestionDeMisiones.IService;

namespace GestionDeMisiones.Web.DTOs
{
    public class LoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public AuthUser User { get; set; } = new();
    }

    public class RegisterResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public AuthUser User { get; set; } = new();
    }

    public class MeResponse
    {
        public AuthUser User { get; set; } = new();
    }
}
