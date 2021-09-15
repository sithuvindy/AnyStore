using AnyStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class transactionDetailDAL
    {
        //static string method for data base connection string
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

        #region Insert transaction details in Database
        public bool InsertTransactionDetail(transactionDetailBLL td)
        {
            //Creating a default return type(boolean value) and setting its value to false
            bool isSuccess = false;

            //Connect database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //Create a SQL Query to insert Data(writhing query to  add new category)
                string sql = "INSERT INTO tbl_transaction_detail (product_id,rate,qty,total,dea_cust_id, added_date,added_by) VALUES(@product_id,@rate,@qty,@total,@dea_cust_id,@added_date,@added_by)";

                //Creating a sql command to pass values into the query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Creating parameters to add data
                cmd.Parameters.AddWithValue("@product_id", td.product_id);
                cmd.Parameters.AddWithValue("@rate", td.rate);
                cmd.Parameters.AddWithValue("@qty", td.qty);
                cmd.Parameters.AddWithValue("@total", td.total);
                cmd.Parameters.AddWithValue("@dea_cust_id", td.dea_cust_id);
                cmd.Parameters.AddWithValue("@added_date", td.added_date);
                cmd.Parameters.AddWithValue("@added_by", td.added_by);

                //Database Connection Open Here
                conn.Open();

                //Craeting the int variable to execute query
                int rows = cmd.ExecuteNonQuery();

                //If the query runs successfully then the value of rows will be greater than zero else its value will be zero
                if (rows > 0)
                {
                    //Query Successfull
                    isSuccess = true;
                }
                else
                {
                    //Query Failed
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
        #endregion
    }
}
