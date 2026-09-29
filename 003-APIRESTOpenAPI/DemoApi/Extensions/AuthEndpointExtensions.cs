using DemoApi.Dtos;
using DemoApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoApi.Extensions;

public static class AuthEndpointExtensions
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", (LoginRequestDto request, IConfiguration configuration) =>
        {
            var errors = request.GetValidationErrors();
            if (errors.Count > 0)
                return Results.BadRequest(new ApiResponse<object>
                {
                    IsSuccess = false,
                    Message = "La solicitud contiene datos inválidos.",
                    Errors = errors
                });

            // Demo only: production must validate credentials against an identity provider or user store.
            if (request.Username != "admin" || request.Password != "P@ssw0rd!")
                return Results.Unauthorized();

            var key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is required.");
            var issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is required.");
            var audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is required.");
            var expiresInMinutes = configuration.GetValue("Jwt:ExpiresInMinutes", 60);
            var expiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, request.Username),
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, "Administrator")
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return Results.Ok(new ApiResponse<object>
            {
                IsSuccess = true,
                Message = "Token generado correctamente.",
                Data = new
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    TokenType = "Bearer",
                    ExpiresAt = expiresAt
                }
            });
        })
        .WithName("Login")
        .WithTags("Auth")
        .AllowAnonymous()
        .Produces<ApiResponse<object>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized);

        return app;
    }
}