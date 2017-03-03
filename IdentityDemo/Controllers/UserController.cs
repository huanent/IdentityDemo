using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;
using IdentityDemo.ViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityDemo.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        UserManager<IdentityUser> _userManager;
        SignInManager<IdentityUser> _signInManager;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IEnumerable<IdentityUser> Get()
        {
            return _userManager.Users;
        }

        [HttpPost, Route("Register")]
        public async Task<bool> RegisterAsync([FromBody]RegisterViewModel register)
        {
            var v = HttpContext;
            var user = new IdentityUser(register.UserName);
            user.Email = register.Email;
            var result = await _userManager.CreateAsync(user, register.Password);
            return result.Succeeded;
        }

        [HttpPost, Route("Login")]
        public async Task<bool> LoginAsync([FromBody]LoginViewModel login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
            }
            return result.Succeeded;
        }

        [HttpPost, Route("GetChangePasswordToken/{userName}")]
        public async Task<string> GetChangePasswordTokenAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        [HttpPost, Route("ChangePassword")]
        public async Task<bool> ChangePasswordAsync([FromBody]ChangePasswordViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            return result.Succeeded;
        }
    }
}