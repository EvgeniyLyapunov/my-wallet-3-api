using Microsoft.AspNetCore.Mvc;
using MyWallet3.API.DTOs.Auth;
using MyWallet3.API.DTOs.Common;
using MyWallet3.Application.Interfaces.Auth;
using MyWallet3.Application.Interfaces.Services;

namespace MyWallet3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Result>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                await _authService.RegisterAsync(request.Login, request.Password);
                return Ok(Result.Success());
            }
            catch (Exception ex)
            {
                return BadRequest(Result.Fail(ex.Message));
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(Result<JwtTokens>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<JwtTokens>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result<JwtTokens>>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var tokens = await _authService.LoginAsync(request.Login, request.Password);
                return Ok(Result<JwtTokens>.Success(tokens));
            }
            catch (Exception ex)
            {
                return Unauthorized(Result<JwtTokens>.Fail(ex.Message));
            }
        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(Result<JwtTokens>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<JwtTokens>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Result<JwtTokens>>> Refresh([FromBody] RefreshRequest request)
        {
            try
            {
                var tokens = await _authService.RefreshTokensAsync(request.RefreshToken);
                return Ok(Result<JwtTokens>.Success(tokens));
            }
            catch (Exception ex)
            {
                return Unauthorized(Result<JwtTokens>.Fail(ex.Message));
            }
        }

        [HttpPost("logout")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result>> Logout([FromBody] RefreshRequest request)
        {
            await _authService.LogoutAsync(request.RefreshToken);
            return Ok(Result.Success());
        }
    }
}
