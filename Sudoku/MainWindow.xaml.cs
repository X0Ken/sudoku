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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private int Army { set; get; }
        public MainWindow()
        {
            InitializeComponent();
            Army =1;
            AnswerArmy.Background = Army1.Background;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Army == 0)
            {
                MessageBox.Show("请先选择队伍");
                return;
            }
            Button button = sender as Button;
            if (button != null)
            {
                Question.QuestionNum = button.Content as string;
                Question question = new Question();
                bool? r = question.ShowDialog();
                if (r == true)
                {
                    if (Army == 1)
                    {
                        button.Background = Army1.Background;
                        Army = 2;
                        AnswerArmy.Background = Army2.Background;
                    }
                    else
                    {
                        button.Background = Army2.Background;
                        Army = 1;
                        AnswerArmy.Background = Army1.Background;
                    }
                }
                else
                {
                    if (Army == 1)
                    {
                        button.Background = Army2.Background;
                        Army = 2;
                        AnswerArmy.Background = Army2.Background;
                    }
                    else
                    {
                        button.Background = Army1.Background;
                        Army = 1;
                        AnswerArmy.Background = Army1.Background;
                    }
                }

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Army = 1;
            AnswerArmy.Background = Army1.Background;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Army = 2;
            AnswerArmy.Background = Army2.Background;

        }
    }
}
