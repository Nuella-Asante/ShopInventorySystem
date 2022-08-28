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
    public partial class CatetgoryModule : Form
    {
        public CatetgoryModule()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            txtCategory.Clear();
            txtCategory.Focus();
            btnSave.Enabled = true;
            
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this Category?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    db_con.openConn();
                    MySqlCommand command;
                    string query = "Insert into product_category(name) values(@name)";
                    command = new MySqlCommand(query, db_con.con);
                    command.Parameters.AddWithValue("@name", txtCategory.Text);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Record has been successful saved.", "Point Of Sales");
                    Clear();
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
