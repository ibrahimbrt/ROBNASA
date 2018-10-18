using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WpfRobot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Color GraphColor = Colors.DarkGray; 
        private readonly SolidColorBrush _color1 = new SolidColorBrush(GraphColor); 
        private int _robot1Diretion= (int)DirectionsEnums.N, _robot2Diretion=(int)DirectionsEnums.S;
        private int coordinateFrequency = 10;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            DrawGraph(coordinateFrequency, coordinateFrequency, pnlMain);
            StartSetting();
        }

        private void StartSetting()
        {
            robot1X.Text = robot1Y.Text ="0";
            robot2X.Text = "0"; robot2Y.Text = "40";
            Robot1cmb.ItemsSource= Enum.GetValues(typeof(DirectionsEnums)).Cast<DirectionsEnums>().ToList<DirectionsEnums>();
            Robot2cmb.ItemsSource = Enum.GetValues(typeof(DirectionsEnums)).Cast<DirectionsEnums>().ToList<DirectionsEnums>();
            Robot1cmb.SelectedValue = DirectionsEnums.N;
            Robot2cmb.SelectedValue = DirectionsEnums.S;
        }

        private void DrawGraph(int yoffSet, int xoffSet, Canvas mainCanvas)
        {
            RemoveGraph(mainCanvas);
            var lines = new Image();
            lines.SetValue(Panel.ZIndexProperty, -100);
            //Draw the canvas        
            var gridLinesVisual = new DrawingVisual();
            var dct = gridLinesVisual.RenderOpen();
            var pen = new Pen(_color1, 0.5);
            pen.Freeze(); 

            int yOffset = yoffSet,
                xOffset = xoffSet,
                 rows = (int)mainCanvas.ActualHeight,
                columns = (int)mainCanvas.ActualWidth,
                alternate = yOffset == 5 ? yOffset : 1,
                j = 0;

            //Draw the horizontal lines        
            var x = new Point(0, 0.5);
            var y = new Point(SystemParameters.PrimaryScreenWidth, 0.5);

            for (var i = 0; i <= rows; i++, j++)
            {
                dct.DrawLine(pen, x, y);
                x.Offset(0, yOffset);
                y.Offset(0, yOffset);
            }
            j = 0;
            //Draw the vertical lines        
            x = new Point(0.5, 0);
            y = new Point(0.5, SystemParameters.PrimaryScreenHeight);

            for (var i = 0; i <= columns; i++, j++)
            {
                dct.DrawLine(pen, x, y);
                x.Offset(xOffset, 0);
                y.Offset(xOffset, 0);
            }

            dct.Close();

            var bmp = new RenderTargetBitmap((int)mainCanvas.ActualWidth,(int)mainCanvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(gridLinesVisual);
            bmp.Freeze();
            lines.Source = bmp;
            mainCanvas.Children.Add(lines);

        }

        private void RemoveGraph(Canvas mainCanvas)
        {
            try
            {
                foreach (UIElement obj in mainCanvas.Children)
                {
                    if (obj is Image)
                    {
                        mainCanvas.Children.Remove(obj);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
               
            }
           
        }

        private void Rec_OnKeyDown(object sender, KeyEventArgs e)
        {

            if (rdyRobot1.IsChecked == true) Robot1Move(e); else Robot2Move(e); 
        }

        private void Robot2Move(KeyEventArgs e)
        {
           
            var top = Convert.ToInt32(Canvas.GetTop(robot2));
            var left = Convert.ToInt32(Canvas.GetLeft(robot2));
            var X = (left / 10);
            var Y = (400 - top) / 10;
           

            if (e.Key == Key.L)
            {
                Direction.GetAngle(out _robot2Diretion, _robot2Diretion, "L");
                var rt = new RotateTransform(_robot2Diretion);
                robot2.RenderTransform = rt;
                txtinput.Text = txtinput.Text + "\n" + " L";
            }
            else if (e.Key == Key.R)
            {
                Direction.GetAngle(out _robot2Diretion, _robot2Diretion, "R");
                var rt = new RotateTransform(_robot2Diretion);
                robot2.RenderTransform = rt;
                txtinput.Text = txtinput.Text + "\n" + " R";
            }
            else if (e.Key == Key.M)
            {
                var xlimit = 600 / coordinateFrequency;
                var ylimit = 400 / coordinateFrequency;
                if (X >= 0 && X <= xlimit && Y >= 0 && Y <= ylimit)
                {
                    if (_robot2Diretion == (int)DirectionsEnums.N) Canvas.SetTop(robot2, top - 10);
                    else if (_robot2Diretion == (int)DirectionsEnums.S) Canvas.SetTop(robot2, top + 10);
                    else if (_robot2Diretion == (int)DirectionsEnums.W) Canvas.SetLeft(robot2, left + 10);
                    else if (_robot2Diretion == (int)DirectionsEnums.E) Canvas.SetLeft(robot2, left - 10);
                    CheckNewCoordinate(robot2);
                }
            }
            txtNewCoordinate2.Text =X +" "+Y ;
            txtDirecktion2.Text = _robot2Diretion == (int)DirectionsEnums.N ? "N" : (_robot2Diretion == (int)DirectionsEnums.W ? "E" : (_robot2Diretion == (int)DirectionsEnums.S ? "S" : "W"));

        }

        private void Robot1Move(KeyEventArgs e)
        {
           
           var top = Convert.ToInt32(Canvas.GetTop(robot1));
           var left = Convert.ToInt32(Canvas.GetLeft(robot1));
            var X = (left / coordinateFrequency);
            var Y = (400 - top) / coordinateFrequency;

            if (e.Key == Key.L)
            {
                
                Direction.GetAngle(out _robot1Diretion, _robot1Diretion, "L");
                var rt = new RotateTransform(_robot1Diretion);
                robot1.RenderTransform = rt;
                txtinput.Text = txtinput.Text+"\n"+" L";
            }
            else if (e.Key == Key.R)
            {
                Direction.GetAngle(out _robot1Diretion, _robot1Diretion, "R");
                var rt = new RotateTransform(_robot1Diretion);
                robot1.RenderTransform = rt;
                txtinput.Text = txtinput.Text + "\n" + " R";
            }
            else if (e.Key == Key.M)
            {
                var xlimit = 600 / coordinateFrequency;
                var ylimit = 400 / coordinateFrequency;
                if (X >= 0 && X <= xlimit && Y >= 0 && Y <= ylimit)
                {
                    if (_robot1Diretion == (int)DirectionsEnums.N) Canvas.SetTop(robot1, top - coordinateFrequency);
                    else if (_robot1Diretion == (int)DirectionsEnums.S) Canvas.SetTop(robot1, top + coordinateFrequency);
                    else if (_robot1Diretion == (int)DirectionsEnums.W) Canvas.SetLeft(robot1, left + coordinateFrequency);
                    else if (_robot1Diretion == (int)DirectionsEnums.E) Canvas.SetLeft(robot1, left - coordinateFrequency);
                    txtinput.Text = txtinput.Text + "\n" + txtNewCoordinate.Text + " " + txtDirecktion.Text + " M"; 
                    CheckNewCoordinate(robot1);
                }

            }

            txtNewCoordinate.Text = X + " " + Y;
            txtDirecktion.Text = _robot1Diretion == (int)DirectionsEnums.N ? "N" : (_robot1Diretion == (int)DirectionsEnums.W ? "E" : (_robot1Diretion == (int)DirectionsEnums.S ? "S" : "W"));
        }

        private void CheckNewCoordinate(Polyline robot)
        {
            var xlimit = 600 / coordinateFrequency;
            var ylimit = 400 / coordinateFrequency;
            var top = Convert.ToInt32(Canvas.GetTop(robot));
            var left = Convert.ToInt32(Canvas.GetLeft(robot));
            var X = (left / coordinateFrequency);
            var Y = (400 - top) / coordinateFrequency;
            if (X < 0) Canvas.SetLeft(robot, 0);
            if (X > xlimit) Canvas.SetLeft(robot, 600);
            if (Y < 0) Canvas.SetTop(robot, 400);
            if (Y > ylimit) Canvas.SetTop(robot, 0);
        }

        #region Start
        private void SliderValue_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                coordinateFrequency = (int)SliderValue.Value;
                DrawGraph(coordinateFrequency, coordinateFrequency, pnlMain);
                txtCord.Text = 600 / coordinateFrequency + " " + 400 / coordinateFrequency;
            }
            catch (Exception exception)
            {

            }


            //  SetRobotsNewCoordinate(robot1);
        }

        private void btnSaveSetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SliderValue.Value = 10;
                SetRobotCoordinateRobot1();
                SetRobotCoordinateRobot2();
                grdSetting.Visibility = Visibility.Collapsed;
               
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void SetRobotCoordinateRobot1()
        {
            var angle = (int)((DirectionsEnums)Robot1cmb.SelectedItem);
            var rt = new RotateTransform(angle);
            robot1.RenderTransform = rt;
            var x = int.Parse(robot1X.Text) * coordinateFrequency;
            var y = 400 - (int.Parse(robot1Y.Text) * coordinateFrequency);
            Canvas.SetTop(robot1, y);
            Canvas.SetLeft(robot1, x);
            txtNewCoordinate.Text = robot1X.Text + " " + robot1Y.Text;
            txtDirecktion.Text = angle == (int)DirectionsEnums.N ? "N" : (angle == (int)DirectionsEnums.W ? "E" : (angle == (int)DirectionsEnums.S ? "S" : "W"));
            _robot1Diretion = angle;
        }

        private void SetRobotCoordinateRobot2()
        {
            var angle = (int)((DirectionsEnums)Robot2cmb.SelectedItem);
            var rt = new RotateTransform((int)angle);
            robot2.RenderTransform = rt;
            var x = int.Parse(robot2X.Text) * coordinateFrequency;
            var y = 400 - (int.Parse(robot2Y.Text) * coordinateFrequency);
            Canvas.SetTop(robot2, y);
            Canvas.SetLeft(robot2, x);
            txtNewCoordinate2.Text = robot2X.Text + " " + robot2Y.Text;
            txtDirecktion2.Text = angle == (int)DirectionsEnums.N ? "N" : (angle == (int)DirectionsEnums.W ? "E" : (angle == (int)DirectionsEnums.S ? "S" : "W"));
            _robot2Diretion = angle;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            grdSetting.Visibility = Visibility.Visible;
            btncancel.Visibility = Visibility.Visible;
        }

        private void Number_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Btncancel_OnClick(object sender, RoutedEventArgs e)
        {
            grdSetting.Visibility = Visibility.Collapsed;
        }

        #endregion


    }
}
