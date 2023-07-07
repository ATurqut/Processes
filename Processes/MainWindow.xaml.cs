using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Processes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread thread;
        private string? sourceFilePath;
        private string? destinationFolderPath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenButton1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                textBox1.Text = selectedFilePath;
            }
        }

        private void OpenButton2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                textBox2.Text = selectedFilePath;
            }
        }

        private void ResumeButton_Click(object sender, RoutedEventArgs e)
        {
            thread.Resume();
            SuspendButton.IsEnabled = true;
            ResumeButton.IsEnabled = false;

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(textBox1.Text))
                {
                    SuspendButton.IsEnabled = true;
                    string text = File.ReadAllText(textBox1.Text);
                    progressBar.Maximum = text.Length;
                    thread = new Thread(
                        () =>
                        {
                            StringBuilder writeText = new StringBuilder();
                            foreach (var c in text)
                            {
                                writeText.Append(c);
                                Dispatcher.Invoke(() =>
                                {
                                    File.WriteAllText(textBox1.Text, writeText.ToString());
                                    progressBar.Value++;
                                });
                                Thread.Sleep(20);
                            }

                            MessageBox.Show("File copied succesfully", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                            Dispatcher.Invoke(() =>
                            {
                                progressBar.Value = 0;
                                textBox1.Clear();
                                textBox2.Clear();
                                StartButton.IsEnabled = false;
                            });
                        });
                    thread.Start();
                    StartButton.IsEnabled = false;
                }
                else
                {
                    throw new NotImplementedException("File path not exist");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                textBox1.Clear();
                textBox2.Clear();
            }
        }

        private void SuspendButton_Click(object sender, RoutedEventArgs e)
        {
            thread.Resume();
            SuspendButton.IsEnabled = true;
            ResumeButton.IsEnabled = false;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox1.Text != string.Empty && textBox2.Text != string.Empty)
                StartButton.IsEnabled = true;
        }
    }
}
