using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace BookLendingApplication.Models;

/// <summary>
/// Book model
/// </summary>
[DynamoDBTable("Books")]
public class Book
{
    /// <summary>
    /// Unique identifier for the book
    /// </summary>
    [DynamoDBHashKey("Id")]
    [Required(ErrorMessage = "Book Id is required.")]
    public Guid Id { get; set; }

    /// <summary>
    /// Title or name of the book
    /// </summary>
    [DynamoDBProperty("Name")]
    [Required(ErrorMessage = "Book name is required.")]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "Book name must be between 1 and 500 characters.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether book is available for lending
    /// </summary>
    [DynamoDBProperty("IsAvailable")]
    public bool IsAvailable { get; set; }

    /// <summary>
    /// Author of the book
    /// </summary>
    [DynamoDBProperty("Author")]
    [Required(ErrorMessage = "Author is required.")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Author name must be between 1 and 200 characters.")]
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// ISBN number of the book
    /// </summary>
    [DynamoDBProperty("ISBN")]
    [Required(ErrorMessage = "ISBN is required.")]
    [RegularExpression(@"^(?:ISBN(?:-1[03])?:? )?(?=[0-9X]{10}$|(?=(?:[0-9]+[- ]){3})[- 0-9X]{13}$|97[89][0-9]{10}$|(?=(?:[0-9]+[- ]){4})[- 0-9]{17}$)(?:97[89][- ]?)?[0-9]{1,5}[- ]?[0-9]+[- ]?[0-9]+[- ]?[0-9X]$", ErrorMessage = "Invalid ISBN format.")]
    public string ISBN { get; set; } = string.Empty;

    /// <summary>
    /// Publisher of the book
    /// </summary>
    [DynamoDBProperty("Publisher")]
    [StringLength(200, ErrorMessage = "Publisher name cannot exceed 200 characters.")]
    public string Publisher { get; set; } = string.Empty;

    /// <summary>
    /// Checkout date of the book
    /// </summary>
    [DynamoDBProperty("CheckoutDate")]
    public DateTime CheckoutDate { get; set; }
}