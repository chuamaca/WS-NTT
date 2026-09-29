using System.ComponentModel.DataAnnotations;

namespace DemoApi.Dtos;

/// <summary>
/// Credenciales utilizadas para solicitar un token de acceso.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario.
    /// </summary>
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}