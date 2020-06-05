using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace DictionaryWarCoDex
{
    /// <summary>
    /// Interaction logic for WordAD.xaml
    /// </summary>
    public partial class WordAD : Window
    {
        readonly WarcoDictionaryDataContext DC = new WarcoDictionaryDataContext();

        public WordAD()
        {
            InitializeComponent();
            FillComboDictrionary();
            FillUpBoxes();
        }

        private void BtnAddNewWord_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbNewWord.Text) && !String.IsNullOrWhiteSpace(tbTranslation.Text) && cbDictionary.SelectedValue != null)
            {
                Word pom = new Word
                {
                    WordMain = tbNewWord.Text.TrimEnd(),
                    Pronouncement = tbProunc.Text,
                    WordTranslated = tbTranslation.Text.TrimEnd(),
                    DictionaryID = (int)cbDictionary.SelectedValue,
                    Description = tbDescription.Text,
                    DateOfInput = DateTime.Today,
                    Rating = 3,
                    UserID = LoginUserData.UseridLOG
                };
                DC.Words.InsertOnSubmit(pom);
                try
                {
                    DC.SubmitChanges();
                    MessageBox.Show("You added new word!");
                    ClearEverything();
                    tbNewWord.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error !" + ex);
                }
            }
            else
            {
                MessageBox.Show("Preas insert data in focused fields!");
                if (String.IsNullOrWhiteSpace(tbNewWord.Text))
                {
                    tbNewWord.Focus();
                }
                else if (String.IsNullOrWhiteSpace(tbTranslation.Text))
                {
                    tbTranslation.Focus();
                }
                else if (cbDictionary.SelectedValue == null)
                {
                    cbDictionary.Focus();
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbDelete.Text))
            {
                var result = from s in DC.Results
                             join w in DC.Words
                             on s.WordID equals w.WordID
                             where w.WordMain == tbDelete.Text &&
                             s.WordID == w.WordID
                             select s;
                DC.Results.DeleteAllOnSubmit(result);


                Word pom = (from s in DC.Words
                            where s.WordMain == tbDelete.Text &&
                            s.UserID == LoginUserData.UseridLOG
                            select s).FirstOrDefault();
                try
                {
                    DC.Words.DeleteOnSubmit(pom);
                    DC.SubmitChanges();
                    tbDelete.Clear();
                    System.Windows.MessageBox.Show("Deleted");

                }
                catch (Exception)
                {
                    MessageBox.Show("Please make sure you insert a word that exist in your dictionary!", "Mistakes are fine", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("You must input the word you want to delete", "Insert a word", MessageBoxButton.OK, MessageBoxImage.Information);
                tbDelete.Focus();
            }
        }
        private void ClearEverything()
        {
            tbNewWord.Clear();
            tbProunc.Clear();
            tbTranslation.Clear();
            tbDescription.Clear();
        }
        public void AddFirstDictionary()
        {
            MessageBox.Show("To add words you need to create dictionary", "Make Dictionary", MessageBoxButton.OK, MessageBoxImage.Information);
            DictionaryAD main = new DictionaryAD();
            main.ShowDialog();
            FillComboDictrionary();
        }

        private void FillComboDictrionary()
        {
            var pom = from s in DC.Dictionaries
                      where s.UserID == LoginUserData.UseridLOG
                      select s;
            cbDictionary.ItemsSource = pom;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearEverything();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbNewWord.Text) || !String.IsNullOrEmpty(tbTranslation.Text))
            {
                Word pom = (from s in DC.Words
                           where s.WordMain == QuizzDictionary.word.WordMain
                           select s).FirstOrDefault();
                pom.WordMain = tbNewWord.Text.TrimEnd();
                pom.WordTranslated = tbTranslation.Text.TrimEnd();
                pom.DictionaryID = (int)cbDictionary.SelectedValue;
                pom.Description = tbDescription.Text;
                pom.Pronouncement = tbProunc.Text;
                try
                {
                    DC.SubmitChanges();
                    MessageBox.Show("you successfully changed your data!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    BtnUpdate.Visibility = Visibility.Hidden;
                }
                catch
                {
                    MessageBox.Show("Someting went wrong!");
                }
            }
            else
            {
                MessageBox.Show("You need to input some data in textboxes !", "Imput data", MessageBoxButton.OK);
            }
            ClearEverything();
        }

        private void FillUpBoxes()
        {
            if(QuizzDictionary.word.WordMain != null)
            {
                tbNewWord.Text = QuizzDictionary.word.WordMain;
                tbTranslation.Text = QuizzDictionary.word.WordTranslated;
                tbProunc.Text = QuizzDictionary.word.Pronouncement;
                tbDescription.Text = QuizzDictionary.word.Description;
                cbDictionary.SelectedValue = QuizzDictionary.word.DictionaryID;
                BtnUpdate.IsDefault = true;
                btnAddNewWord.IsDefault = false;
            }
            else
            {
                BtnUpdate.Visibility = Visibility.Hidden;
            }
        }

    }
}
