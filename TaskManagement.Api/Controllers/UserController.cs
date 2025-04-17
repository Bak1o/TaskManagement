using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Models.Enums;
using TaskManagement.Identity.Requests;
using TaskManagement.Identity.Responses;
using TaskManagement.Identity.Services.Abstractions;
using TaskManagement.Service.Services.Abstractions;
using LoginRequest = TaskManagement.Identity.Requests.LoginRequest;
using RegisterRequest = TaskManagement.Identity.Requests.RegisterRequest;
using ResetPasswordRequest = TaskManagement.Identity.Requests.ResetPasswordRequest;


namespace TaskManagement.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
       

        public UserController(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
            
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<LoginResponse>> AuthenticateAsync([FromBody] LoginRequest request)
        {
            return Ok(await _identityService.AuthenticateAsync(request));
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginResponse?>> RegisterAsync([FromBody] RegisterRequest request)
        {


            return Ok(await _identityService.RegisterAsync(request));

            //var result = await _identityService.RegisterAsync(request);
            //if (result is not null)
            //{
            //    //var customer = new Customer(request.Email,request.Email,request.FirstName, request.LastName,result.UserId)
            //    //{

            //    //};

            //    //Role role = Enum.Parse<Role>(request.Role); // Convert string to Enum if needed
            //    //customer.open(role);

            //    //// Save customer to repository or perform other actions as needed
            //    //await _repository.CreateAsync(customer);
            //    return Ok(result);
            //}
            //return BadRequest("Registration failed");
        }

        [HttpPost("confirm-email")]
        public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            await _identityService.ConfirmEmailAsync(request);
            return Ok();
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<LoginResponse>> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
        {
            return Ok(await _identityService.ChangePasswordAsync(request));
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            await _identityService.ResetPasswordAsync(request);
            return Ok();
        }

        [HttpPost("new-password")]
        public async Task<ActionResult<LoginResponse>> NewPasswordAsync([FromBody] NewPasswordRequest request)
        {
            return Ok(await _identityService.NewPasswordAsync(request));
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginResponse>> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
        {
            return Ok(await _tokenService.RefreshTokenAsync(request.RefreshToken));
        }

    }
}
