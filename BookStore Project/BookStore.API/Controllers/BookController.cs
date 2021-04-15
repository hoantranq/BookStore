using BookStore.API.Helpers.ApplicationLogger;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookStore.API.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        public BookController(ILoggerManager logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> GetBooks()
        {
            _logger.LogInfo("Here is the info message from GetBooks....");

            return new string[] { "Book_1", "Book_2" };
        }
    }
}
