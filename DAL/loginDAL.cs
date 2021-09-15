using AnyStore.BLL;
using EncryptionandDecryptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class loginDAL
    {
        //Static String method to connect database
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
        
        public bool loginCheck(loginBLL l)
        {
            //create a boolean variable and set it's value to false and return it
            bool isSuccess = false;

            //connecting to data base
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                string Password = Cryptography.Encrypt(l.password);
                //SQL query to check login
                string sql = "SELECT * FROM tbl_users WHERE username = @username AND password = @password AND user_type=@user_type ";
                //Creating SQL command to pass value
                SqlCommand cmd = new SqlCommand(sql,conn);

                cmd.Parameters.AddWithValue("@username",l.username );
                cmd.Parameters.AddWithValue("@password", Password);
                cmd.Parameters.AddWithValue("@user_type", l.user_type);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                adapter.Fill(dt);

                //Checking the rows in Datatables
                if(dt.Rows.Count>0)
                {
                    //Login Successful
                    isSuccess = true;
                }
                else
                {
                    //Login Failed
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }
    }
}
