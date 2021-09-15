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
    class DeaCustDAL
    {
        //static string method for data base connection string
        static string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

        #region Selecting Data from Database
        //Region of Selecting Data from Database
        public DataTable Select()
        {
            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstring);
            //Creating Table to hold data from database
            DataTable dt = new DataTable();
            try
            {
                //Writing SQL Query to select all the data from data base
                string sql = "SELECT * FROM tbl_dea_cust";
                //Creating sql command using sql query and connection(For executing command)
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Creating SQL DataAdapter using command (we get data using sql dataadapter from data base)
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                // Database connection open
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
        //End region
        #endregion

        #region Insert Data in Database
        public bool Insert(DeaCustBLL dc)
        {
            //Creating a default return type(boolean value) and setting its value to false
            bool isSuccess = false;

            //Connect database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //Create a SQL Query to insert Data(writhing query to  add new category)
                string sql = "INSERT INTO tbl_dea_cust (type,name,email,contact,address, added_date,added_by) VALUES(@type,@name,@email,@contact,@address, @added_date, @added_by)";

                //Creating a sql command to pass values into the query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Creating parameters to add data
                cmd.Parameters.AddWithValue("@type", dc.type);
                cmd.Parameters.AddWithValue("@name", dc.name);
                cmd.Parameters.AddWithValue("@email", dc.email);
                cmd.Parameters.AddWithValue("@contact", dc.contact);
                cmd.Parameters.AddWithValue("@address", dc.address);
                cmd.Parameters.AddWithValue("@added_date", dc.added_date);
                cmd.Parameters.AddWithValue("@added_by", dc.added_by);

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

        #region update data in the data base
        public bool Update(DeaCustBLL dc)
        {
            //Create a default return type and set its default value false
            bool isSuccess = false;

            //Connect database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {

                //SQL to update data in our Database
                string sql = "Update tbl_dea_cust SET type=@type, name=@name, email=@email ,contact=@contact, address=@address,  added_date=@added_date ,added_by=@added_by WHERE id=@id";

                //Creating SQL command using SQL and conn(Creating a sql command to pass values into the query)
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Creating parameters to add data
                cmd.Parameters.AddWithValue("@type", dc.type);
                cmd.Parameters.AddWithValue("@name", dc.name);
                cmd.Parameters.AddWithValue("@email", dc.email);
                cmd.Parameters.AddWithValue("@contact", dc.contact);
                cmd.Parameters.AddWithValue("@address", dc.address);
                cmd.Parameters.AddWithValue("@added_date", dc.added_date);
                cmd.Parameters.AddWithValue("@added_by", dc.added_by);
                cmd.Parameters.AddWithValue("@id", dc.id);

                //Connection Open Here
                conn.Open();

                //Craeting the int variable to execute query
                int rows = cmd.ExecuteNonQuery();

                //If the query runs successfully then the value of rows will be greater than zero else its value will be zero
                if (rows > 0)
                {
                    //Query Successful
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

        #region Delete data from database

        public bool Delete(DeaCustBLL dc)
        {
            //create a default return value and set its value to false
            bool isSuccess = false;
            //create SQL connection
            SqlConnection conn = new SqlConnection(myconnstring);
            try
            {
                //SQL to delete data
                string sql = "DELETE FROM tbl_dea_cust WHERE id=@id";
                //Creating SQL command using SQL and conn
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", dc.id);
                //open connection
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                //If the query runs successfully then the value of rows will be greater than zero else its value will be zero
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
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

        #region Search method to dealer and customer module

        public DataTable Search(String keywords)
        {
            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstring);

            //Creating Table to hold data from database
            DataTable dt = new DataTable();
            try
            {
                //Writing SQL Query to select all the data from data base
                string sql = "SELECT * FROM tbl_dea_cust WHERE id Like '%" + keywords + "%' OR type Like '%" + keywords + "%' OR name Like '%" + keywords + "%'";
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

        #region Method to search dealer or customer for transaction module

        public DeaCustBLL SearchDealerCustomerForTransaction(string keyword)
        {
            //create an object for DeaCustBLL class 
            DeaCustBLL dc = new DeaCustBLL();

            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstring);

            //Creating Table to hold data from database
            DataTable dt = new DataTable();

            try
            {
                //Writing SQL Query to search Dealer or customer based on keywords from data base
                string sql = "SELECT name,email,contact,address  FROM tbl_dea_cust WHERE id Like '%" + keyword + "%' OR name Like '%" + keyword + "%'";
                //Creating sql command using sql query and connection(For executing command)
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Creating SQL DataAdapter using command (we get data using sql dataadapter from data base)
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //Database connection open
                conn.Open();
                //Fill the data into the data table
                adapter.Fill(dt);
                //If we have values on dt we need to save it in dealercustomer BLL
                if(dt.Rows.Count>0)
                {
                    dc.name = dt.Rows[0]["name"].ToString();
                    dc.email = dt.Rows[0]["email"].ToString();
                    dc.contact = dt.Rows[0]["contact"].ToString();
                    dc.address = dt.Rows[0]["address"].ToString();

                }
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


            return dc;
        }

        #endregion

        #region Method to get the id of the dealer or customer based on name

        public DeaCustBLL GetDeaCustIDFromName(string Name)
        {
            //create an object for DeaCustBLL class 
            DeaCustBLL dc = new DeaCustBLL();

            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstring);

            //Creating Table to hold data from database
            DataTable dt = new DataTable();

            try
            {
                //Writing SQL Query to getid based on name
                string sql = "SELECT id FROM tbl_dea_cust WHERE name='"+Name+"'";
                //Creating sql command using sql query and connection(For executing command)
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Creating SQL DataAdapter using command (we get data using sql dataadapter from data base)
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //Database connection open
                conn.Open();
                //Fill the data into the data table
                adapter.Fill(dt);
                //If we have values on dt we need to save it in dealercustomer BLL
                if (dt.Rows.Count > 0)
                {
                    dc.id = int.Parse(dt.Rows[0]["id"].ToString());
                    
                }
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


            return dc;
        }

        #endregion

    }
}
