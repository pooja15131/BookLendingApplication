using BookLendingApplication.Models;

namespace BookLendingApplication.Repositories;

/// <summary>
/// Book repository to perform CRUD operations on Book table in DynamoDB
/// </summary>
public interface IBookRepository
{
    /// <summary>
    /// Get all books
    /// </summary>
    /// <returns>Returns list of books</returns>
    Task<IEnumerable<Book>> GetAllAsync();

    /// <summary>
    /// Get book by Id
    /// </summary>
    /// <param name="id">book id</param>
    /// <returns>Returns book</returns>
    Task<Book?> GetByIdAsync(Guid id);

    /// <summary>
    /// Save book
    /// </summary>
    /// <param name="book">book</param>
    /// <returns>Returns book added/updated</returns>
    Task<Book?> SaveAsync(Book book);
}