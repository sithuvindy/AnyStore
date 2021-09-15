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
using System.Transactions;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmPurchaseAndSale : Form
    {
        public frmPurchaseAndSale()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        DeaCustDAL dcDAL = new DeaCustDAL();
        productsDAL pDAL = new productsDAL();
        userDAL uDAL = new userDAL();
        transactionDAL tDAL = new transactionDAL();
        transactionDetailDAL tdDAL = new transactionDetailDAL();

        DataTable transactionDT = new DataTable();
        private void frmPurchaseAndSale_Load(object sender, EventArgs e)
        {
            //get the transactionType value from frmUserDashboard
            string type = frmUserDashBoard.transactionType;
            //set the value on lblTop
            lblTop.Text = type;

            //Specify columns for our transaction table
            transactionDT.Columns.Add("Product Name");
            transactionDT.Columns.Add("Rate");
            transactionDT.Columns.Add("Quantity");
            transactionDT.Columns.Add("Total");
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //get the keyword from the textbox
            string keyword = txtSearch.Text;

            if(keyword=="")
            {
                //clear all the textboxes
                txtName.Text = "";
                txtEmail.Text = "";
                txtContact.Text = "";
                txtAdress.Text = "";
                return;
            }
            //write the code to get the details and set the value on text box
            DeaCustBLL dc = dcDAL.SearchDealerCustomerForTransaction(keyword);

            //now transfer or set the value from DeaCustBLL to textboxes
            txtName.Text = dc.name;
            txtEmail.Text = dc.email;
            txtContact.Text = dc.contact;
            txtAdress.Text = dc.address;

        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            //get the keyword from the textbox
            string keyword = txtSearchProduct.Text;

            if (keyword == "")
            {
                //clear all the textboxes
                txtProductName.Text = "";
                txtInventory.Text = "";
                txtRate.Text = "";
                txtQty.Text = "";
                return;
            }
            //write the code to get the details and set the value on text box
            productsBLL p = pDAL.SearchProductsForTransaction(keyword);

            //now transfer or set the value from DeaCustBLL to textboxes
            txtProductName.Text = p.name;
            txtInventory.Text = p.qty.ToString();
            txtRate.Text = p.rate.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Get product name , rate , and qty customer wants to buy
            string productName = txtProductName.Text;
            decimal Rate = decimal.Parse(txtRate.Text);
            decimal Qty = decimal.Parse(txtQty.Text);

            decimal Total = Rate * Qty;

            //display the sub total in text box
            //get the subtotal value from text box
            decimal subTotal = decimal.Parse(txtSubTotal.Text);
            subTotal = subTotal + Total;

            //Check whether the product is selected or not
            if(productName=="")
            {
                //Display error message
                MessageBox.Show("Select the product first. Try again!");
            }
            else
            {
                //Add product to the data grid view 
                transactionDT.Rows.Add(productName,Rate,Qty,Total);

                //Show in Data Grid View
                dgvAddedProducts.DataSource = transactionDT;
                //Display the subtotal in textbox
                txtSubTotal.Text = subTotal.ToString();

                //clear the textboxes
                txtSearchProduct.Text = "";
                txtProductName.Text = "";
                txtInventory.Text = "0.00";
                txtRate.Text = "0.00";
                txtQty.Text = "0.00";
            }
            
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            //get the value from discount textbox
            string value = txtDiscount.Text;

            if(value=="")
            {
                //Display error message
                MessageBox.Show("Please add discount first");
            }
            else
            {
                //Get the discount in decimal value
                decimal SubTotal = decimal.Parse(txtSubTotal.Text);
                decimal discount = decimal.Parse(txtDiscount.Text);

                //Calculate the grand total based on discount
                decimal grandTotal = ((100 - discount ) / 100) * SubTotal;

                //Display the grandTotal in TextBox
                txtGrandTotal.Text = grandTotal.ToString();
            }
        }

        private void txtVAT_TextChanged(object sender, EventArgs e)
        {
            //check if the grand total has a value or not if it doesn't have the value then calculate the discount first
            string check = txtGrandTotal.Text;
            if(check =="")
            {
                //display the error message to calculate discount
                MessageBox.Show("Calculate the discount and set the grand total first.");
            }
            else
            {
                //calculate VAT
                //Geting the Vat precent first
                decimal previousGT = decimal.Parse(txtGrandTotal.Text);
                decimal vat = decimal.Parse(txtVAT.Text);
                decimal grandTotalWithVAT = ((100 + vat) / 100) * previousGT;

                //displaying the new grandtotal with vat
                txtGrandTotal.Text = grandTotalWithVAT.ToString();
            }
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            //get the paid amount and the grand total
            decimal grandTotal = decimal.Parse(txtGrandTotal.Text);
            decimal paidAmount = decimal.Parse(txtPaidAmount.Text);

            decimal returnAmount = paidAmount - grandTotal;

            //display the return amount as well
            txtReturnAmount.Text = returnAmount.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                //Get the values from purchase form first
                transactionsBLL transaction = new transactionsBLL();

                transaction.type = lblTop.Text;

                //Get the ID of dealer or customer here
                //let's get name of the dealer or customer first
                string deaCustName = txtName.Text;
                DeaCustBLL dc = dcDAL.GetDeaCustIDFromName(deaCustName);

                transaction.dea_cust_id = dc.id;
                transaction.grandtotal = Math.Round(decimal.Parse(txtGrandTotal.Text), 2);
                transaction.transaction_date = DateTime.Now;
                transaction.tax = decimal.Parse(txtVAT.Text);
                transaction.discount = decimal.Parse(txtDiscount.Text);

                //get the user name of logged in user 
                string username = frmLogin.loggedIn;
                userBLL u = uDAL.GetIDFromUsername(username);

                transaction.added_by = u.id;
                transaction.transactionDetails = transactionDT;

                //let's Create a Boolena varaible and set its value to false
                bool success = false;

                //Actual code to insert transaction and transaction details
                using (TransactionScope scope = new TransactionScope())
                {
                    int transactionID = -1;
                    //Create a boolean value and insert transaction
                    bool w = tDAL.Insert_Transaction(transaction, out transactionID);

                    //use for loop to insert transaction details
                    for (int i = 0; i < transactionDT.Rows.Count; i++)
                    {
                        //Get all the details of the product
                        transactionDetailBLL transactionDetail = new transactionDetailBLL();
                        //Get the product name and convert it to id
                        string ProductName = transactionDT.Rows[i][0].ToString();
                        productsBLL p = pDAL.GetProductIDFromName(ProductName);

                        transactionDetail.product_id = p.id;
                        transactionDetail.rate = decimal.Parse(transactionDT.Rows[i][1].ToString());
                        transactionDetail.qty = decimal.Parse(transactionDT.Rows[i][2].ToString());
                        transactionDetail.total = Math.Round(decimal.Parse(transactionDT.Rows[i][3].ToString()), 2);
                        transactionDetail.dea_cust_id = dc.id;
                        transactionDetail.added_date = DateTime.Now;
                        transactionDetail.added_by = u.id;

                        //Here Increase or Decrease Product Quantity based on Purchase or sales
                        string transactionType = lblTop.Text;

                        //Let's check whether we are on Purchase or Sales 
                        bool x = false;
                        if (transactionType == "Purchase")
                        {
                            //Increase the product
                            x = pDAL.IncreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                        }
                        else if(transactionType == "Sales")
                        {
                            //Decrease the Product Quntity
                            x = pDAL.DecreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                        }

                        //Insert transaction details inside the database
                        bool y = tdDAL.InsertTransactionDetail(transactionDetail);
                        success = w && x && y;

                    }

                    if (success == true)
                    {
                        //Transaction complete
                        scope.Complete();
                        MessageBox.Show("Transaction Completed Successfully");
                        //Clear the data grid view and clear all the textboxes
                        dgvAddedProducts.DataSource = null;
                        dgvAddedProducts.Rows.Clear();

                        txtSearch.Text = "";
                        txtName.Text = "";
                        txtEmail.Text = "";
                        txtContact.Text = "";
                        txtAdress.Text = "";
                        txtSearchProduct.Text = "";
                        txtProductName.Text = "";
                        txtInventory.Text = "0";
                        txtRate.Text = "0";
                        txtQty.Text = "0";
                        txtSubTotal.Text = "0";
                        txtDiscount.Text = "0";
                        txtVAT.Text = "0";
                        txtGrandTotal.Text = "0";
                        txtPaidAmount.Text = "0";
                    }
                    else
                    {
                        //Transaction failed
                        MessageBox.Show("Transaction failed!!");
                    }



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Transaction failed!!");
               
            }
           

        }
    }
}
