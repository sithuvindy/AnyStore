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
using System.Text.RegularExpressions;

namespace AnyStore.DAL
{
    class userDAL
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
                string sql = "SELECT * FROM tbl_users";
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
        //End region
        #endregion

        #region Insert Data in Database
        public bool Insert(userBLL u)
        {
            //Creating a default return type and setting its value to false
            bool isSuccess = false;

            //Connect database
            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {
                SqlCommand cmd = new SqlCommand("Select * from tbl_users where username = @user_name", conn);
                cmd.Parameters.AddWithValue("@user_name", u.username);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                
                if (dr.Read())
                {
                    
                     MessageBox.Show("username = " + dr[4].ToString() + " Already exist");           
                    
                }
                else
                {
                    dr.Close();

                    if (u.username != "" && u.password != "" && u.confirm_password != "" && u.email != "")
                    {
                        string email = u.email;
                        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                        Match match = regex.Match(email);
                        if (match.Success)
                        {
                            if (u.password.ToString().Trim().ToLower() == u.confirm_password.ToString().Trim().ToLower())
                            {

                                string Password = Cryptography.Encrypt(u.password);

                                //Create a SQL Query to insert Data
                                cmd.CommandText = "INSERT INTO tbl_users (first_name,last_name,email,username,password,confirm_password,contact,address,gender,user_type,added_date,added_by) VALUES(@first_name,@last_name,@email,@username,@password,@confirm_password,@contact,@address,@gender,@user_type,@added_date,@added_by)";
                                //string sql = "INSERT INTO tbl_users (first_name,last_name,email,username,password,contact,address,gender,user_type,added_date,added_by) VALUES(@first_name,@last_name,@email,@username,@password,@contact,@address,@gender,@user_type,@added_date,@added_by)";
                                //SqlCommand cmd = new SqlCommand(sql, conn);
                                //Creating parameters to add data
                                cmd.Parameters.AddWithValue("@first_name", u.firstname);
                                cmd.Parameters.AddWithValue("@last_name", u.lastname);
                                cmd.Parameters.AddWithValue("@email", u.email);
                                cmd.Parameters.AddWithValue("@username", u.username);
                                cmd.Parameters.AddWithValue("@Password", Password);
                                cmd.Parameters.AddWithValue("@confirm_password", Password);
                                cmd.Parameters.AddWithValue("@contact", u.contact);
                                cmd.Parameters.AddWithValue("@address", u.address);
                                cmd.Parameters.AddWithValue("@gender", u.gender);
                                cmd.Parameters.AddWithValue("@user_type", u.user_type);
                                cmd.Parameters.AddWithValue("@added_date", u.added_date);
                                cmd.Parameters.AddWithValue("@added_by", u.added_by);

                            }
                            else
                            {
                                MessageBox.Show("Password and Confirm Password doesn't match!! Please Check.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Email!","Error!",MessageBoxButtons.OK);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please fill all the fields!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);  //showing the error message if any fields is empty  
                    }

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

        public bool Update(userBLL u)
        {
            //Create a default return type and set its default value false
            bool isSuccess = false;

            SqlConnection conn = new SqlConnection(myconnstring);

            try
            {

                //SQL to update data in our Database
                string sql = "Update tbl_users SET first_name=@first_name, last_name=@last_name, email=@email, username=@username, password=@password, contact=@contact, address=@address, gender=@gender, user_type=@user_type, added_date=@added_date ,added_by=@added_by WHERE id=@id";
                //Creating SQL command using SQL and conn
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Creating parameters to add data
                cmd.Parameters.AddWithValue("@first_name", u.firstname);
                cmd.Parameters.AddWithValue("@last_name", u.lastname);
                cmd.Parameters.AddWithValue("@email", u.email);
                cmd.Parameters.AddWithValue("@username", u.username);
                cmd.Parameters.AddWithValue("@password", u.password);
                cmd.Parameters.AddWithValue("@contact", u.contact);
                cmd.Parameters.AddWithValue("@address", u.address);
                cmd.Parameters.AddWithValue("@gender", u.gender);
                cmd.Parameters.AddWithValue("@user_type", u.user_type);
                cmd.Parameters.AddWithValue("@added_date", u.added_date);
                cmd.Parameters.AddWithValue("@added_by", u.added_by);
                cmd.Parameters.AddWithValue("@id", u.id);

                //Connection Open Here
                conn.Open();
               
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

        /*public bool Delete(userBLL u)
        {
            
            //create a default return value and set its value to false
            bool isSuccess = false;
            //create SQL connection
            SqlConnection conn = new SqlConnection(myconnstring);
            try
            {
                //SQL to delete data
                string sql = "DELETE FROM tbl_users WHERE id=@id";
                //Creating SQL command using SQL and conn
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", u.id);
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
        }*/


        #endregion

        #region Search User on DataBase using Keywords

        public DataTable Search(String keywords)
        {
            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstring);
            //Creating Table to hold data from database
            DataTable dt = new DataTable();
            try
            {
                //Writing SQL Query to select all the data from data base
                string sql = "SELECT * FROM tbl_users WHERE id Like '%"+keywords+"%' OR first_name Like '%"+ keywords+"%' OR last_name Like '%"+keywords+ "%' OR username Like '%" + keywords + "%'";
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

        #region getting user id from username

        public userBLL GetIDFromUsername (string username)
        {
            userBLL u = new userBLL();
            //Creating Database Connection
            SqlConnection conn = new SqlConnection(myconnstring);
            //Creating Table to hold data from database
            DataTable dt = new DataTable();
            try
            {
                //Writing SQL Query to select all the data from data base
                string sql = "SELECT id FROM tbl_users WHERE username='"+username+"'";
                
                //Creating SQL DataAdapter using query and connection (we get data using sql dataadapter from data base)
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                //Database connection open
                conn.Open();
                //Fill the data into the data table
                adapter.Fill(dt);
                if(dt.Rows.Count>0)
                {
                    u.id = int.Parse(dt.Rows[0]["id"].ToString());
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
            return u;
        }

        #endregion

       
    }
}