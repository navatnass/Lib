public class Reservation
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string Type { get; set; } // "Book" or "Audiobook"
    public bool QuickPickup { get; set; }
    public int Days { get; set; }
    public decimal TotalCost { get; set; } // Add this property to store the calculated cost
    public Book Book { get; set; } // Navigation property
}