using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthenticationSystemV2.Context;
using UserAuthenticationSystemV2.Generators.PasswordEncryptor;
using UserAuthenticationSystemV2.Models;

namespace UserAuthenticationSystemV2.Controllers
{
      public class AccountController : Controller
    {
        private readonly ApplicationContext _context;

        private readonly IPasswordEncryptor _passwordEncryptor;

        public AccountController(ApplicationContext context, IPasswordEncryptor passwordEncryptor)
        {
            _context = context;
            _passwordEncryptor = passwordEncryptor;
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var userPassword = _passwordEncryptor.Encrypt(model.Password);

                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == userPassword);
                if (user != null)
                {
                    await Authenticate(user);
 
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    var userPassword = _passwordEncryptor.Encrypt(model.Password);

                    user = new User(GenerateUserId(), model.Email, userPassword);

                    _context.Users.Add(user);

                    await _context.SaveChangesAsync();
 
                    await Authenticate(user);
 
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpPost]
        public async Task UnAuthenticate()
        {
            await HttpContext.SignOutAsync();
        }
        
        private async Task Authenticate(User user)
        {
            UnAuthenticate();
            
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim("password", _passwordEncryptor.Decrypt(user.Password))
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
          
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        private Guid GenerateUserId()
        {
            const int maxGenerationIterations = 100;

            Guid id = Guid.NewGuid();

            var generationIterations = 0;
            
            while (_context.Users.Any(user => user.Id.Equals(id)))
            {
                id = Guid.NewGuid();

                generationIterations++;

                if (generationIterations >= maxGenerationIterations)
                    throw new StackOverflowException();
            }

            return id;
        }
    }
}