using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopInventorySystem
{
    public partial class LookUpProduct : Form
    {
        

        public LookUpProduct()
        {
            InitializeComponent();
            LoadProduct();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            DB_Connect.openConn();
            MySqlCommand command;
            string query = "SELECT p.id, p.barcode, p.name, c.name, p.price, p.qty FROM products AS p INNER JOIN category AS c on c.name = p.category WHERE CONCAT(p.name, c.name) LIKE '%" + txtSearch.Text + "%'";
            command = new MySqlCommand(query, DB_Connect.con);
            MySqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            DB_Connect.closeConn();

        }

            private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "Select")
            {
                /*
                Qty qty = new Qty(cashier);
                qty.ProductDetails(dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString(), double.Parse(dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString()), cashier.lblTranNo.Text, int.Parse(dgvProduct.Rows[e.RowIndex].Cells[7].Value.ToString()));
                qty.ShowDialog();
                */
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void LookUpProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
