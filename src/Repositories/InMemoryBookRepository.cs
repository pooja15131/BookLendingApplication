using BookLendingApplication.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace BookLendingApplication.Repositories
{
    /// <summary>
    /// In memory book repository to perform CRUD operations on Book model
    /// </summary>
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly ConcurrentDictionary<Guid, Book> _books = new();
        private const string CacheKey = "InMemoryBookRepository_Books";
        private readonly IMemoryCache _cache;

        public InMemoryBookRepository(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
          
            _books = _cache.Get<ConcurrentDictionary<Guid, Book>>(CacheKey) ?? new ConcurrentDictionary<Guid, Book>();
            _cache.Set(CacheKey, _books, TimeSpan.FromMinutes(10));
        }

        /// <summary>
        /// Get all books
        /// </summary>
        /// <returns>Returns list of books</returns>
        public Task<IEnumerable<Book>> GetAllAsync() =>
            Task.FromResult(_books.Values.AsEnumerable());

        /// <summary>
        /// Get book by Id
        /// </summary>
        /// <param name="id">book id</param>
        /// <returns>Returns book</returns>
        public Task<Book?> GetByIdAsync(Guid id) =>
            Task.FromResult(_books.TryGetValue(id, out var book) ? book : null);

        /// <summary>
        /// Save book
        /// </summary>
        /// <param name="book">book</param>
        /// <returns>Returns book added/updated</returns>
        public Task<Book?> SaveAsync(Book book)
        {
            _books.AddOrUpdate(book.Id, book, (key, existing) => 
            {
                existing.Name = book.Name;
                existing.Author = book.Author;
                existing.ISBN = book.ISBN;
                existing.Publisher = book.Publisher;
                existing.IsAvailable = book.IsAvailable;
                existing.CheckoutDate = book.CheckoutDate;
                return existing;
            });
            
            return Task.FromResult(_books.TryGetValue(book.Id, out var savedBook) ? savedBook : null);
        }
    }
}
