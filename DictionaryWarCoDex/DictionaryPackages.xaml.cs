using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DictionaryWarCoDex
{
    /// <summary>
    /// Interaction logic for DictionaryPackages.xaml
    /// </summary>
    public partial class DictionaryPackages : Window
    {
        readonly WarcoDictionaryDataContext DC = new WarcoDictionaryDataContext();

        public DictionaryPackages()
        {
            InitializeComponent();
            FillUpComboBox();
        }

        private void FillUpEng()
        {
            var pom = from s in DC.EngSrbPackages
                      select s;
            dataGrid1.ItemsSource = pom;
            BtnSearch.IsEnabled = true;
        }

        private void FillUpComboBox()
        {
            var pom = from s in DC.Dictionaries
                      where s.UserID == LoginUserData.UseridLOG
                      select s;
            CbDictionary.ItemsSource = pom;
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            FillUpEng();
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            if (CbDictionary.SelectedValue != null)
            {
                if (dataGrid1.SelectedIndex >= 0)
                {
                    for (int i = 0; i < dataGrid1.SelectedItems.Count; i++)
                    {
                        var pom = (from s in DC.EngSrbPackages
                                  where s.Word == ((EngSrbPackage)dataGrid1.SelectedItems[i]).Word
                                   select s).FirstOrDefault();
                        Word word = new Word()
                        {
                            WordMain = pom.Word,
                            WordTranslated = pom.Meaning,
                            DictionaryID = (int)CbDictionary.SelectedValue,
                            UserID = LoginUserData.UseridLOG,
                            DateOfInput = DateTime.Today,
                            Rating = 3
                        };
                        DC.Words.InsertOnSubmit(word);
                        try
                        {
                            DC.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show("Error" + ex);
                        }
                    }
                }
                else
                    System.Windows.MessageBox.Show("Please select row that you want in you dictionary");

            }
            else
                System.Windows.MessageBox.Show("Please select dictionary");
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            
            FillUpComboBox();
            if (CbDictionary.Visibility == Visibility.Hidden)
            {
                CbDictionary.Visibility = Visibility.Visible;
                BtnCopy.Visibility = Visibility.Visible;
                Txt.Visibility = Visibility.Visible;
            }
            else
            {
                CbDictionary.Visibility = Visibility.Hidden;
                BtnCopy.Visibility = Visibility.Hidden;
                Txt.Visibility = Visibility.Hidden;
            }
        }

        private void TbSearch_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var filtered = DC.EngSrbPackages.Where(word => word.Word.StartsWith(TbSearch.Text)|| word.Meaning.StartsWith(TbSearch.Text));

            dataGrid1.ItemsSource = filtered;
        }
    }
}
