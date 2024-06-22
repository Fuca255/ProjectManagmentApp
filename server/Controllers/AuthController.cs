using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using server.Data;
using server.DTOs.Users;
using server.Interfaces;
using server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repos;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IProjectRepository _project_repo;
        private readonly IRoleRepository _role_repo;

        public AuthController(IUserRepository userRepository,IProjectRepository project_repository, IConfiguration configuration,IEmailService emailService, IRoleRepository role_repo)
        {
            _emailService = emailService;
            _repos = userRepository;
            _project_repo = project_repository;
            _configuration = configuration;
            _role_repo = role_repo;
        }

        //Registracija
        
        [HttpPost("register"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> Register(UserRegistrationDto request)
        {
            var existingUserByUsername = await _repos.GetUserByUsernameAsync(request.Username);
            if (existingUserByUsername != null)
            {
                return Ok(new { message = "This username already exists in the database." });
            }
            
            var existingUser = await _repos.GetUserByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return Ok(new { message = "This email already exists in the database." });
                }
            //hash
            string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.Password);

            //Kreiranje novog korisnika
            var newUser = new User
            {
                Username = request.Username,
                Password = passwordHash,
                Email = request.Email,
                Name = request.Name,
                Lastname = request.Lastname,
                RoleId = request.RoleId,
                IsActivated = true,
                Department = request.Department,
                Organisation = request.Organisation
            };

            var role = await _role_repo.GetRoleByIdAsync(request.RoleId);
            if (role != null)
            {
                newUser.JobTitle = role.Name;
            }
            else
            {
                newUser.JobTitle = "Unknown";
            }

            //Dodavanje korsnika u DBContext
            await _repos.AddUserAsync(newUser);

            var emailRequest = new EmailDto
            {
                To = request.Email,
                Subject = "Registration Successful",
                Body = $"Dear {request.Username},\n\nYour registration is successful!\n\nUsername: {request.Username}\nPassword: {request.Password}\n\nThank you for registering."
                //TO DO Napraviti klasu sa telom email-a.
            };

            await _emailService.SendEmailAsync(emailRequest);

            return Ok(newUser);
        }

        //Login
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto request)
        {

            var user = await _repos.GetUserByUsernameAsync(request.Username);
            if(user==null || !user.IsActivated)
            {
                return BadRequest("User or Password incorrect");
            }

            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest("User or Password incorrect");
            }

            var token = await CreateToken(user);
            if(token == null)
                return BadRequest("Login unsuccessful");

            return Ok(token);
        }

        [NonAction]
        public async Task<string?> CreateToken(User user)
        {
            var role = await _role_repo.GetRoleByIdAsync(user.RoleId);
            if(role==null)
                return null;
            string projects = string.Empty;
            var user_projects = await _project_repo.GetAllUserProjectsAsync(user.Id);
            foreach (var project in user_projects)
            {
                projects+=project.Id + ",";
            }
            //postavi username kao claim
            List<Claim> claims = new List<Claim>
            {
                //izmenjeno je zbog toga sto na frontu nije moglo da se uzme
                new Claim("user_id",user.Id.ToString()),
                new Claim("fullname",user.Name + " "+user.Lastname),
                new Claim("role_id", user.RoleId.ToString()),
                new Claim("role", role.Name.ToString()),
                new Claim("projects", projects)
            };

            //kreiraj i verifikuj json token
            var tokenValue = _configuration.GetValue<string>("Jwt:Token");
            if(tokenValue==null)
                return null;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValue));

            //kredencijali
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            //token
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            //write token
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            //return token
            return jwt;

        }





        [HttpPost("forgotpassword")]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            var user = await _repos.GetUserByEmailAsync(email);
            if (user == null)
            {
                // Korisnik nije pronadjen
                return Ok(new { message = "If the provided email exists in our system, a password reset email has been sent." });
            }

            // Reset token
            string resetToken = GenerateResetToken();
            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiry = DateTime.Now.AddHours(1); // Vreme isticanja tokena

            await _repos.SaveChangesAsync(); 

            string resetPasswordUrl = _configuration["ResetPasswordUrl:BaseUrl"];
            var resetLink = $"{resetPasswordUrl}?token={resetToken}";
            
            var emailRequest = new EmailDto
            {
                To = email,
                Subject = "Password Reset Request",
                Body = $@"
            <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                    <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; background-color: #f9f9f9;'>
                        <h2 style='color: #333;'>Password Reset Request</h2>
                        <p>Hi there,</p>
                        <p>You recently requested to reset your password for your account. Click the button below to reset it.</p>
                        <p style='text-align: center;'>
                            <a href='{resetLink}' style='display: inline-block; padding: 10px 20px; margin: 10px 0; font-size: 16px; color: #fff; background-color: #007bff; border-radius: 5px; text-decoration: none;'>Reset Your Password</a>
                        </p>
                        <p>If you did not request a password reset, please ignore this email or let us know.</p>
                        <p>Thanks,<br>Komakai</p>
                        <hr style='border: 0; border-top: 1px solid #eee;'>
                        <p style='font-size: 12px; color: #777;'>If you’re having trouble clicking the password reset button, copy and paste the URL below into your web browser:</p>
                        <p style='font-size: 12px; color: #007bff;'>{resetLink}</p>
                    </div>
                </body>
            </html>"
            };
            await _emailService.SendEmailAsync(emailRequest);

            return Ok(new { message = "If the provided email exists in our system, a password reset email has been sent." });
        }


        [HttpPost("resetpassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto model)
        {
            // Find the user by the reset token
            var user = await _repos.GetUserByResetTokenAsync(model.ResetToken);
            if (user == null)
            {
                // Invalid or expired reset token
                return BadRequest("The reset link is invalid or has expired. Please request a new password reset.");
            }

            // Check if the reset token is expired
            if (user.PasswordResetTokenExpiry < DateTime.Now)
            {
                // Expired reset token
                return BadRequest("The reset link has expired. Please request a new password reset.");
            }

            // Hash the new password
            string newPasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            user.Password = newPasswordHash;

            // Clear the reset token and expiry
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            // Update the user in the database
            await _repos.SaveChangesAsync();

            // Return a success message
            return Ok(new { message = "Your password has been changed successfully. You can now log in with your new password." });
        }

        // Generisanje tokena
        private string GenerateResetToken()
        {
            return Guid.NewGuid().ToString();
        }

    }

}
