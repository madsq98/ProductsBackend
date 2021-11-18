﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsBackend.CoreWebAPI.DTO;
using ProductsBackend.CoreWebAPI.PolicyHandlers;
using ProductsBackend.Security;
using ProductsBackend.Security.Model;

namespace ProductsBackend.CoreWebAPI.Controllers
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
        
        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var tokenString = _authService.GenerateJwtToken(new LoginUser
            {
                UserName = dto.Username,
                HashedPassword = _authService.Hash(dto.Password)
            });
            if (string.IsNullOrEmpty(tokenString))
            {
                return BadRequest("Please pass the valid Username and Password");
            }
            return Ok(new { Token = tokenString, Message = "Success" });
        }
        
        [Authorize(Policy=nameof(CanReadProductsHandler))]
        [HttpGet(nameof(GetProfile))]
        public ActionResult<ProfileDto> GetProfile()
        {
            var user = HttpContext.Items["LoginUser"] as LoginUser;
            if (user != null)
            {
                List<Permission> permissions = _authService.GetPermissions(user.Id);
                return Ok(new ProfileDto
                {
                    Permissions = permissions.Select(p => p.Name).ToList(),
                    Name = user.UserName
                });
            }

            return Unauthorized();
        }
        
    }
}