using BookLendingApplication.Models;
using Microsoft.Extensions.Caching.Memory;

namespace BookLendingApplication.Repositories
{
    /// <summary>
    /// In memory book repository to perform CRUD operations on Book model
    /// </summary>
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly List<Book> _books = new();
        private const string CacheKey = "InMemoryBookRepository_Books";
        private IMemoryCache _cache;

        public InMemoryBookRepository(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _books = _cache.Get<List<Book>>(CacheKey) ?? new List<Book>();
            _cache.Set(CacheKey, _books, TimeSpan.FromMinutes(10));
        }

        /// <summary>
        /// Get all books
        /// </summary>
        /// <returns>Returns list of books</returns>
        public Task<IEnumerable<Book>> GetAllAsync() =>
            Task.FromResult(_books.AsEnumerable());

        /// <summary>
        /// Get book by Id
        /// </summary>
        /// <param name="id">book id</param>
        /// <returns>Returns book</returns>
        public Task<Book?> GetByIdAsync(Guid id) =>
            Task.FromResult(_books.FirstOrDefault(b => b.Id == id));

        /// <summary>
        /// Save book
        /// </summary>
        /// <param name="book">book</param>
        /// <returns>Returns book added/updated</returns>
        public Task<Book?> SaveAsync(Book book)
        {
            var existing = _books.FirstOrDefault(x => x.Id == book.Id);
            if (existing is not null)
            {
                existing.IsAvailable = book.IsAvailable;
            }
            else
            {
                _books.Add(book);
                existing = _books.FirstOrDefault(x => x.Id == book.Id);
            }
            return Task.FromResult(existing);
        }
    }
}
