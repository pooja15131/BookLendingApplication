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
    [DynamoDBProperty("Id")]
    [Required(ErrorMessage = "Book Id is required.")]
    public Guid Id { get; set; }

    /// <summary>
    /// Title or name of the book
    /// </summary>
    [DynamoDBProperty("Name")]
    [Required(ErrorMessage = "Book name is required.")]
    public string? Name { get; set; }

    /// <summary>
    /// Indicates whether book is available for lending
    /// </summary>
    [DynamoDBProperty("IsAvailable")]
    public bool IsAvailable { get; set; }

    /// <summary>
    /// Author of the book
    /// </summary>
    [DynamoDBProperty("Author")]
    public string? Author { get; set; }

    /// <summary>
    /// ISBN number of the book
    /// </summary>
    [DynamoDBProperty("ISBN")]
    public string? ISBN { get; set; }

    /// <summary>
    /// Publisher of the book
    /// </summary>
    [DynamoDBProperty("Publisher")]
    public string? Publisher { get; set; }

    /// <summary>
    /// Checkout date of the book
    /// </summary>
    [DynamoDBProperty("CheckoutDate")]
    public DateTime CheckoutDate { get; set; }
}