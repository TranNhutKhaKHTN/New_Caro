using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Caro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int xStart = 120;
        int yStart = 120;
        int height = 40;
        int width = 40;
        int Cols = 10;
        int Rows = 10;
        int[,] a;

        int Sumcount=0;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            a = new int[Rows, Cols];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    a[i, j] = 0;
                }
            }
            VeBanCo();

            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += dtTicker;
        }

        public void VeBanCo()
        {
            for (int j = 0; j <= Cols; j++)
            {
                Line l = new Line();
                l.Stroke = Brushes.Black;
                l.X1 = xStart + j * width;
                l.Y1 = yStart;

                l.X2 = xStart + j * width;
                l.Y2 = yStart + Rows * height;
                canvas.Children.Add(l);
            }

            for (int j = 0; j <= Rows; j++)
            {
                Line l = new Line();
                l.Stroke = Brushes.Black;
                l.Y1 = yStart + j * height;
                l.X1 = xStart;

                l.Y2 = yStart + j * height;
                l.X2 = xStart + Rows * width;
                canvas.Children.Add(l);
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var mouse = e.GetPosition(this);

            //int i = ((int)mouse.Y - yStart) / heigh;
            //int j = ((int)mouse.X - xStart) / width;

            //if(i<Cols&&j<Rows&&a[i,j]==0)
            //{

            //}
        }

        bool Xturn = true;
        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var mouse = e.GetPosition(this);

            int i = ((int)mouse.Y - yStart) / height;
            int j = ((int)mouse.X - xStart) / width;

            if (i < Cols && j < Rows && i >= 0 && j >= 0)
            {
                if (a[i, j] == 0)
                {
                    var img = new Image();
                    img.Width = height;
                    img.Height = width;

                    if (Xturn)
                    {
                        img.Source = new BitmapImage(
                            new Uri("X.png", UriKind.Relative));                     
                        a[i, j] = 1;
                    }
                    else
                    {
                        img.Source = new BitmapImage(
                            new Uri("O.png", UriKind.Relative));
                        
                        a[i, j] = 2;
                    }
                    canvas.Children.Add(img);
                    Canvas.SetLeft(img, xStart + j * width);
                    Canvas.SetTop(img, yStart + i * height);
                    Xturn = !Xturn;
                }

                Sumcount++;

                Setturn();

                countTime = 10;

                if (CheckWin(i,j)==1)
                {
                    MessageBox.Show("X Won!");
                    Reset();
                }
                if(CheckWin(i,j)==2)
                {
                    MessageBox.Show("O Won!");
                    Reset();
                }
                if(Sumcount==Cols*Rows)
                {
                    MessageBox.Show("Tie!");
                }
            }
        }

        public int CheckWin(int i, int j)
        {
            const int conditionWin = 5;
            int count;
            int di, dj;
            //---------------loang theo chiều ngan----------------
            count = 1;
            //loang bên trái
            di = 0;
            dj = -1;
            count += Loang(di, dj, i, j);
            //loang bên phải
            di = 0;
            dj = 1;
            count += Loang(di, dj, i, j);

            if (count >= conditionWin)
            {
                return a[i, j];
            }

            //---------------loang theo chiều dọc----------------
            count = 1;
            //loang bên trên
            di = -1;
            dj = 0;
            count += Loang(di, dj, i, j);
            //loang bên dưới
            di = 1;
            dj = 0;
            count += Loang(di, dj, i, j);

            if (count >= conditionWin)
            {
                return a[i, j];
            }


            //---------------loang theo đường chéo chính----------------
            count = 1;
            //loang bên trên
            di = -1;
            dj = -1;
            count += Loang(di, dj, i, j);
            //loang bên dưới
            di = 1;
            dj = 1;
            count += Loang(di, dj, i, j);

            if (count >= conditionWin)
            {
                return a[i, j];
            }

            //---------------loang theo đường chéo phụ----------------
            count = 1;
            //loang bên trên
            di = -1;
            dj = 1;
            count += Loang(di, dj, i, j);
            //loang bên dưới
            di = 1;
            dj = -1;
            count += Loang(di, dj, i, j);

            if (count >= conditionWin)
            {
                return a[i, j];
            }


            return 0;
        }



        /// <summary>
        /// Hàm loang
        /// </summary>
        /// <param name="di"></param>
        /// <param name="dj"></param>
        /// <param name="i">vị trí i bắt đầu</param>
        /// <param name="j">vị trí j bắt đầu</param>
        /// <returns></returns>
        int Loang(int di, int dj, int i, int j)
        {
            int count = 0;
            int StartI = i;
            int StartJ = j;

            while (true)
            {
                j += dj;
                i += di;
                if (i > Rows - 1 || i < 0 || j > Cols - 1 || j < 0)
                {
                    break;
                }
                //nếu khác giá trị với start thì không cộng nửa
                if (a[StartI, StartJ] != a[i, j])
                {
                    break;
                }
                else
                    count++;
            }
            return count;
        }

        public void Reset()
        {
            //xóa hết
            canvas.Children.Clear();
            //xẽ lại bàn cờ
            VeBanCo();
            //for(int i=Rows+Cols+2+1;i<canvas.Children.Count;i++)
            //{
            //    canvas.Children.RemoveAt(i);
            //}
            Xturn = true;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    a[i, j] = 0;
                }
            }
            Sumcount = 0;
            Setturn();
            time.Text = "10";
            time.Foreground = Brushes.Blue;
            countTime = 10;
            dt.Stop();
            
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var screen = new FileName();
            dt.Stop();
            PlayTime.Content = "Contunute";
            StreamWriter Writer = null;
            if (screen.ShowDialog() == true)
            {

                if (!screen.filename.ToString().Equals(""))
                {
                    Writer = new StreamWriter(screen.filename);
                    //lưu lại lượt đi hiện tại
                    //nếu Xturn = true lưu X không thi lưu O
                    Writer.WriteLine(Xturn ? "X" : "0");

                    //lưu SumCount
                    Writer.WriteLine($"{Sumcount}");

                    //lưu lại thời gian
                    Writer.WriteLine($"{countTime}");

                    //lưu ma trận biểu diễn
                    for (int i = 0; i < Rows; i++)
                    {
                        for (int j = 0; j < Cols; j++)
                        {
                            Writer.Write($"{a[i, j]} ");

                            if (j == Cols - 1)
                            {
                                Writer.Write(" ");
                            }
                        }
                        Writer.WriteLine("");
                    }

                    Writer.Close();
                    MessageBox.Show("Lưu thành công!");
                }
            }
        }

        private void Setturn()
        {
            if(Xturn)
            {
                turn.Content = "X";
                turn.Foreground = Brushes.Red;
            }
            else
            {
                turn.Content = "O";
                turn.Foreground = Brushes.Green;
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            if(screen.ShowDialog()==true)
            {
                var FileName = screen.FileName;

                var Reader = new StreamReader(FileName);

                //đọc dòng đầu lấy lượt đi
                var firstline = Reader.ReadLine();
                Xturn = firstline == "X";
                Setturn();

                //đọc dòng kế tiếp lấy biến kiểm tra hòa
                var secondline = Reader.ReadLine();
                Sumcount = int.Parse(secondline);

                //dọc dòng kế tiếp lấy time
                var thirdline= Reader.ReadLine();
                countTime = int.Parse(thirdline);
                //xét màu cho text block
                setLabelTime();

                for (int i = 0; i < Rows; i++)
                {
                    var token = Reader.ReadLine().Split(new string[] { " " }, StringSplitOptions.None);
                    for (int j = 0; j < Cols; j++)
                    {
                        var img = new Image();
                        img.Width = height;
                        img.Height = width;

                        a[i, j] = int.Parse(token[j]);
                        if(a[i,j]==1)
                        {
                            img.Source = new BitmapImage(
                            new Uri("X.png", UriKind.Relative));
                        }
                        if(a[i,j]==2)
                        {
                            img.Source = new BitmapImage(
                            new Uri("O.png", UriKind.Relative));
                        }
                        canvas.Children.Add(img);
                        Canvas.SetLeft(img, xStart + j * width);
                        Canvas.SetTop(img, yStart + i * height);
                    }
                }
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        DispatcherTimer dt = new DispatcherTimer();
        int countTime = 10;
        private void dtTicker(object sender, EventArgs e)
        {

            countTime--;
            setLabelTime();
            if (countTime == 0)
            {
                if (Xturn)
                {
                    dt.Stop();
                    MessageBox.Show("O Won!");
                    Reset();
                }
                else
                {
                    dt.Stop();
                    MessageBox.Show("X Won!");
                    Reset();
                }

            }
        }

        void setLabelTime()
        {
            if (countTime <= 5)
            {
                time.Foreground = Brushes.Red;
            }
            else
            {
                time.Foreground = Brushes.Blue;
            }
            string Time = (countTime).ToString();
            if (countTime < 10)
            {
                Time = "0" + Time;
            }
            time.Text = Time;
        }

        private void PlayTime_Click(object sender, RoutedEventArgs e)
        {
            dt.Start();
        }
    }
}
