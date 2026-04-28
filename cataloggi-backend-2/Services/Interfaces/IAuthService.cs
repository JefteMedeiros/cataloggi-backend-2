using cataloggi_backend_2.DTOs.Auth;

namespace cataloggi_backend_2.Services.Interfaces;

public interface IAuthService
{
    LoginResponseDto? Login(LoginRequestDto dto);
}
