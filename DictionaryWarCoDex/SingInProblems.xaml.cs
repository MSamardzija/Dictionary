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
    
    public partial class SingInProblems : Window
    {
        readonly WarcoDictionaryDataContext DC = new WarcoDictionaryDataContext();
        public SingInProblems()
        {
            InitializeComponent();
        }
        User user = new User();

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(TbCheck.Text))
            {
                var pom = from s in DC.Users
                        where s.Username == TbCheck.Text
                        select s;
                if (pom.Count() > 0)
                {
                    MessageBox.Show("This username exists in our system\nYou can try to recover your account","Exists", MessageBoxButton.OK);
                    Grid1.Visibility = Visibility.Visible;
                    BtnCheck.Visibility = Visibility.Hidden;
                    TbCheck.IsReadOnly = true;
                    user = pom.First();
                }
                else
                    MessageBox.Show("Sorry but it seems that you are not in our system. \nYou can register new account !", "No results", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Please insert your username", "Username", MessageBoxButton.OK, MessageBoxImage.Information);
                TbCheck.Focus();
            }
        }

        private void BtnCheckALL_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            if (user.FirstName == tbFirstName.Text)
                i++;
            if (user.LastName == tbLastName.Text)
                i++;
            if (user.Email == tbEmail.Text)
                i += 2;
            if (i > 3)
            {
                MessageBox.Show("Awesome now you can change your password", "Verification Successful");
                Grid1.Visibility = Visibility.Hidden;
                GroupBox2.Visibility = Visibility.Visible;
            }
            else
                MessageBox.Show("It seems you don't remember enough information to recover your account", "Try again", MessageBoxButton.OK);
        }

        private void BtnSu_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(PsBox1.Password) || !String.IsNullOrWhiteSpace(PsBox2.Password))
            {
                if (PsBox1.Password == PsBox2.Password)
                {
                    var pom = (from s in DC.Users
                               where s.Username == user.Username
                               select s).FirstOrDefault();
                    pom.Password = PsBox1.Password;
                    try
                    {
                        DC.SubmitChanges();
                        MessageBox.Show("Password Successfully Changed\n Now you can login with your new password", "Account recovered !", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    catch(Exception)
                    {
                        MessageBox.Show("Someting went wrong!");
                    }
                }
            }
            else
                MessageBox.Show("Please inser your new password !");
        }
    }
}
