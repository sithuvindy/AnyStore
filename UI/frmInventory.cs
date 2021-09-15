﻿using AnyStore.DAL;
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
    public partial class frmInventory : Form
    {
        public frmInventory()
        {
            InitializeComponent();
        }
        CategoriesDAL cdal = new CategoriesDAL();
        productsDAL pdal = new productsDAL();
        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void frmInventory_Load(object sender, EventArgs e)
        {
            //Display the category in combo box
            DataTable cDt = cdal.Select();

            cmbCategories.DataSource = cDt;

            //Give the value member and display member for combo box
            cmbCategories.DisplayMember = "title";
            cmbCategories.ValueMember = "title";

            //Display all the products in data grid view when the form is loaded
            DataTable pdt = pdal.Select();
            dgvProducts.DataSource = pdt;


        }

        private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Display all the products based on selected category
            string category = cmbCategories.Text;
            DataTable dt = pdal.DisplayProductsByCategory(category);
            dgvProducts.DataSource = dt;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            //Display all the products when this button is clicked
            DataTable dt = pdal.Select();
            dgvProducts.DataSource = dt;
        }
    }
}
