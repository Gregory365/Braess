namespace Braess.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Braess.Model;
    using Braess.ViewModel.Tools;

    public class MainWindowVM : BaseVM
    {
        private readonly Canvas canvas;

        public MainWindowVM()
        {
            canvas = new Canvas(10, Brushes.Black, 16, Brushes.Orange, 2, Brushes.Blue);
            canvas.SelectedCircleChanged += Canvas_SelectedCircleChanged;
        }

        public double MouseX { get; set; }

        public double MouseY { get; set; }

        public string Title => "Braess Paradox";

        public ReadOnlyCollection<Circle> Circles => canvas.Circles;

        public ReadOnlyCollection<Line> Lines => canvas.Lines;

        public Circle SelectedCircle => canvas.SelectedCircle;

        public ICommand MouseClickedCommand => new RelayCommand(MouseClicked);

        public void MouseClicked()
        {
            Circle closestCircle = canvas.GetClosestCircle(new Point(MouseX, MouseY));

            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                canvas.AddCircle(new Point(MouseX, MouseY));
            }
            else if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                canvas.RemoveCircle(closestCircle);
            }
            else
            {
                canvas.Process(closestCircle);
            }
        }

        private void Canvas_SelectedCircleChanged(object sender, System.EventArgs e)
        {
            OnPropertyChanged(nameof(SelectedCircle));
        }
    }
}
