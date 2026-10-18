
namespace HospitalMDI;
public class frmHospitalMain : Form
{
    private readonly ToolStripStatusLabel lblDoctor = new("Bác sĩ trực: BS. Nguyễn Minh An");
    private readonly ToolStripStatusLabel lblProgress = new("Sẵn sàng");
    private readonly ToolStripProgressBar progress = new() { Minimum = 0, Maximum = 100, Value = 0, Size = new Size(150, 16) };
    private readonly System.Windows.Forms.Timer timer = new() { Interval = 35 };
    private int progressValue;

    public frmHospitalMain()
    {
        Text = "Hospital MDI - Quản lý bệnh viện";
        IsMdiContainer = true;
        WindowState = FormWindowState.Maximized;
        BuildMenu();
        var status = new StatusStrip();
        status.Items.Add(lblDoctor);
        status.Items.Add(new ToolStripStatusLabel { Spring = true });
        status.Items.Add(lblProgress);
        status.Items.Add(progress);
        Controls.Add(status);

        timer.Tick += (_, _) =>
        {
            progressValue = Math.Min(100, progressValue + 5);
            progress.Value = progressValue;
            if (progressValue == 100) { timer.Stop(); lblProgress.Text = "Nạp dữ liệu hoàn tất"; }
        };
    }

    private void BuildMenu()
    {
        var menu = new MenuStrip();
        var system = new ToolStripMenuItem("Hệ thống");
        system.DropDownItems.Add("Đăng xuất", null, (_, _) => Logout());
        system.DropDownItems.Add("Nạp lại dữ liệu", null, (_, _) => ReloadData());
        system.DropDownItems.Add(new ToolStripSeparator());
        system.DropDownItems.Add("Thoát", null, (_, _) => Close());

        var manage = new ToolStripMenuItem("Quản lý");
        manage.DropDownItems.Add("Hồ sơ Bệnh nhân", null, (_, _) => OpenChild<frmPatientProfiles>());
        manage.DropDownItems.Add("Danh mục Bác sĩ / Lịch trực", null, (_, _) => OpenChild<frmDoctorSchedule>());
        manage.DropDownItems.Add("Danh mục Khoa/Phòng", null, (_, _) => OpenChild<frmDepartments>());

        var windows = new ToolStripMenuItem("Cửa sổ");
        windows.DropDownItems.Add("Sắp xếp xếp tầng (Cascade)", null, (_, _) => Arrange(MdiLayout.Cascade));
        windows.DropDownItems.Add("Ngang (Tile Horizontal)", null, (_, _) => Arrange(MdiLayout.TileHorizontal));
        windows.DropDownItems.Add("Dọc (Tile Vertical)", null, (_, _) => Arrange(MdiLayout.TileVertical));
        windows.DropDownItems.Add(new ToolStripSeparator());
        windows.DropDownItems.Add("Đóng tất cả", null, (_, _) => CloseAllChildren());

        menu.Items.AddRange(new ToolStripItem[] { system, manage, windows });
        MainMenuStrip = menu;
        Controls.Add(menu);
    }

    private void OpenChild<T>() where T : Form, new()
    {
        foreach (Form child in MdiChildren)
        {
            if (child is T)
            {
                if (child.WindowState == FormWindowState.Minimized) child.WindowState = FormWindowState.Normal;
                child.Activate();
                return;
            }
        }
        var form = new T { MdiParent = this, StartPosition = FormStartPosition.CenterScreen };
        form.Show();
    }

    private void Arrange(MdiLayout layout)
    {
        if (MdiChildren.Length > 0) LayoutMdi(layout);
    }

    private void CloseAllChildren()
    {
        foreach (Form child in MdiChildren.ToArray()) child.Close();
    }

    private void ReloadData()
    {
        timer.Stop();
        progressValue = 0;
        progress.Value = 0;
        lblProgress.Text = "Đang nạp dữ liệu...";
        foreach (Form child in MdiChildren)
            if (child is IReloadable reloadable) reloadable.ReloadData();
        timer.Start();
    }

    private void Logout()
    {
        if (MessageBox.Show("Đăng xuất và đóng tất cả cửa sổ con?", "Xác nhận",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        CloseAllChildren();
        lblDoctor.Text = "Bác sĩ trực: Chưa đăng nhập";
        Text = "Hospital MDI - Đã đăng xuất";
        MessageBox.Show("Đã đăng xuất (mô phỏng).");
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        timer.Stop();
        timer.Dispose();
        base.OnFormClosed(e);
    }
}

public interface IReloadable { void ReloadData(); }
