using AnyStore.BLL;
using AnyStore.DAL;
using EncryptionandDecryptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmUsers : Form
    {
        public frmUsers()
        {
            InitializeComponent();
        }

        //creating an object to userBLL.cs class
        userBLL u = new userBLL();
        //creating an object to userDAL.cs class
        userDAL dal = new userDAL();

       

        private void lblTop_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblContact_Click(object sender, EventArgs e)
        {

        }

        private void txtContact_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'anyStoreDataSet.tbl_users' table. You can move, or remove it, as needed.
            this.tbl_usersTableAdapter.Fill(this.anyStoreDataSet.tbl_users);
            //Load Data on Data Grid view (Refreshing Data Grid View)
            //DataTable dt = dal.Select();
            //dgvUsers.DataSource = dt;

            //Call this method of header checkbox mouse click..
            //first add header checkbox than mouseclick. without checkbox what will u click?

            show_chkBox();

            //HeaderCheckBox.MouseClick += new MouseEventHandler(HeaderCheckBox_MouseClick);

            //Load Data on Data Grid view (Refreshing Data Grid View)
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            //Getting Data from UI(get values from input fields)
            u.firstname = txtFirstName.Text;
            u.lastname = txtLastName.Text;
            u.email = txtEmail.Text;
            u.username = txtUsername.Text;
            u.password = txtPassword.Text;
            u.confirm_password = txtConfirmPassword.Text;
            u.contact = txtContact.Text;
            u.address = txtAdress.Text;
            u.gender = cmbGender.Text;
            u.user_type = cmbUserType.Text;
            u.added_date = DateTime.Now;

            //getting the username of the logged in user
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = dal.GetIDFromUsername(loggedUser);
            u.added_by = usr.id;

            

            //Inserting data into DataBase using method we created
            bool success = dal.Insert(u);
            //If the data is successfully inserted then the value of Success will be true
            if (success == true)
            {
                              
                //Successfully Inserted data
                MessageBox.Show("New User Successfully Inserted");                
                clear();
            }
            else
            {
                //Failed to insert data
                MessageBox.Show("Failed to Add new user. Try Again.");
            }
            //Load Data on Data Grid view (Refreshing Data Grid View)
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;

        }
        //Method to Clear Fields
        private void clear()
        {
            txtUserID.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtContact.Text = "";
            txtAdress.Text = "";
            cmbGender.Text = "";
            cmbUserType.Text = "";            
        }

        private void dgvUsers_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //get the index of the particular row
            int rowIndex = e.RowIndex;

            txtUserID.Text = dgvUsers.Rows[rowIndex].Cells[1].Value.ToString();
            txtFirstName.Text = dgvUsers.Rows[rowIndex].Cells[2].Value.ToString();
            txtLastName.Text = dgvUsers.Rows[rowIndex].Cells[3].Value.ToString();
            txtEmail.Text = dgvUsers.Rows[rowIndex].Cells[4].Value.ToString();
            txtUsername.Text = dgvUsers.Rows[rowIndex].Cells[5].Value.ToString();
            txtPassword.Text = dgvUsers.Rows[rowIndex].Cells[6].Value.ToString();
            txtConfirmPassword.Text = dgvUsers.Rows[rowIndex].Cells[7].Value.ToString();
            txtContact.Text = dgvUsers.Rows[rowIndex].Cells[8].Value.ToString();
            txtAdress.Text = dgvUsers.Rows[rowIndex].Cells[9].Value.ToString();
            cmbGender.Text = dgvUsers.Rows[rowIndex].Cells[10].Value.ToString();
            cmbUserType.Text = dgvUsers.Rows[rowIndex].Cells[11].Value.ToString();


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Getting Data from UI(get values from input fields)
            u.id = Convert.ToInt32(txtUserID.Text);
            u.firstname = txtFirstName.Text;
            u.lastname = txtLastName.Text;
            u.email = txtEmail.Text;
            u.username = txtUsername.Text;
            u.password = txtPassword.Text;
            u.contact = txtContact.Text;
            u.address = txtAdress.Text;
            u.gender = cmbGender.Text;
            u.user_type = cmbUserType.Text;
            u.added_date = DateTime.Now;
            u.added_by = 1;

            //Update DATA in database
            bool success = dal.Update(u);
            //If the data is updated successfully then the value of success will be true else it will be false
            if (success == true)
            {
                //Data Update Successfully
                MessageBox.Show("User has been successfully updated.");
                               
                //Call clear method
                clear();
            }
            else
            {
                //Failed to update user 
                MessageBox.Show("Failed to update the user");
            }
            //Load Data on Data Grid view (Refreshing Data Grid View)
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<string> selectedItem = new List<string>();
            DataGridViewRow drow = new DataGridViewRow();
            for (int i = 0; i <= dgvUsers.Rows.Count - 1; i++)
            {
                drow = dgvUsers.Rows[i];
                if (Convert.ToBoolean(drow.Cells[0].Value) == true) //checking if  checked or not.  
                {
                    string id = drow.Cells[1].Value.ToString();
                    selectedItem.Add(id); //If checked adding it to the list  
                }
            }

            if (DialogResult.Yes == MessageBox.Show("Do You Want to Delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                string myconnstring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
                SqlConnection conn = new SqlConnection(myconnstring);

                conn.Open();

                foreach (string s in selectedItem) //using foreach loop to delete the records stored in list.  
                {
                    SqlCommand cmd = new SqlCommand("delete from tbl_users where id='" + s + "'", conn);
                    int result = cmd.ExecuteNonQuery();
                }

                conn.Close();

            }
            
            //Load Data on Data Grid view (Refreshing Data Grid View)
            DataTable dt = dal.Select();
            dgvUsers.DataSource = dt;
        }

        private void show_chkBox()
        {
            Rectangle rect = dgvUsers.GetCellDisplayRectangle(0, -1 , true);
            // set checkbox header to center of header cell. +1 pixel to position 
            rect.Y = 2;
            rect.X = rect.Location.X + (rect.Width / 3);
            CheckBox checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";
            //dgvUsers[0, 0].ToolTipText = "sdfsdf";
            checkboxHeader.Size = new Size(18, 18);
            checkboxHeader.Location = rect.Location;         
            checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);
            dgvUsers.Controls.Add(checkboxHeader);
        }
        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerBox = ((CheckBox)dgvUsers.Controls.Find("checkboxHeader", true)[0]);
            
            for (int i = 0 ; i < dgvUsers.RowCount; i++)
            {
                dgvUsers.Rows[i].Cells[0].Value = headerBox.Checked;
            }
            
        }
        ////Adding a header check box
        //CheckBox HeaderCheckBox = null;
        ////bool IsHeaderCheckBoxClicked = false;
        //private void AddheaderCheckBox()
        //{
        //    HeaderCheckBox = new CheckBox();
        //    //HeaderCheckBox.Size = new Size(15, 15);

        //    //Add the CheckBox into the DataGridView
        //    this.dgvUsers.Controls.Add(HeaderCheckBox);
        //}


        //Header check box click event
        /*private void HeaderCheckBoxClick(CheckBox HCheckBox)
        {
            IsHeaderCheckBoxClicked = true;

            foreach(DataGridViewRow Row in dgvUsers.Rows)
            {
                DataGridViewCheckBoxCell checkBox = (Row.Cells["chk"] as DataGridViewCheckBoxCell);
                checkBox.Value = HCheckBox.Checked;
            }

            dgvUsers.RefreshEdit();

            //TotalCheckedCheckBoxes = HCheckBox.Checked ? TotalCheckBoxes : 0;

            IsHeaderCheckBoxClicked = false;
        }*/



        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //get keyword from textbox
            string keywords = txtSearch.Text;
            //Check if the keywords has value or not
            if (keywords!=null)
            {
                //show user based on keywords
                DataTable dt = dal.Search(keywords);
                dgvUsers.DataSource = dt;
            }
            else
            {
                // show all users from the data base
                DataTable dt = dal.Select();
                dgvUsers.DataSource = dt;
            }
        }

        
    }
}
