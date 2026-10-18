namespace BookManagerApp;

partial class frmBookManager
{
    private System.ComponentModel.IContainer? components = null;
    private MenuStrip menuStrip1 = null!;
    private ToolStrip toolStrip1 = null!;
    private StatusStrip statusStrip1 = null!;
    private ToolStripStatusLabel lblBookCount = null!;
    private ToolStripStatusLabel lblStockValue = null!;
    private ToolStripStatusLabel lblClock = null!;
    private SplitContainer splitContainer1 = null!;
    private TableLayoutPanel inputLayout = null!;
    private TextBox txtBookCode = null!;
    private TextBox txtTitle = null!;
    private TextBox txtAuthor = null!;
    private TextBox txtCategory = null!;
    private NumericUpDown nudQuantity = null!;
    private NumericUpDown nudUnitPrice = null!;
    private PictureBox picBookCover = null!;
    private Button btnLoadCover = null!;
    private Button btnAdd = null!;
    private Button btnUpdate = null!;
    private Button btnDelete = null!;
    private Button btnClear = null!;
    private TextBox txtSearch = null!;
    private DataGridView dgvBooks = null!;
    private OpenFileDialog openFileDialog1 = null!;
    private SaveFileDialog saveFileDialog1 = null!;
    private System.Windows.Forms.Timer clockTimer = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        menuStrip1 = new MenuStrip(); toolStrip1 = new ToolStrip(); statusStrip1 = new StatusStrip();
        lblBookCount = new ToolStripStatusLabel(); lblStockValue = new ToolStripStatusLabel(); lblClock = new ToolStripStatusLabel();
        splitContainer1 = new SplitContainer(); inputLayout = new TableLayoutPanel();
        txtBookCode = new TextBox(); txtTitle = new TextBox(); txtAuthor = new TextBox(); txtCategory = new TextBox();
        nudQuantity = new NumericUpDown(); nudUnitPrice = new NumericUpDown(); picBookCover = new PictureBox();
        btnLoadCover = new Button(); btnAdd = new Button(); btnUpdate = new Button(); btnDelete = new Button(); btnClear = new Button();
        txtSearch = new TextBox(); dgvBooks = new DataGridView(); openFileDialog1 = new OpenFileDialog(); saveFileDialog1 = new SaveFileDialog(); clockTimer = new System.Windows.Forms.Timer(components);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit(); splitContainer1.Panel1.SuspendLayout(); splitContainer1.Panel2.SuspendLayout(); splitContainer1.SuspendLayout();
        inputLayout.SuspendLayout(); ((System.ComponentModel.ISupportInitialize)nudQuantity).BeginInit(); ((System.ComponentModel.ISupportInitialize)nudUnitPrice).BeginInit(); ((System.ComponentModel.ISupportInitialize)picBookCover).BeginInit(); ((System.ComponentModel.ISupportInitialize)dgvBooks).BeginInit();
        SuspendLayout();
        // Menu
        menuStrip1.Items.AddRange(new ToolStripItem[] { CreateFileMenu(), CreateEditMenu() }); menuStrip1.Dock = DockStyle.Top;
        // ToolStrip
        toolStrip1.Dock = DockStyle.Top; toolStrip1.Items.Add(MakeToolButton("📂 Mở file", (_,_) => ImportCsv())); toolStrip1.Items.Add(MakeToolButton("💾 Lưu file", (_,_) => ExportCsv())); toolStrip1.Items.Add(MakeToolButton("➕ Thêm mới", (_,_) => AddBook()));
        // Status
        lblBookCount.Text = "Tổng số đầu sách: 0"; lblStockValue.Text = "Tổng giá trị kho: 0"; lblClock.Text = "";
        statusStrip1.Items.AddRange(new ToolStripItem[] { lblBookCount, new ToolStripStatusLabel(" | "), lblStockValue, new ToolStripStatusLabel { Spring = true }, lblClock }); statusStrip1.Dock = DockStyle.Bottom;
        // Split layout
        splitContainer1.Dock = DockStyle.Fill; 
        splitContainer1.Orientation = Orientation.Vertical;
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Orientation = Orientation.Vertical;

        // Đặt kích thước ban đầu trước khi cấu hình khoảng cách chia.
        splitContainer1.Size = new Size(1280, 680);

        splitContainer1.Panel1MinSize = 320;
        splitContainer1.Panel2MinSize = 350;

        splitContainer1.SplitterDistance = 360;
        inputLayout.Dock = DockStyle.Fill; inputLayout.Padding = new Padding(10); inputLayout.ColumnCount = 2; inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 112)); inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); inputLayout.RowCount = 10;
        for (int i=0;i<6;i++) inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        inputLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38)); inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); inputLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
        AddField("Mã sách", txtBookCode, 0); AddField("Tên sách", txtTitle, 1); AddField("Tác giả", txtAuthor, 2); AddField("Thể loại", txtCategory, 3);
        nudQuantity.Minimum = 0; nudQuantity.Maximum = 1000000; nudQuantity.Dock = DockStyle.Fill; AddField("Số lượng", nudQuantity, 4);
        nudUnitPrice.Minimum = 0; nudUnitPrice.Maximum = 1000000000; nudUnitPrice.DecimalPlaces = 0; nudUnitPrice.ThousandsSeparator = true; nudUnitPrice.Dock = DockStyle.Fill; AddField("Đơn giá (VNĐ)", nudUnitPrice, 5);
        picBookCover.Dock = DockStyle.Fill; picBookCover.SizeMode = PictureBoxSizeMode.Zoom; picBookCover.BorderStyle = BorderStyle.FixedSingle; picBookCover.BackColor = SystemColors.ControlLight; picBookCover.Margin = new Padding(3);
        inputLayout.Controls.Add(new Label { Text = "Ảnh bìa", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 6); inputLayout.Controls.Add(picBookCover, 1, 6);
        btnLoadCover.Text = "Tải ảnh bìa..."; btnLoadCover.Dock = DockStyle.Fill; btnLoadCover.Click += (_,_) => LoadCover(); inputLayout.Controls.Add(btnLoadCover, 1, 7);
        FlowLayoutPanel actionPanel = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = true };
        btnAdd.Text = "Thêm"; btnUpdate.Text = "Cập nhật"; btnDelete.Text = "Xóa"; btnClear.Text = "Làm mới";
        foreach (var b in new[] { btnAdd, btnUpdate, btnDelete, btnClear }) { b.AutoSize = true; b.Margin = new Padding(2); actionPanel.Controls.Add(b); }
        btnAdd.Click += (_,_) => AddBook(); btnUpdate.Click += (_,_) => UpdateBook(); btnDelete.Click += (_,_) => DeleteBook(); btnClear.Click += (_,_) => ClearInputs();
        inputLayout.Controls.Add(actionPanel, 0, 8); inputLayout.SetColumnSpan(actionPanel, 2);
        inputLayout.Controls.Add(new Label { Text = "Nhập thông tin rồi chọn Thêm/Cập nhật.", Dock = DockStyle.Fill, ForeColor = SystemColors.GrayText, TextAlign = ContentAlignment.MiddleLeft }, 0, 9); inputLayout.SetColumnSpan(inputLayout.GetControlFromPosition(0,9)!, 2);
        splitContainer1.Panel1.Controls.Add(inputLayout);
        // List area
        TableLayoutPanel listLayout = new() { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2, Padding = new Padding(8) }; listLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34)); listLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        txtSearch.PlaceholderText = "Tìm theo mã sách, tên sách hoặc tác giả..."; txtSearch.Dock = DockStyle.Fill; txtSearch.TextChanged += (_,_) => ApplySearch(); listLayout.Controls.Add(txtSearch,0,0);
        dgvBooks.Dock = DockStyle.Fill; dgvBooks.AllowUserToAddRows = false; dgvBooks.AllowUserToDeleteRows = false; dgvBooks.ReadOnly = true; dgvBooks.MultiSelect = false; dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect; dgvBooks.AutoGenerateColumns = false; dgvBooks.RowHeadersVisible = false; dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; dgvBooks.SelectionChanged += (_,_) => LoadSelectedBook();
        AddGridColumn("BookCode", "Mã sách", "Mã sách", 80); AddGridColumn("Title", "Tên sách", "Tên sách", 160); AddGridColumn("Author", "Tác giả", "Tác giả", 100); AddGridColumn("Category", "Thể loại", "Thể loại", 90); AddGridColumn("Quantity", "Số lượng", "Số lượng", 65); AddGridColumn("UnitPrice", "Đơn giá", "Đơn giá", 90, "N0"); AddGridColumn("StockValue", "Giá trị kho", "Giá trị kho", 100, "N0"); AddGridColumn("CoverFileName", "Ảnh bìa", "Ảnh bìa", 100);
        listLayout.Controls.Add(dgvBooks,0,1); splitContainer1.Panel2.Controls.Add(listLayout);
        openFileDialog1.Filter = "Ảnh bìa (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|Tất cả tệp (*.*)|*.*"; openFileDialog1.Title = "Chọn ảnh bìa sách";
        saveFileDialog1.Filter = "CSV UTF-8 (*.csv)|*.csv"; saveFileDialog1.DefaultExt = "csv"; saveFileDialog1.AddExtension = true; saveFileDialog1.Title = "Lưu danh sách sách";
        clockTimer.Interval = 1000; clockTimer.Tick += (_,_) => lblClock.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"); clockTimer.Start();
        Controls.Add(splitContainer1); Controls.Add(statusStrip1); Controls.Add(toolStrip1); Controls.Add(menuStrip1); MainMenuStrip = menuStrip1;
        Text = "Quản lý tài liệu sách & báo cáo nhà sách"; StartPosition = FormStartPosition.CenterScreen; MinimumSize = new Size(1000, 620); ClientSize = new Size(1280, 760);
        Load += (_,_) => UpdateStatus(); FormClosed += (_,_) => { if (picBookCover.Image is not null) { picBookCover.Image.Dispose(); picBookCover.Image = null; } };
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit(); splitContainer1.Panel1.ResumeLayout(false); splitContainer1.Panel2.ResumeLayout(false); splitContainer1.ResumeLayout(false);
        inputLayout.ResumeLayout(false); inputLayout.PerformLayout(); ((System.ComponentModel.ISupportInitialize)nudQuantity).EndInit(); ((System.ComponentModel.ISupportInitialize)nudUnitPrice).EndInit(); ((System.ComponentModel.ISupportInitialize)picBookCover).EndInit(); ((System.ComponentModel.ISupportInitialize)dgvBooks).EndInit(); ResumeLayout(false); PerformLayout();
    }

    private ToolStripMenuItem CreateFileMenu()
    {
        var file = new ToolStripMenuItem("File"); file.DropDownItems.Add("Mở file CSV", null, (_,_) => ImportCsv()); file.DropDownItems.Add("Lưu file CSV", null, (_,_) => ExportCsv()); file.DropDownItems.Add(new ToolStripSeparator()); file.DropDownItems.Add("Thoát", null, (_,_) => Close()); return file;
    }
    private ToolStripMenuItem CreateEditMenu()
    {
        var edit = new ToolStripMenuItem("Edit"); edit.DropDownItems.Add("Thêm", null, (_,_) => AddBook()); edit.DropDownItems.Add("Xóa", null, (_,_) => DeleteBook()); edit.DropDownItems.Add("Tìm kiếm", null, (_,_) => { txtSearch.Focus(); txtSearch.SelectAll(); }); return edit;
    }
    private ToolStripButton MakeToolButton(string text, EventHandler click) { var b = new ToolStripButton(text) { DisplayStyle = ToolStripItemDisplayStyle.Text, ToolTipText = text }; b.Click += click; return b; }
    private void AddField(string label, Control control, int row) { inputLayout.Controls.Add(new Label { Text = label, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row); control.Dock = DockStyle.Fill; control.Margin = new Padding(3,5,3,5); inputLayout.Controls.Add(control,1,row); }
    private void AddGridColumn(string property, string header, string name, int minWidth, string? format = null) { var c = new DataGridViewTextBoxColumn { DataPropertyName = property, HeaderText = header, Name = name, MinimumWidth = minWidth, SortMode = DataGridViewColumnSortMode.Automatic }; if (format is not null) c.DefaultCellStyle.Format = format; dgvBooks.Columns.Add(c); }
}
