using AnyStore.BLL;
using AnyStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmDeaCust : Form
    {
        public frmDeaCust()
        {
            InitializeComponent();
        }

        private void frmDeaCust_Load(object sender, EventArgs e)
        {
            //Load Data on Data Grid view (Refreshing Data Grid View)
            DataTable dt = dcdal.Select();
            dgvDeaCust.DataSource = dt;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            //Code to close this form
            this.Hide();
        }

        DeaCustBLL dc = new DeaCustBLL();
        DeaCustDAL dcdal = new DeaCustDAL();
        userDAL udal = new userDAL();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Getting Data from UI(get values from input fields)
            dc.type = cmbType.Text;
            dc.name = txtName.Text;
            dc.email = txtEmail.Text;
            dc.contact = txtContact.Text;
            dc.address = txtAdress.Text;
            dc.added_date = DateTime.Now;
            //getting the username of the logged in user
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            dc.added_by = usr.id;

            //Inserting data into DataBase using method we created
            bool success = dcdal.Insert(dc);
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
            DataTable dt = dcdal.Select();
            dgvDeaCust.DataSource = dt;

        }

        //Method to Clear Fields
        private void clear()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            txtAdress.Text = "";
            txtSearch.Text = "";
        }

        private void dgvDeaCust_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //get the index of the particular row
            int rowIndex = e.RowIndex;
            txtID.Text = dgvDeaCust.Rows[rowIndex].Cells[0].Value.ToString();
            cmbType.Text = dgvDeaCust.Rows[rowIndex].Cells[1].Value.ToString();
            txtName.Text = dgvDeaCust.Rows[rowIndex].Cells[2].Value.ToString();
            txtEmail.Text = dgvDeaCust.Rows[rowIndex].Cells[3].Value.ToString();           
            txtContact.Text = dgvDeaCust.Rows[rowIndex].Cells[4].Value.ToString();
            txtAdress.Text = dgvDeaCust.Rows[rowIndex].Cells[5].Value.ToString();
           
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Getting Data from UI(get values from input fields)
            dc.id = Convert.ToInt32(txtID.Text);
            dc.type = cmbType.Text;
            dc.name = txtName.Text;
            dc.email = txtEmail.Text;           
            dc.contact = txtContact.Text;
            dc.address = txtAdress.Text;           
            dc.added_date = DateTime.Now;
            //getting the username of the logged in user
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            dc.added_by = usr.id;

            //Update DATA in database
            bool success = dcdal.Update(dc);
            //If the data is updated successfully then the value of success will be true else it will be false
            if (success == true)
            {
                //Data Update Successfully
                MessageBox.Show("Dealer and customer has been successfully updated.");

                //Call clear method
                clear();
                //Load Data on Data Grid view (Refreshing Data Grid View)
                DataTable dt = dcdal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                //Failed to update user 
                MessageBox.Show("Failed to update the dealer and customer");
            }
           
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //get the User ID from the application
            dc.id = Convert.ToInt32(txtID.Text);

            //craete a boolen variable to check whethre the dealer or customer deleted
            bool success = dcdal.Delete(dc);

            if (success == true)
            {
                //Successsfully delete
                MessageBox.Show("Successfully deleted.");

                //Call the clear method here
                clear();
                //Load Data on Data Grid view (Refreshing Data Grid View)
                DataTable dt = dcdal.Select();
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                //failed to delete
                MessageBox.Show("Failed to delete. Try again!");

            }
            
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //get keyword from textbox
            string keywords = txtSearch.Text;
            //Check if the keywords has value or not
            if (keywords != null)
            {
                //show products based on keywords
                DataTable dt = dcdal.Search(keywords);
                dgvDeaCust.DataSource = dt;
            }
            else
            {
                // show all products from the data base
                DataTable dt = dcdal.Select();
                dgvDeaCust.DataSource = dt;
            }
        }
    }
}
