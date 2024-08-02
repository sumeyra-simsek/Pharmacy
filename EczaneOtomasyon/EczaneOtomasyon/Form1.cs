using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EczaneOtomasyon
{
    public partial class Form1 : Form
    {
        public int ilacID;
        public int customerId;
        public int tedarikciID;
        DataClasses1DataContext db = new DataClasses1DataContext();
        public Form1()
        {
            InitializeComponent();
        }
        private void btnturkaydet_Click(object sender, EventArgs e)
        {
            ilacturleri ituru = new ilacturleri();
            ituru.turadi = txtType.Text;
            db.ilacturleris.InsertOnSubmit(ituru);
            db.SubmitChanges();
            txtType.Text = "";
            MessageBox.Show("Data Successfully Inserted");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'pharmacyDBDataSet4.ilaclar' table. You can move, or remove it, as needed.
            this.ilaclarTableAdapter1.Fill(this.pharmacyDBDataSet4.ilaclar);
            // TODO: This line of code loads data into the 'pharmacyDBDataSet3.satis' table. You can move, or remove it, as needed.
            this.satisTableAdapter.Fill(this.pharmacyDBDataSet3.satis);
            // TODO: This line of code loads data into the 'pharmacyDBDataSet2.tedarikci' table. You can move, or remove it, as needed.
            this.tedarikciTableAdapter.Fill(this.pharmacyDBDataSet2.tedarikci);
            // TODO: This line of code loads data into the 'pharmacyDBDataSet1.musteri' table. You can move, or remove it, as needed.
            this.musteriTableAdapter.Fill(this.pharmacyDBDataSet1.musteri);
            // TODO: This line of code loads data into the 'pharmacyDBDataSet.ilaclar' table. You can move, or remove it, as needed.
            this.ilaclarTableAdapter.Fill(this.pharmacyDBDataSet.ilaclar);
            dgvilac.DataSource = db.ilaclars.Where(x => x.deleted == false).ToList();

        }

      

        private void cbEnterProductName_Enter(object sender, EventArgs e)
        {
            cbilacturu.DataSource = from ituru in db.ilacturleris select ituru.turadi;
            cbEnterProductName.DataSource = from ilac in db.ilaclars
                                            where ilac.deleted == false
                                            select ilac.ilacAdi;
        }

        private void btnilackaydet_Click(object sender, EventArgs e)
        {

            ilaclar ilac = new ilaclar(); // product nesnesi
            try
            {
                ilac.ilacAdi = txtilacAdi.Text; // textekileri kolonlara ekleme
                ilac.TETT = dateTimePicker1.Value;
                ilac.BirimFiyat = Convert.ToInt32(txtBirimFiyatı.Text); // sayılara donusturme
                ilac.ilacTuru = cbilacturu.Text;
                ilac.SeriNo = txtSeriNo.Text;
                ilac.Miktar = Convert.ToInt32(txtMiktar.Text); // format exception
                ilac.FirmaAdi = txtFirmaAdi.Text;
                ilac.deleted = false;
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Dönüşüm hatası: " + ex.Message);
                // Hata durumunda yapılacak işlemler
            }
            db.ilaclars.InsertOnSubmit(ilac); // products da bir tablo ancak kullanılam yapıdan dolayı s getiriyoruz oyle kullanıyoruz. insertonsubmit ise eklemek icin kullanılıyor.
            db.SubmitChanges();

            MessageBox.Show("Data Successfully Inserted");
            // dgvilac.DataSource = db.ilaclars.ToList();
            dgvilac.DataSource = db.ilaclars.Where(x => x.deleted == false).ToList();

            txtilacAdi.Clear();
            txtBirimFiyatı.Clear();
            txtSeriNo.Clear();
            txtMiktar.Clear();
            txtFirmaAdi.Clear();

        }
        
        private void dgvilac_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 9)
            {
                int ilacId = Convert.ToInt32(dgvilac.Rows[e.RowIndex].Cells[0].Value.ToString());
                ilaclar ilac = db.ilaclars.FirstOrDefault(x => x.ilacid == ilacId); // x her bir ilac ogesini temsil eder. her bir urunun id ozelliginin productidye esit olanları filtreler
                if (ilac != null)
                {
                    if (ilac.deleted == false) // veya ilac.deleted = 1; kullanabilirsiniz
                    {
                            db.ilaclars.DeleteOnSubmit(ilac);
                            db.SubmitChanges();
                            MessageBox.Show("Data kullanılmamıs ve silindi");
                    }
                    else
                    {
                        MessageBox.Show ("Veriyi silemeyiz , veri saklandı.");

                    }
                    dgvilac.DataSource = db.ilaclars.Where(x => x.deleted == false).ToList();

                }

            }
        }

        private void dgvilac_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (e.ColumnIndex == 8) // e tıkladıgın sey demek. item diyebilirsin. eger tıklanan kısım 12.indeksli kolon ise diyoruz.
                {
                    ilacID = Convert.ToInt16(dgvilac.Rows[e.RowIndex].Cells[0].Value);
                    txtilacAdi.Text = ((dgvilac.Rows[e.RowIndex].Cells[1].Value) ?? "").ToString();
                    cbilacturu.Text = ((dgvilac.Rows[e.RowIndex].Cells[2].Value) ?? "").ToString();
                    txtBirimFiyatı.Text = ((dgvilac.Rows[e.RowIndex].Cells[3].Value) ?? "").ToString();
                    txtMiktar.Text = ((dgvilac.Rows[e.RowIndex].Cells[4].Value) ?? "").ToString();
                    dateTimePicker1.Text = ((dgvilac.Rows[e.RowIndex].Cells[5].Value) ?? "").ToString();
                    txtFirmaAdi.Text = ((dgvilac.Rows[e.RowIndex].Cells[6].Value) ?? "").ToString();
                    txtSeriNo.Text = ((dgvilac.Rows[e.RowIndex].Cells[7].Value) ?? "").ToString();
                    btnUpdate.Enabled = true;
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
                btnUpdate.Enabled = true;

            }
        }

        private void cbEnterProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvilac.DataSource = db.ilaclars.Where(x => x.ilacAdi == cbEnterProductName.Text).ToList();
        }


        private int sayiDonusumu(string cevrilecekMetin)
        {
            try
            {
                return Convert.ToInt32(cevrilecekMetin);
            }
            catch
            {
                return 0;
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var ilac = db.ilaclars.Where(x => x.ilacid == ilacID).FirstOrDefault();
            ilac.ilacAdi = txtilacAdi.Text;
            ilac.TETT = Convert.ToDateTime(dateTimePicker1.Text);
            ilac.BirimFiyat = sayiDonusumu(txtBirimFiyatı.Text); // sayılara donusturme
            ilac.ilacTuru = cbilacturu.Text;
            ilac.SeriNo = txtSeriNo.Text;
            ilac.Miktar = sayiDonusumu(txtMiktar.Text); // format exception
            ilac.FirmaAdi = txtFirmaAdi.Text;
            db.ilaclars.Append(ilac);
            db.SubmitChanges();
            MessageBox.Show("Data Updated");
            dgvilac.DataSource = db.ilaclars.ToList();

            txtilacAdi.Clear();
            txtBirimFiyatı.Clear();
            txtSeriNo.Clear();
            txtMiktar.Clear();
            txtFirmaAdi.Clear();
        }


        private void btnCsave_Click(object sender, EventArgs e)
        {
            musteri musteri = new musteri()
            {
                madi = txtCName.Text,
                telno = txtCPhone.Text,
                adres = txtCAddress.Text
            };
            db.musteris.InsertOnSubmit(musteri);
            db.SubmitChanges();
            MessageBox.Show("Data successfully inserted");
            dataGridViewCustomer.DataSource = db.musteris.ToList();

            txtCName.Text = "";
            txtCPhone.Text = "";
            txtCAddress.Text = "";
        }

        private void dataGridViewCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 5)
            {

                int musteriID = Convert.ToInt32(dataGridViewCustomer.Rows[e.RowIndex].Cells[0].Value.ToString());
                musteri musteri = db.musteris.FirstOrDefault(x => x.mid == musteriID); // x her bir product ogesini temsil eder. her bir urunun pid ozelliginin productidye esit olanları filtreler
                if (musteri != null)
                {
                    db.musteris.DeleteOnSubmit(musteri);
                    db.SubmitChanges();
                    MessageBox.Show("Data Successfully Deleted");
                    dataGridViewCustomer.DataSource = db.musteris.ToList();
                }
                else MessageBox.Show("You should select a data to delete");
            }
        }
        
        private void dataGridViewCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                customerId = Convert.ToInt16(dataGridViewCustomer.Rows[e.RowIndex].Cells[0].Value);
                txtCName.Text = dataGridViewCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtCAddress.Text = dataGridViewCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtCPhone.Text = dataGridViewCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();
            }
        }

        private void btnCupdate_Click(object sender, EventArgs e)
        {
            musteri musteri = db.musteris.Where(x => x.mid == customerId).FirstOrDefault();
            musteri.madi = txtCName.Text;
            musteri.adres = txtCAddress.Text;
            musteri.telno = txtCPhone.Text;

            db.musteris.Append(musteri);
            db.SubmitChanges();
            MessageBox.Show("Data Updated");

            dataGridViewCustomer.DataSource = db.musteris.ToList();

            txtCPhone.Text = "";
            txtCName.Text = "";
            txtCAddress.Text = "";


        }

        private void btnSsave_Click(object sender, EventArgs e)
        {
            tedarikci tedarikci = new tedarikci();
            tedarikci.tedadres = txtSaddress.Text;
            tedarikci.tedarikciadi = txtSName.Text;
            tedarikci.tedtelno = txtSphone.Text;

            db.tedarikcis.InsertOnSubmit(tedarikci);
            db.SubmitChanges();

            MessageBox.Show("Data Saved");
            dataGridSupplier.DataSource = db.tedarikcis.ToList();
        }



        private void dataGridSupplier_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 5)
            {
                tedarikciID = Convert.ToInt32(dataGridSupplier.Rows[e.RowIndex].Cells[0].Value);
                tedarikci tedarikci = db.tedarikcis.FirstOrDefault(x => x.tedarikciid == tedarikciID);
                db.tedarikcis.DeleteOnSubmit(tedarikci);
                db.SubmitChanges();
                MessageBox.Show("Data deleted");
                dataGridSupplier.DataSource = db.tedarikcis.ToList();

            }
        }

        private void dataGridSupplier_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                tedarikciID = Convert.ToInt32(dataGridSupplier.Rows[e.RowIndex].Cells[0].Value);
                txtSName.Text = dataGridSupplier.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtSaddress.Text = dataGridSupplier.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtSphone.Text = dataGridSupplier.Rows[e.RowIndex].Cells[3].Value.ToString();

            }
        }

        private void btnSupdate_Click(object sender, EventArgs e)
        {
            tedarikci tedarikci = db.tedarikcis.Where(x => x.tedarikciid == tedarikciID).FirstOrDefault();
            tedarikci.tedarikciadi = txtSName.Text;
            tedarikci.tedtelno = txtSphone.Text;
            tedarikci.tedadres = txtSaddress.Text;

            db.tedarikcis.Append(tedarikci);
            db.SubmitChanges();
            MessageBox.Show("Data updated");
            dataGridSupplier.DataSource = db.tedarikcis.ToList();
        }

        private void guna2TabControl1_Selected(object sender, TabControlEventArgs e)
        {
            cbSaleCName.DataSource = from musteri in db.musteris select musteri.madi; // urunu ve musterinin isimlerini comboboxa ekledik
            cbSalePName.DataSource = from ilac in db.ilaclars where ilac.deleted == false select ilac.ilacAdi;

            int faturano = Convert.ToInt16(db.faturas.Max(x => x.fno)); // ????????????????????????
            if (faturano != 0)
            {
                txtbillno.Text = (faturano + 1).ToString();

            }
            else txtbillno.Text = "1";


        }

        private void cbSaleCName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var musteri = db.musteris.Where(x => x.madi == cbSaleCName.Text).FirstOrDefault();
            if (musteri != null)
            {
                txtCustomerId.Text = musteri.mid.ToString();
                txtSaleCPhone.Text = musteri.telno;
                txtSaleCAddress.Text = musteri.adres;
            }
        }

        private void cbSalePName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ilac = db.ilaclars.Where(x => x.ilacAdi == cbSalePName.Text).FirstOrDefault();
            if (ilac != null)
            {
                txtProductId.Text = ilac.ilacid.ToString();
                txtSaleBatcno.Text = ilac.SeriNo;
                txtSaleQuantity.Text = ilac.Miktar.ToString();
                txtPrice.Text = ilac.BirimFiyat.ToString();
                cbSalePtype.Text = ilac.ilacTuru;

            }
        }

        private void txtOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sati satis = new sati();
                satis.musteriid = Convert.ToInt16(txtCustomerId.Text);
                satis.ilacid = Convert.ToInt16(txtProductId.Text);
                satis.satistarihi = DateTime.Now;
                satis.miktari = Convert.ToInt16(txtOrder.Text);
                satis.toplamfiyat = (Convert.ToInt16(txtOrder.Text) * Convert.ToInt16(txtPrice.Text));
                db.satis.InsertOnSubmit(satis);
                db.SubmitChanges();

                DataGridViewRow row = (DataGridViewRow)dataGridViewSale.Rows[0].Clone();
                row.Cells[0].Value = db.satis.Max(x => x.sid);
                row.Cells[1].Value = txtCustomerId.Text;
                row.Cells[2].Value = cbSalePName.Text;
                row.Cells[3].Value = txtSaleBatcno.Text;
                row.Cells[4].Value = (Convert.ToInt16(txtOrder.Text) * Convert.ToInt16(txtPrice.Text));
                row.Cells[5].Value = txtOrder.Text;
                dataGridViewSale.Rows.Add(row);

                int  ilacId = Convert.ToInt16( txtProductId.Text);
                ilaclar ilac = db.ilaclars.FirstOrDefault(x => x.ilacid == ilacId);
                if (ilac != null)
                {
                    if (db.satis.Any(x => x.ilacid == ilac.ilacid))
                    {
                        ilac.deleted = true; 
                        db.SubmitChanges();
                        MessageBox.Show( "İlac faturalandı." + ilac.deleted.ToString());
                    }
                    
                }
             

                fatura fatura = new fatura();
                fatura.fno = Convert.ToInt16(txtbillno.Text);
                fatura.satisid = Convert.ToInt16(satis.sid);

                db.faturas.InsertOnSubmit(fatura);
                db.SubmitChanges();
                if (txttotalamo.Text != "")
                {
                    int Total = Convert.ToInt32(txttotalamo.Text);
                    txttotalamo.Text = (Convert.ToInt32(txtOrder.Text) * Convert.ToInt32(txtPrice.Text) + Total).ToString();
                }
                else txttotalamo.Text = (Convert.ToInt32(txtOrder.Text) * Convert.ToInt32(txtPrice.Text)).ToString();

            }
        }

        private void cbSalePName_Enter(object sender, EventArgs e)
        {

        }
    }


}
