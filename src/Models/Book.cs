using Amazon.DynamoDBv2.DataModel;

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
    public Guid Id { get; set; }

    /// <summary>
    /// Title or name of the book
    /// </summary>
    [DynamoDBProperty("Name")]
    public string? Name { get; set; }

    /// <summary>
    /// Indicates whether book is available for lending
    /// </summary>
    [DynamoDBProperty("IsAvailable")]
    public bool IsAvailable { get; set; }
}