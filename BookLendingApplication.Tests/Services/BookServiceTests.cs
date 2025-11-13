using BookLendingApplication.Models;
using BookLendingApplication.Repositories;
using BookLendingApplication.Services;
using FakeItEasy;

namespace BookLendingApplication.Tests.Services
{
    /// <summary>
    /// Unit test for BookService
    /// </summary>
    [TestFixture]
    public class BookServiceTests
    {
        private IBookRepository _fakeRepo;
        private BookService _service;

        [SetUp]
        public void SetUp()
        {
            _fakeRepo = A.Fake<IBookRepository>();
            _service = new BookService(_fakeRepo);
        }

        /// <summary>
        /// Test to verify whether book is already checked out
        /// </summary>
        [Test]
        public void IsBookAlreadyCheckedOut_ReturnsTrue_WhenNotAvailable()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = false };
            var result = BookService.IsBookAlreadyCheckedOut(book);
            Assert.That(result, Is.True);
        }

        /// <summary>
        /// Test to verify whether book is not checked out and available to check out
        /// </summary>
        [Test]
        public void IsBookAlreadyCheckedOut_ReturnsFalse_WhenAvailable()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = true };
            var result = BookService.IsBookAlreadyCheckedOut(book);
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Test to retrieve all books
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAllBooks_ReturnsBooks()
        {
            var books = new List<Book> { new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = true } };
            A.CallTo(() => _fakeRepo.GetAllAsync()).Returns(books);

            var result = await _service.GetAllBooksAsync();

            Assert.That(result, Is.EquivalentTo(books));
        }

        /// <summary>
        /// Test to add a new book
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task AddBook_SavesAndReturnsBook()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = false };
            var savedBook = new Book { Id = book.Id, Name = book.Name, IsAvailable = true };
            A.CallTo(() => _fakeRepo.SaveAsync(A<Book>._)).Returns(savedBook);

            var result = await _service.AddBookAsync(book);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(book.Id));
            Assert.That(result.Name, Is.EqualTo(book.Name));
            Assert.That(result.IsAvailable, Is.True); // Service sets new books as available
        }

        /// <summary>
        /// Test to verify AddBook throws exception when save fails
        /// </summary>
        /// <returns></returns>
        [Test]
        public void AddBook_ThrowsException_WhenSaveFails()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = true };
            A.CallTo(() => _fakeRepo.SaveAsync(A<Book>._)).Returns((Book?)null);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AddBookAsync(book));
            Assert.That(ex.Message, Is.EqualTo("Failed to save book"));
        }

        /// <summary>
        /// Test to retrieve book by Id
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetBookById_ReturnsBook()
        {
            var id = Guid.NewGuid();
            var book = new Book { Id = id, Name = "Test", IsAvailable = true };
            A.CallTo(() => _fakeRepo.GetByIdAsync(id)).Returns(book);

            var result = await _service.GetBookByIdAsync(id);

            Assert.That(result, Is.EqualTo(book));
        }

        /// <summary>
        /// Test to retrieve book by Id when not found
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetBookById_ReturnsNull_WhenNotFound()
        {
            var id = Guid.NewGuid();
            A.CallTo(() => _fakeRepo.GetByIdAsync(id)).Returns((Book?)null);

            var result = await _service.GetBookByIdAsync(id);

            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Test to checkout a book
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Checkout_SetsIsAvailableFalse_AndSaves()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = true };
            Book? savedBook = null;
            A.CallTo(() => _fakeRepo.SaveAsync(A<Book>._))
                .Invokes((Book b) => savedBook = b)
                .ReturnsLazily((Book b) => b);

            var result = await _service.CheckoutAsync(book);

            Assert.That(result.IsAvailable, Is.False);
            Assert.That(result.CheckoutDate, Is.Not.EqualTo(default(DateTime)));
            Assert.That(savedBook, Is.Not.Null);
            Assert.That(savedBook!.IsAvailable, Is.False);
        }

        /// <summary>
        /// Test to verify Checkout throws exception when save fails
        /// </summary>
        /// <returns></returns>
        [Test]
        public void Checkout_ThrowsException_WhenSaveFails()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = true };
            A.CallTo(() => _fakeRepo.SaveAsync(A<Book>._)).Returns((Book?)null);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.CheckoutAsync(book));
            Assert.That(ex.Message, Is.EqualTo("Failed to checkout book"));
        }

        /// <summary>
        /// Test to return a book
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Return_SetsIsAvailableTrue_AndSaves()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = false, CheckoutDate = DateTime.UtcNow };
            Book? savedBook = null;
            A.CallTo(() => _fakeRepo.SaveAsync(A<Book>._))
                .Invokes((Book b) => savedBook = b)
                .ReturnsLazily((Book b) => b);

            var result = await _service.ReturnAsync(book);

            Assert.That(result.IsAvailable, Is.True);
            Assert.That(result.CheckoutDate, Is.EqualTo(default(DateTime)));
            Assert.That(savedBook, Is.Not.Null);
            Assert.That(savedBook!.IsAvailable, Is.True);
        }

        /// <summary>
        /// Test to verify Return throws exception when save fails
        /// </summary>
        /// <returns></returns>
        [Test]
        public void Return_ThrowsException_WhenSaveFails()
        {
            var book = new Book { Id = Guid.NewGuid(), Name = "Test", IsAvailable = false };
            A.CallTo(() => _fakeRepo.SaveAsync(A<Book>._)).Returns((Book?)null);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.ReturnAsync(book));
            Assert.That(ex.Message, Is.EqualTo("Failed to return book"));
        }
    }
}
