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
    /// <summary>
    /// Interaction logic for NewUserRegistration.xaml
    /// </summary>
    public partial class NewUserRegistration : Window
    {
        readonly WarcoDictionaryDataContext DC = new WarcoDictionaryDataContext();
        public NewUserRegistration()
        {
            InitializeComponent();
        }
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbUsername.Text) && !String.IsNullOrWhiteSpace(tbEmail.Text) && !String.IsNullOrWhiteSpace(tbFirstName.Text)
                && !String.IsNullOrWhiteSpace(tbLastName.Text) && !String.IsNullOrWhiteSpace(PasswordBox1.Password) && !String.IsNullOrWhiteSpace(RepleatPasswordBox1.Password))
            {
                if (PasswordBox1.Password == RepleatPasswordBox1.Password)
                {
                    User newUser = new User
                    {
                        DateCreated = DateTime.Now,
                        Email = tbEmail.Text,
                        FirstName = tbFirstName.Text,
                        LastName = tbLastName.Text,
                        Username = tbUsername.Text,
                        Password = PasswordBox1.Password
                    };
                    try
                    {
                        DC.Users.InsertOnSubmit(newUser);
                        DC.SubmitChanges();
                        MessageBox.Show("Welcome" + tbUsername.Text + "\nYou can now log in!", "Registration is successful", MessageBoxButton.OK);
                        this.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Oh Something went wrong here !");
                    }
                }
                else
                    MessageBox.Show("Make sure your passwords match", "Passwords do not match", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Some of your text boxes are empty. Make sure you do not have empty text boxes", "Fill up the form", MessageBoxButton.OK);
        }

        private void BtnCancle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
