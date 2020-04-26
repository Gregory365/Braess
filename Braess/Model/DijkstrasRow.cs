namespace Braess.Model
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Braess.Model.Tools;
    using MoreLinq;

    public class DijkstrasRow
    {
        public bool Visited { get; set; } = false;

        public double Distance { get; set; } = double.MaxValue;

        public Point PreviousPoint { get; set; }

        public Line Line { get; set; }
    }
}
