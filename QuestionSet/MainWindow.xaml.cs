using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using QuestionClass;
using Newtonsoft.Json;
using Microsoft.Win32;
using System;
using System.Security.Permissions;
using System.Drawing.Imaging;

namespace QuestionSet
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    
    
    public partial class MainWindow : Window
    {
        List<QuestionInfo> questionList = new List<QuestionInfo>();
        int Edited = 0;
        MainWindow mainWindow;
        public MainWindow()
        {
            
            InitializeComponent();
            QuestionText.Navigate(new Uri(Environment.CurrentDirectory+"/Editor/html/index.html", UriKind.Absolute));
            mainWindow = this;
            QuestionText.ObjectForScripting = new ObjectForScriptingHelper(this);
            QuestionText.LoadCompleted += QuestionText_LoadCompleted;
            
            
        }

        void QuestionText_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            NewQuestion();
        }

        public void GetQusetionText(string text)
        {
            questionList[Edited].Qusetion = text;
        }
        public string SetQusetionText()
        {
            return questionList[Edited].Qusetion;
        }

        private void Answer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            questionList[Edited].Answer = (string)(Answer.SelectedItem as ComboBoxItem).Content;
        }



        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            NewQuestion();
        }

        private void NewQuestion()
        {
            questionList.Clear();
            questionList.Add(new QuestionInfo { Qusetion = "", Answer = "A" });
            questionList.Add(new QuestionInfo { Qusetion = "", Answer = "A" });
            questionList.Add(new QuestionInfo { Qusetion = "", Answer = "A" });
            questionList.Add(new QuestionInfo { Qusetion = "", Answer = "A" });
            questionList.Add(new QuestionInfo { Qusetion = "", Answer = "A" });
            questionList.Add(new QuestionInfo { Qusetion = "", Answer = "A" });
            questionList.Add(new QuestionInfo { Qusetion = "", Answer = "A" });
            questionList.Add(new QuestionInfo { Qusetion = "", Answer = "A" });
            questionList.Add(new QuestionInfo { Qusetion = "", Answer = "A" });
            SelectIndex(0);
            this.Title = "新题组";
        }

        private void SelectIndex(int index)
        {
            Edited = index;
            //QuestionText.Text = questionList[Edited].Qusetion;
            string answerTemp = questionList[Edited].Answer;
            Answer.SelectedIndex = answerTemp == "A" ? 0 : answerTemp == "B" ? 1 : answerTemp == "C" ? 2 : 3;
            QuestionNum.Text = "问题：" + (Edited + 1).ToString();
            QuestionText.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            int index = int.Parse((string)(sender as Button).Tag);
            SelectIndex(index);
        }

        private void Save(string path)
        {
            string jsonText = JsonConvert.SerializeObject(questionList);
            StreamWriter sw = new StreamWriter(path);
            sw.Write(jsonText);
            sw.Close();
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "题组文件(*.json)|*.json*";
            file.InitialDirectory = Environment.CurrentDirectory;
            bool? r = file.ShowDialog();
            if (r == true)
            {
                StreamReader sr = new StreamReader(file.FileName);
                questionList = JsonConvert.DeserializeObject<List<QuestionInfo>>(sr.ReadToEnd());
                sr.Close();
                this.Title = file.SafeFileName.Substring(0,file.SafeFileName.Length-5);
                SelectIndex(0);
            }
            
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "题组文件(*.json)|*.json*";
            file.InitialDirectory = Environment.CurrentDirectory;
            file.FileName = "题组1.json";
            bool? r = file.ShowDialog();
            if (r == true)
            {
                Save(file.FileName);
                this.Title = file.SafeFileName.Substring(0, file.SafeFileName.Length - 5);
            }
        }

        private void MenuItemSet_Click(object sender, RoutedEventArgs e)
        {
            Save("Question.json");
            MessageBox.Show("应用成功");
        }
    }

    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class ObjectForScriptingHelper
    {
        MainWindow mainWindow;

        public ObjectForScriptingHelper(MainWindow main)
        {
            mainWindow = main;
        }

        //JS调WinForm方法的接口
        public void SetText(string str)
        {
            mainWindow.GetQusetionText(str);
        }

        public string GetText()
        {
            return mainWindow.SetQusetionText();
        }

        public string getGetFilePath()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.CheckFileExists = true;
            if(file.ShowDialog() == true)
            {
                return file.FileName;
            }
            return "";
        }

        public string getImgBase64()
        {
            OpenFileDialog fbd = new OpenFileDialog();
            fbd.Title = "请选择一张图片：";
            fbd.CheckFileExists = true;
            fbd.Filter = "图片|*.jpg;*.png;*.gif";
            fbd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (fbd.ShowDialog() == true)
            {
                
                try
                {

                    string fileName = fbd.FileName;
                    string ext = Path.GetExtension(fileName);
                    MemoryStream ms = new MemoryStream();
                    System.IO.MemoryStream m = new System.IO.MemoryStream();
                    System.Drawing.Bitmap bp = new System.Drawing.Bitmap(fileName);
                    ImageFormat fmt = ImageFormat.Jpeg;
                    string media_type;
                    switch (ext.ToLower())
                    {
                        case ".gif":
                            fmt = System.Drawing.Imaging.ImageFormat.Gif;
                            media_type = "gif";
                            break;
                        case ".jpg":
                            fmt = System.Drawing.Imaging.ImageFormat.Jpeg;
                            media_type = "jpeg";
                            break;
                        case ".png":
                            fmt = System.Drawing.Imaging.ImageFormat.Png;
                            media_type = "png";
                            break;
                        default:
                            throw new Exception("无法识别的图片");
                        
                    }  

                    bp.Save(m, fmt);
                    byte[]b= m.GetBuffer();
                    string base64string=Convert.ToBase64String(b);
                    return "data:" + media_type + ";base64," + base64string;
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return "";
                }
            }
            return "";
        }
    }

}
