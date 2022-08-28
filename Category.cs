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
    public partial class Category : Form
    {
        public Category()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            int i = 0;
            dgvCategory.Rows.Clear();
            string query = "Select * from category order by name";
            DB_Connect.openConn();
            MySqlCommand command;
            command = new MySqlCommand(query, DB_Connect.con);
            MySqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCategory.Rows.Add(i,dr["name"].ToString(), dr["description"].ToString());
            }
            dr.Close();
            DB_Connect.closeConn();
        }

        private void ProductCategory_Load(object sender, EventArgs e)
        {

        }

        private void dgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CatetgoryModule addCategory = new CatetgoryModule();
            addCategory.ShowDialog(this);
        }
    }
}
