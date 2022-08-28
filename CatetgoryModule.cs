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
            txtDesc.Clear();
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
                    DB_Connect.openConn();
                    MySqlCommand command;
                    string query = "Insert into category(name,description) values(@name,@desc)";
                    command = new MySqlCommand(query, DB_Connect.con);
                    command.Parameters.AddWithValue("@name", txtCategory.Text);
                    command.Parameters.AddWithValue("@desc", txtDesc.Text);
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
