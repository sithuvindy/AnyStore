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
    class CategoriesDAL
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
                string sql = "SELECT * FROM tbl_categories";
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
        public bool Insert(CategorieBLL c)
        {
            //Creating a default return type(boolean value) and setting its value to false
            bool isSuccess = false;

            //Connect database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //Create a SQL Query to insert Data(writhing query to  add new category)
                string sql = "INSERT INTO tbl_categories (title,description,added_date,added_by) VALUES(@title,@description,@added_date,@added_by)";
                
                //Creating a sql command to pass values into the query
                SqlCommand cmd = new SqlCommand(sql, conn);
                
                //Creating parameters to add data
                cmd.Parameters.AddWithValue("@title", c.title);
                cmd.Parameters.AddWithValue("@description", c.description);
                cmd.Parameters.AddWithValue("@added_date", c.added_date);
                cmd.Parameters.AddWithValue("@added_by", c.added_by);
                
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

        #region Update data in the Database

        public bool Update(CategorieBLL c)
        {
            //Create a default return type and set its default value false
            bool isSuccess = false;

            //Connect database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {

                //SQL to update data in our Database
                string sql = "Update tbl_categories SET title=@title, description=@description, added_date=@added_date ,added_by=@added_by WHERE id=@id";

                //Creating SQL command using SQL and conn(Creating a sql command to pass values into the query)
                SqlCommand cmd = new SqlCommand(sql, conn);
                
                //Creating parameters to add data
                cmd.Parameters.AddWithValue("@title", c.title);
                cmd.Parameters.AddWithValue("@description", c.description);
                cmd.Parameters.AddWithValue("@added_date", c.added_date);
                cmd.Parameters.AddWithValue("@added_by", c.added_by);
                cmd.Parameters.AddWithValue("@id", c.id);

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

        public bool Delete(CategorieBLL c)
        {
            //create a default return value and set its value to false
            bool isSuccess = false;

            //create SQL connection
            SqlConnection conn = new SqlConnection(myconnstring);
            try
            {
                //SQL to delete data
                string sql = "DELETE FROM tbl_categories WHERE id=@id";

                //Creating SQL command using SQL and conn
                SqlCommand cmd = new SqlCommand(sql, conn);

                //passing the values using cmd
                cmd.Parameters.AddWithValue("@id", c.id);

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

        #region Search Categories on DataBase using Keywords

        public DataTable Search(String keywords)
        {
            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstring);
            //Creating Table to hold data from database
            DataTable dt = new DataTable();
            try
            {
                //Writing SQL Query to select all the data from data base
                string sql = "SELECT * FROM tbl_categories WHERE id Like '%" + keywords + "%' OR title Like '%" + keywords + "%' OR description Like '%" + keywords + "%'";
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
