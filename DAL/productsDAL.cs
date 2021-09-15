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
    class productsDAL
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
                string sql = "SELECT * FROM tbl_products";
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
        public bool Insert(productsBLL p)
        {
            //Creating a default return type(boolean value) and setting its value to false
            bool isSuccess = false;

            //Connect database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //Create a SQL Query to insert Data(writhing query to  add new category)
                string sql = "INSERT INTO tbl_products (name,category,description,rate,qty,added_date,added_by) VALUES(@name, @category,@description,@rate,@qty,@added_date,@added_by)";

                //Creating a sql command to pass values into the query
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Creating parameters to add data
                cmd.Parameters.AddWithValue("@name", p.name);
                cmd.Parameters.AddWithValue("@category", p.category);
                cmd.Parameters.AddWithValue("@description", p.description);
                cmd.Parameters.AddWithValue("@rate", p.rate);
                cmd.Parameters.AddWithValue("@qty", p.qty);
                cmd.Parameters.AddWithValue("@added_date", p.added_date);
                cmd.Parameters.AddWithValue("@added_by", p.added_by);

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

        public bool Update(productsBLL p)
        {
            //Create a default return type and set its default value false
            bool isSuccess = false;

            //Connect database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {

                //SQL to update data in our Database
                string sql = "Update tbl_products SET name=@name, category=@category, description=@description,rate=@rate, qty=@qty,  added_date=@added_date ,added_by=@added_by WHERE id=@id";

                //Creating SQL command using SQL and conn(Creating a sql command to pass values into the query)
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Creating parameters to add data
                cmd.Parameters.AddWithValue("@name", p.name);
                cmd.Parameters.AddWithValue("@category", p.category);
                cmd.Parameters.AddWithValue("@description", p.description);
                cmd.Parameters.AddWithValue("@rate", p.rate);
                cmd.Parameters.AddWithValue("@qty", p.qty);
                cmd.Parameters.AddWithValue("@added_date", p.added_date);
                cmd.Parameters.AddWithValue("@added_by", p.added_by);
                cmd.Parameters.AddWithValue("@id", p.id);

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

        public bool Delete(productsBLL p)
        {
            //create a default return value and set its value to false
            bool isSuccess = false;

            //create SQL connection
            SqlConnection conn = new SqlConnection(myconnstring);
            try
            {
                //SQL to delete data
                string sql = "DELETE FROM tbl_products WHERE id=@id";

                //Creating SQL command using SQL and conn
                SqlCommand cmd = new SqlCommand(sql, conn);

                //passing the values using cmd
                cmd.Parameters.AddWithValue("@id", p.id);

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


        #region Search Products on DataBase using Keywords

        public DataTable Search(String keywords)
        {
            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstring);
            //Creating Table to hold data from database
            DataTable dt = new DataTable();
            try
            {
                //Writing SQL Query to select all the data from data base
                string sql = "SELECT * FROM tbl_products WHERE id Like '%" + keywords + "%' OR name Like '%" + keywords + "%' OR category Like '%" + keywords + "%'";
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


        #region Method to search product in transaction module

        public productsBLL SearchProductsForTransaction(string keyword)
        {
            //create an object for productsBLL class 
            productsBLL p = new productsBLL();

            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstring);

            //Creating Table to hold data from database
            DataTable dt = new DataTable();

            try
            {
                //Writing SQL Query to search products based on keywords from data base
                string sql = "SELECT name, rate, qty  FROM tbl_products WHERE id Like '%" + keyword + "%' OR name Like '%" + keyword + "%'";
                //Creating sql command using sql query and connection(For executing command)
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Creating SQL DataAdapter using command (we get data using sql dataadapter from data base)
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //Database connection open
                conn.Open();
                //Fill the data into the data table
                adapter.Fill(dt);
                //If we have values on dt we need to save it in productsBLL
                if (dt.Rows.Count > 0)
                {
                    p.name = dt.Rows[0]["name"].ToString();
                    p.rate = decimal.Parse(dt.Rows[0]["rate"].ToString());
                    p.qty = decimal.Parse(dt.Rows[0]["qty"].ToString());
                   

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


            return p;
        }

        #endregion


        #region Method to get the id of the product based on name

        public productsBLL GetProductIDFromName(string ProductName)
        {
            //create an object for DeaCustBLL class 
            productsBLL p = new productsBLL();

            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstring);

            //Creating Table to hold data from database
            DataTable dt = new DataTable();

            try
            {
                //Writing SQL Query to getid based on name
                string sql = "SELECT id FROM tbl_products WHERE name='" + ProductName + "'";
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
                    p.id = int.Parse(dt.Rows[0]["id"].ToString());

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


            return p;
        }

        #endregion


        #region Method to get current id  quantity from the data base based on product ID

        public decimal GetProductQty(int ProductID)
        {
            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstring);

            //Create a decimal variable and set its default value to 0
            decimal qty = 0;

            //Create Data Table to save the data from database temporarily                        
            DataTable dt = new DataTable();

            try
            {
                //Writing SQL Query to search products based on keywords from data base
                string sql = "SELECT qty FROM tbl_products WHERE id = " + ProductID;
                //Creating sql command using sql query and connection(For executing command)
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Creating SQL DataAdapter using command (we get data using sql dataadapter from data base)
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                //Database connection open
                conn.Open();
                //Fill the data into the data table from data adapter
                adapter.Fill(dt);
                //If we have values on data table we need to save it in qty
                if (dt.Rows.Count > 0)
                {
                    
                    qty = decimal.Parse(dt.Rows[0]["qty"].ToString());


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


            return qty;
        }

        #endregion 


        #region Method to update quantity

        public bool UpdateQuantity(int ProductID , decimal Qty)
        {
            //Create a default return type(a boolean variable) and set its default value false
            bool isSuccess = false;

            //Connect database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {

                //SQL to update data in our Database
                string sql = "Update tbl_products SET qty = @qty WHERE id=@id";

                //Creating SQL command using SQL and conn(Creating a sql command to pass values into the query)
                SqlCommand cmd = new SqlCommand(sql, conn);

                //Creating parameters to add data
                
                cmd.Parameters.AddWithValue("@qty", Qty);               
                cmd.Parameters.AddWithValue("@id", ProductID);

                //Connection Open Here
                conn.Open();

                //Craeting the int variable to check whether the query is executed Successfully or not
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


        #region Method to increase product

        public bool IncreaseProduct(int ProductID, decimal IncreaseQty)
        {
            //Create a default return type(a boolean variable) and set its default value false
            bool Success = false;

            //Connect database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {

                //Get the Current Qty From database based on id
                decimal currentQty = GetProductQty(ProductID);

                //Increase the current Quantity by the qty purchased from Dealer 
                decimal NewQty = currentQty + IncreaseQty;

                //Update the producty quantity now
                Success = UpdateQuantity(ProductID, NewQty);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return Success;
        }


        #endregion


        #region Method to decrease product

        public bool DecreaseProduct(int ProductID, decimal Qty)
        {
            //Create a default return type(a boolean variable) and set its default value false
            bool Success = false;

            //Connect database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                //Get the Current Qty From database based on id
                decimal currentQty = GetProductQty(ProductID);

                //Decrease the current Quantity by the qty purchased from Dealer 
                decimal NewQty = currentQty - Qty;

                //Update the producty quantity now
                Success = UpdateQuantity(ProductID, NewQty);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return Success;
        }


        #endregion


        #region DISPLAY PRODUCTS BASED ON CATEGORIES

        public DataTable DisplayProductsByCategory(string category)
        {
            //sql connection first
            SqlConnection conn = new SqlConnection(myconnstring);
            DataTable dt = new DataTable();

            try
            {
                //Writing SQL Query to display product based on category
                string sql = "SELECT * FROM tbl_products WHERE category = '"+ category + "'";
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
