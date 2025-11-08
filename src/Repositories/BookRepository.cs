using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using BookLendingApplication.Models;
using System.Diagnostics.CodeAnalysis;

namespace BookLendingApplication.Repositories;

/// <summary>
/// Book repository to perform CRUD operations on Book table in DynamoDB
/// </summary>
[ExcludeFromCodeCoverage]
public class BookRepository(IAmazonDynamoDB dynamoClient) : IBookRepository
{
    private readonly DynamoDBContext _context = new(dynamoClient);

    /// <summary>
    /// Get all books
    /// </summary>
    /// <returns>Returns list of books</returns>
    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.ScanAsync<Book>(new List<ScanCondition>()).GetRemainingAsync();
    }

    /// <summary>
    /// Get book by Id
    /// </summary>
    /// <param name="id">book id</param>
    /// <returns>Returns book</returns>
    public async Task<Book?> GetByIdAsync(Guid id) =>
        await _context.LoadAsync<Book>(id);

    /// <summary>
    /// Save book
    /// </summary>
    /// <param name="book">book</param>
    /// <returns>Returns book added/updated</returns>
    public async Task<Book?> SaveAsync(Book book)
    {
        await _context.SaveAsync(book);
        return await _context.LoadAsync<Book>(book.Id);
    }
}