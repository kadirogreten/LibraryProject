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
    [Authorize]
    public class BookController : Controller
    {


        private ILogger<BookController> _logger;
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="uow"></param>
        public BookController(ILogger<BookController> logger, IUnitOfWork uow)
        {
            _logger = logger;
            _uow = uow;
        }


        /// <summary>
        /// Rezerve edilebilir kitaplar listesi
        /// </summary>
        /// <returns></returns>
        ///
        [HttpGet]
        [Route("GetAvailableBooks")]
        public IActionResult GetAvailableBooks()
        {
            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;
            List<string> errors = new List<string>();
            _logger.LogInformation($"GetAvailableBooks", DateTime.Now);

            ResponseMessageFilter response;
            try
            {
                var availableBooks = _uow.Book
                    .Where(a => a.IsAvailable && a.StockCount > 0 && a.Status == 1)
                    .Select(a=> new { Id = a.Id, Title = a.Title, Author = a.Author, StockCount = a.StockCount, Available = a.IsAvailable})
                    .ToList();

                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                _logger.LogInformation($"GetAvailableBooks list: {availableBooks.Count}", DateTime.Now);
                response = new ResponseMessageFilter
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Message = "Uygun kitaplar listelendi.",
                    ExecTime = exec_time,
                    Data = availableBooks
                };
            }
            catch (Exception ex)
            {

                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                _logger.LogError($"GetAvailableBooks list exception: {ex.ToString()}", DateTime.Now);

                errors.Add(ex.Message);

                response = new ResponseMessageFilter
                {
                    Code = System.Net.HttpStatusCode.InternalServerError,
                    Success = false,
                    Errors = errors.ToArray(),
                    ExecTime = exec_time,
                    Data = new
                    {

                    }
                };
            }


            return Ok(response);
        }



        /// <summary>
        /// Rezerve edilebilir kitap detayı
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAvailableBook")]
        public IActionResult GetAvailableBook(int? id)
        {
            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;
            List<string> errors = new List<string>();
            _logger.LogInformation($"GetAvailableBook id :{id}", DateTime.Now);

            ResponseMessageFilter response;


            if (id == null)
            {
                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                _logger.LogError($"GetAvailableBook id boş olamaz: {id}", DateTime.Now);

                errors.Add("id boş olamaz");

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

            try
            {
                var availableBook = _uow.Book
                    .Where(a => a.IsAvailable && a.Id == id && a.StockCount > 0)
                    .Select(a => new { Id = a.Id, Title = a.Title, Author = a.Author, StockCount = a.StockCount, Available = a.IsAvailable }).FirstOrDefault();

                if (availableBook == null)
                {
                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;

                    _logger.LogError($"availableBook null", DateTime.Now);

                    errors.Add("Kitap bulunamadı!");

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

                _logger.LogInformation($"GetAvailableBook detail: {availableBook.Title}", DateTime.Now);
                response = new ResponseMessageFilter
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Message = $"{availableBook.Title} getirildi.",
                    ExecTime = exec_time,
                    Data = availableBook
                };

                return Ok(response);
            }
            catch (Exception ex)
            {

                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                _logger.LogError($"GetAvailableBook exception: {ex.ToString()}", DateTime.Now);

                errors.Add(ex.Message);

                response = new ResponseMessageFilter
                {
                    Code = System.Net.HttpStatusCode.InternalServerError,
                    Success = false,
                    Errors = errors.ToArray(),
                    ExecTime = exec_time,
                    Data = new
                    {

                    }
                };

                return Ok(response);
            }



        }




        /// <summary>
        /// Kitap rezerve et.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("RezerveBook")]
        public IActionResult RezerveBook(int? id)
        {
            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;
            List<string> errors = new List<string>();
            _logger.LogInformation($"RezerveBook id :{id}", DateTime.Now);

            ResponseMessageFilter response;


            if (id == null)
            {
                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                _logger.LogError($"RezerveBook id boş olamaz: {id}", DateTime.Now);

                errors.Add("id boş olamaz");

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

            try
            {
                var availableBook = _uow.Book.FindOne(a => a.IsAvailable && a.Id == id && a.StockCount > 0);

                if (availableBook == null)
                {
                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;

                    _logger.LogError($"RezerveBook null", DateTime.Now);

                    errors.Add("Kitap bulunamadı!");

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

                var user = _uow.LibraryUser.FindOne(a => a.UserName == User.Identity.Name);

                if (user == null)
                {
                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;

                    _logger.LogError($"user null", DateTime.Now);

                    errors.Add("Kullanıcı bulunamadı!");

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

                var userRezervations = _uow.BookRezervation.Where(a => a.UserId == user.Id).ToList();

                if (userRezervations.Count > 3)
                {
                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;

                    _logger.LogError($"userRezervations.count > 3", DateTime.Now);

                    errors.Add("Rezerve ettiğiniz kitap sayısı 3'ten fazla olamaz!");

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


                BookRezervation rezervation = new BookRezervation
                {
                    Status = 1,
                    BookId = availableBook.Id,
                    Created = DateTime.Now,
                    CreatedBy = User.Identity.Name,
                    UserId = user.Id,
                    ReturnedDate = null,

                };

                _uow.BookRezervation.Insert(rezervation);


                availableBook.StockCount--;
                if (availableBook.StockCount <= 0)
                {
                    availableBook.IsAvailable = false;
                }
                

                _uow.Book.Update(availableBook);

                _uow.Complete();


                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                _logger.LogInformation($"User: {user.UserName}, Rezerved a book: {availableBook.Title}", DateTime.Now);
                response = new ResponseMessageFilter
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Message = $"{availableBook.Title} rezerve edildi.",
                    ExecTime = exec_time,
                    Data = new
                    {

                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {

                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                _logger.LogError($"RezerveBook exception: {ex.ToString()}", DateTime.Now);

                errors.Add(ex.Message);

                response = new ResponseMessageFilter
                {
                    Code = System.Net.HttpStatusCode.InternalServerError,
                    Success = false,
                    Errors = errors.ToArray(),
                    ExecTime = exec_time,
                    Data = new
                    {

                    }
                };

                return Ok(response);
            }



        }






        /// <summary>
        /// Kitap iade et.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rezervationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ReturnedRezerveBook")]
        public IActionResult ReturnedRezerveBook(int? id, int? rezervationId)
        {
            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;
            List<string> errors = new List<string>();
            _logger.LogInformation($"ReturnedRezerveBook id :{id}", DateTime.Now);

            ResponseMessageFilter response;


            if (id == null || rezervationId == null)
            {
                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                _logger.LogError($"ReturnedRezerveBook id veya rezervationId  boş olamaz: {id}", DateTime.Now);

                errors.Add("id veya rezervationId boş olamaz");

                response = new ResponseMessageFilter
                {
                    Code = System.Net.HttpStatusCode.NotAcceptable,
                    Success = true,
                    Errors = errors.ToArray(),
                    ExecTime = exec_time,
                    Data = new
                    {

                    }
                };


                return Ok(response);
            }

            try
            {
                var availableBook = _uow.Book.FindOne(a => a.Id == id);

                if (availableBook == null)
                {
                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;

                    _logger.LogError($"ReturnedRezerveBook null", DateTime.Now);

                    errors.Add("Kitap bulunamadı!");

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

                var user = _uow.LibraryUser.FindOne(a => a.UserName == User.Identity.Name);

                if (user == null)
                {
                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;

                    _logger.LogError($"user null", DateTime.Now);

                    errors.Add("Kullanıcı bulunamadı!");

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

                var userRezervation = _uow.BookRezervation.FindOne(a => a.UserId == user.Id && a.BookId == availableBook.Id && a.Id == rezervationId);

                if (userRezervation == null)
                {
                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;

                    _logger.LogError($"userRezervation == null ", DateTime.Now);

                    errors.Add("Kullanıcı rezervasyonu bulunamadı!");

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


                userRezervation.ReturnedDate = DateTime.Now;
                userRezervation.UserId = user.Id;
                userRezervation.ModifiedBy = user.UserName;
                userRezervation.ReturnedDate = DateTime.Now;


                availableBook.IsAvailable = true;
                availableBook.StockCount++;

                _uow.Book.Update(availableBook);

                _uow.BookRezervation.Update(userRezervation);

                _uow.Complete();


                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                _logger.LogInformation($"User: {user.UserName}, Returned a book: {availableBook.Title}", DateTime.Now);
                response = new ResponseMessageFilter
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Message = $"{availableBook.Title} iade edildi.",
                    ExecTime = exec_time,
                    Data = new
                    {

                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {

                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                _logger.LogError($"RezerveBook exception: {ex.ToString()}", DateTime.Now);

                errors.Add(ex.Message);

                response = new ResponseMessageFilter
                {
                    Code = System.Net.HttpStatusCode.InternalServerError,
                    Success = false,
                    Errors = errors.ToArray(),
                    ExecTime = exec_time,
                    Data = new
                    {

                    }
                };

                return Ok(response);
            }



        }


        /// <summary>
        /// Kitap yarat
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateBook")]
        public IActionResult CreateBook(BookViewModel model)
        {
            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;
            List<string> errors = new List<string>();
            _logger.LogInformation($"CreateBook book :{model}", DateTime.Now);

            ResponseMessageFilter response;

            if (!ModelState.IsValid)
            {
                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                _logger.LogError($"CreateBook Modelstate is not valid: {ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage}", DateTime.Now);

                errors = ModelState.Values.FirstOrDefault().Errors.Select(a => a.ErrorMessage).ToList();

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


            Book book = new Book
            {
                Status = 1,
                Author = model.Author,
                StockCount = model.StockCount,
                IsAvailable = true,
                Title = model.Title,
                Created = DateTime.Now,
                CreatedBy = User.Identity.Name
            };


            _uow.Book.Insert(book);
            _uow.Complete();


            exp.Stop();

            exec_time = exp.ElapsedMilliseconds;

            _logger.LogInformation($"Created a book: {book.Title}", DateTime.Now);
            response = new ResponseMessageFilter
            {
                Code = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = $"{book.Title} oluşturuldu.",
                ExecTime = exec_time,
                Data = new
                {

                }
            };

            return Ok(response);
        }



    }
}
