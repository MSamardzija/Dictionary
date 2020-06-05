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
    /// Interaction logic for UserQuizResults.xaml
    /// </summary>
    public partial class UserQuizResults : Window
    {
        readonly WarcoDictionaryDataContext DC = new WarcoDictionaryDataContext();

        public UserQuizResults()
        {
            InitializeComponent();
            PuniListView();
        }

        private void PuniListView()
        {
            var pom = from s in DC.Tests
                      where s.UserID == LoginUserData.UseridLOG
                      select s;
            ListV1.ItemsSource = pom;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (ListV1.HasItems)
            {
                try
                {
                    var result = from s in DC.Results
                                 where s.TestID == (int)ListV1.SelectedValue
                                 select s;
                    DC.Results.DeleteAllOnSubmit(result);

                    Test test = (from s in DC.Tests
                                 where s.TestID == (int)ListV1.SelectedValue
                                 select s).First();
                    DC.Tests.DeleteOnSubmit(test);
                    DC.SubmitChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("You did not select test to delete" + ex);
                }
                finally
                {
                    PuniListView();
                }
            }
            else
                MessageBox.Show("All tests are deleted already !", "Deleted already", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item && item.IsSelected)
            {
                var pom = from s in DC.Results
                          join w in DC.Words
                          on s.WordID equals w.WordID
                          where s.TestID == (int)ListV1.SelectedValue &&
                          s.WordID == w.WordID
                          select new { w.WordMain, s.Correct };
                StringBuilder message = new StringBuilder();
                foreach (var selectedItem in pom)
                {
                    message.AppendLine(selectedItem.WordMain.ToString() + "\t\t\t" + selectedItem.Correct.ToString());
                }
                MessageBox.Show(message.ToString(), "Words from this test and your answers");
            }
        }
    }
}
