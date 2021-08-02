using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.ViewModels
{
    public class AuthorizeErrorViewModel
    {
        public string Error { get; }

        public AuthorizeErrorViewModel(string message)
        {
            Error = message;
        }
    }
}
