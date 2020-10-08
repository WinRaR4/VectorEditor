using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using Brush = System.Windows.Media.Brush;
using Point = System.Windows.Point;
using System.Windows.Forms.DataVisualization.Charting;
using System;
using Color = System.Windows.Media.Color;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        int width = 0;
        public struct Viewport { double x, y, scale; }
        Viewport vp = new Viewport();

        WriteableBitmap Paint = BitmapFactory.New(1078, 601);
        WriteableBitmap PaintSmall = BitmapFactory.New(42, 41);
        WriteableBitmap buffer;
        Point point;
        System.Drawing.Color color = System.Drawing.Color.Black;
        System.Windows.Media.Color mediaColor = System.Windows.Media.Color.FromArgb(255, 0, 0, 0);
        System.Windows.Media.Color rightColor = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);

        bool clicked = false, rightClick = false;
        enum Tools { Pencil = 0, Quad = 1, Line = 2, Ellipse = 3, Triangle = 4, Zoom = 5 };
        Tools tool = Tools.Ellipse;


        public MainWindow()
        {
            InitializeComponent();
            icon.Source = Paint;
            Paint.Clear(Colors.White);
            PaletteColor.Source = PaintSmall;
            PaintSmall.Clear(Colors.Black);
        }


        private void Mouse_Move(object sender, System.Windows.Input.MouseEventArgs e)
        {
            switch (tool)
            {
                case Tools.Pencil:
                    {
                        if (clicked)
                        {
                            Point P = e.GetPosition(icon);
                            for (int i = 0; i < width; i++)
                            {
                                Paint.DrawLine((int)point.X, (int)point.Y + i, (int)P.X, (int)P.Y - i, mediaColor);
                                Paint.DrawLine((int)point.X - i, (int)point.Y, (int)P.X + i, (int)P.Y, mediaColor);
                            } // Это рисовка одинаковых линий рядом
                            if (rightClick)
                                for (int i = 0; i < width; i++)
                                {
                                    Paint.DrawLine((int)point.X, (int)point.Y + i, (int)P.X, (int)P.Y - i, rightColor);
                                    Paint.DrawLine((int)point.X - i, (int)point.Y, (int)P.X + i, (int)P.Y, rightColor);
                                }
                            point.X = P.X;
                            point.Y = P.Y;
                            if (!clicked)
                            {
                                if (buffer != null) Paint = buffer.Clone();
                                icon.Source = Paint;
                            }
                        }
                        break;
                    }
                case Tools.Ellipse:
                    {
                        if (clicked)
                        {
                            Point P = e.GetPosition(icon);
                            if (buffer != null) Paint = buffer.Clone();
                                icon.Source = Paint;
                            if (!rightClick)

                                for (int i = 0; i < width; i++)
                                {
                                    if (P.X <= point.X && P.Y <= point.Y)
                                    {
                                        Paint.DrawEllipse((int)P.X, (int)P.Y - i, (int)point.X, (int)point.Y + i, mediaColor);
                                        Paint.DrawEllipse((int)P.X + i, (int)P.Y, (int)point.X - i, (int)point.Y, mediaColor);
                                    }
                                    else
                                        if (P.X >= point.X && P.Y <= point.Y)
                                    {
                                        Paint.DrawEllipse((int)point.X, (int)P.Y - i, (int)P.X, (int)point.Y + i, mediaColor);
                                        Paint.DrawEllipse((int)point.X - i, (int)P.Y, (int)P.X + i, (int)point.Y, mediaColor);
                                    }
                                    else
                                        if (P.X <= point.X && P.Y >= point.Y)
                                    {
                                        Paint.DrawEllipse((int)point.X, (int)P.Y - i, (int)P.X, (int)point.Y + i, mediaColor);
                                        Paint.DrawEllipse((int)point.X - i, (int)P.Y, (int)P.X + i, (int)point.Y, mediaColor);
                                    }
                                    else
                                    {
                                        Paint.DrawEllipse((int)point.X, (int)point.Y + i, (int)P.X, (int)P.Y - i, mediaColor);
                                        Paint.DrawEllipse((int)point.X - i, (int)point.Y, (int)P.X + i, (int)P.Y, mediaColor);
                                    }
                                }
                            if (rightClick)
                                for (int i = 0; i < width; i++)
                                {
                                    Paint.DrawEllipse((int)point.X + i, (int)point.Y + i, (int)P.X - i, (int)P.Y - i, rightColor);
                                    Paint.DrawEllipse((int)point.X - i, (int)point.Y - i, (int)P.X + i, (int)P.Y + i, rightColor);
                                }
                        }
                        break;
                    }

                case Tools.Quad:
                    {
                        if (clicked)
                        {
                            Point P = e.GetPosition(icon);
                            if (buffer != null) Paint = buffer.Clone();
                            icon.Source = Paint;
                            if (!rightClick)
                                for (int i = 0; i < width; i++)
                                {
                                    Paint.DrawRectangle((int)point.X, (int)point.Y + i, (int)P.X, (int)P.Y - i, mediaColor);
                                    Paint.DrawRectangle((int)point.X - i, (int)point.Y, (int)P.X + i, (int)P.Y, mediaColor);
                                }
                        }
                        break;
                    }

                case Tools.Triangle:
                    {
                        if (clicked)
                        {
                            Paint.Clear(Colors.Red);
                            icon.Source = Paint;
                        }
                        break;
                    }

                case Tools.Line:
                    {
                        if (clicked)
                        {
                            Point P = e.GetPosition(icon);
                            if (buffer != null) Paint = buffer.Clone();
                            icon.Source = Paint;
                            if (!rightClick)

                                for (int i = 0; i < width; i++)
                                {
                                    {
                                        Paint.DrawLineAa((int)point.X, (int)point.Y + i, (int)P.X, (int)P.Y - i, mediaColor);
                                        Paint.DrawLineAa((int)point.X - i, (int)point.Y, (int)P.X + i, (int)P.Y, mediaColor);
                                    }
                                }
                            if (rightClick)
                                for (int i = 0; i < width; i++)
                                {
                                    Paint.DrawEllipse((int)point.X + i, (int)point.Y + i, (int)P.X - i, (int)P.Y - i, rightColor);
                                    Paint.DrawEllipse((int)point.X - i, (int)point.Y - i, (int)P.X + i, (int)P.Y + i, rightColor);
                                }
                        }
                        break;
                    }
                case Tools.Zoom:
                    {
                        buffer = Paint;
                        if (clicked)
                        {
                            Point P = e.GetPosition(icon);
                            Paint.DrawRectangle((int)point.X, (int)point.Y, (int)P.X, (int)P.Y, Color.FromArgb(0,0,0,0));
                            Paint = BitmapFactory.New((int)P.X, (int)P.Y).Clone();
                            
                            icon.Source = Paint;
                        }
                        else
                            if (rightClick)
                        {
                            icon.Source = buffer;
                        }
                    }
                    break;
            }
        }


        private void WidthOf_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            width = (int)WidthOf.Value;
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            Paint.Clear(Colors.White);
            if (buffer != null) buffer = Paint;
            clicked = false;
            rightClick = false;
        }

        private void Click(object sender, MouseButtonEventArgs e)
        {
            point = e.GetPosition(icon);
            buffer = Paint.Clone();
            clicked = true;
        }

        private void UnClick(object sender, MouseButtonEventArgs e)
        {
            clicked = false;
        }

        private void RightClick(object sender, MouseButtonEventArgs e)
        {
            rightClick = true;
        }

        private void RightUnClick(object sender, MouseButtonEventArgs e)
        {
            rightClick = false;
        }

        private void Palette_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog Palette = new ColorDialog();
            DialogResult Col = Palette.ShowDialog();
            if (Col == System.Windows.Forms.DialogResult.OK) color = Palette.Color;
            mediaColor.R = color.R;
            mediaColor.G = color.G;
            mediaColor.B = color.B;
            rightColor.R = (byte)(255-color.R);
            rightColor.G = (byte)(255-color.G);
            rightColor.B = (byte)(255-color.B);
            PaintSmall.Clear(mediaColor);
        }

        private void Pencil(object sender, RoutedEventArgs e)
        {
            tool = Tools.Pencil;
        }
        private void Line(object sender, RoutedEventArgs e)
        {
            tool = Tools.Line;
        }
        private void Ellipse(object sender, RoutedEventArgs e)
        {
            tool = Tools.Ellipse;
        }
        private void Quad(object sender, RoutedEventArgs e)
        {
            tool = Tools.Quad;
        }

        private void Zoomer(object sender, RoutedEventArgs e)
        {
            tool = Tools.Zoom;
        }

        private void Triangle(object sender, RoutedEventArgs e)
        {
            tool = Tools.Triangle;
        }
    }
}
