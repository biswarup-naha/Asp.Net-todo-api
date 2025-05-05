using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Models;
using TodoApi.Services;
using TodoApi.Utils;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (await _userService.GetByEmail(registerDto.Email) != null)
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "User already exists"
                    });
                }
                var user = new User
                {
                    Id = "",
                    Name = registerDto.Name,
                    Email = registerDto.Email,
                    Phone = (long)registerDto.Phone,
                    Password = registerDto.Password
                };
    
                await _userService.Add(user);

                var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("Email", user.Email.ToString()),
                        new Claim("Name", user.Name.ToString()),
                        new Claim("Phone", user.Phone.ToString())
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiryInMinutes"])), signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                    );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new ApiResponse<UserDto>
                {
                    Success = true,
                    Message = "User registered",
                    Data = new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Phone = user.Phone
                    },
                    Token = tokenString
                });
            }
            catch (System.Exception e)
            {                
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Something went wrong: "+e.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _userService.GetByEmail(loginDto.Email);
                if (user != null && _userService.CheckPassword(user, loginDto.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString()),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("Email", user.Email.ToString()),
                        new Claim("Name", user.Name.ToString()),
                        new Claim("Phone", user.Phone.ToString())
                    };
    
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiryInMinutes"])), signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                        );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
    
                    return Ok(new ApiResponse<UserDto>
                    {
                        Success = true,
                        Message = "Login successful",
                        Data = new UserDto
                        {
                            Id = user.Id,
                            Name = user.Name,
                            Email = user.Email,
                            Phone = user.Phone
                        },
                        Token = tokenString
                    });
                }
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid credentials"
                });
            }
            catch (System.Exception e)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Something went wrong: "+e.Message
                });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAll();
                return Ok(new ApiResponse<List<User>>
                {
                    Success = true,
                    Message = "Users fetched",
                    Data = users
                });
            }
            catch (System.Exception)
            {
                return BadRequest(new ApiResponse<List<User>>
                {
                    Success = false,
                    Message = "Users not found"
                });
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var user = await _userService.GetById(id);
                return Ok(new ApiResponse<User>
                {
                    Success = true,
                    Message = "User fetched",
                    Data = user
                });
            }
            catch (System.Exception)
            {
                return BadRequest(new ApiResponse<User>
                {
                    Success = false,
                    Message = "User not found"
                });
            }
        }

        // [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(User user)
        {
            try
            {
                await _userService.Add(user);
                return Ok(new ApiResponse<User>
                {
                    Success = true,
                    Message = "User added",
                    Data = user
                });
            }
            catch (System.Exception)
            {
                return BadRequest(new ApiResponse<User>
                {
                    Success = false,
                    Message = "User not added"
                });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User user)
        {
            try
            {
                await _userService.Update(id, user);
                return Ok(new ApiResponse<User>
                {
                    Success = true,
                    Message = "User updated",
                    Data = user
                });
            }
            catch (System.Exception)
            {
                return NotFound(new ApiResponse<User>
                {
                    Success = false,
                    Message = "User with this id not found"
                });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userService.Delete(id);
                return Ok(new ApiResponse<User>
                {
                    Success = true,
                    Message = "User deleted"
                });
            }
            catch (System.Exception)
            {
                return NotFound(new ApiResponse<User>
                {
                    Success = false,
                    Message = "User with this id not found"
                });
            }
        }
    }
}
