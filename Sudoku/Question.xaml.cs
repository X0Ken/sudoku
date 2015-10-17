using Newtonsoft.Json;
using QuestionClass;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Xps.Packaging;

namespace Sudoku
{
    /// <summary>
    /// Question.xaml 的交互逻辑
    /// </summary>
    public partial class Question : Window
    {
        static public string QuestionNum { set; get; }
        string Answer { set; get; }
        public Question()
        {
            InitializeComponent();
            Init();
            
        }

        private void Init()
        {
            if (File.Exists("Question.json"))
            {
                StreamReader sr = new StreamReader("Question.json");
                List<QuestionInfo> question = JsonConvert.DeserializeObject<List<QuestionInfo>>(sr.ReadToEnd());
                sr.Close();
                string htmlTemp = @"<html>
	<head>
		<meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head>
	<body>
		<div id='sample'>{Temp}</div>
		<script type='text/javascript'>
			var a = document.getElementsByTagName('a');
			for(var i = 0; i < a.length;i++){
				a[i].onclick = function(){
					window.external.openFile(this.href);
					return false;
				};
			}
		</script>
	</body>
</html>
";
                //QusetionText.NavigateToString(question[int.Parse(QuestionNum) - 1].Qusetion);
                QusetionText.NavigateToString(htmlTemp.Replace("{Temp}",question[int.Parse(QuestionNum) - 1].Qusetion));
                QusetionText.ObjectForScripting = new ObjectForScriptingHelper(this);

                Answer = question[int.Parse(QuestionNum) - 1].Answer;
            }
            else
            {
                MessageBox.Show("请先设计题目");
            }
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if ((button.Content as string) == Answer)
            {
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
            }
        }
    }

    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class ObjectForScriptingHelper
    {
        Question mainWindow;

        public ObjectForScriptingHelper(Question main)
        {
            mainWindow = main;
        }

        public void openFile(string path)
        {
            System.Diagnostics.Process.Start(path);  
        }
    }
}
