using BookManagementSystem.DTO;
using BookManagementSystem.Interfaces;
using BookManagementSystem.Models;
using BookManagementSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly mydbcontext _context;
        private readonly ITokenService _tokenService;

        public AccountController( mydbcontext context, ITokenService tokenService)
        {
            this._context = context;
            this._tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await UserExists(registerDto.Username)) return BadRequest("UserName Is Already Taken");
            var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,

            };

            _context.ApplicationUser.Add(user);
            await _context.SaveChangesAsync();
            //return user;

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };


            //if (ModelState.IsValid)
            //{
            //    var user = new User { UserName = model.Username, Email = model.Email };
            //    var result = await _userManager.CreateAsync(user, model.Password);

            //    if (result.Succeeded)
            //    {
            //        await _signInManager.SignInAsync(user, isPersistent: false);
            //        return Ok("User created Successfully");
            //    }
            //    else
            //    {
            //        return BadRequest(result.Errors);
            //    }
            //}

            //return BadRequest(ModelState);
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.ApplicationUser.AnyAsync(x => x.UserName == username.ToLower());
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login(LoginModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        if ( user == null)
        //        {
        //            //User not found
        //            return BadRequest("Invalid Login attemps");
        //        }
        //        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

        //        if (result.Succeeded)
        //        {
        //            // User login successful
        //            return Ok("Login in Successfully");
        //        }
        //        else
        //        {
        //            // Handle login errors
        //            return BadRequest("Invalid login attempt.");
        //        }
        //    }

        //    // Invalid login model
        //    return BadRequest(ModelState);
        //}

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.ApplicationUser
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            if (user == null) return Unauthorized("Invalid UserName");

            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            //return user;

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
