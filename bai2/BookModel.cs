namespace BookManagerApp;

public class BookModel
{
    public string BookCode { get; set; } = "";
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Category { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string CoverPath { get; set; } = "";
    public decimal StockValue => Quantity * UnitPrice;
    public string CoverFileName => string.IsNullOrWhiteSpace(CoverPath) ? "" : Path.GetFileName(CoverPath);
}
