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
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO.MemoryMappedFiles;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1
{
    public static partial class gApp
    {
        static public byte[] BitData;


    }

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Sample1 sample1 = new Sample1();

            //this.DataContext = sample1;

            //string filePath = Directory.GetCurrentDirectory() + "\\image3.bmp";
            //var sample1.GetBitmapImage(filePath);

        }

        int _count = 1;
        //string filePath = Directory.GetCurrentDirectory() + "\\image3.bmp";
        byte[] _BitData;
        byte[] _BitArray;

        int _width;
        int _height;

        private void image_load(object sender, RoutedEventArgs e)
        {
            string filePath = Directory.GetCurrentDirectory() + "\\image3.bmp";

            WpfApp1.BitmapBinary.BITMAPFILEHEADER bfh;
            WpfApp1.BitmapBinary.BITMAPINFOHEADER bih;
            System.Drawing.Color[] ColorPal;

            WpfApp1.BitmapBinary.Load(filePath, out bfh, out bih, out ColorPal, out _BitData);
            _width = bih.biWidth;
            _height = bih.biHeight;


            System.IO.FileStream fs = new System.IO.FileStream(
                filePath,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);

            _BitArray = new byte[fs.Length];
            fs.Read(_BitArray, 0, _BitArray.Length);
            fs.Close();





            MessageBox.Show("load complete.");
        }

        /// <summary>
        /// パターン１
        /// </summary>
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            var bitmap = new WriteableBitmap(_width, _height, 96, 96, PixelFormats.Gray8, null);

            for (int cnt = 0; cnt < _count; cnt++)
            {
                bitmap.WritePixels(new Int32Rect(0, 0, _width, _height), _BitData, _width, 0, 0);
            }
            this.imgName.Source = bitmap;
        }


        /// <summary>
        /// パターン２
        /// </summary>
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            var ms = new MemoryStream(_BitArray);
            var bi = new BitmapImage();

            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.CreateOptions = BitmapCreateOptions.None;

            bi.StreamSource = ms;
            bi.EndInit();
            this.imgName.Source = bi;
            bi.Freeze();

        }

        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        
        /// <summary>
        /// パターン３
        /// </summary>
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            Bitmap bitmap = new Bitmap(_width, _height);

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    int value = _BitData[y * _width + x];
                    System.Drawing.Color color = System.Drawing.Color.FromArgb(255, value, value, value);
                    bitmap.SetPixel(x, y, color);
                }
            }

            // 表示
            IntPtr hbitmap = bitmap.GetHbitmap();
            imgName.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(hbitmap);

        }
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);




        class Sample1
        {
            public BitmapSource BmpSrc { get; set; }

            public BitmapImage GetBitmapImage(string file)
            {
                var bmpImg = new BitmapImage();
                bmpImg.BeginInit();
                bmpImg.CacheOption = BitmapCacheOption.OnLoad;

                bmpImg.EndInit();

                bmpImg.CreateOptions = BitmapCreateOptions.None;

                bmpImg.UriSource = new Uri(file);
                bmpImg.EndInit();

                return bmpImg;
            }
        }


    }
}
