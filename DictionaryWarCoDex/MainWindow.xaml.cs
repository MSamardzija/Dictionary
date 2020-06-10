using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DictionaryWarCoDex
{
    public partial class MainWindow : Window
    {
        readonly WarcoDictionaryDataContext DC = new WarcoDictionaryDataContext();

        public MainWindow()
        {
            InitializeComponent();
            PuniDataGrid();
        }


        public void PuniDataGrid()
        {
            var pom = from s in DC.Words
                      where s.UserID == LoginUserData.UseridLOG
                      select s;
            dataGrid1.ItemsSource = pom;
        }
        private void AddNewWord()
        {
            //Word
            WordAD main = new WordAD();
            if (main.cbDictionary.HasItems)
                main.ShowDialog();
            else
            {
                main.AddFirstDictionary();
                if (main.cbDictionary.HasItems)
                    main.ShowDialog();
                else
                    main.Close();
            }
            PuniDataGrid();
        }
        /// 
        /// MenuButtons
        /// 
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Dictionary
            DictionaryAD main = new DictionaryAD();
            main.ShowDialog();
            PuniDataGrid();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //Word
            AddNewWord();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            AboutWarCoProject main = new AboutWarCoProject();
            main.ShowDialog();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            //Testing
            if (dataGrid1.Items.Count >= 10)
            {
                TestingSettings main = new TestingSettings();
                this.Close();
                main.ShowDialog();
            }
            else
                System.Windows.MessageBox.Show("You need at least 10 words in your dictionary to do testing \n" +
                    "So far you have: " + (dataGrid1.Items.Count) + " words !", "Minimum is 10 words !", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            UserSettings main = new UserSettings();
            main.ShowDialog();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            //Logout
            LoginUserData.EmptyLoginData();
            Login pom = new Login();
            this.Close();
            pom.ShowDialog();
        }
        
        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            DictionaryPackages packages = new DictionaryPackages();
            if (packages.CbDictionary.HasItems)
            {
                packages.ShowDialog();
                PuniDataGrid();
            }
            else
                MessageBox.Show("Please make sure you have dictionary !");
        }

        /// 
        /// Context Menu buttons
        /// 
        private void UpdateRow_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid1.Items.Count > 0)
            {
                try
                {
                    var pom = (from s in DC.Words
                               where s.WordID == (int)dataGrid1.SelectedValue
                               select s).FirstOrDefault();
                    QuizzDictionary.word = pom;
                    AddNewWord();
                    MainWindow main = new MainWindow();
                    this.Close();
                    main.Show();
                    QuizzDictionary.EmptyWord();
                }
                catch(Exception)
                {
                    MessageBox.Show("Please make sure you selected one word");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("You dont have any words to update !", "Insert some words !", MessageBoxButton.OK, MessageBoxImage.Information);
                AddNewWord();
            }
        }
        private void DeleteRow_Click(object sender, RoutedEventArgs e)
        {
            var result = from s in DC.Results
                         where s.WordID == (int)dataGrid1.SelectedValue
                         select s;
            DC.Results.DeleteAllOnSubmit(result);

            Word pom = (from s in DC.Words
                        where s.WordID == (int)dataGrid1.SelectedValue
                        select s).FirstOrDefault();
            try
            {                
                DC.Words.DeleteOnSubmit(pom);
                DC.SubmitChanges();
                System.Windows.MessageBox.Show("Deleted");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Please make sure you insert a word that exist in your dictionary!" +ex, "Check again", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                PuniDataGrid();
            }
        }
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            AddNewWord();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (TbSearch.Visibility == Visibility.Hidden)
            {
                TbSearch.Visibility = Visibility.Visible;
                TbSearch.Focus();
            }
            else
            {
                TbSearch.Visibility = Visibility.Hidden; 
                TbSearch.Clear();
            }
        }    

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            BtnSearch_Click(sender, e);
        }

        private void OpenResultsWin_Click(object sender, RoutedEventArgs e)
        {
            UserQuizResults main = new UserQuizResults();
            main.ShowDialog();
        }

        private void FilterDictionary_Click(object sender, RoutedEventArgs e)
        {
            if (CbDictionaryMain.Visibility == Visibility.Visible)
            {
                CbDictionaryMain.Visibility = Visibility.Hidden;
                PuniDataGrid();
            }
            else
            {
                CbDictionaryMain.Visibility = Visibility.Visible;
                CbMainFillUp();
            }
        }
        private void CbMainFillUp()
        {
            var pom = from s in DC.Dictionaries
                      where s.UserID == LoginUserData.UseridLOG
                      select s;
            CbDictionaryMain.ItemsSource = pom;
        }

        private void CbDictionaryMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pom = from s in DC.Words
                      where s.DictionaryID == (int)CbDictionaryMain.SelectedValue
                      select s;
            dataGrid1.ItemsSource = pom ;
        }

        private void TbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            var pom = DC.Words.Where(word => word.UserID == LoginUserData.UseridLOG  && word.WordMain.StartsWith(TbSearch.Text) || word.WordTranslated.StartsWith(TbSearch.Text));

            dataGrid1.ItemsSource = pom;
        }
    }
}
