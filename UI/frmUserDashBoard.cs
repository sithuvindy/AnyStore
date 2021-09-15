using AnyStore.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore
{
    public partial class frmUserDashBoard : Form
    {
        public frmUserDashBoard()
        {
            InitializeComponent();
        }

        //set a public static method to specify whether the form is purchase or sales
        public static string transactionType;

        private void frmUserDashBoard_Load(object sender, EventArgs e)
        {
            lblLoggedInUser.Text = frmLogin.loggedIn;
        }

        private void frmUserDashBoard_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Hide();
        }

        private void dealerCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDeaCust DeaCust = new frmDeaCust();
            DeaCust.Show();
        }

       

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInventory inventory = new frmInventory();
            inventory.Show();
        }

        private void purchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //set value on transaction type static method
            transactionType = "Purchase";
            frmPurchaseAndSale purchase = new frmPurchaseAndSale();
            purchase.Show();
            
        }

        private void saleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //set value on transaction type static method
            transactionType = "Sales";
            frmPurchaseAndSale sales = new frmPurchaseAndSale();
            sales.Show();
           
        }
    }
}
