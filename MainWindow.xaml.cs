using Microsoft.Win32;
using ProjektASM;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
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
using static System.Net.Mime.MediaTypeNames;
using Color = System.Windows.Media.Color;

namespace AssmblerRGB
{

    public partial class MainWindow : Window
    {
        private Bitmap bitmap = new Bitmap(800, 600);
        private Bitmap bitmap2 = new Bitmap(800, 600);
        private bool csLanguage = true;

        private uint threadsNumber = 1;
        public MainWindow()
        {
            InitializeComponent();

        }

        private async void ThreadSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            try
            {
                threadsNumber = (uint)(int)slValue.Value;
            }
            catch (Exception) { }

        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.ShowDialog();
                bitmap = (Bitmap)Bitmap.FromFile(openFileDialog.FileName);
                ImageOne.Source = Convert(bitmap);

            }
            catch (Exception)
            {
                MessageBox.Show("Proszê podaæ zdjêcie w poprawnym formacie", "Uwaga");
            }
        }
        private BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;

        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            bitmap2 = bitmap.ColorBalance(threadsNumber, csLanguage);
            watch.Stop();
            ImageTwo.Source = Convert(bitmap2);
            timeLabel.Content = "Time: " + watch.Elapsed.Milliseconds.ToString() + " ms";
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            csLanguage = true;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            csLanguage = false;
        }


    }

}
