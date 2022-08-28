using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace ShopInventorySystem
{
    public partial class CreateUpdateProduct : Form
    {
        DB_Connect db = new DB_Connect();
        public CreateUpdateProduct()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void Clear()
        {
            txtPcode.Clear();
            txtBarcode.Clear();
            txtPdesc.Clear();
            txtPrice.Clear();
            cboCategory.SelectedIndex = 0;
            UDReOrder.Value = 1;
            UDQty.Value = 1;

            txtPcode.Enabled = true;
            txtPcode.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        public void LoadCategory()
        {
            cboCategory.Items.Clear();
            cboCategory.DataSource = db.getTable("SELECT * FROM product_category");
            cboCategory.DisplayMember = "name";
            cboCategory.ValueMember = "id";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure want to save this product?", "Save Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DB_Connect.openConn();
                    MySqlCommand command;
                    string query = "Insert into products(product_code,product_barcode,product_name,product_category,product_description,price,qty,exp_date,re_order) values(@product_code,@product_barcode,@product_name,@product_category,@product_description,@price,@qty,@exp_date,@re_order)";
                    command = new MySqlCommand(query, DB_Connect.con);
                    command.Parameters.AddWithValue("@product_code", txtPcode.Text);
                    command.Parameters.AddWithValue("@product_barcode", txtBarcode.Text);
                    command.Parameters.AddWithValue("@product_name", txtPname.Text);
                    command.Parameters.AddWithValue("@product_category", cboCategory.Text);
                    command.Parameters.AddWithValue("@product_description", txtPdesc.Text);
                    command.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
                    command.Parameters.AddWithValue("@qty", UDQty.Value);
                    command.Parameters.AddWithValue("@exp_date", expDate.Value.Date);
                    command.Parameters.AddWithValue("@re_order", UDReOrder.Value);
                    command.ExecuteNonQuery();
                    DB_Connect.closeConn();
                    MessageBox.Show("Product has been successfully saved.");
                    Clear();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure want to update this product?", "Update Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DB_Connect.openConn();
                    MySqlCommand cm;
                    string query = "UPDATE products SET product_code=@code,product_barcode=@barcode,product_name=@product_name,product_category=@product_category,price=@price,re_order=@re_order WHERE product_code LIKE @code";
                    cm = new MySqlCommand(query, DB_Connect.con);
                    cm.Parameters.AddWithValue("@code", txtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@product_name", txtPname.Text);
                    cm.Parameters.AddWithValue("@product_category", cboCategory.SelectedValue);
                    cm.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@re_order", UDReOrder.Value);
                    cm.ExecuteNonQuery();
                    MessageBox.Show("Product has been successfully updated.");
                    DB_Connect.closeConn();
                    Clear();
                    this.Dispose();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void AddProduct_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void AddProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void metroDateTime1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtPname_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
