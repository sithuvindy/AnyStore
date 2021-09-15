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
    public partial class frmCategories : Form
    {
        public frmCategories()
        {
            InitializeComponent();
        }

        private void lblTop_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        CategorieBLL c = new CategorieBLL();
        CategoriesDAL dal = new CategoriesDAL();
        userDAL udal = new userDAL(); 
        private void btnAdd_Click(object sender, EventArgs e)
        {

            //getting values from Category form (input fields)
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;

            //Getting ID in Added_by field(getting the username of the logged in user)
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            //Passing the id of Logged in user in added by field
            c.added_by = usr.id;

            //Inserting data into DataBase using method we created by creating a bool value
            bool success = dal.Insert(c);
            //If the data is successfully inserted then the value of Success will be true
            if (success == true)
            {
                //Successfully Inserted data
                MessageBox.Show("New Category Successfully Inserted");
                Clear();
                //Load Data on Data Grid view (Refreshing Data Grid View)
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
            else
            {
                //Failed to insert data
                MessageBox.Show("Failed to Add new Category. Try Again.");
            }
            
        }

        public void Clear()
        {
            txtCategoryID.Text = "";
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtSearch.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Getting Data from UI(get values from input fields)
            c.id = Convert.ToInt32(txtCategoryID.Text);
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;           
            c.added_date = DateTime.Now;

            //Getting ID in Added_by field(getting the username of the logged in user)
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            //Passing the id of Logged in user in added by field
            c.added_by = usr.id;

            //Update DATA in database
            bool success = dal.Update(c);
            //If the data is updated successfully then the value of success will be true else it will be false
            if (success == true)
            {
                //Data Update Successfully
                MessageBox.Show("Category has been successfully updated.");

                //Call clear method
                Clear();

                //Load Data on Data Grid view (Refreshing Data Grid View)
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;

            }
            else
            {
                //Failed to update user 
                MessageBox.Show("Failed to update the user");
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //get the ID of the category which we want to delete from the application
            c.id = Convert.ToInt32(txtCategoryID.Text);

            //Creating a boolean variable to delete the category
            bool success = dal.Delete(c);
            if (success == true)
            {
                //Successsfully delete
                MessageBox.Show("Category successfully deleted.");

                //Call the clear method here
                Clear();

            }
            else
            {
                //failed to delete
                MessageBox.Show("Failed to delete. Try again!");

            }
            //Load Data on Data Grid view (Refreshing Data Grid View)
            DataTable dt = dal.Select();
            dgvCategories.DataSource = dt;
        }

        private void frmCategories_Load(object sender, EventArgs e)
        {
            //Load Data on Data Grid view (Refreshing Data Grid View)
            DataTable dt = dal.Select();
            dgvCategories.DataSource = dt;
        }

        private void dgvCategories_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //get the index of the particular row
            int rowIndex = e.RowIndex;
            txtCategoryID.Text = dgvCategories.Rows[rowIndex].Cells[0].Value.ToString();
            txtTitle.Text = dgvCategories.Rows[rowIndex].Cells[1].Value.ToString();
            txtDescription.Text = dgvCategories.Rows[rowIndex].Cells[2].Value.ToString();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //get keyword from textbox
            string keywords = txtSearch.Text;
            //Check if the keywords has value or not
            if (keywords != null)
            {
                //show categories based on keywords
                DataTable dt = dal.Search(keywords);
                dgvCategories.DataSource = dt;
            }
            else
            {
                // show all categories from the data base
                DataTable dt = dal.Select();
                dgvCategories.DataSource = dt;
            }
        }
    }
}
