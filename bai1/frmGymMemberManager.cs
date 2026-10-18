using System.ComponentModel;

namespace GymMemberManager
{
    public class frmGymMemberManager : Form
    {
        // Danh sach hoi vien
        private BindingList<MemberModel> danhSach = new BindingList<MemberModel>();
        private BindingSource bindingSource = new BindingSource();
        private ErrorProvider errorProvider = new ErrorProvider();

        private TextBox txtMaHV = new TextBox();
        private TextBox txtHoTen = new TextBox();
        private TextBox txtSDT = new TextBox();
        private ComboBox cboGoiTap = new ComboBox();
        private DateTimePicker dtpNgayDK = new DateTimePicker();
        private TextBox txtChiPhi = new TextBox();
        private TextBox txtTimKiem = new TextBox();
        private DataGridView dgvHoiVien = new DataGridView();

        private Button btnThem = new Button();
        private Button btnSua = new Button();
        private Button btnXoa = new Button();
        private Button btnLamMoi = new Button();
        private Button btnTimKiem = new Button();

        public frmGymMemberManager()
        {
            Text = "Quan ly hoi vien Gym";
            Size = new Size(1000, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;
            ForeColor = Color.Black;

            TaoGiaoDien();
            TaoCotBang();

            bindingSource.DataSource = danhSach;
            dgvHoiVien.DataSource = bindingSource;

            errorProvider.ContainerControl = this;

            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            btnTimKiem.Click += btnTimKiem_Click;
            dgvHoiVien.SelectionChanged += dgvHoiVien_SelectionChanged;

            LamMoi();
        }

        private void TaoGiaoDien()
        {
            Label lblMaHV = new Label();
            lblMaHV.Text = "Ma hoi vien:";
            lblMaHV.SetBounds(20, 20, 120, 25);
            Controls.Add(lblMaHV);

            txtMaHV.SetBounds(140, 20, 200, 25);
            txtMaHV.ReadOnly = true;
            Controls.Add(txtMaHV);

            Label lblHoTen = new Label();
            lblHoTen.Text = "Ho va ten:";
            lblHoTen.SetBounds(380, 20, 120, 25);
            Controls.Add(lblHoTen);

            txtHoTen.SetBounds(500, 20, 200, 25);
            Controls.Add(txtHoTen);

            Label lblSDT = new Label();
            lblSDT.Text = "So dien thoai:";
            lblSDT.SetBounds(20, 60, 120, 25);
            Controls.Add(lblSDT);

            txtSDT.SetBounds(140, 60, 200, 25);
            Controls.Add(txtSDT);

            Label lblGoiTap = new Label();
            lblGoiTap.Text = "Goi tap:";
            lblGoiTap.SetBounds(380, 60, 120, 25);
            Controls.Add(lblGoiTap);

            cboGoiTap.SetBounds(500, 60, 200, 25);
            cboGoiTap.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGoiTap.Items.Add("Theo thang");
            cboGoiTap.Items.Add("6 thang");
            cboGoiTap.Items.Add("1 nam");
            Controls.Add(cboGoiTap);

            Label lblNgayDK = new Label();
            lblNgayDK.Text = "Ngay dang ky:";
            lblNgayDK.SetBounds(20, 100, 120, 25);
            Controls.Add(lblNgayDK);

            dtpNgayDK.SetBounds(140, 100, 200, 25);
            dtpNgayDK.Format = DateTimePickerFormat.Short;
            Controls.Add(dtpNgayDK);

            Label lblChiPhi = new Label();
            lblChiPhi.Text = "Chi phi (VND):";
            lblChiPhi.SetBounds(380, 100, 120, 25);
            Controls.Add(lblChiPhi);

            txtChiPhi.SetBounds(500, 100, 200, 25);
            Controls.Add(txtChiPhi);

            btnThem.Text = "Them";
            btnThem.SetBounds(20, 145, 100, 30);
            Controls.Add(btnThem);

            btnSua.Text = "Sua";
            btnSua.SetBounds(130, 145, 100, 30);
            Controls.Add(btnSua);

            btnXoa.Text = "Xoa";
            btnXoa.SetBounds(240, 145, 100, 30);
            Controls.Add(btnXoa);

            btnLamMoi.Text = "Lam moi";
            btnLamMoi.SetBounds(350, 145, 100, 30);
            Controls.Add(btnLamMoi);

            txtTimKiem.SetBounds(500, 145, 200, 30);
            Controls.Add(txtTimKiem);

            btnTimKiem.Text = "Tim kiem";
            btnTimKiem.SetBounds(710, 145, 100, 30);
            Controls.Add(btnTimKiem);

            dgvHoiVien.SetBounds(20, 200, 940, 390);
            dgvHoiVien.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHoiVien.ReadOnly = true;
            dgvHoiVien.AllowUserToAddRows = false;
            dgvHoiVien.AllowUserToDeleteRows = false;
            dgvHoiVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoiVien.MultiSelect = false;
            dgvHoiVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoiVien.BackgroundColor = Color.White;
            dgvHoiVien.ForeColor = Color.Black;
            dgvHoiVien.DefaultCellStyle.SelectionBackColor = Color.Black;
            dgvHoiVien.DefaultCellStyle.SelectionForeColor = Color.White;
            Controls.Add(dgvHoiVien);
        }

        private void TaoCotBang()
        {
            dgvHoiVien.AutoGenerateColumns = false;

            dgvHoiVien.Columns.Add("MaHV", "Ma hoi vien");
            dgvHoiVien.Columns[0].DataPropertyName = "MemberId";

            dgvHoiVien.Columns.Add("HoTen", "Ho va ten");
            dgvHoiVien.Columns[1].DataPropertyName = "FullName";

            dgvHoiVien.Columns.Add("SDT", "So dien thoai");
            dgvHoiVien.Columns[2].DataPropertyName = "Phone";

            dgvHoiVien.Columns.Add("GoiTap", "Goi tap");
            dgvHoiVien.Columns[3].DataPropertyName = "Package";

            dgvHoiVien.Columns.Add("NgayDK", "Ngay dang ky");
            dgvHoiVien.Columns[4].DataPropertyName = "StartDate";
            dgvHoiVien.Columns[4].DefaultCellStyle.Format = "dd/MM/yyyy";

            dgvHoiVien.Columns.Add("ChiPhi", "Chi phi");
            dgvHoiVien.Columns[5].DataPropertyName = "Fee";
            dgvHoiVien.Columns[5].DefaultCellStyle.Format = "N0";
        }

        private string TaoMaHoiVien()
        {
            int soLonNhat = 0;
            foreach (MemberModel hoiVien in danhSach)
            {
                int so;
                if (hoiVien.MemberId.StartsWith("HV") && int.TryParse(hoiVien.MemberId.Substring(2), out so))
                {
                    if (so > soLonNhat)
                        soLonNhat = so;
                }
            }
            return "HV" + (soLonNhat + 1).ToString("000");
        }

        private bool KiemTraDuLieu()
        {
            errorProvider.Clear();
            bool hopLe = true;
            decimal chiPhi;

            if (txtHoTen.Text.Trim() == "")
            {
                errorProvider.SetError(txtHoTen, "Vui long nhap ho ten");
                hopLe = false;
            }

            if (txtSDT.Text.Trim().Length != 10 || !LaSo(txtSDT.Text.Trim()))
            {
                errorProvider.SetError(txtSDT, "So dien thoai phai co 10 chu so");
                hopLe = false;
            }

            if (cboGoiTap.SelectedIndex == -1)
            {
                errorProvider.SetError(cboGoiTap, "Vui long chon goi tap");
                hopLe = false;
            }

            if (!decimal.TryParse(txtChiPhi.Text, out chiPhi) || chiPhi <= 0)
            {
                errorProvider.SetError(txtChiPhi, "Chi phi phai la so lon hon 0");
                hopLe = false;
            }

            return hopLe;
        }

        private bool LaSo(string chuoi)
        {
            foreach (char kyTu in chuoi)
            {
                if (!char.IsDigit(kyTu))
                    return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieu()) return;

            MemberModel hoiVien = new MemberModel();
            hoiVien.MemberId = TaoMaHoiVien();
            hoiVien.FullName = txtHoTen.Text.Trim();
            hoiVien.Phone = txtSDT.Text.Trim();
            hoiVien.Package = cboGoiTap.Text;
            hoiVien.StartDate = dtpNgayDK.Value.Date;
            hoiVien.Fee = decimal.Parse(txtChiPhi.Text);

            danhSach.Add(hoiVien);
            MessageBox.Show("Them hoi vien thanh cong");
            LamMoi();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (bindingSource.Current == null)
            {
                MessageBox.Show("Hay chon hoi vien can sua");
                return;
            }

            if (!KiemTraDuLieu()) return;

            MemberModel hoiVien = (MemberModel)bindingSource.Current;
            hoiVien.FullName = txtHoTen.Text.Trim();
            hoiVien.Phone = txtSDT.Text.Trim();
            hoiVien.Package = cboGoiTap.Text;
            hoiVien.StartDate = dtpNgayDK.Value.Date;
            hoiVien.Fee = decimal.Parse(txtChiPhi.Text);

            bindingSource.ResetCurrentItem();
            MessageBox.Show("Cap nhat thanh cong");
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (bindingSource.Current == null)
            {
                MessageBox.Show("Hay chon hoi vien can xoa");
                return;
            }

            DialogResult ketQua = MessageBox.Show("Ban co chac muon xoa hoi vien nay khong?", "Xac nhan", MessageBoxButtons.YesNo);
            if (ketQua == DialogResult.Yes)
            {
                danhSach.Remove((MemberModel)bindingSource.Current);
                LamMoi();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();
            int soKetQua = 0;

            foreach (DataGridViewRow dong in dgvHoiVien.Rows)
            {
                string ma = Convert.ToString(dong.Cells[0].Value).ToLower();
                string ten = Convert.ToString(dong.Cells[1].Value).ToLower();
                string sdt = Convert.ToString(dong.Cells[2].Value).ToLower();

                if (ma.Contains(tuKhoa) || ten.Contains(tuKhoa) || sdt.Contains(tuKhoa))
                {
                    dong.Visible = true;
                    soKetQua++;
                }
                else
                {
                    dong.Visible = false;
                }
            }

            if (soKetQua == 0)
                MessageBox.Show("Khong tim thay hoi vien");
        }

        private void dgvHoiVien_SelectionChanged(object sender, EventArgs e)
        {
            if (bindingSource.Current == null) return;

            MemberModel hoiVien = (MemberModel)bindingSource.Current;
            txtMaHV.Text = hoiVien.MemberId;
            txtHoTen.Text = hoiVien.FullName;
            txtSDT.Text = hoiVien.Phone;
            cboGoiTap.Text = hoiVien.Package;
            dtpNgayDK.Value = hoiVien.StartDate;
            txtChiPhi.Text = hoiVien.Fee.ToString();
        }

        private void LamMoi()
        {
            errorProvider.Clear();
            bindingSource.Position = -1;
            dgvHoiVien.ClearSelection();
            txtMaHV.Text = TaoMaHoiVien();
            txtHoTen.Clear();
            txtSDT.Clear();
            cboGoiTap.SelectedIndex = -1;
            dtpNgayDK.Value = DateTime.Today;
            txtChiPhi.Clear();
            txtTimKiem.Clear();

            foreach (DataGridViewRow dong in dgvHoiVien.Rows)
            {
                dong.Visible = true;
            }
        }
    }
}
