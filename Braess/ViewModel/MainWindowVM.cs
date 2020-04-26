namespace Braess.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using System.Windows.Media;
    using Braess.Model;
    using Braess.ViewModel.Tools;

    public class MainWindowVM : BaseVM
    {
        private readonly Canvas canvas;

        public MainWindowVM()
        {
            canvas = new Canvas();
            canvas.SelectedPointChanged += Canvas_SelectedPointChanged;
            canvas.SelectedLineChanged += Canvas_SelectedLineChanged;
        }

        public double? TotalTime => canvas.TotalTime;

        public double? Population
        {
            get
            {
                return SelectedPoint?.Population;
            }

            set
            {
                SelectedPoint.Population = value.Value;
                SelectedPointChanged();
            }
        }

        public double? NumberOfCars
        {
            get
            {
                return SelectedLine?.NumberOfCars;
            }

            set
            {
                SelectedLine.NumberOfCars = value.Value;
                SelectedLineChanged();
            }
        }

        public double? Lanes
        {
            get
            {
                return SelectedLine?.Lanes;
            }

            set
            {
                SelectedLine.Lanes = value.Value;
                SelectedLineChanged();
            }
        }

        public double? SpeedLimit
        {
            get
            {
                return SelectedLine?.SpeedLimit;
            }

            set
            {
                SelectedLine.SpeedLimit = value.Value;
                SelectedLineChanged();
            }
        }

        public double? Delay
        {
            get
            {
                return SelectedLine?.Delay;
            }

            set
            {
                SelectedLine.Delay = value.Value;
                SelectedLineChanged();
            }
        }

        public double? Length => SelectedLine?.Length;

        public double? Time => SelectedLine?.Time;

        public double MouseX { get; set; }

        public double MouseY { get; set; }

        public string Title => "Braess Paradox";

        public SolidColorBrush LineColor => Brushes.LightBlue;

        public double LineWidth => 2;

        public SolidColorBrush PointColor => Brushes.Black;

        public double PointDiameter => 10;

        public SolidColorBrush SelectedPointColor => Brushes.Orange;

        public double SelectedPointDiameter => 16;

        public SolidColorBrush SelectedLineColor => Brushes.Blue;

        public double SelectedLineWidth => 5;

        public ReadOnlyCollection<Point> Points => canvas.Points;

        public ReadOnlyCollection<Line> Lines => canvas.Lines;

        public Point SelectedPoint => canvas.SelectedPoint;

        public Line SelectedLine => canvas.SelectedLine;

        public ICommand MouseClickedCommand => new RelayCommand(() => MouseClicked());

        public ICommand FindAllRoutesCommand => new RelayCommand(() =>
        {
            canvas.FindAllRoutes();
            OnPropertyChanged(nameof(TotalTime));
        });

        public ICommand FindBestRoutesCommand => new RelayCommand(() =>
        {
            canvas.FindBestRoutes();
            OnPropertyChanged(nameof(TotalTime));
        });

        public void MouseClicked()
        {
            Point mousePoint = new Point(MouseX, MouseY);
            bool isShiftDown = Keyboard.IsKeyDown(Key.LeftShift);
            bool isCtrlDown = Keyboard.IsKeyDown(Key.LeftCtrl);

            if (isShiftDown)
            {
                if (isCtrlDown)
                {
                    canvas.RemoveClosestPoint(mousePoint);
                }
                else
                {
                    canvas.AddPoint(mousePoint);
                }
            }
            else
            {
                if (isCtrlDown)
                {
                    canvas.ProcessLine(mousePoint);
                }
                else
                {
                    canvas.ProcessPoint(mousePoint);
                }
            }
        }

        private void Canvas_SelectedPointChanged(object sender, System.EventArgs e)
        {
            SelectedPointChanged();
        }

        private void SelectedPointChanged()
        {
            OnPropertyChanged(nameof(Points));
            OnPropertyChanged(nameof(SelectedPoint));
            OnPropertyChanged(nameof(Population));
            OnPropertyChanged(nameof(TotalTime));
        }

        private void Canvas_SelectedLineChanged(object sender, System.EventArgs e)
        {
            SelectedLineChanged();
        }

        private void SelectedLineChanged()
        {
            OnPropertyChanged(nameof(Lines));
            OnPropertyChanged(nameof(SelectedLine));
            OnPropertyChanged(nameof(NumberOfCars));
            OnPropertyChanged(nameof(Lanes));
            OnPropertyChanged(nameof(SpeedLimit));
            OnPropertyChanged(nameof(Delay));
            OnPropertyChanged(nameof(Length));
            OnPropertyChanged(nameof(Time));
            OnPropertyChanged(nameof(TotalTime));
        }
    }
}
