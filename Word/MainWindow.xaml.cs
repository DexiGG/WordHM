using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Word
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string copyOrCut ="";
        bool saveChanged = false;

        public MainWindow()
        {
            InitializeComponent();
            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                // FontFamily.Source contains the font family name.
                fontFamilyListBox.Items.Add(fontFamily.Source);
            }
        }

        private void ClickToSave(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "(*.txt)|*.txt";

            if (saveFileDialog1.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog1.OpenFile(), System.Text.Encoding.Default))
                {
                    sw.Write(textInput.Text);
                    sw.Close();
                }
            }
          saveChanged = false;
        }

        private void ClickToOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Текст(*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);

                StreamReader reader = new StreamReader(fileInfo.Open(FileMode.Open, FileAccess.Read), Encoding.GetEncoding(1251));

                textInput.Text = reader.ReadToEnd();

                reader.Close();
                return;
            }
        }
        private void FontFamilyItemClick(object sender, RoutedEventArgs e)
        {
            textInput.FontFamily = new FontFamily(fontFamilyListBox.SelectedValue.ToString());
        }

        private void ClickBack(object sender, RoutedEventArgs e)
        {
            textInput.Text = string.Empty;
        }

   

        private void ClickToCopy(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(textInput.Text)&&textInput.SelectedText !=null)
            {
                copyOrCut = textInput.Text;
            }
            
        }

        private void ClickToInsert(object sender, RoutedEventArgs e)
        {
            textInput.Text += copyOrCut;
        }


        private void ClickToCut(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textInput.Text) && textInput.SelectedText != null)
            {
                copyOrCut = textInput.Text;
                textInput.Text = string.Empty;
            }
        }

        private void ClickToDelete(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textInput.Text) && textInput.SelectedText != null)
            {
                textInput.Text = textInput.Text.Replace(textInput.SelectedText, "");
            }
        }

        private void ClickToPrint(object sender, RoutedEventArgs e)
        {
            PrintDialog print = new PrintDialog();
            if(print.ShowDialog()==true)
            {
                print.PrintVisual(textInput, "");
            }
        }

        private void ClickToExit(object sender, RoutedEventArgs e)
        {
            if (saveChanged == true )
            {
                Close();
            }
            else
            {

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "(*.txt)|*.txt";

                if (saveFileDialog1.ShowDialog() == true)
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog1.OpenFile(), System.Text.Encoding.Default))
                    {
                        sw.Write(textInput.Text);
                        sw.Close();
                    }
                }
            }

        }

        private void ClickToShowAbout(object sender, RoutedEventArgs e)
        {
            var currentProcess = Process.GetCurrentProcess();
            Process.Start("https://www.bing.com/search?q=get+help+with+notepad+in+windows+10&filters=guid:%224466414-en-dia%22%20lang:%22en%22&form=T00032&ocid=HelpPane-BingIA");

            var chromeProcesses = Process.GetProcessesByName("chrome");
            foreach (var process in chromeProcesses)
            {
                process.Kill();
            }
        }

        private void AutoSave()
        {
            ////////////
            string path = "";
            try
            {
                textInput.SelectAll();

                using (FileStream fs = File.Create(path))
                {
                    Byte[] title = new UTF8Encoding(true).GetBytes("AutoSave");
                    fs.Write(title, 0, title.Length);
                    byte[] author = new UTF8Encoding(true).GetBytes(textInput.Text);
                    fs.Write(author, 0, author.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //Assembly.GetExecutingAssembly().Location;
        }

    }
}
