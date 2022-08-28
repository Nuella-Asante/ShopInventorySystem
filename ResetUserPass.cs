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
    public partial class ResetUserPass : Form
    {
        //User user;
        public ResetUserPass()
        {
            InitializeComponent();
            //user = acc_user;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtResPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtPass.Text != txtComPass.Text)
            {
                MessageBox.Show("The password you typed do not match. Type the password for this account in both text boxes.", "Add User Wizard", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (MessageBox.Show("Reset password?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    /*
                    string query = "UPDATE user set password = '" + txtPass.Text + "'WHERE user_id = '" + user.user_id + "'";
                    db_con.openConn();
                    MySqlCommand command;
                    command = new MySqlCommand(query, db_con.con);
                    command.ExecuteNonQuery();
                    MySqlDataReader dr = command.ExecuteReader();
                    dbcon.ExecuteQuery("UPDATE user set password = '" + txtPass.Text + "'WHERE user_id = '" + user.user_id + "'");
                    MessageBox.Show("Password has been successfully reset", "Reset Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                    */
                    MessageBox.Show("Password has been successfully reset", "Reset Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }
            }
        }

        private void ResetPassword_Load(object sender, EventArgs e)
        {

        }
    }
}
