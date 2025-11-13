using BookLendingApplication.Models;
using BookLendingApplication.Repositories;
using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;

namespace BookLendingApplication.Tests.Repositories
{
    /// <summary>
    /// Unit tests for InMemoryBookRepository
    /// </summary>
    [TestFixture]
    public class InMemoryBookRepositoryTests
    {
        private IMemoryCache _fakeCache;
        private InMemoryBookRepository _repository;

        [TearDown]
        public void TearDown()
        {
            if (_fakeCache is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        [SetUp]
        public void SetUp()
        {
            _fakeCache = A.Fake<IMemoryCache>();
            
            // Mock cache TryGetValue to return false (no cached data)
            object? cachedValue;
            A.CallTo(() => _fakeCache.TryGetValue(A<object>._, out cachedValue))
                .Returns(false);
            
            _repository = new InMemoryBookRepository(_fakeCache);
        }

        /// <summary>
        /// Test to verify SaveAsync adds a new book when it does not exist
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SaveAsync_AddsBook_WhenNotExists()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "New Book", IsAvailable = true };

            var result = await _repository.SaveAsync(book);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(book.Name));
            Assert.That(result.Id, Is.EqualTo(book.Id));
            Assert.That(result.IsAvailable, Is.EqualTo(book.IsAvailable));
        }

        /// <summary>
        /// Test to verify GetAllAsync returns empty list when no books exist
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAllAsync_ReturnsEmptyList_WhenNoBooks()
        {
            var result = await _repository.GetAllAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        /// <summary>
        /// Test to verify GetByIdAsync returns the correct book when it exists
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetByIdAsync_ReturnsBook_WhenExists()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "New Book", IsAvailable = true };
            var newBook = await _repository.SaveAsync(book);

            var result = await _repository.GetByIdAsync(book.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(newBook?.Id));
            Assert.That(result.Name, Is.EqualTo(newBook?.Name));
            Assert.That(result.IsAvailable, Is.EqualTo(newBook?.IsAvailable));
        }

        /// <summary>
        /// Test to verify GetByIdAsync returns null when the book does not exist
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            var result = await _repository.GetByIdAsync(Guid.NewGuid());
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Test to verify SaveAsync updates an existing book
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SaveAsync_UpdatesBook_WhenExists()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = true };
            var newBook = await _repository.SaveAsync(book); // add new book

            newBook!.IsAvailable = false;
            newBook.Name = "Updated Test";
            var updatedBook = await _repository.SaveAsync(newBook); // update book

            Assert.That(updatedBook, Is.Not.Null);
            Assert.That(updatedBook.Name, Is.EqualTo("Updated Test"));
            Assert.That(updatedBook.IsAvailable, Is.EqualTo(false));
        }

        /// <summary>
        /// Test to verify GetAllAsync returns all saved books
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAllAsync_ReturnsAllBooks_WhenMultipleExist()
        {
            var book1 = new Book { Id = Guid.NewGuid(), Name = "Book 1", IsAvailable = true };
            var book2 = new Book { Id = Guid.NewGuid(), Name = "Book 2", IsAvailable = false };
            
            await _repository.SaveAsync(book1);
            await _repository.SaveAsync(book2);

            var result = await _repository.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(b => b.Name == "Book 1"), Is.True);
            Assert.That(result.Any(b => b.Name == "Book 2"), Is.True);
        }
    }
}
