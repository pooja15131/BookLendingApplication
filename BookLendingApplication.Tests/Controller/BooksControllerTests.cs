using BookLendingApplication.Controllers;
using BookLendingApplication.Models;
using BookLendingApplication.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

namespace BookLendingApplication.Tests.Controllers
{
    /// <summary>
    /// Unit tests for BooksController
    /// </summary>
    [TestFixture]
    public class BooksControllerTests
    {
        private IBookService _bookService;
        private BooksController _controller;

        [SetUp]
        public void SetUp()
        {
            _bookService = A.Fake<IBookService>();
            _controller = new BooksController(_bookService);
        }

        /// <summary>
        /// Test to verify GetBooks returns OK with books
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetBooks_ReturnsOkWithBooks()
        {
            var books = new List<Book> { new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = true } };
            A.CallTo(() => _bookService.GetAllBooksAsync()).Returns(Task.FromResult<IEnumerable<Book>>(books));

            var result = await _controller.GetBooks() as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            var response = result.Value as ApiResponse<IEnumerable<Book>>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Books retrieved successfully"));
            Assert.That(response!.Data!.Count(), Is.EqualTo(1));
        }

        /// <summary>
        /// Test to verify GetBooks handles exceptions
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetBooks_WhenException_Returns500()
        {
            A.CallTo(() => _bookService.GetAllBooksAsync()).Throws(new Exception("fail"));

            var result = await _controller.GetBooks() as ObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(500));
            var response = result.Value as ApiResponse<IEnumerable<Book>>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Failed to retrieve books"));
        }

        /// <summary>
        /// Test to verify GetBook returns OK when book exists
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetBook_ReturnsOk_WhenBookExists()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = true };
            A.CallTo(() => _bookService.GetBookByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<Book?>(book));

            var result = (OkObjectResult)await _controller.GetBook(book.Id);

            Assert.That(result, Is.Not.Null);
            var response = result.Value as ApiResponse<Book>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Book retrieved successfully"));
            Assert.That(response!.Data!.Id, Is.EqualTo(book.Id));
        }

        /// <summary>
        /// Test to verify GetBook returns NotFound when book does not exist
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var id = Guid.NewGuid();
            A.CallTo(() => _bookService.GetBookByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<Book?>(null));

            var result = await _controller.GetBook(id) as NotFoundObjectResult;

            Assert.That(result, Is.Not.Null);
            var response = result.Value as ApiResponse<Book>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Book not found"));
            Assert.That(response.Code, Is.EqualTo(404));
        }

        /// <summary>
        /// Test to verify GetBook handles exceptions
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetBook_WhenException_Returns500()
        {
            var id = Guid.NewGuid();
            A.CallTo(() => _bookService.GetBookByIdAsync(id)).Throws(new Exception("fail"));

            var result = await _controller.GetBook(id) as ObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(500));
            var response = result.Value as ApiResponse<Book>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Failed to retrieve book"));
        }

        /// <summary>
        /// Tests to verify AddBook returns BadRequest when book exists
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task AddBook_ReturnsBadRequest_WhenBookExists()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = true };
            A.CallTo(() => _bookService.GetBookByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<Book?>(book));

            var result = await _controller.AddBook(book) as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo("Book Id already available in records. Please provide different Id"));
        }

        /// <summary>
        /// Test to verify AddBook creates new book when book does not exist
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task AddBook_ReturnsCreated_WhenBookDoesNotExist()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = true };
            A.CallTo(() => _bookService.GetBookByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<Book?>(null));
            A.CallTo(() => _bookService.AddBookAsync(book)).Returns(Task.FromResult(book));

            var result = await _controller.AddBook(book) as CreatedAtActionResult;

            Assert.That(result, Is.Not.Null);
            var response = result.Value as ApiResponse<Book>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Book added successfully"));
            Assert.That(response!.Data!.Id, Is.EqualTo(book.Id));
            Assert.That(result.ActionName, Is.EqualTo(nameof(_controller.GetBook)));
        }

        /// <summary>
        /// Test to verify AddBook handles exceptions
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task AddBook_WhenException_ReturnsBadRequest()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = true };
            A.CallTo(() => _bookService.GetBookByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<Book?>(null));
            A.CallTo(() => _bookService.AddBookAsync(book)).Throws(new Exception("fail"));

            var result = await _controller.AddBook(book) as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            var response = result.Value as ApiResponse<Book>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Failed to create book"));
        }

        /// <summary>
        /// Test to verify CheckOutBook returns NotFound when book does not exist
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CheckOutBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var id = Guid.NewGuid();
            A.CallTo(() => _bookService.GetBookByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<Book?>(null));

            var result = await _controller.CheckOutBook(id) as NotFoundObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo("Book does not exist in records. Please verify Id again."));
        }

        /// <summary>
        /// Test to verify CheckOutBook returns BadRequest when book is not available
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CheckOutBook_ReturnsBadRequest_WhenBookNotAvailable()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = false };
            A.CallTo(() => _bookService.GetBookByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<Book?>(book));

            var result = await _controller.CheckOutBook(book.Id) as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo("Book not available for checkout."));
        }

        /// <summary>
        /// Test to verify CheckOutBook returns BadRequest when checkout fails
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CheckOutBook_ReturnsBadRequest_WhenCheckoutFails()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = true };
            A.CallTo(() => _bookService.GetBookByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<Book?>(book));
            A.CallTo(() => _bookService.CheckoutAsync(book)).Returns(Task.FromResult<Book>(null));

            var result = await _controller.CheckOutBook(book.Id) as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            var response = result.Value as ApiResponse<Book>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Checkout failed"));
        }

        /// <summary>
        /// Test to verify CheckOutBook returns OK when checkout succeeds
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CheckOutBook_ReturnsOk_WhenCheckoutSucceeds()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = true };
            A.CallTo(() => _bookService.GetBookByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<Book?>(book));
            A.CallTo(() => _bookService.CheckoutAsync(book)).Returns(Task.FromResult(book));

            var result = await _controller.CheckOutBook(book.Id) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            var response = result.Value as ApiResponse<Book>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Book checked out successfully."));
        }

        /// <summary>
        /// Test to verify CheckOutBook handles exceptions
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task CheckOutBook_WhenException_ReturnsBadRequest()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = true };
            A.CallTo(() => _bookService.GetBookByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<Book?>(book));
            A.CallTo(() => _bookService.CheckoutAsync(book)).Throws(new Exception("fail"));

            var result = await _controller.CheckOutBook(book.Id) as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            var response = result.Value as ApiResponse<Book>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Failed to checkout book."));
        }

        /// <summary>
        /// Test to verify ReturnBook returns NotFound when book does not exist
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ReturnBook_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var id = Guid.NewGuid();
            A.CallTo(() => _bookService.GetBookByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<Book?>(null));

            var result = await _controller.ReturnBook(id) as NotFoundObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo("Book does not exist in records. Please re-try with correct book Id."));
        }

        /// <summary>
        /// Test to verify ReturnBook returns BadRequest when return fails
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ReturnBook_ReturnsBadRequest_WhenReturnFails()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = false };
            A.CallTo(() => _bookService.GetBookByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<Book?>(book));
            A.CallTo(() => _bookService.ReturnAsync(book)).Returns(Task.FromResult<Book>(null));

            var result = await _controller.ReturnBook(book.Id) as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            var response = result.Value as ApiResponse<Book>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Return failed."));
        }

        /// <summary>
        /// Test to verify ReturnBook returns OK when return succeeds
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ReturnBook_ReturnsOk_WhenReturnSucceeds()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = false };
            var returnedBook = new Book { Id = book.Id, Name = "Test", IsAvailable = true };
            A.CallTo(() => _bookService.GetBookByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<Book?>(book));
            A.CallTo(() => _bookService.ReturnAsync(book)).Returns(Task.FromResult(returnedBook));

            var result = await _controller.ReturnBook(book.Id) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            var response = result.Value as ApiResponse<Book>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Book returned successfully."));
            Assert.That(response!.Data!.IsAvailable, Is.True);
        }

        /// <summary>
        /// Test to verify ReturnBook handles exceptions
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ReturnBook_WhenException_ReturnsBadRequest()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = false };
            A.CallTo(() => _bookService.GetBookByIdAsync(A<Guid>.Ignored)).Returns(Task.FromResult<Book?>(book));
            A.CallTo(() => _bookService.ReturnAsync(book)).Throws(new Exception("fail"));

            var result = await _controller.ReturnBook(book.Id) as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            var response = result.Value as ApiResponse<Book>;
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Message, Is.EqualTo("Failed to return book."));
        }
    }
}
