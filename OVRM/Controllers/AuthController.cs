﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OVRM.Data;
using OVRM.DTO;
using OVRM.Models;

namespace OVRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly OVRMDbContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        public AuthController(UserManager<IdentityUser> userManager,RoleManager<IdentityRole> rolemanager,IConfiguration config,OVRMDbContext context, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _roleManager = rolemanager;
            _context = context;
            _config = config;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminDTO registerAdminDTO)
        {
            var user = new IdentityUser { UserName = registerAdminDTO.Email, Email = registerAdminDTO.Email };
            var result = await _userManager.CreateAsync(user, registerAdminDTO.Password);
            if (result.Succeeded)
            {
                const string role = "Admin";

                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                await _userManager.AddToRoleAsync(user, role);
                return Ok("Admin registered successfully");
            }
            return BadRequest(result.Errors);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCustomerDTO registerCustomerDTO)
        {
            var user = new IdentityUser { UserName = registerCustomerDTO.Email, Email = registerCustomerDTO.Email, PhoneNumber = registerCustomerDTO.PhoneNumber };
            var result = await _userManager.CreateAsync(user, registerCustomerDTO.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(registerCustomerDTO.Role))
                    await _roleManager.CreateAsync(new IdentityRole(registerCustomerDTO.Role));

                await _userManager.AddToRoleAsync(user, registerCustomerDTO.Role);

                // Save customer info if role is Customer
                if (registerCustomerDTO.Role == "Customer")
                {
                    var customer = new Customer
                    {
                        IdentityUserId = user.Id,
                        Name = registerCustomerDTO.FullName,
                        Email = registerCustomerDTO.Email,
                        Phone = registerCustomerDTO.PhoneNumber
                    };
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                }

                return Ok("User registered successfully");
            }

            return BadRequest(result.Errors);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                _logger.LogError("Invalid login attempt for user: {Email}", loginDto.Email);
                return Unauthorized("Invalid credentials");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty), // Ensure non-null value
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            authClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token=new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])), SecurityAlgorithms.HmacSha256)
            );
            _logger.LogInformation("User {Email} logged in successfully", loginDto.Email);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                roles
            });
        }

    }
}
