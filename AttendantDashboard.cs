using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopInventorySystem
{
    public partial class AttendantDashboard : Form
    {
        int qty;
        string id;
        string price;
        string query = "";
        string stitle = "Point Of Sales";

        DB_Connect _Con = new DB_Connect();
        public AttendantDashboard()
        {
            InitializeComponent();
            GetTranNo();
            lblDate.Text = DateTime.Now.ToShortDateString();
        }

        public void slide(Button button)
        {
            panelSlide.BackColor = Color.White;
            panelSlide.Height = button.Height;
            panelSlide.Top = button.Top;
        }

        public void GetTranNo()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                int count;
                string transno;
                DB_Connect.openConn();
                MySqlCommand command;
                query = "SELECT transno FROM cart WHERE transno LIKE '" + sdate + "%' ORDER BY id desc limit 1";
                command = new MySqlCommand(query, DB_Connect.con);
                MySqlDataReader dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTranNo.Text = sdate + (count + 1);
                }
                else
                {
                    transno = sdate + "1001";
                    lblTranNo.Text = transno;
                }
                dr.Close();
                DB_Connect.closeConn();
            }
            catch (Exception ex)
            {

                DB_Connect.closeConn();
                MessageBox.Show(ex.Message, stitle);

            }

        }
        public void LoadCart()
        {
            try
            {
                Boolean hascart = false;
                int i = 0;
                double total = 0;
                double discount = 0;
                dgvCash.Rows.Clear();
                DB_Connect.openConn();
                MySqlCommand command;
                query = "SELECT c.id, c.product_id, p.name, c.price, c.qty, c.disc, c.total FROM cart AS c INNER JOIN products AS p ON c.product_id=p.id WHERE c.transno LIKE @transno and c.status LIKE 'Pending'";
                command = new MySqlCommand(query, DB_Connect.con);
                command.Parameters.AddWithValue("@transno", lblTranNo.Text);
                MySqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {

                    i++;
                    total += Convert.ToDouble(dr["total"].ToString());
                    discount += Convert.ToDouble(dr["disc"].ToString());
                    dgvCash.Rows.Add(i, dr["id"].ToString(), dr["product_id"].ToString(), dr["name"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), double.Parse(dr["total"].ToString()).ToString("#,##0.00"));//
                    hascart = true;
                }
                dr.Close();
                DB_Connect.closeConn();
                lblSaleTotal.Text = total.ToString("#,##0.00");
                lblDiscount.Text = discount.ToString("#,##0.00");
                //GetCartTotal();
                //if (hascart) { btnClear.Enabled = true; btnSettle.Enabled = true; btnDiscount.Enabled = true; }
                //else { btnClear.Enabled = false; btnSettle.Enabled = false; btnDiscount.Enabled = false; }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, stitle);
            }

        }

        public void AddToCart(string _pid, double _price, int _qty)
        {
            try
            {
                string id = "";
                int cart_qty = 0;
                bool found = false;
                DB_Connect.openConn();
                MySqlCommand command;
                query = "Select * from cart Where transno = @transno and product_id = @pid";
                command = new MySqlCommand(query, DB_Connect.con);
                command.Parameters.AddWithValue("@transno", lblTranNo.Text);
                command.Parameters.AddWithValue("@pid", _pid);
                MySqlDataReader dr = command.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    id = dr["id"].ToString();
                    cart_qty = int.Parse(dr["qty"].ToString());
                    found = true;
                }
                else found = false;
                dr.Close();
                DB_Connect.closeConn();
                if (found)
                {
                    if (qty < (int.Parse(txtQty.Text) + cart_qty))
                    {
                        MessageBox.Show("Unable to procced. Remaining quantity on hand is " + qty, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    DB_Connect.openConn();
                    query = "Update cart set qty = (qty + " + _qty + ")Where id= '" + id + "'";
                    command = new MySqlCommand(query, DB_Connect.con);
                    command.ExecuteReader();
                    DB_Connect.closeConn();
                    txtBarcode.SelectionStart = 0;
                    txtBarcode.SelectionLength = txtBarcode.Text.Length;
                    LoadCart();
                }
                else
                {
                    if (qty < (int.Parse(txtQty.Text) + cart_qty))
                    {
                        MessageBox.Show("Unable to procced. Remaining qty on hand is" + qty, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    DB_Connect.openConn();
                    query = "INSERT INTO cart(transno, product_id, price, qty, sdate, attendant) VALUES(@transno, @pid, @price, @qty, @sdate, @attendant)";
                    command = new MySqlCommand(query, DB_Connect.con);
                    command.Parameters.AddWithValue("@transno", lblTranNo.Text);
                    command.Parameters.AddWithValue("@pid", _pid);
                    command.Parameters.AddWithValue("@price", _price);
                    command.Parameters.AddWithValue("@qty", _qty);
                    command.Parameters.AddWithValue("@sdate", DateTime.Now);
                    command.Parameters.AddWithValue("@attendant", lblUsername.Text);
                    command.ExecuteNonQuery();
                    DB_Connect.closeConn();
                    LoadCart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, stitle);
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelSlide_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPass_Click(object sender, EventArgs e)
        {
            slide(btnPass);
            ChangePassword changePassword = new ChangePassword();
            changePassword.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            slide(btnLogout);
            if (dgvCash.Rows.Count > 0)
            {
                MessageBox.Show("Unable to logout. Please cancel the transaction.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Logout Application?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                Login login = new Login();
                login.ShowDialog();
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            slide(btnSearch);
            LookUpProduct lookUp = new LookUpProduct();
            lookUp.LoadProduct();
            lookUp.ShowDialog(this);
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtBarcode.Text == string.Empty) return;
                else
                {
                    string _pid;
                    double _price;
                    int _qty;
                    DB_Connect.openConn();
                    MySqlCommand command;
                    string query = "SELECT * FROM products WHERE barcode LIKE '" + txtBarcode.Text + "'";
                    command = new MySqlCommand(query, DB_Connect.con);
                    MySqlDataReader dr = command.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        qty = int.Parse(dr["qty"].ToString());
                        _pid = dr["id"].ToString();
                        _price = double.Parse(dr["price"].ToString());
                        _qty = int.Parse(txtQty.Text);
                        dr.Close();
                        DB_Connect.closeConn();
                        //insert to tbCart
                        AddToCart(_pid, _price, _qty);
                    }
                    dr.Close();
                    DB_Connect.closeConn();
                }
            }
            catch (Exception ex)
            {
                DB_Connect.closeConn();
                MessageBox.Show(ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimer.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCash.Columns[e.ColumnIndex].Name;


            if (colName == "Delete")
            {
                if (MessageBox.Show("Remove this item", "Remove item", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _Con.ExecuteQuery("Delete from cart where id like'" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                    MessageBox.Show("Items has been successfully remove", "Remove item", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCart();
                }
            }
            else if (colName == "colAdd")
            {
                int i = 0;
                DB_Connect.openConn();
                MySqlCommand command;
                query = "SELECT SUM(qty) as qty FROM products WHERE id LIKE'" + dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString() + "' GROUP BY id";
                command = new MySqlCommand(query, DB_Connect.con);
                i = int.Parse(command.ExecuteScalar().ToString());
                DB_Connect.closeConn();
                if (int.Parse(dgvCash.Rows[e.RowIndex].Cells[5].Value.ToString()) < i)
                {
                    _Con.ExecuteQuery("UPDATE cart SET qty = qty + " + int.Parse(txtQty.Text) + " WHERE transno LIKE '" + lblTranNo.Text + "'  AND id LIKE '" + dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString() + "'");
                    LoadCart();
                }
                else
                {
                    MessageBox.Show("Remaining qty on hand is " + i + "!", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (colName == "colReduce")
            {
                int i = 0;
                DB_Connect.openConn();
                MySqlCommand command;
                query = "SELECT SUM(qty) as qty FROM cart WHERE product_id LIKE'" + dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString() + "' GROUP BY product_id";
                command = new MySqlCommand(query, DB_Connect.con);
                i = int.Parse(command.ExecuteScalar().ToString());
                DB_Connect.closeConn();
                if (i > 1)
                {
                    _Con.ExecuteQuery("UPDATE cart SET qty = qty - " + int.Parse(txtQty.Text) + " WHERE transno LIKE '" + lblTranNo.Text + "'  AND product_id LIKE '" + dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString() + "'");
                    LoadCart();
                }
                else
                {
                    MessageBox.Show("Remaining qty on cart is " + i + " !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }
    }
    
}
