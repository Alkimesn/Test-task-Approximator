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

namespace Approximator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int userpointnum = 5;
        TextBox[] tboxesedit = new TextBox[userpointnum];
        Ellipse[] userpoints = new Ellipse[userpointnum];
        double[] uservals = new double[userpointnum];
        double pointSize = 10;
        Brush pointBrush = new SolidColorBrush(Colors.Purple);
        Brush lineBrush = new SolidColorBrush(Colors.Black);
        Brush lagrbrush = new SolidColorBrush(Colors.Orange);
        Brush sqbrush = new SolidColorBrush(Colors.Red);
        Func<double, double> XConvert, YConvert, XConvertBack, YConvertBack;
        Func<double, double, Point> PConvert;

        int ymax = 10;
        int ymin = -10;
        int xmax = 7;
        int xmin = -1;

        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Tag == null) return;
            int i = 0;
            if(!int.TryParse((string)tb.Tag, out i)) return;
            double val = 0;
            if (!double.TryParse(tb.Text, out val)) return;
            if(val!=uservals[i])
            {
                uservals[i] = double.Parse(tb.Text);
                DrawPoints();
            }
        }
        Ellipse DraggedPoint = null;
        bool IsMouseDragged = false;
        Point startshift = new Point(0,0);
        private void Cv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var el = Mouse.DirectlyOver;
            for (int i=0;i<userpointnum;i++)
                if(el==userpoints[i])
                {
                    IsMouseDragged = true;
                    DraggedPoint = userpoints[i];
                }
        }

        private void Cv_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsMouseDragged = false;
        }

        private void Cv_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsMouseDragged) return;
            Point newshift = e.GetPosition(DraggedPoint);
            double shift = newshift.Y - startshift.Y;
            double shiftconv = (YConvertBack(shift) - YConvertBack(0));
            int i = (int)(DraggedPoint.Tag);
            tboxesedit[i].Text = (uservals[i] + shiftconv).ToString("N12");
        }

        public MainWindow()
        {
            InitializeComponent();
            tboxesedit[0] = tb1;
            tboxesedit[1] = tb2;
            tboxesedit[2] = tb3;
            tboxesedit[3] = tb4;
            tboxesedit[4] = tb5;
            for(int i=0;i<userpointnum; i++)
            {
                userpoints[i] = new Ellipse
                {
                    Height = pointSize,
                    Width = pointSize,
                    Fill = pointBrush,
                    Tag = i
                };
                Canvas.SetZIndex(userpoints[i], 5);
            }
            this.Loaded += (s, e) => DrawStart();
            //DrawStart();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Polynomial lagr = LagrApprox.GetApprox(new double[] { 1, 2, 3, 4, 5 }, uservals);
            Polynomial squares=LeastSquaresApprox.GetApprox(new double[] { 1, 2, 3, 4, 5 }, uservals);
            DrawStart();
            DrawGraph(lagr, lagrbrush);
            DrawGraph(squares, sqbrush);
            DrawPoints();
            tb0L.Text = lagr.Calculate(0).ToString("N12");
            tb0S.Text = squares.Calculate(0).ToString("N12");
            tb6L.Text = lagr.Calculate(6).ToString("N12");
            tb6S.Text = squares.Calculate(6).ToString("N12");
        }

        void DrawStart()
        {
            cv.Children.Clear();
            Rectangle rect = new Rectangle()
            {
                Height = cv.ActualHeight,
                Width = cv.ActualWidth,
                Fill=Brushes.White
            };
            //Canvas.SetLeft(rect, 0);
            //Canvas.SetTop(rect, 0);
            cv.Children.Add(rect);
            double xscale = cv.ActualWidth / (xmax-xmin+1);
            double yscale = cv.ActualHeight / (ymax-ymin+1);
            XConvert = x => xscale * (x - xmin+0.5);
            YConvert = y => yscale * (ymax - y+0.5);
            XConvertBack = x => x / xscale + xmin;
            YConvertBack = y => ymax - y / yscale;
            PConvert = (x, y) => new Point(XConvert(x),YConvert(y));
            Line xaxis = new Line()
            {
                X1 = 0,
                X2 = cv.ActualWidth,
                Y1 = YConvert(0),
                Y2 = YConvert(0),
                StrokeThickness=2,
                Stroke=lineBrush
            };
            cv.Children.Add(xaxis);
            Line yaxis = new Line()
            {
                X1 = XConvert(0),
                X2 = XConvert(0),
                Y1 = 0,
                Y2 = cv.ActualWidth,
                StrokeThickness = 2,
                Stroke = lineBrush
            };
            cv.Children.Add(yaxis);
            for(int i=xmin; i<=xmax;i++)
            {
                if (i == 0) continue;
                Line l = new Line()
                {
                    X1 = XConvert(i),
                    X2 = XConvert(i),
                    Y1 = YConvert(0.2),
                    Y2 = YConvert(-0.2),
                    Stroke = lineBrush,
                    StrokeThickness = 1
                };
                cv.Children.Add(l);
                TextBlock tb = new TextBlock()
                {
                    Text = i.ToString()
                };
                Canvas.SetLeft(tb, XConvert(i)-3);
                Canvas.SetTop(tb, YConvert(-0.5));
                cv.Children.Add(tb);
            }
            for (int i = ymin; i <= ymax; i++)
            {
                if (i == 0) continue;
                Line l = new Line()
                {
                    X1 = XConvert(-0.2),
                    X2 = XConvert(0.2),
                    Y1 = YConvert(i),
                    Y2 = YConvert(i),
                    Stroke = lineBrush,
                    StrokeThickness = 1
                };
                cv.Children.Add(l);
                TextBlock tb = new TextBlock()
                {
                    Text = i.ToString()
                };
                Canvas.SetLeft(tb, XConvert(0.3));
                Canvas.SetTop(tb, YConvert(i)-6);
                cv.Children.Add(tb);
            }
            foreach (var el in userpoints)
                cv.Children.Add(el);
            DrawPoints();
        }
        void DrawPoints()
        {
            for(int i=0;i<userpointnum;i++)
            {
                Canvas.SetLeft(userpoints[i], XConvert(i + 1) - pointSize / 2);
                Canvas.SetTop(userpoints[i], YConvert(uservals[i]) - pointSize / 2);
            }
        }
        void DrawGraph(Polynomial gr, Brush br)
        {
            double step = 0.01;
            Point last = new Point(0, 0);
            bool IsFirst = true;
            List<Line> lines = new List<Line>();
            for (double x = xmin; x <= xmax; x += step)
                {
                    double y = gr.Calculate(x);
                    Point p = PConvert(x, y);
                    if (!IsFirst)
                    {
                        Line l = new Line()
                        {
                            X1 = last.X,
                            X2 = p.X,
                            Y1 = last.Y,
                            Y2 = p.Y,
                            Stroke = br,
                            StrokeThickness = 1
                        };
                        lines.Add(l);
                    }
                    last = p;
                    IsFirst = false;
                }
            foreach (var l in lines)
                cv.Children.Add(l);
        }
    }
}
