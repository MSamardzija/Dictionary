using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DictionaryWarCoDex
{
    public partial class Login : Window
    {
        readonly WarcoDictionaryDataContext DC = new WarcoDictionaryDataContext();
        public Login()
        {
            InitializeComponent();
            tbUsername.Focus();
        }

        private void BtnCancle_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbUsername.Text) && !String.IsNullOrWhiteSpace(PasswordBox1.Password))
            {
                try
                {
                    var pom = (from s in DC.Users
                               where s.Username == tbUsername.Text
                               && s.Password == PasswordBox1.Password
                               select s).First();
                    if (pom != null)
                    {

                        LoginUserData.UserNameLOG = pom.Username;
                        LoginUserData.UseridLOG = pom.UserID;
                        LoginUserData.UserPassLOG = pom.Password;
                        MainWindow novi = new MainWindow();
                        novi.Show();
                        this.Close();
                    }
                    else
                        MessageBox.Show("Your username and password do not match", "Check your input", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception)
                {
                    MessageBox.Show("Username and password do not match", "Check your input", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please fill up login form.");
                if (String.IsNullOrEmpty(tbUsername.Text))
                    tbUsername.Focus();
                else if (String.IsNullOrEmpty(PasswordBox1.Password))
                    PasswordBox1.Focus();
            }
        }

        private void BtnRestartPassword_Click(object sender, RoutedEventArgs e)
        {
            SingInProblems singIn = new SingInProblems();
            singIn.ShowDialog();
        }

        private void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            NewUserRegistration main = new NewUserRegistration();
            main.ShowDialog();
        }
    }
}
