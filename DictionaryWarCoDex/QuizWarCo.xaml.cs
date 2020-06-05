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
    /// Interaction logic for QuizWarCo.xaml
    /// </summary>
    public partial class QuizWarCo : Window
    {
        readonly WarcoDictionaryDataContext DC = new WarcoDictionaryDataContext();
        private int i = 0;

        public QuizWarCo()
        {
            InitializeComponent();
        }



        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            while (i < QuizzDictionary.nova.Count)
            {
                if (QuizzDictionary.nova.TryGetValue(TbWordMain.Text, out string lol) && lol == TbTranslate.Text.TrimEnd())
                {
                    MessageBox.Show("Correct");
                    Tacno(i);
                    QuizzDictionary.BrojTacno += 1;
                    ClearTextBoxes();
                }
                else
                {
                    MessageBox.Show("Not Correct");
                    Netacno(i);
                    QuizzDictionary.BrojNetacno += 1;
                    ClearTextBoxes();

                }
                i++;
                if (i < QuizzDictionary.nova.Count)
                    TbWordMain.AppendText(QuizzDictionary.nova.Keys.ElementAt(i));
                else
                {
                    MessageBox.Show("Test is finished");
                    BtnCheck.Visibility = Visibility.Hidden;
                    BtnEnd.Visibility = Visibility.Hidden;
                    BtnFinish.Visibility = Visibility.Visible;
                    TbNameOfTest.Visibility = Visibility.Visible;
                    TextBlock1.Visibility = Visibility.Visible;
                    TbNameOfTest.Focus();
                    //Treba da moze da se vidi rezultat mozda...
                }
                break;
            }

        }
        private void BtnStartTest_Click(object sender, RoutedEventArgs e)
        {
            if (QuizzDictionary.nova.Count > 0)
            {
                BtnCheck.Visibility = Visibility.Visible;
                TbWordMain.AppendText(QuizzDictionary.nova.Keys.First());
                BtnStartTest.Visibility = Visibility.Hidden;
                TbTranslate.Focus();
            }
            else
            {
                MessageBox.Show("You made a test with no words in it !", "Change your testing settings !", MessageBoxButton.OK);
                this.Close();
                TestingSettings testing = new TestingSettings();
                testing.ShowDialog();
            }
        }

        private void BtnEnd_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Quiz will be closed - results won't be saved", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                MainWindow main = new MainWindow();
                this.Close();
                main.Show();
            }
        }

        ///Methods
        ///
        ///
        private void ClearTextBoxes()
        {
            TbWordMain.Clear();
            TbTranslate.Clear();
        }

        private void Netacno(int z)
        {
            var pom = (from s in DC.Words
                       where s.UserID == LoginUserData.UseridLOG &&
                       s.WordMain == QuizzDictionary.nova.Keys.ElementAt(z)
                       select s).FirstOrDefault();
            if (pom.Rating > 0)
                pom.Rating -= 1;
            else
                pom.Rating = 0;

            QuizzDictionary.rezultati.Add(pom.WordID, false);
            try
            {
                DC.SubmitChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show("Someting went wrong !" + e);
            }
        }

        private void Tacno(int z)
        {
            var pom = (from s in DC.Words
                       where s.UserID == LoginUserData.UseridLOG &&
                       s.WordMain == QuizzDictionary.nova.Keys.ElementAt(z)
                       select s).FirstOrDefault();
            if (pom.Rating == 5)
                pom.Rating = 5;
            else
                pom.Rating += 1;

            QuizzDictionary.rezultati.Add(pom.WordID, true);
            try
            {
                DC.SubmitChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show("Someting went wrong !" + e);
            }
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(TbNameOfTest.Text))
            {
                Test mainTest = new Test
                {
                    DateOfTest = DateTime.Now,
                    Correct = QuizzDictionary.BrojTacno,
                    Inccorect = QuizzDictionary.BrojNetacno,
                    NameOfTest = TbNameOfTest.Text,
                    UserID = LoginUserData.UseridLOG,
                    NumberOfWordsTested = i,
                    PercentageOfCorrect = Math.Round(Convert.ToDouble(QuizzDictionary.BrojTacno) / Convert.ToDouble(i)*100, 2),
                };
                DC.Tests.InsertOnSubmit(mainTest);
                try
                {
                    DC.SubmitChanges();
                    MessageBox.Show("Test Saved\nYou Answerd correct on " + QuizzDictionary.BrojTacno +
                        " questions\n" + "You had " + QuizzDictionary.BrojNetacno + " Incorrect answers\nYou test is saved!");
                    UpisiRezultate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Oh Something went wrong here ! " + ex);
                }
                finally
                {
                    QuizzDictionary.EmptyDictionaryQuiz();
                    QuizzDictionary.EmptyResults();
                    MainWindow window = new MainWindow();
                    this.Close();
                    window.Show();
                }
            }
            else
                MessageBox.Show("Please make sure you name your test !");            
        }        
        private void UpisiRezultate()
        {
            var pom = (from s in DC.Tests
                       select s.TestID).Max();
            for(int i = 0; i< QuizzDictionary.rezultati.Keys.Count; i++)
            {
                Result result = new Result
                {
                    WordID = QuizzDictionary.rezultati.Keys.ElementAt(i),
                    Correct = QuizzDictionary.rezultati.Values.ElementAt(i),
                    TestID = pom
                };
                DC.Results.InsertOnSubmit(result);
            }
            try
            {
                DC.SubmitChanges();
            }
            catch(Exception)
            {
                MessageBox.Show("Ne");
            }
        }
    }
}
