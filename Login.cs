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
    public partial class Login : Form
    {
        public string _pass = "";
        public string _name = "";
        public string _role = "";
        public bool _isactive;

        public Login()
        {
            InitializeComponent();
        }

        private void show_Click(object sender, EventArgs e)
        {
            if (password.PasswordChar == '*')
            {
                hide.BringToFront();
                password.PasswordChar = '\0';
            }
        }

        private void hide_click(object sender, EventArgs e)
        {
            if (password.PasswordChar == '\0')
            {
                show.BringToFront();
                password.PasswordChar = '*';
            }
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            
            DB_Connect.openConn();
            MySqlCommand command;
            if (username.Text != "" && password.Text != "")
            {
                

                try
                {
                    string  enc_pass = Encrypt.HashString(password.Text);
                    bool found = false;
                    string query = "Select * from users where username = '" + username.Text + "' && password ='" + enc_pass + "'";
                    command = new MySqlCommand(query, DB_Connect.con);
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        found = true;
                        _name = reader["name"].ToString();
                        _role = reader["role"].ToString();
                        _pass = reader["password"].ToString();
                        _isactive = bool.Parse(reader["is_active"].ToString());
                        

                    }
                    else
                    {
                        found = false;
                        MessageBox.Show("Incorrect credentials!!");
                    }
                    reader.Close();

                    if (found)
                    {
                        if (!_isactive)
                        {
                            MessageBox.Show("Account is deactivate. Unable to login", "Inactive Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (_role == "attendant")
                        {
                            _role = "Attendant";
                            //MessageBox.Show("Welcome " + _name + " |"+ _role, "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            username.Clear();
                            password.Clear();
                            this.Hide();
                            AttendantDashboard attendant = new AttendantDashboard();
                            attendant.lblUsername.Text = _name;
                            attendant.lblname.Text = _name + " | " + _role;
                            attendant.ShowDialog();
                        }
                        else if (_role == "admin")
                        {
                            _role = "Admin";
                            //MessageBox.Show("Welcome " + _name + " |" + _role, "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            username.Clear();
                            password.Clear();
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

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void username_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
