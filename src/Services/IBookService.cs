using BookLendingApplication.Models;

namespace BookLendingApplication.Services;

/// <summary>
/// Book service to handle book lending operations
/// </summary>
public interface IBookService
{
    /// <summary>
    /// Retrieves all books
    /// </summary>
    /// <returns>List of books</returns>
    Task<IEnumerable<Book>> GetAllBooksAsync();

    /// <summary>
    /// Adds new book
    /// </summary>
    /// <param name="book">book</param>
    /// <returns>Returns book added</returns>
    Task<Book> AddBookAsync(Book book);

    /// <summary>
    /// Retrieves book by Id
    /// </summary>
    /// <param name="id">book</param>
    /// <returns>Returns book</returns>
    Task<Book?> GetBookByIdAsync(Guid id);

    /// <summary>
    /// Checks out a book
    /// </summary>
    /// <param name="book">book</param>
    /// <returns>Returns book checked out</returns>
    Task<Book> CheckoutAsync(Book book);

    /// <summary>
    /// Returns a book
    /// </summary>
    /// <param name="book">book</param>
    /// <returns>Returns book returned</returns>
    Task<Book> ReturnAsync(Book book);
}