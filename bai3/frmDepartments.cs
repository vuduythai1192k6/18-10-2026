using System.ComponentModel;
namespace HospitalMDI;
public class frmDepartments : Form, IReloadable
{
    private readonly BindingList<Department> departments = new();
    private readonly BindingSource source = new();
    public frmDepartments()
    {
        Text = "Danh mục Khoa/Phòng";
        Size = new Size(700, 420);
        var grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, DataSource = source };
        Controls.Add(grid);
        ReloadData();
    }
    public void ReloadData()
    {
        departments.Clear();
        departments.Add(new Department { MaKhoa = "K001", TenKhoa = "Khoa Cấp cứu", ViTri = "Tầng 1" });
        departments.Add(new Department { MaKhoa = "K002", TenKhoa = "Khoa Nội tổng hợp", ViTri = "Tầng 2" });
        departments.Add(new Department { MaKhoa = "K003", TenKhoa = "Khoa Ngoại", ViTri = "Tầng 3" });
        departments.Add(new Department { MaKhoa = "P001", TenKhoa = "Phòng Xét nghiệm", ViTri = "Tầng 1" });
        source.DataSource = departments;
    }
    private sealed class Department
    {
        public string MaKhoa { get; set; } = "";
        public string TenKhoa { get; set; } = "";
        public string ViTri { get; set; } = "";
    }
}
