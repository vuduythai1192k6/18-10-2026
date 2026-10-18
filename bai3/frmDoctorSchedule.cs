using System.ComponentModel;
namespace HospitalMDI;
public class frmDoctorSchedule : Form, IReloadable
{
    private readonly BindingList<DoctorShift> shifts = new();
    private readonly BindingSource source = new();
    public frmDoctorSchedule()
    {
        Text = "Danh mục Bác sĩ / Lịch trực";
        Size = new Size(850, 500);
        var grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, DataSource = source };
        Controls.Add(grid);
        ReloadData();
    }
    public void ReloadData()
    {
        shifts.Clear();
        shifts.Add(new DoctorShift { MaBS = "BS001", BacSi = "Nguyễn Minh An", Khoa = "Cấp cứu", CaTruc = "07:00 - 15:00", NgayTruc = DateTime.Today });
        shifts.Add(new DoctorShift { MaBS = "BS002", BacSi = "Phạm Thu Trang", Khoa = "Nội tổng hợp", CaTruc = "15:00 - 23:00", NgayTruc = DateTime.Today });
        shifts.Add(new DoctorShift { MaBS = "BS003", BacSi = "Đỗ Quang Huy", Khoa = "Ngoại", CaTruc = "23:00 - 07:00", NgayTruc = DateTime.Today });
        source.DataSource = shifts;
    }
    private sealed class DoctorShift
    {
        public string MaBS { get; set; } = "";
        public string BacSi { get; set; } = "";
        public string Khoa { get; set; } = "";
        public string CaTruc { get; set; } = "";
        public DateTime NgayTruc { get; set; }
    }
}
