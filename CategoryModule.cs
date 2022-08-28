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
    public partial class CategoryModule : Form
    {
        public CategoryModule()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            int i = 0;
            dgvCategory.Rows.Clear();
            string query = "Select * from product_category order by name";
            db_con.openConn();
            MySqlCommand command;
            command = new MySqlCommand(query, db_con.con);
            MySqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCategory.Rows.Add(i,dr["name"].ToString(), dr["name"].ToString(), dr["name"]);
            }
            dr.Close();
            db_con.closeConn();
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
