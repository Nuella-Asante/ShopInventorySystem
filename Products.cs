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
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
            LoadProducts();
        }

        public void LoadProducts()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            string query = "Select * from products order by name";
            DB_Connect.openConn(); 
            MySqlCommand command;
            command = new MySqlCommand(query, DB_Connect.con);

            MySqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                
                i++;
                dgvProduct.Rows.Add(i, dr["id"].ToString(), dr["barcode"].ToString(), dr["name"].ToString(), dr["category"].ToString(), dr["price"].ToString(), dr["re_order"].ToString());
            }
            dr.Close();
            DB_Connect.closeConn();
        }

        private void ProductsList_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CreateUpdateProduct addProduct = new CreateUpdateProduct();
            addProduct.btnUpdate.Visible = false;
            addProduct.ShowDialog(this);
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                CreateUpdateProduct product = new CreateUpdateProduct();
                product.txtBarcode.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                product.txtPname.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();

                //product.cboBrand.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                product.cboCategory.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                product.txtPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                product.UDReOrder.Value = int.Parse(dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString());
                


















                product.txtBarcode.Enabled = false;
                product.btnSave.Visible = false;
                product.btnSave.Enabled = false;
                product.btnCancel.Visible = false;
                product.btnUpdate.Enabled = true;
                product.ShowDialog(this);
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DB_Connect.openConn();
                    MySqlCommand command;
                    string query = "DELETE FROM products where product_code like '" + dgvProduct[1, e.RowIndex].Value.ToString() + "'";
                    command = new MySqlCommand(query, DB_Connect.con);
                    command.ExecuteNonQuery();
                    DB_Connect.closeConn();
                    MessageBox.Show("Product has been successfully deleted.", "Point Of Sales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadProducts();
        }
    }
}
