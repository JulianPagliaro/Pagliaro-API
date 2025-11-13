using GameStore.Application.Dtos.Identity.User;
using GameStore.Application.Dtos.Login;
using GameStore.Entities.MicrosoftIdentity;
using GameStore.Services.AuthServices;
using GameStore.WebApi.Configurations;
using GameStore.WebAPI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenHandlerService _servicioToken;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<EditoresController> _logger;
        public AuthController(
            UserManager<User> userManager
            , ILogger<EditoresController> logger, ITokenHandlerService servicioToken)
        {
            _userManager = userManager;
            _logger = logger;
            _servicioToken = servicioToken;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UserRegistroRequestDto user)
        {
            if (ModelState.IsValid)
            {
                var existeUsuario = await _userManager.FindByEmailAsync(user.Email);
                if (existeUsuario != null)
                {
                    return BadRequest("Existe un usuario registrado con el mal " + user.Email + ".");
                }
                var Creado = await _userManager.CreateAsync(new User()
                {
                    Email = user.Email,
                    UserName = user.Email.Substring(0, user.Email.IndexOf('@')),
                    Nombres = user.Nombres,
                    Apellidos = user.Apellidos,
                    FechaNacimiento = user.FechaNacimiento
                }, user.Password);
                if (Creado.Succeeded)
                {
                    return Ok(new UserRegistroResponseDto
                    {
                        NombreCompleto = string.Join(" ", user.Nombres, user.Apellidos),
                        Email = user.Email,
                        UserName = user.Email.Substring(0, user.Email.IndexOf('@'))
                    });
                }

                else
                {
                    return BadRequest(Creado.Errors.Select(e => e.Description).ToList());
                }
            }
            else
            {
                return BadRequest("Los datos enviados no son validos.");
            }
        }

        [HttpPost]
        [Route("RegisterSincronico")]
        public IActionResult RegistrarUsuarioincronico([FromBody] UserRegistroRequestDto user)
        {
            if (ModelState.IsValid)
            {
                var existeUsuario = _userManager.FindByEmailAsync(user.Email).Result;
                if (existeUsuario != null)
                {
                    return BadRequest("Existe un usuario registrado con el mal " + user.Email + ".");
                }
                var Creado = _userManager.CreateAsync(new User()
                {
                    Email = user.Email,
                    UserName = user.Email.Substring(0, user.Email.IndexOf('@')),
                    Nombres = user.Nombres,
                    Apellidos = user.Apellidos,
                    FechaNacimiento = user.FechaNacimiento
                }, user.Password).Result;
                if (Creado.Succeeded)
                {
                    var userBack = _userManager.FindByEmailAsync(user.Email);
                    _ = _userManager.AddToRoleAsync(userBack.Result, "Admin");
                    return Ok(new UserRegistroResponseDto
                    {
                        NombreCompleto = string.Join(" ", user.Nombres, user.Apellidos),
                        Email = user.Email,
                        UserName = user.Email.Substring(0, user.Email.IndexOf('@'))
                    });
                }
                else
                {
                    return BadRequest(Creado.Errors.Select(e => e.Description).ToList());
                }
            }
            else
            {
                return BadRequest("Los datos enviados no son validos.");
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestDto userlogin)
        {
            if (ModelState.IsValid)
            {
                var existeUsuario = await _userManager.FindByEmailAsync(userlogin.Email);
                if (existeUsuario != null)
                {
                    var isCorrect = await _userManager.CheckPasswordAsync(existeUsuario, userlogin.Password);
                    if (isCorrect)
                    {
                        try
                        {
                            var roles = await _userManager.GetRolesAsync(existeUsuario);
                            var parametros = new TokenParameters()
                            {
                                Id = existeUsuario.Id.ToString(),
                                PaswordHash = existeUsuario.PasswordHash,
                                UserName = existeUsuario.UserName,
                                Email = existeUsuario.Email,
                                Roles= roles
                            };
                            var jwt = _servicioToken.GenerateJwtTokens(parametros);
                            return Ok(new LoginUserResponseDto()
                            {
                                Login = true,
                                Token = jwt,
                                UserName = existeUsuario.UserName,
                                Mail = existeUsuario.Email
                            });
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
            }
            return BadRequest(new LoginUserResponseDto()
            {
                Login = false,
                Errores = new List<string>()
                    {
                       "Usuario o contraseña incorrecto!"
                    }
            });
        }
    }
}