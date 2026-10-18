using System.ComponentModel;
namespace HospitalMDI;
public class frmPatientProfiles : Form, IReloadable
{
    private readonly BindingList<Patient> patients = new();
    private readonly BindingSource source = new();
    private readonly DataGridView grid = new();
    private readonly TextBox txtId = new(), txtName = new(), txtAge = new(), txtDiagnosis = new();

    public frmPatientProfiles()
    {
        Text = "Hồ sơ Bệnh nhân";
        Size = new Size(950, 600);
        MinimumSize = new Size(700, 420);
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), ColumnCount = 1, RowCount = 2 };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Controls.Add(root);

        var group = new GroupBox { Text = "Thông tin bệnh nhân", Dock = DockStyle.Top, AutoSize = true };
        var fields = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 4, RowCount = 2 };
        for (int i = 0; i < 4; i++) fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        AddField(fields, "Mã bệnh nhân", txtId, 0);
        AddField(fields, "Họ tên", txtName, 1);
        AddField(fields, "Tuổi", txtAge, 2);
        AddField(fields, "Chẩn đoán", txtDiagnosis, 3);
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, Padding = new Padding(4) };
        buttons.Controls.Add(MakeButton("Thêm", (_, _) => AddPatient()));
        buttons.Controls.Add(MakeButton("Cập nhật", (_, _) => UpdatePatient()));
        buttons.Controls.Add(MakeButton("Xóa", (_, _) => DeletePatient()));
        buttons.Controls.Add(MakeButton("Làm mới", (_, _) => ClearFields()));
        fields.Controls.Add(buttons, 0, 1);
        fields.SetColumnSpan(buttons, 4);
        group.Controls.Add(fields);
        root.Controls.Add(group, 0, 0);

        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.DataSource = source;
        grid.SelectionChanged += (_, _) =>
        {
            if (grid.CurrentRow?.DataBoundItem is Patient p)
            {
                txtId.Text = p.PatientId; txtName.Text = p.FullName;
                txtAge.Text = p.Age.ToString(); txtDiagnosis.Text = p.Diagnosis;
            }
        };
        root.Controls.Add(grid, 0, 1);
        ReloadData();
    }

    private static void AddField(TableLayoutPanel panel, string label, Control control, int col)
    {
        control.Dock = DockStyle.Fill;
        var box = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 1, RowCount = 2, Padding = new Padding(4) };
        box.Controls.Add(new Label { Text = label, AutoSize = true }, 0, 0);
        box.Controls.Add(control, 0, 1);
        panel.Controls.Add(box, col, 0);
    }

    private static Button MakeButton(string text, EventHandler handler)
    {
        var b = new Button { Text = text, AutoSize = true, Padding = new Padding(8, 4, 8, 4) };
        b.Click += handler;
        return b;
    }

    private bool ValidateFields()
    {
        if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtName.Text))
        { MessageBox.Show("Mã bệnh nhân và họ tên không được để trống."); return false; }
        if (!int.TryParse(txtAge.Text, out int age) || age < 0 || age > 130)
        { MessageBox.Show("Tuổi phải là số nguyên từ 0 đến 130."); return false; }
        return true;
    }

    private Patient ReadFields() => new()
    { PatientId = txtId.Text.Trim(), FullName = txtName.Text.Trim(), Age = int.Parse(txtAge.Text), Diagnosis = txtDiagnosis.Text.Trim() };

    private void AddPatient()
    {
        if (!ValidateFields()) return;
        if (patients.Any(p => p.PatientId.Equals(txtId.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
        { MessageBox.Show("Mã bệnh nhân đã tồn tại."); return; }
        patients.Add(ReadFields()); source.ResetBindings(false); ClearFields();
    }

    private void UpdatePatient()
    {
        if (grid.CurrentRow?.DataBoundItem is not Patient selected) { MessageBox.Show("Chọn bệnh nhân cần cập nhật."); return; }
        if (!ValidateFields()) return;
        string id = txtId.Text.Trim();
        if (patients.Any(p => !ReferenceEquals(p, selected) && p.PatientId.Equals(id, StringComparison.OrdinalIgnoreCase)))
        { MessageBox.Show("Mã bệnh nhân đã tồn tại."); return; }
        var p = ReadFields();
        selected.PatientId = p.PatientId; selected.FullName = p.FullName; selected.Age = p.Age; selected.Diagnosis = p.Diagnosis;
        source.ResetBindings(false);
    }

    private void DeletePatient()
    {
        if (grid.CurrentRow?.DataBoundItem is not Patient selected) return;
        if (MessageBox.Show($"Xóa hồ sơ của {selected.FullName}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        patients.Remove(selected); source.ResetBindings(false); ClearFields();
    }

    private void ClearFields()
    { txtId.Clear(); txtName.Clear(); txtAge.Clear(); txtDiagnosis.Clear(); grid.ClearSelection(); }

    public void ReloadData()
    {
        patients.Clear();
        patients.Add(new Patient { PatientId = "BN001", FullName = "Nguyễn Văn An", Age = 45, Diagnosis = "Tăng huyết áp" });
        patients.Add(new Patient { PatientId = "BN002", FullName = "Trần Thị Bình", Age = 32, Diagnosis = "Khám tổng quát" });
        patients.Add(new Patient { PatientId = "BN003", FullName = "Lê Minh Khang", Age = 61, Diagnosis = "Đái tháo đường type 2" });
        source.DataSource = patients;
    }

    private sealed class Patient
    {
        public string PatientId { get; set; } = "";
        public string FullName { get; set; } = "";
        public int Age { get; set; }
        public string Diagnosis { get; set; } = "";
    }
}
