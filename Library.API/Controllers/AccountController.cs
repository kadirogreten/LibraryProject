using Library.API.Helpers;
using Library.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Library.Domain;
using Library.Domain.Models;
using Library.ServiceLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Library.API.Filters;

namespace Library.API.Controllers
{
    /// <summary>
    /// /
    /// </summary>
    [EnableCors("MyPolicy")]
    [ServiceFilter(typeof(IpAccessActionFilter))]
    [ApiVersion("1.0")]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class AccountsController : Controller
    {

        private UserManager<LibraryUser> _userManager;

        private SignInManager<LibraryUser> _signInManager;
        private ILogger<AccountsController> _logger;



        private readonly IUnitOfWork _uow;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="logger"></param>
        /// <param name="uow"></param>
        public AccountsController(UserManager<LibraryUser> userManager, SignInManager<LibraryUser> signInManager,
            ILogger<AccountsController> logger, IUnitOfWork uow)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;

            _uow = uow;



        }

        /// <summary>
        /// Kullanıcı login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;
            List<string> errors = new List<string>();
            _logger.LogInformation($"Login: {model.Username}", DateTime.Now);

            ResponseMessageFilter response;

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null)
            {
                //var userRole = await _userManager.GetRolesAsync(user);

                if (await IsUsernameAndPassword(model.Username, model.Password))
                {
                    var data = await GenerateToken(model.Username);
                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;
                    _logger.LogInformation($"Login successfull: {model.Username}", DateTime.Now);
                    response = new ResponseMessageFilter
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Message = "Giriş başarılı!",
                        ExecTime = exec_time,
                        Data = data
                    };


                    return Ok(response);
                }
                else
                {
                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;
                    errors.Add("Kullanıcı adı veya şifre hatalı!");
                    response = new ResponseMessageFilter
                    {
                        Code = System.Net.HttpStatusCode.Unauthorized,
                        Success = false,
                        ExecTime = exec_time,
                        Errors = errors.ToArray(),
                        Data = new
                        {

                        }
                    };


                    return Unauthorized(response);
                }
            }
            else
            {
                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                _logger.LogError($"Login not successfull: {model.Username}", DateTime.Now);

                errors.Add("Kullanıcı adı veya şifre hatalı!");
                response = new ResponseMessageFilter
                {
                    Code = System.Net.HttpStatusCode.Unauthorized,
                    Success = false,
                    ExecTime = exec_time,
                    Errors = errors.ToArray(),
                    Data = new
                    {

                    }
                };



                return Unauthorized(response);
            }




        }


        private async Task<bool> IsUsernameAndPassword(string username, string password)
        {
            var user = await this._userManager.FindByEmailAsync(username);

            return await this._userManager.CheckPasswordAsync(user, password);
        }


        private async Task<dynamic> GenerateToken(string username)
        {

            var user = await _userManager.FindByNameAsync(username);

            var userRole = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
                };

            RoleExtension.AddRolesToClaims(claims, userRole);

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KadirOgreten2020!!!!"));

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                expires: DateTime.Now.AddYears(1),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );


            var response = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                userName = user.UserName,
                userId = user.Id
            };

            return response;

        }

        /// <summary>
        /// Kullanıcı kayıt olma
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {

            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;
            List<string> errors = new List<string>();
            _logger.LogInformation($"Register: {model.Email}", DateTime.Now);

            ResponseMessageFilter response;

            if (!ModelState.IsValid)
            {
                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                _logger.LogError($"Register not successfull. validation error: {model.Email}", DateTime.Now);

                errors = ModelState.Values.FirstOrDefault().Errors.Select(a=>a.ErrorMessage).ToList();
                response = new ResponseMessageFilter
                {
                    Code = System.Net.HttpStatusCode.Unauthorized,
                    Success = false,
                    ExecTime = exec_time,
                    Errors = errors.ToArray(),
                    Data = new
                    {

                    }
                };
            }

            

            var user = new LibraryUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,

            };
            
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var firstError = result.Errors.FirstOrDefault()?.Description;
                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;
                errors = result.Errors.Select(a => a.Description).ToList();

                _logger.LogError($"Register not successfully: {errors.FirstOrDefault()}", DateTime.Now);
                response = new ResponseMessageFilter
                {
                    Code = System.Net.HttpStatusCode.NotAcceptable,
                    Success = false,
                    Errors = errors.ToArray(),
                    ExecTime = exec_time,
                    Data = new
                    {
                       
                    }
                };
                return Ok(response);
            }
            exp.Stop();

            exec_time = exp.ElapsedMilliseconds;

            _logger.LogInformation($"Register successfully: {model.Email}", DateTime.Now);
            response = new ResponseMessageFilter
            {
                Code = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = "Kayıt işlemi başarılı!",
                ExecTime = exec_time,
                Data = new
                {
                    
                }
            };

            return Ok(response);
        }



        /// <summary>
        /// Kullanıcı kitap rezervasyok geçmişi
        /// </summary>
        /// <returns></returns>
        [Route("GetUserRezervationHistory")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserRezervationHistory()
        {
            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;
            List<string> errors = new List<string>();
            _logger.LogInformation($"GetUserRezervationHistory:", DateTime.Now);

            ResponseMessageFilter response;

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var rezervations = _uow.BookRezervation.GetBookRezervations(a => a.UserId == user.Id).ToList();

            exp.Stop();

            exec_time = exp.ElapsedMilliseconds;

            _logger.LogInformation($"GetUserRezervationHistory: {rezervations.Count}", DateTime.Now);
            response = new ResponseMessageFilter
            {
                Code = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = "Kullanıcı kitap rezervleri getirildi.",
                ExecTime = exec_time,
                Data = new
                {
                    ReturnedBooks = rezervations.Where(a => a.ReturnedDate != null).Select(a=> new {Id = a.Id, BookId = a.BookId, BookTitle = a.Book.Title }).ToList(),
                    NotReturnedBooks = rezervations.Where(a => a.ReturnedDate == null).Select(a => new { Id = a.Id, BookId = a.BookId, BookTitle = a.Book.Title }).ToList()
                }
            };

            return Ok(response);
        }




        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("Logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok(new { Logout = true, Message = "" });
            }
            catch (System.Exception ex)
            {
                return Ok(new { Logout = false, Message = ex.Message });
            }

        }

    }
}
