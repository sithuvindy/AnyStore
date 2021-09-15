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
    public partial class frmProducts : Form
    {
        public frmProducts()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        CategoriesDAL cdal = new CategoriesDAL();
        productsBLL p = new productsBLL();
        productsDAL pdal = new productsDAL();
        userDAL udal = new userDAL();
       
        
        private void frmProducts_Load(object sender, EventArgs e)
        {
            //Creating Data Table to Hold the categories from database
            DataTable categoriesDT = cdal.Select();
            //Specify Datasource for Category combobox
            cmbCategory.DataSource = categoriesDT;
            //Specify Display Member and Value Member for Combobox
            cmbCategory.DisplayMember = "title";
            cmbCategory.ValueMember = "title";

            //Load all the products in data grid view
            DataTable dt = pdal.Select();
            dgvProducts.DataSource = dt;
        }


        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Getting Data from UI(get values from input fields)
            p.name = txtName.Text;
            p.category = cmbCategory.Text;
            p.description = txtDescription.Text;
            p.rate = decimal.Parse(txtRate.Text);
            p.qty = 0;
            p.added_date = DateTime.Now;

            //getting the username of the logged in user
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            p.added_by = usr.id;

            //Inserting data into DataBase using method we created(creating a boolean variable to check if the product is added succesfully or not)
            bool success = pdal.Insert(p);
            //If the data(product) is successfully inserted then the value of Success will be true
            if (success == true)
            {
                //Successfully Inserted data
                MessageBox.Show("New Product Successfully Inserted");

                Clear();

                //Load Data on Data Grid view (Refreshing Data Grid View)
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
            else
            {
                //Failed to insert data
                MessageBox.Show("Failed to Add new product. Try Again.");
            }
            
        }
        //Method to Clear Fields
        private void Clear()
        {
            txtProductID.Text = "";
            txtName.Text = "";
            txtDescription.Text = "";
            txtRate.Text = "";
            txtSearch.Text = "";
           
        }

        private void dgvProducts_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //get the index of the particular row(create integer variable to know which product was clicked)
            int rowIndex = e.RowIndex;
            txtProductID.Text = dgvProducts.Rows[rowIndex].Cells[0].Value.ToString();
            txtName.Text = dgvProducts.Rows[rowIndex].Cells[1].Value.ToString();
            cmbCategory.Text = dgvProducts.Rows[rowIndex].Cells[2].Value.ToString();
            txtDescription.Text = dgvProducts.Rows[rowIndex].Cells[3].Value.ToString();
            txtRate.Text = dgvProducts.Rows[rowIndex].Cells[4].Value.ToString();           

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Getting Data from UI(get values from input fields)
            p.id = Convert.ToInt32(txtProductID.Text);
            p.name = txtName.Text;           
            p.category = cmbCategory.Text;
            p.description = txtDescription.Text;            
            p.rate = decimal.Parse(txtRate.Text);
            p.added_date = DateTime.Now;

            //Getting ID in Added_by field(getting the username of the logged in user)
            string loggedUser = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUser);
            //Passing the id of Logged in user in added by field
            p.added_by = usr.id;

            //Update DATA in database
            bool success = pdal.Update(p);
            //If the data is updated successfully then the value of success will be true else it will be false
            if (success == true)
            {
                //Data Update Successfully
                MessageBox.Show("Product has been successfully updated.");

                //Call clear method
                Clear();

                //Load Data on Data Grid view (Refreshing Data Grid View)
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;

            }
            else
            {
                //Failed to update user 
                MessageBox.Show("Failed to update the product");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //get the ID of the product which we want to delete from the application
            p.id = Convert.ToInt32(txtProductID.Text);

            //Creating a boolean variable to delete the category
            bool success = pdal.Delete(p);
            if (success == true)
            {
                //Successsfully delete
                MessageBox.Show("Product successfully deleted.");

                //Call the clear method here
                Clear();
                //Load Data on Data Grid view (Refreshing Data Grid View)
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;

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
                DataTable dt = pdal.Search(keywords);
                dgvProducts.DataSource = dt;
            }
            else
            {
                // show all products from the data base
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
        }
    }
}
