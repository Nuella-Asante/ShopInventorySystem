using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace ShopInventorySystem
{
    internal class DB_Connect
    {
        public static string cs = @"server=localhost; userid=root; password=; database=inventory_system;";
        public static MySqlConnection con = new MySqlConnection(cs);
        public static MySqlCommand cmd = new MySqlCommand();

        public static void openConn()
        {
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void closeConn()
        {
            try
            {
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public double ExtractDBData(string query)
        {
            openConn();
            cmd = new MySqlCommand(query, con);
            double data = double.Parse(cmd.ExecuteScalar().ToString());
            closeConn();
            return data;
        }

        public DataTable getTable(string query)
        {
            openConn();
            cmd = new MySqlCommand(query, con);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            closeConn();
            return table;
        }

        public void ExecuteQuery(String sql)
        {
            try
            {
                openConn();
                cmd = new MySqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                closeConn();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
