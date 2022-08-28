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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Xml.Linq;
namespace ShopInventorySystem
{
    public partial class Login : Form
    {
        public string _pass = "";
        public string _name = "";
        public string _role = "";
        public bool _status;
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            DB_Connect.openConn();
            MySqlCommand command;
            if (txtName.Text != "" && txtPass.Text != "")
            {


                try
                {
                    string encrypt_pass = Encrypt.HashString(txtPass.Text);
                    bool found = false;
                    string query = "Select * from users where username = '" + txtName.Text + "' && password ='" + encrypt_pass + "'";
                    command = new MySqlCommand(query, DB_Connect.con);
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        found = true;
                        _name = reader["fullname"].ToString();
                        _role = reader["role"].ToString();
                        _pass = reader["password"].ToString();
                        _status = bool.Parse(reader["status"].ToString());


                    }
                    else
                    {
                        found = false;
                        MessageBox.Show("Incorrect credentials!!");
                    }
                    reader.Close();

                    if (found)
                    {
                        if (!_status)
                        {
                            MessageBox.Show("Account is deactivate. Unable to login", "Inactive Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (_role == "Attendant")
                        {
                            _role = "Attendant";
                            txtName.Clear();
                            txtPass.Clear();
                            this.Hide();
                            AttendantDashboard attendant = new AttendantDashboard();
                            attendant.lblUsername.Text = _name;
                            attendant.lblname.Text = _name + " | " + _role;
                            attendant.ShowDialog();
                        }
                        else if (_role == "Admin")
                        {
                            _role = "Admin";
                            txtName.Clear();
                            txtPass.Clear();
                            this.Hide();
                            AdminDashboard admin = new AdminDashboard();
                            admin.lblUsername.Text = _name;
                            admin._pass = _pass;
                            admin.ShowDialog();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username and password!", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    DB_Connect.closeConn();
                }
            }
            else
            {
                MessageBox.Show("Username or Password cannot be empty");
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Exit Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
