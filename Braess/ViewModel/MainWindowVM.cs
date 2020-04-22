namespace Braess.ViewModel
{
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
        }

        public double MouseX { get; set; }

        public double MouseY { get; set; }

        public string Title => "Braess Paradox";

        public SolidColorBrush LineColor => Brushes.Blue;

        public double LineWidth => 2;

        public SolidColorBrush PointColor => Brushes.Black;

        public double PointDiameter => 10;

        public SolidColorBrush SelectedPointColor => Brushes.Orange;

        public double SelectedPointDiameter => 16;

        public ReadOnlyCollection<Point> Points => canvas.Points;

        public ReadOnlyCollection<Line> Lines => canvas.Lines;

        public Point SelectedPoint => canvas.SelectedPoint;

        public ICommand MouseClickedCommand => new RelayCommand(MouseClicked);

        public void MouseClicked()
        {
            Point mousePoint = new Point(MouseX, MouseY);
            bool isShiftDown = Keyboard.IsKeyDown(Key.LeftShift);
            bool isCtrlDown = Keyboard.IsKeyDown(Key.LeftCtrl);

            if (isShiftDown && !isCtrlDown)
            {
                canvas.AddPoint(mousePoint);
            }
            else if (isCtrlDown && !isShiftDown)
            {
                canvas.RemoveClosestPoint(mousePoint);
            }
            else
            {
                canvas.Process(mousePoint);
            }
        }

        private void Canvas_SelectedPointChanged(object sender, System.EventArgs e)
        {
            OnPropertyChanged(nameof(SelectedPoint));
        }
    }
}
