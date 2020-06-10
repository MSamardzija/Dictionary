using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for TestingSettings.xaml
    /// </summary>
    public partial class TestingSettings : Window
    {
        readonly WarcoDictionaryDataContext DC = new WarcoDictionaryDataContext();
        readonly QuizWarCo main = new QuizWarCo();


        public TestingSettings()
        {
            InitializeComponent();
            DpOneDictionary.SelectedDate = DateTime.Today;
        }


        private void RbFull_Checked_1(object sender, RoutedEventArgs e)
        {
            UseComboBox();
        }

        private void RbOneDictionary_Checked(object sender, RoutedEventArgs e)
        {
            UseComboBox();
            FillUpComboBox();
        }

        /// <summary>
        /// Checking and unchecking
        /// </summary>
        private void CheckBoxTestWordsOne_Click(object sender, RoutedEventArgs e)
        {
            FillUpDatePicker();
            if (CheckBoxTestWordsOne.IsChecked == true)
                DpOneDictionary.IsEnabled = true;
            else
            {
                DpOneDictionary.IsEnabled = false;
                DpOneDictionary.SelectedDate = DateTime.Today;
            }
        }

        private void CheckBoxRatingOne_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxRatingOne.IsChecked == true)
                SliderOneDictionaryTest.IsEnabled = true;
            else
            {
                SliderOneDictionaryTest.IsEnabled = false;
                SliderOneDictionaryTest.Value = 3;
            }
        }

        private void CheckBoxLimitOne_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxLimitOne.IsChecked == true)
                tbLimitOneDictionary.IsEnabled = true;
            else
            {
                tbLimitOneDictionary.IsEnabled = false;
                tbLimitOneDictionary.Text = "10";
            }
        }

        //BUTTON
        private void BtnStart(object sender, RoutedEventArgs e)
        {
            QuizzDictionary.EmptyDictionaryQuiz();
            if (rbFull.IsChecked == true)
            {
                MakeQuizFull();
                this.Close();
                main.ShowDialog();
            }
            else if (rbOneDictionary.IsChecked == true)
            {
                if (CbChoseOneDictionary.SelectedItem != null)
                {
                    MakeQuizOneDictionary();
                    this.Close();
                    main.ShowDialog();
                }
                else
                {
                    System.Windows.MessageBox.Show("Please chose your one dictionary that you want to test", "Chose one dictionary", MessageBoxButton.OK);
                    CbChoseOneDictionary.Focus();
                }
            }
            else
                System.Windows.MessageBox.Show("Please chose your test type !", "Chose test type", MessageBoxButton.OK);
        }



        /// 
        /// METHODS
        ///
        private void UseComboBox()
        {
            if (rbFull.IsChecked == true)
            {
                CbChoseOneDictionary.Visibility = Visibility.Hidden;
                lbChose.Visibility = Visibility.Hidden;
            }
            else
            {
                CbChoseOneDictionary.Visibility = Visibility.Visible;
                lbChose.Visibility = Visibility.Visible;
            }
        }

        private void FillUpComboBox()
        {
            //Ne bi bilo lose napraviti da proverava da li nalazi 10 reci u recniku, jer moze biti 10 reci ukupno na 3 recnika...
            var p = from s in DC.Dictionaries
                    where s.UserID == LoginUserData.UseridLOG
                    select s;
            CbChoseOneDictionary.ItemsSource = p;
        }

        private void FillUpDatePicker()
        {
            //Omogucava izbor datuma od kad je korisnik uneo prvu rec
            var p = (from s in DC.Words
                     where s.UserID == LoginUserData.UseridLOG
                     select s.DateOfInput).Min();
            DpOneDictionary.DisplayDateStart = p;
        }

        private void TbLimitOneDictionary_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public void MakeQuizFull()
        {
            var zzz = (from s in DC.Words
                       where s.UserID == LoginUserData.UseridLOG &&
                       DpOneDictionary.SelectedDate <= s.DateOfInput &&
                       s.Rating <= SliderOneDictionaryTest.Value
                       select new KeyValuePair<string, string>(s.WordMain, s.WordTranslated)).Take(Convert.ToInt32(tbLimitOneDictionary.Text));
            foreach (var item in zzz)
            {
                QuizzDictionary.nova.Add(item.Key.ToString(), item.Value.ToString());
            }
        }
        public void MakeQuizOneDictionary()
        {
            //Samo za jedan recnik
            var zzz = (from s in DC.Words
                       where s.UserID == LoginUserData.UseridLOG &&
                       s.DictionaryID == (int)CbChoseOneDictionary.SelectedValue &&
                       DpOneDictionary.SelectedDate <= s.DateOfInput &&
                       s.Rating <= SliderOneDictionaryTest.Value
                       select new KeyValuePair<string, string>(s.WordMain, s.WordTranslated)).Take(Convert.ToInt32(tbLimitOneDictionary.Text));
            foreach (var item in zzz)
            {
                QuizzDictionary.nova.Add(item.Key.ToString(), item.Value.ToString());
            }
        }

        private void BtnQuit(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Close();
            main.ShowDialog();
        }
    }
}
