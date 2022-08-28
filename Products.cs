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
            string query = "Select * from products order by product_name";
            db_con.openConn(); 
            MySqlCommand command;
            command = new MySqlCommand(query, db_con.con);

            MySqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                
                i++;
                dgvProduct.Rows.Add(i, dr["product_code"].ToString(), dr["product_barcode"].ToString(), dr["product_name"].ToString(), dr["product_category"].ToString(), dr["price"].ToString(), dr["re_order"].ToString());
            }
            dr.Close();
            db_con.closeConn();
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
                product.txtPcode.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                product.txtBarcode.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                product.txtPname.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();

                //product.cboBrand.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                product.cboCategory.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                product.txtPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                product.UDReOrder.Value = int.Parse(dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString());
                
                product.txtPcode.Enabled = false;
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
                    db_con.openConn();
                    MySqlCommand command;
                    string query = "DELETE FROM products where product_code like '" + dgvProduct[1, e.RowIndex].Value.ToString() + "'";
                    command = new MySqlCommand(query, db_con.con);
                    command.ExecuteNonQuery();
                    db_con.closeConn();
                    MessageBox.Show("Product has been successfully deleted.", "Point Of Sales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadProducts();
        }
    }
}
