using Microsoft.AspNetCore.Http;
using Library.ServiceLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library.API.AuthExtensions
{
    public class AuthenticationExtension : IAuthenticationExtension
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IUnitOfWork _uow;

        public AuthenticationExtension(IHttpContextAccessor httpContextAccessor, IUnitOfWork uow)
        {

            _httpContextAccessor = httpContextAccessor;
            _uow = uow;
        }

        public string GetUserId(string userName)
        {
            return _uow.LibraryUser.Where(a => a.UserName == userName).FirstOrDefault().Id;
        }

        public string GetUserName()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

    }
}
