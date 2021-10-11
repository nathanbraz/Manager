using System;
using System.Threading.Tasks;
using Manager.API.Utilities;
using Manager.API.Utilities.Token;
using Manager.API.ViewModels;
using Manager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Manager.API.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, ITokenGenerator tokenGenerator, IUserService userService)
        {
        _configuration = configuration;
        _tokenGenerator = tokenGenerator;
        _userService = userService;
        }

        [HttpPost]
        [Route("/api/v1/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            try
            {
                var user = await _userService.GetByEmail(loginViewModel.Login);

                if(user == null)
                {
                    return StatusCode(401, Responses.UnautohrizedErrorMessage());
                }

                if(loginViewModel.Login == user.Email && loginViewModel.Password == user.Password)
                {
                    return Ok(new ResultViewModel
                    {
                        Message = "Usuário autenticado com sucesso.",
                        Success = true,
                        Data = new 
                        {
                            Token = _tokenGenerator.GenerateToken(user.Email),
                            TokenExpires = DateTime.UtcNow.AddMinutes(1)
                        }
                    });
                } 
                else
                {
                    return StatusCode(401, Responses.UnautohrizedErrorMessage());
                }
            }
            catch (Exception)
            {                
                return StatusCode(500, Responses.ApplicationErrorMessage());
            } 
        }

  }
}