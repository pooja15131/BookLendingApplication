using BookLendingApplication.Models;
using BookLendingApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookLendingApplication.Controllers;

/// <summary>
/// Controller for managing book lending operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Get all books
    /// </summary>
    /// <returns>List of all books</returns>
    /// <response code="200">Books retrieved successfully</response>
    /// <response code="500">Internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<Book>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<Book>>), 500)]
    public async Task<IActionResult> GetBooks()
    {
        try
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(ApiResponse<IEnumerable<Book>>.Success(books, "Books retrieved successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<Book>>.Failure(ex.Message, "Failed to retrieve books", 500));
        }
    }

    /// <summary>
    /// Get a book by ID
    /// </summary>
    /// <param name="id">Book ID</param>
    /// <returns>Book details</returns>
    /// <response code="200">Book retrieved successfully</response>
    /// <response code="404">Book not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<Book>), 200)]
    [ProducesResponseType(typeof(ApiResponse<Book>), 404)]
    [ProducesResponseType(typeof(ApiResponse<Book>), 500)]
    public async Task<IActionResult> GetBook(Guid id)
    {
        try
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
                return NotFound(ApiResponse<Book>.Failure("Book not found", "Book not found", 404));
            return Ok(ApiResponse<Book>.Success(book, "Book retrieved successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<Book>.Failure(ex.Message, "Failed to retrieve book", 500));
        }
    }

    /// <summary>
    /// Add a new book
    /// </summary>
    /// <param name="book">Book details</param>
    /// <returns>Added book</returns>
    /// <response code="201">Book added successfully</response>
    /// <response code="400">Bad request</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Book>), 201)]
    [ProducesResponseType(typeof(ApiResponse<Book>), 400)]
    public async Task<IActionResult> AddBook(Book book)
    {
        try
        {
            var bookAvailable = await BookExists(book.Id);
            if (bookAvailable != null)
            {
                return BadRequest("Book Id already available in records. Please provide different Id");
            }

            var newBook = await _bookService.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, 
                ApiResponse<Book>.Success(newBook, "Book added successfully", 201));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<Book>.Failure(ex.Message, "Failed to create book"));
        }
    }

    /// <summary>
    /// Check out a book
    /// </summary>
    /// <param name="id">Book ID</param>
    /// <returns>Checked out book</returns>
    /// <response code="200">Book checked out successfully</response>
    /// <response code="400">Book not available for checkout</response>
    [HttpPost("{id}/checkout")]
    [ProducesResponseType(typeof(ApiResponse<Book>), 200)]
    [ProducesResponseType(typeof(ApiResponse<Book>), 400)]
    public async Task<IActionResult> CheckOutBook(Guid id)
    {
        try
        {
            var book = await BookExists(id);
            if (book == null)
            {
                return NotFound("Book does not exist in records. Please verify Id again.");
            }

            if (!book.IsAvailable) return BadRequest("Book not available for checkout.");

            var updatedBook = await _bookService.CheckoutAsync(book);
            if (updatedBook == null)
                return BadRequest(ApiResponse<Book>.Failure("Book not available for checkout.", "Checkout failed"));
            return Ok(ApiResponse<Book>.Success(book, "Book checked out successfully."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<Book>.Failure(ex.Message, "Failed to checkout book."));
        }
    }

    /// <summary>
    /// Return a book
    /// </summary>
    /// <param name="id">Book ID</param>
    /// <returns>Returned book</returns>
    /// <response code="200">Book returned successfully</response>
    /// <response code="400">Book was not checked out</response>
    [HttpPost("{id}/return")]
    [ProducesResponseType(typeof(ApiResponse<Book>), 200)]
    [ProducesResponseType(typeof(ApiResponse<Book>), 400)]
    public async Task<IActionResult> ReturnBook(Guid id)
    {
        try
        {
            var book = await BookExists(id);
            if (book == null)
            {
                return NotFound("Book does not exist in records. Please re-try with correct book Id.");
            }

            var updatedBook = await _bookService.ReturnAsync(book);
            if (updatedBook == null)
                return BadRequest(ApiResponse<Book>.Failure("Return failed.", "Return failed."));
            return Ok(ApiResponse<Book>.Success(updatedBook, "Book returned successfully."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<Book>.Failure(ex.Message, "Failed to return book."));
        }
    }

    /// <summary>
    /// Check whether book exists in records
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private async Task<Book?> BookExists(Guid id)
    {
        return await _bookService.GetBookByIdAsync(id);
    }
}