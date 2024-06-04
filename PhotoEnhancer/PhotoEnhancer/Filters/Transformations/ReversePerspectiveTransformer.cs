using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoEnhancer
{
    public class ReversePerspectiveTransformer : ITransformer<ReversePerspectiveParameters>
    {
        double narrowPercent;
        Size oldSize;
        public Size ResultSize { get; private set; }

        public void Initialize(Size oldSize, ReversePerspectiveParameters parameters)
        {
            this.oldSize = oldSize;
            narrowPercent = parameters.Percentage / 100.0;
            ResultSize = oldSize;
        }

        public Point? MapPoint(Point newPoint)
        {
            int halfOfWidth = oldSize.Width / 2;
            double t = (double)newPoint.Y / oldSize.Height;
            double k = 1.0 - t * (1.0 - narrowPercent);

            int x = (int)((newPoint.X - halfOfWidth) / k + halfOfWidth);
            int y = newPoint.Y;

            if (x < 0 || x >= oldSize.Width || y < 0 || y >= oldSize.Height)
                return null;

            return new Point(x, y);
        }
    }
}
