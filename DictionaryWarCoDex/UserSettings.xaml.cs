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
    /// Interaction logic for UserSettings.xaml
    /// </summary>
    public partial class UserSettings : Window
    {
        readonly WarcoDictionaryDataContext DC = new WarcoDictionaryDataContext();

        public UserSettings()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("You will delete everything, are you sure ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                BrisiRezultate();
                BrisiReci();
                BrisiRecnike();
                BrisiTestove();
                BrisiKorisnika();
                MessageBox.Show("You succesfully deleted your account");
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void BtnUpdateMyData_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbFirstName.Text) || !String.IsNullOrEmpty(tbFirstName.Text) || !String.IsNullOrEmpty(tbLastName.Text))
            {
                var pom = (from s in DC.Users
                           where s.Username == LoginUserData.UserNameLOG
                           select s).First();
                if (!String.IsNullOrEmpty(tbEmail.Text))
                    pom.Email = tbEmail.Text;
                if (!String.IsNullOrEmpty(tbFirstName.Text))
                    pom.FirstName = tbFirstName.Text;
                if (!String.IsNullOrEmpty(tbLastName.Text))
                    pom.LastName = tbLastName.Text;
                try
                {
                    DC.SubmitChanges();
                    MessageBox.Show("you successfully changed your data!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbEmail.Clear();
                    tbFirstName.Clear();
                    tbLastName.Clear();
                }
                catch
                {
                    MessageBox.Show("Someting went wrong!");
                }
            }
            else
            {
                MessageBox.Show("You need to input some data in textboxes !", "Imput data", MessageBoxButton.OK);
                tbFirstName.Focus();
            }
        }

        private void BtnCheckOldPass_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(pbOld.Password))
            {
                if (pbOld.Password == LoginUserData.UserPassLOG)
                {
                    pbNewPass.IsEnabled = true;
                    pbRepeatNewPass.IsEnabled = true;
                    btnCheckOldPass.IsEnabled = false;
                    pbOld.IsEnabled = false;
                    pbNewPass.Focus();
                    btnChangePassword.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("That is not your password !", "Try again", MessageBoxButton.OK);
                    pbOld.Clear();
                }
            }
            else
            {
                MessageBox.Show("To change password we need to verify your identity", "Input your password!", MessageBoxButton.OK);
                pbOld.Focus();
            }
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(pbOld.Password) && !String.IsNullOrEmpty(pbNewPass.Password))
            {
                if (pbRepeatNewPass.Password == pbNewPass.Password)
                {
                    var pom = (from s in DC.Users
                               where s.Username == LoginUserData.UserNameLOG
                               select s).FirstOrDefault();
                    pom.Password = pbNewPass.Password;
                    try
                    {
                        DC.SubmitChanges();
                        MessageBox.Show("Password Successfully Changed !", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoginUserData.UserPassLOG = pbRepeatNewPass.Password;
                        pbNewPass.Clear();
                        pbRepeatNewPass.Clear();
                    }
                    catch
                    {
                        MessageBox.Show("Someting went wrong!");
                    }
                }
                else
                    MessageBox.Show("Your passwords do not match", "Chekc your input!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Input new password!");
        }


        private void BrisiReci()
        {
            var pom = from s in DC.Words
                      where s.UserID == LoginUserData.UseridLOG
                      select s;
            DC.Words.DeleteAllOnSubmit(pom);
            try
            {
                DC.SubmitChanges();
            }
            catch(Exception)
            {
                MessageBox.Show("Something went wrong here !" );
            }
        }

        private void BrisiRecnike()
        {
            var pom = from s in DC.Dictionaries
                      where s.UserID == LoginUserData.UseridLOG
                      select s;
            DC.Dictionaries.DeleteAllOnSubmit(pom);
            try
            {
                DC.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong here !");
            }
        }

        private void BrisiKorisnika()
        {
            User pom = (from s in DC.Users
                      where s.UserID == LoginUserData.UseridLOG
                      select s).FirstOrDefault();
            DC.Users.DeleteOnSubmit(pom);
            try
            {
                DC.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong here !");
            }
        }
        private void BrisiTestove()
        {
            var pom = from s in DC.Tests
                        where s.UserID == LoginUserData.UseridLOG
                        select s;
            DC.Tests.DeleteAllOnSubmit(pom);
            try
            {
                DC.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong here !");
            }
        }
        private void BrisiRezultate()
        {
            var pom = from s in DC.Results
                      join t in DC.Tests
                      on s.TestID equals t.TestID
                      where t.UserID == LoginUserData.UseridLOG
                      select s;
            DC.Results.DeleteAllOnSubmit(pom);
            try
            {
                DC.SubmitChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong here !");
            }
        }


        private void DeleteYourData_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("You will delete your words, dictionaries and tests, are you sure ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                BrisiRezultate();
                BrisiReci();
                BrisiRecnike();
                BrisiTestove();
                MessageBox.Show("You succesfully deleted your data !");
                System.Windows.Application.Current.Shutdown();
            }
        }
    }
}
