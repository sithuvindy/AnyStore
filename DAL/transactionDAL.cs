using AnyStore.BLL;
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
    class transactionDAL
    {

        //static string method for data base connection string
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;


        #region insert Transaction Method
        public bool Insert_Transaction(transactionsBLL t, out int transactionID)
        {
            //Creating a default return type(boolean value) and setting its value to false
            bool isSuccess = false;

            //Set the out transactionID value to negative 1 (-1)
            transactionID = -1;

            //Connect database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //Create a SQL Query to insert Transactions
                string sql = "INSERT INTO tbl_transactions (type,dea_cust_id,grandTotal,transaction_date,tax,discount,added_by) VALUES (@type,@dea_cust_id,@grandTotal,@transaction_date,@tax,@discount,@added_by); SELECT @@IDENTITY;";

                //Creating a sql command to pass values into the query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Creating parameters to add data
                cmd.Parameters.AddWithValue("@type", t.type);
                cmd.Parameters.AddWithValue("@dea_cust_id", t.dea_cust_id);
                cmd.Parameters.AddWithValue("@grandtotal", t.grandtotal);
                cmd.Parameters.AddWithValue("@transaction_date", t.transaction_date);
                cmd.Parameters.AddWithValue("@tax", t.tax);
                cmd.Parameters.AddWithValue("@discount", t.discount);
                cmd.Parameters.AddWithValue("@added_by", t.added_by);

                //Database Connection Open Here
                conn.Open();
                //ExecuteNonQuery returns the no. of rows effected after executing the query. ExecuteScalar returs the value of first column of first row in the result set by the query

                //execute the query(here using executescalar)
                object o = cmd.ExecuteScalar();

                //If the query runs successfully then the value will not be null else it will be null
                if (o != null)
                {
                    //Query executed Successfully
                    transactionID = int.Parse(o.ToString());
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

        #region Method to display all the transactions

        public DataTable DisplayAllTransactions()
        {
            //Connect database
            SqlConnection conn = new SqlConnection(myconnstring);

            //Creating Table to hold data from database
            DataTable dt = new DataTable();

            try
            {
                //Writing SQL Query to select all the data from data base
                string sql = "SELECT * FROM tbl_transactions";
                //Creating sql command using sql query and connection(For executing command)
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Creating SQL DataAdapter using command (we get data using sql dataadapter from data base)
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //Database connection open
                conn.Open();
                //Fill the data into the data table
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                //Throw message if any error occur
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            //return the value in data tables
            return dt;

        }

        #endregion

        #region Method to Display Transaction Based on transaction type

        public DataTable DisplayTransactionByType(string type)
        {
            //Connect database
            SqlConnection conn = new SqlConnection(myconnstring);

            //Creating Table to hold data from database
            DataTable dt = new DataTable();

            try
            {
                //Writing SQL Query to select all the data from data base
                string sql = "SELECT * FROM tbl_transactions WHERE type='" + type + "'";
                //Creating sql command using sql query and connection(For executing command)
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Creating SQL DataAdapter using command (we get data using sql dataadapter from data base)
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //Database connection open
                conn.Open();
                //Fill the data into the data table
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                //Throw message if any error occur
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            //return the value in data tables
            return dt;
        }

        #endregion
    }

}   
