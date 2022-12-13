using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.API.Models;
using WebApp.API.Helper;
using System.Text;
using System.Text.RegularExpressions;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly AdventureWorks2017Context _appDbContext;

        public UserController(AdventureWorks2017Context appDbContext)
        {
            _appDbContext = appDbContext;
        }


        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
               return BadRequest();
         
            //var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userObj.UserName && x.Password == userObj.Password);

            var user = await _appDbContext.Users.Where(u => u.UserName == userObj.UserName && u.Password == userObj.Password).FirstOrDefaultAsync();
            if (user == null)
                return NotFound(new { Message = "Not found" });
         
            return Ok(new
            {
                Message = "Login Success!"

            });

        }

  

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();
            if (await CheckUserNameExistAsync(userObj.UserName))
                return BadRequest(new { Message = "Username Already Exist" });
            if (await CheckEmailExistAsync(userObj.Email))
                return BadRequest(new { Message = "Email Already Exist" });
            //var pass = CheckPasswordStrengthAsync(userObj.Password);
            //if (!string.IsNullOrEmpty(pass))
            //    return BadRequest(new { Message = pass.ToString() });


            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";

            await _appDbContext.Users.AddAsync(userObj);
            await _appDbContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Registered Successfully"
            });
                
        }



        private async Task<bool> CheckUserNameExistAsync(string username) {

            return await _appDbContext.Users.AnyAsync(x => x.UserName == username);

        }
        private async Task<bool> CheckEmailExistAsync(string email)
        {

            return await _appDbContext.Users.AnyAsync(x => x.Email == email);

        }

        //private async Task<bool> CheckPasswordStrengthAsync(string password)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if(password.Length < 8)
        //    {
        //        sb.Append("Minimum password length should be 8" + Environment.NewLine);
        //    }

        //    if ((Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password,"[0-9]")))
        //    {
        //        sb.Append("Password should be Alphanumeric" + Environment.NewLine);
        //    }
           
          

        //}

    }
}

