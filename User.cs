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

    public partial class User : Form
    {
        AdminDashboard admin;
        public string user_id;
        public string name;
        public string email;
        public string phone;
        public string accstatus;
        public string role;
        public string gender;

        public User()
        {
            InitializeComponent();
            LoadUser();
            //admin = dashboard;
        }
         public void Clear()
        {
            txtName.Clear();
            txtUsername.Clear();
            txtPass.Clear();
            txtRePass.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            userRole.Text = "";
            txtName.Focus();
        }

        private string generateUserID()
        {
            string num = "123456789";
            string user_id = string.Empty;
            int otp_digits = 5;
            string otp;
            int get_index;

            for(int i = 0; i < otp_digits; i++)
            {
                do
                {
                    get_index = new Random().Next(0, num.Length);
                    otp = num.ToCharArray()[get_index].ToString();
                }while(user_id.IndexOf(otp) != -1);

                user_id += otp;
            }

            return user_id;
        }

        public void LoadUser()
        {
            int i = 0;
            dgvUser.Rows.Clear();
            string query = "Select * from user";
            DB_Connect.openConn();
            MySqlCommand command;
            command = new MySqlCommand(query, DB_Connect.con);
            
            MySqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvUser.Rows.Add(dr[0].ToString(), dr[2].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[1].ToString());
            }
            dr.Close();
            DB_Connect.closeConn();
        }

        private void User_Load(object sender, EventArgs e)
        {
            //lblUsername.Text = admin.lblUsername.Text;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show("You chose to remove this account from this System's user list. \n\n Are you sure you want to remove '" + name + "' \\ '" + role + "'", "User Account", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes))
            {
                string query = "Delete from user where name = '"+ name +"'";
                DB_Connect.openConn();
                MySqlCommand command;
                command = new MySqlCommand(query, DB_Connect.con);
                command.ExecuteNonQuery();
                MessageBox.Show("Account has been successfully deleted");
                LoadUser();
            }
        }

        private void dgvUser_SelectionChanged(object sender, EventArgs e)
        {
            int i = dgvUser.CurrentRow.Index;
            user_id = dgvUser[0, i].Value.ToString();
            name = dgvUser[1, i].Value.ToString();
            email = dgvUser[2, i].Value.ToString();
            phone = dgvUser[3, i].Value.ToString();
            //accstatus = dgvUser[4, i].Value.ToString();
            role = dgvUser[4, i].Value.ToString();
            
            if (lblUsername.Text == name)
            {
                btnRemove.Enabled = false;
                btnResetPass.Enabled = false;
                lblAccNote.Text = "To change your password, go to change password tag.";

            }
            else
            {
                btnRemove.Enabled = true;
                btnResetPass.Enabled = true;
                lblAccNote.Text = "To change the password for " + name + ", click Reset Password.";
            }
            gbUser.Text = "Password For " + name;

        }


        private void btnAccSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text == "" || txtPass.Text == "" || txtRePass.Text == "" ||  userGender.Text == "" || userRole.Text == "")
                {
                    MessageBox.Show("Fields cannot be Empty!","Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {

                    if (txtPass.Text != txtRePass.Text)
                    {
                        MessageBox.Show("Password does not Match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    DB_Connect.openConn();
                    MySqlCommand command; 
                    string user_id = generateUserID();
                    int is_active = 1;
                    string role = "";
                    string query = "";
                    bool user_exists = false;
                    if (userRole.Text == "Attendant")
                    {
                        role = "attendant";
                    }
                    else if (userRole.Text == "Administrator")
                    {
                        role = "admin";
                    }

                    if (txtUsername.Text != "")
                    {
                        query = "Select * from user where username = '" + txtUsername.Text + "'";
                        command = new MySqlCommand(query, DB_Connect.con);
                        MySqlDataReader dr = command.ExecuteReader();
                        if (dr.HasRows)
                        {
                            MessageBox.Show("Username already Exist!!");
                            user_exists = true;
                        }
                        else
                        {
                            user_exists = false;
                        }
                        dr.Close();
                    }
                    if (!user_exists)
                    {
                        string enc_pass = Encrypt.HashString(txtPass.Text);
                        query = "Insert into user(user_id,username,name,email,dob,phone,role,gender,password,is_active) values(@user_id,@username,@name,@email,@dob,@phone,@role,@gender,@password,@is_active)";
                        command = new MySqlCommand(query, DB_Connect.con);
                        command.Parameters.AddWithValue("@user_id", user_id);
                        command.Parameters.AddWithValue("@username", txtUsername.Text);
                        command.Parameters.AddWithValue("@name", txtName.Text);
                        command.Parameters.AddWithValue("@email", txtEmail.Text);
                        command.Parameters.AddWithValue("@dob", dob.Value.Date);
                        command.Parameters.AddWithValue("@phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@role", role);
                        command.Parameters.AddWithValue("@gender", userGender.Text);
                        command.Parameters.AddWithValue("@password", enc_pass);
                        command.Parameters.AddWithValue("@is_active", is_active);
                        command.ExecuteNonQuery();
                        MessageBox.Show("New account has been successfully saved!", "Save Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUser();
                        Clear();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Warning");
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void email_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAccCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnPassSave_Click(object sender, EventArgs e)
        {
            /*   
               MySqlCommand command;
               if (txtCurPass.Text != admin._pass)
               {
                   MessageBox.Show("Current password did not martch!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   return;
               }
               if (txtNPass.Text != txtRePass2.Text)
               {
                   MessageBox.Show("Confirm new password did not martch!", "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   return;
               }
               string query = "UPDATE tbUser SET password= '" + txtNPass.Text + "' WHERE name ='" + admin._name + "'";
               command = new MySqlCommand(query, db_con.con);
               command.ExecuteNonQuery();
               //dbcon.ExecuteQuery("UPDATE tbUser SET password= '" + txtNPass.Text + "' WHERE username='" + lblUsername.Text + "'");
               MessageBox.Show("Password has been succefully changed!", "Changed Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
               */
            MessageBox.Show("Password has been succefully changed!", "Changed Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnEditUser_Click(object sender, EventArgs e)
        { 
            UserModule userEdit = new UserModule();
            userEdit.Text = name + "\\" + name + " Edit Account";
            userEdit.txtName.Text = name;
            userEdit.txtEmail.Text = email;
            userEdit.txtPhone.Text = phone;
            userEdit.cbRole.Text = role;
            //userEdit.cbActivate.Text = accstatus;
            userEdit.ShowDialog();
        }

        private void btnResetPass_Click(object sender, EventArgs e)
        {
            ResetUserPass resetPassword = new ResetUserPass();
            resetPassword.ShowDialog();
        }
    }
}
