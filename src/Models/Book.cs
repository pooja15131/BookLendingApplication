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
    [StringLength(200, ErrorMessage = "Author name cannot exceed 200 characters.")]
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// ISBN number of the book
    /// </summary>
    [DynamoDBProperty("ISBN")]
    [RegularExpression(@"^(97[89])?[0-9]{9}[0-9X]$", ErrorMessage = "Invalid ISBN format. Use 10 or 13 digit ISBN without hyphens.")]
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