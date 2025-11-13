using BookLendingApplication.Models;
using BookLendingApplication.Repositories;

namespace BookLendingApplication.Services;

/// <summary>
/// Book service to handle book lending operations
/// </summary>
public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    /// <summary>
    /// Checks if the book is already checked out
    /// </summary>
    /// <param name="book">book</param>
    /// <returns>Returns true if book is already checked out else returns false</returns>
    public static bool IsBookAlreadyCheckedOut(Book book)
    {
        return !book.IsAvailable;
    }

    /// <summary>
    /// Retrieves all books
    /// </summary>
    /// <returns>List of books</returns>
    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await _bookRepository.GetAllAsync();
    }

    /// <summary>
    /// Adds new book
    /// </summary>
    /// <param name="book">book</param>
    /// <returns>Returns book added</returns>
    public async Task<Book> AddBookAsync(Book book)
    {
        book.IsAvailable = true; // New books are available by default
        var savedBook = await _bookRepository.SaveAsync(book);
        return savedBook ?? throw new InvalidOperationException("Failed to save book");
    }

    /// <summary>
    /// Retrieves book by Id
    /// </summary>
    /// <param name="id">book</param>
    /// <returns>Returns book</returns>
    public async Task<Book?> GetBookByIdAsync(Guid id)
    {
        return await _bookRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Checks out a book
    /// </summary>
    /// <param name="book">book</param>
    /// <returns>Returns book checked out</returns>
    public async Task<Book> CheckoutAsync(Book book)
    {
        book.IsAvailable = false;
        book.CheckoutDate = DateTime.UtcNow;
        var savedBook = await _bookRepository.SaveAsync(book);
        return savedBook ?? throw new InvalidOperationException("Failed to checkout book");
    }

    /// <summary>
    /// Returns a book
    /// </summary>
    /// <param name="book">book</param>
    /// <returns>Returns book returned</returns>
    public async Task<Book> ReturnAsync(Book book)
    {
        book.IsAvailable = true;
        book.CheckoutDate = default; // Reset checkout date
        var savedBook = await _bookRepository.SaveAsync(book);
        return savedBook ?? throw new InvalidOperationException("Failed to return book");
    }
}