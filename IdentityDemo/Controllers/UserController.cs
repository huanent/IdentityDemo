using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IdentityDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityDemo.Controllers
{

    [Route("api/[controller]")]
    public class UserController : Controller
    {
        readonly UserManager<IdentityUser> _userManager;
        SignInManager<IdentityUser> _signInManager;

        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        // GET: api/values
        [HttpGet,Authorize]
        public IEnumerable<IdentityUser> Get()
        {
            return _userManager.Users;
        }

        // POST api/values
        [HttpPost]
        public bool Post([FromBody]UserViewModel value)
        {
            var user = new IdentityUser(value.Name);
            var result = _userManager.CreateAsync(user, value.Password).Result;
            return result.Succeeded;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost, Route(nameof(Login))]
        public bool Login(UserViewModel model)
        {
            var signInfo = _signInManager.PasswordSignInAsync(model.Name, model.Password, true, false).Result;
            return signInfo.Succeeded;
        }
    }
}
