using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OnlineExamSystem.Models;
using OnlineExamSystem.Services;
using System.Security.Claims;

namespace OnlineExamSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        // In-memory storage for refresh tokens (in production, use a database)
        private static readonly Dictionary<string, RefreshTokenInfo> _refreshTokens = new Dictionary<string, RefreshTokenInfo>();

        public AuthController(IUserService userService, IJwtService jwtService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid request data"
                    });
                }

                var user = await _userService.AuthenticateAsync(request.Email, request.Password);
                if (user == null)
                {
                    return Unauthorized(new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    });
                }

                var accessToken = _jwtService.GenerateAccessToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken();
                var expiresAt = DateTime.UtcNow.AddMinutes(60); // 1 hour

                // Store refresh token
                _refreshTokens[refreshToken] = new RefreshTokenInfo
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    CreatedAt = DateTime.UtcNow
                };

                _logger.LogInformation($"User {user.Email} logged in successfully");

                return Ok(new AuthResponse
                {
                    Success = true,
                    Token = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt,
                    User = user,
                    Message = "Login successful"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
                return StatusCode(500, new AuthResponse
                {
                    Success = false,
                    Message = "An error occurred during login"
                });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid request data"
                    });
                }

                var user = await _userService.RegisterAsync(request);
                if (user == null)
                {
                    return BadRequest(new AuthResponse
                    {
                        Success = false,
                        Message = "User with this email already exists"
                    });
                }

                var accessToken = _jwtService.GenerateAccessToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken();
                var expiresAt = DateTime.UtcNow.AddMinutes(60);

                // Store refresh token
                _refreshTokens[refreshToken] = new RefreshTokenInfo
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    CreatedAt = DateTime.UtcNow
                };

                _logger.LogInformation($"New user registered: {user.Email} with role: {user.Role}");

                return Ok(new AuthResponse
                {
                    Success = true,
                    Token = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt,
                    User = user,
                    Message = "Registration successful"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
                return StatusCode(500, new AuthResponse
                {
                    Success = false,
                    Message = "An error occurred during registration"
                });
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (!_refreshTokens.TryGetValue(request.RefreshToken, out var refreshTokenInfo))
                {
                    return Unauthorized(new AuthResponse
                    {
                        Success = false,
                        Message = "Invalid refresh token"
                    });
                }

                if (refreshTokenInfo.ExpiresAt < DateTime.UtcNow)
                {
                    _refreshTokens.Remove(request.RefreshToken);
                    return Unauthorized(new AuthResponse
                    {
                        Success = false,
                        Message = "Refresh token has expired"
                    });
                }

                var user = await _userService.GetUserByIdAsync(refreshTokenInfo.UserId);
                if (user == null)
                {
                    return Unauthorized(new AuthResponse
                    {
                        Success = false,
                        Message = "User not found"
                    });
                }

                var newAccessToken = _jwtService.GenerateAccessToken(user);
                var newRefreshToken = _jwtService.GenerateRefreshToken();
                var expiresAt = DateTime.UtcNow.AddMinutes(60);

                // Remove old refresh token and add new one
                _refreshTokens.Remove(request.RefreshToken);
                _refreshTokens[newRefreshToken] = new RefreshTokenInfo
                {
                    UserId = user.Id,
                    Token = newRefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    CreatedAt = DateTime.UtcNow
                };

                return Ok(new AuthResponse
                {
                    Success = true,
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresAt = expiresAt,
                    User = user,
                    Message = "Token refreshed successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return StatusCode(500, new AuthResponse
                {
                    Success = false,
                    Message = "An error occurred during token refresh"
                });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (_refreshTokens.ContainsKey(request.RefreshToken))
                {
                    _refreshTokens.Remove(request.RefreshToken);
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                _logger.LogInformation($"User {userId} logged out successfully");

                return Ok(new { Success = true, Message = "Logout successful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(500, new { Success = false, Message = "An error occurred during logout" });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserInfo>> GetCurrentUser()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user");
                return StatusCode(500, "An error occurred while getting user information");
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid request data");
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                var success = await _userService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
                if (!success)
                {
                    return BadRequest("Current password is incorrect");
                }

                _logger.LogInformation($"User {userId} changed password successfully");
                return Ok(new { Success = true, Message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                return StatusCode(500, "An error occurred while changing password");
            }
        }

        // Helper class for refresh token storage
        private class RefreshTokenInfo
        {
            public int UserId { get; set; }
            public string Token { get; set; } = string.Empty;
            public DateTime ExpiresAt { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
