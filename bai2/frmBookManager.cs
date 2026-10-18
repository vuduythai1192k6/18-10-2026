using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace BookManagerApp;

public partial class frmBookManager : Form
{
    private readonly BindingList<BookModel> _books = new();
    private readonly BindingSource _source = new();
    private BookModel? _selectedBook;
    private string _currentCoverPath = "";
    private bool _loadingSelection;

    public frmBookManager()
    {
        InitializeComponent();
        _source.DataSource = _books;
        dgvBooks.DataSource = _source;
        _books.ListChanged += (_, _) => UpdateStatus();
        // Sample data helps demonstrate the interface on first launch.
        _books.Add(new BookModel { BookCode = "B001", Title = "Dế Mèn phiêu lưu ký", Author = "Tô Hoài", Category = "Văn học", Quantity = 12, UnitPrice = 65000 });
        _books.Add(new BookModel { BookCode = "B002", Title = "Lập trình C# cơ bản", Author = "Nguyễn Văn A", Category = "Công nghệ", Quantity = 7, UnitPrice = 120000 });
        UpdateStatus();
    }

    private void AddBook()
    {
        if (!ValidateInput()) return;
        string code = txtBookCode.Text.Trim();
        if (_books.Any(b => string.Equals(b.BookCode, code, StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show("Mã sách đã tồn tại. Vui lòng nhập mã khác.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtBookCode.Focus(); return;
        }
        var book = ReadInputs();
        _books.Add(book);
        ApplySearch();
        SelectBook(book);
        UpdateStatus();
        MessageBox.Show("Đã thêm đầu sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void UpdateBook()
    {
        if (_selectedBook is null)
        {
            MessageBox.Show("Vui lòng chọn đầu sách cần cập nhật trong danh sách.", "Chưa chọn sách", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
        }
        if (!ValidateInput()) return;
        string code = txtBookCode.Text.Trim();
        if (_books.Any(b => !ReferenceEquals(b, _selectedBook) && string.Equals(b.BookCode, code, StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show("Mã sách đã được sử dụng bởi đầu sách khác.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtBookCode.Focus(); return;
        }
        var updated = ReadInputs();
        _selectedBook.BookCode = updated.BookCode;
        _selectedBook.Title = updated.Title;
        _selectedBook.Author = updated.Author;
        _selectedBook.Category = updated.Category;
        _selectedBook.Quantity = updated.Quantity;
        _selectedBook.UnitPrice = updated.UnitPrice;
        _selectedBook.CoverPath = updated.CoverPath;
        _source.ResetBindings(false);
        UpdateStatus();
        MessageBox.Show("Đã cập nhật đầu sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void DeleteBook()
    {
        if (_selectedBook is null) { MessageBox.Show("Vui lòng chọn đầu sách cần xóa.", "Chưa chọn sách", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        var answer = MessageBox.Show($"Bạn có chắc muốn xóa sách '{_selectedBook.Title}' ({_selectedBook.BookCode})?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (answer != DialogResult.Yes) return;
        int index = _books.IndexOf(_selectedBook);
        _books.Remove(_selectedBook);
        _selectedBook = null;
        ApplySearch();
        ClearInputs();
        if (dgvBooks.Rows.Count > 0) dgvBooks.CurrentCell = dgvBooks.Rows[Math.Min(index, dgvBooks.Rows.Count - 1)].Cells[0];
        UpdateStatus();
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(txtBookCode.Text)) return Invalid(txtBookCode, "Vui lòng nhập mã sách.");
        if (string.IsNullOrWhiteSpace(txtTitle.Text)) return Invalid(txtTitle, "Vui lòng nhập tên sách.");
        if (string.IsNullOrWhiteSpace(txtAuthor.Text)) return Invalid(txtAuthor, "Vui lòng nhập tên tác giả.");
        if (string.IsNullOrWhiteSpace(txtCategory.Text)) return Invalid(txtCategory, "Vui lòng nhập thể loại sách.");
        if (nudQuantity.Value < 0) return Invalid(nudQuantity, "Số lượng không được âm.");
        if (nudUnitPrice.Value < 0) return Invalid(nudUnitPrice, "Đơn giá không được âm.");
        return true;
    }

    private bool Invalid(Control control, string message)
    {
        MessageBox.Show(message, "Thiếu hoặc sai dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        control.Focus(); return false;
    }

    private BookModel ReadInputs() => new()
    {
        BookCode = txtBookCode.Text.Trim(), Title = txtTitle.Text.Trim(), Author = txtAuthor.Text.Trim(), Category = txtCategory.Text.Trim(),
        Quantity = Decimal.ToInt32(nudQuantity.Value), UnitPrice = nudUnitPrice.Value, CoverPath = _currentCoverPath
    };

    private void LoadCover()
    {
        if (openFileDialog1.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            // Load into memory so the source image file is not locked by PictureBox.
            using var stream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var temp = Image.FromStream(stream);
            var copy = new Bitmap(temp);
            var old = picBookCover.Image;
            picBookCover.Image = copy;
            old?.Dispose();
            _currentCoverPath = openFileDialog1.FileName;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể mở ảnh bìa. Chi tiết: " + ex.Message, "Lỗi ảnh", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ImportCsv()
    {
        using var dialog = new OpenFileDialog { Filter = "CSV (*.csv)|*.csv|Tất cả tệp (*.*)|*.*", Title = "Mở danh sách sách từ CSV", CheckFileExists = true };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            var lines = File.ReadAllLines(dialog.FileName, Encoding.UTF8);
            var imported = new List<BookModel>();
            var seenCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                if (i == 0 && lines[i].TrimStart('\uFEFF').StartsWith("BookCode", StringComparison.OrdinalIgnoreCase)) continue;
                var fields = ParseCsvLine(lines[i]);
                if (fields.Count < 7) throw new FormatException($"Dòng {i + 1} không đủ 7 cột.");
                if (!int.TryParse(fields[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out int quantity) || quantity < 0)
                    throw new FormatException($"Số lượng không hợp lệ ở dòng {i + 1}.");
                if (!decimal.TryParse(fields[5], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price) || price < 0)
                    throw new FormatException($"Đơn giá không hợp lệ ở dòng {i + 1}.");
                var book = new BookModel { BookCode = fields[0], Title = fields[1], Author = fields[2], Category = fields[3], Quantity = quantity, UnitPrice = price, CoverPath = fields[6] };
                if (string.IsNullOrWhiteSpace(book.BookCode) || string.IsNullOrWhiteSpace(book.Title)) throw new FormatException($"Thiếu mã sách hoặc tên sách ở dòng {i + 1}.");
                if (!seenCodes.Add(book.BookCode)) throw new FormatException($"Mã sách bị trùng trong file: {book.BookCode}.");
                imported.Add(book);
            }
            _books.RaiseListChangedEvents = false;
            try { _books.Clear(); foreach (var book in imported) _books.Add(book); }
            finally { _books.RaiseListChangedEvents = true; }
            _books.ResetBindings(); ApplySearch(); ClearInputs(); UpdateStatus();
            MessageBox.Show($"Đã nhập {imported.Count} đầu sách từ file CSV.", "Nhập dữ liệu thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể nhập file CSV. Hãy kiểm tra định dạng file.\n\n" + ex.Message, "Lỗi nhập dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ExportCsv()
    {
        if (saveFileDialog1.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            using var writer = new StreamWriter(saveFileDialog1.FileName, false, new UTF8Encoding(true));
            writer.WriteLine("BookCode,Title,Author,Category,Quantity,UnitPrice,CoverPath");
            foreach (BookModel book in _source)
            {
                writer.WriteLine(string.Join(",", new[] { book.BookCode, book.Title, book.Author, book.Category,
                    book.Quantity.ToString(CultureInfo.InvariantCulture), book.UnitPrice.ToString(CultureInfo.InvariantCulture), book.CoverPath }.Select(EscapeCsv)));
            }
            MessageBox.Show("Đã xuất danh sách sách ra CSV.", "Xuất báo cáo thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) { MessageBox.Show("Không thể lưu file CSV.\n" + ex.Message, "Lỗi xuất dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    private static string EscapeCsv(string value)
    {
        value ??= "";
        return value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r')
            ? "\"" + value.Replace("\"", "\"\"") + "\"" : value;
    }

    private static List<string> ParseCsvLine(string line)
    {
        var result = new List<string>(); var field = new StringBuilder(); bool quoted = false;
        for (int i = 0; i < line.Length; i++)
        {
            char ch = line[i];
            if (quoted)
            {
                if (ch == '"' && i + 1 < line.Length && line[i + 1] == '"') { field.Append('"'); i++; }
                else if (ch == '"') quoted = false;
                else field.Append(ch);
            }
            else if (ch == ',' ) { result.Add(field.ToString()); field.Clear(); }
            else if (ch == '"' && field.Length == 0) quoted = true;
            else field.Append(ch);
        }
        if (quoted) throw new FormatException("Có dấu ngoặc kép chưa đóng trong file CSV.");
        result.Add(field.ToString()); return result;
    }

    private void ApplySearch()
    {
        string keyword = txtSearch.Text.Trim();
        if (string.IsNullOrEmpty(keyword)) _source.DataSource = _books;
        else
        {
            var filtered = _books.Where(b => b.BookCode.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) || b.Title.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) || b.Author.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)).ToList();
            _source.DataSource = new BindingList<BookModel>(filtered);
        }
        dgvBooks.DataSource = _source;
        UpdateStatus();
    }

    private void LoadSelectedBook()
    {
        if (_loadingSelection || dgvBooks.CurrentRow?.DataBoundItem is not BookModel book) return;
        _loadingSelection = true;
        try
        {
            _selectedBook = book; txtBookCode.Text = book.BookCode; txtTitle.Text = book.Title; txtAuthor.Text = book.Author; txtCategory.Text = book.Category;
            nudQuantity.Value = Math.Clamp(book.Quantity, (int)nudQuantity.Minimum, (int)nudQuantity.Maximum);
            nudUnitPrice.Value = Math.Clamp(book.UnitPrice, nudUnitPrice.Minimum, nudUnitPrice.Maximum);
            _currentCoverPath = book.CoverPath;
            Image? image = null;
            if (!string.IsNullOrWhiteSpace(book.CoverPath) && File.Exists(book.CoverPath))
            {
                try { using var stream = new FileStream(book.CoverPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite); using var temp = Image.FromStream(stream); image = new Bitmap(temp); }
                catch { image = null; }
            }
            var old = picBookCover.Image; picBookCover.Image = image; old?.Dispose();
        }
        finally { _loadingSelection = false; }
    }

    private void SelectBook(BookModel book)
    {
        for (int i = 0; i < dgvBooks.Rows.Count; i++)
        {
            if (ReferenceEquals(dgvBooks.Rows[i].DataBoundItem, book)) { dgvBooks.CurrentCell = dgvBooks.Rows[i].Cells[0]; dgvBooks.Rows[i].Selected = true; break; }
        }
    }

    private void ClearInputs()
    {
        _selectedBook = null; txtBookCode.Clear(); txtTitle.Clear(); txtAuthor.Clear(); txtCategory.Clear(); nudQuantity.Value = 0; nudUnitPrice.Value = 0; _currentCoverPath = "";
        var old = picBookCover.Image; picBookCover.Image = null; old?.Dispose(); dgvBooks.ClearSelection(); txtBookCode.Focus();
    }

    private void UpdateStatus()
    {
        int count = dgvBooks.DataSource is not null && _source.DataSource is BindingList<BookModel> visible ? visible.Count : _books.Count;
        IEnumerable<BookModel> displayed = _source.List.Cast<BookModel>();
        decimal value = displayed.Sum(b => b.StockValue);
        lblBookCount.Text = $"Tổng số đầu sách: {count}";
        lblStockValue.Text = $"Tổng giá trị kho: {value:N0} VNĐ";
        if (lblClock is not null) lblClock.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
    }
}
