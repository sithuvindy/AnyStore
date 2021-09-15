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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        loginBLL l = new loginBLL();
        loginDAL dal = new loginDAL();
        public static string loggedIn;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //Code to close this form
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            l.username = txtUsername.Text.Trim();
            l.password = txtPassword.Text.Trim();
            l.user_type = cmbUsertype.Text.Trim();

            //Checking the login credentials
            bool success = dal.loginCheck(l);
            if (success==true)
            {
                //login successfull
                MessageBox.Show("Login Successful");
                loggedIn = l.username;
                //need to open respective forms based on user type
                switch(l.user_type)
                {
                    case "Admin":
                        {
                            frmAdminDashBoard admin = new frmAdminDashBoard();
                            admin.Show();
                            this.Hide();
                        }
                        break;
                    case "User":
                        {
                            frmUserDashBoard user = new frmUserDashBoard();
                            user.Show();
                            this.Hide();
                        }
                        break;
                    default:
                        {
                            //Display an error message
                            MessageBox.Show("Invalid User type");
                        }
                        break;
                }
            }
            else
            {
                //login failed
                MessageBox.Show("Login Failed!!!");
            }
        }
    }
}
