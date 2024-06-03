using PhotoEnhancer.Filters.Transformations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoEnhancer
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new MainForm();

            mainForm.AddFilter(new PixelFilter<LighteningParameters>(
                "Осветление/затемнение",
                (p, parameters) => p * parameters.Coefficient
                ));

            mainForm.AddFilter(new PixelFilter<EmptyParameters>(
                "Оттенки серого",
                (p, parameters) =>
                {
                    var lightness = 0.3 * p.R + 0.6 * p.G + 0.1 * p.B;
                    return new Pixel(lightness, lightness, lightness);
                }
                ));


            mainForm.AddFilter(new PixelFilter<ContrastParameters>(
                "Контраст",
                (p, parameters) =>
                {
                    double ContrastFunction(double color, double k)
                    {
                        return TrimChannel(k * (color - 0.5) + 0.5);
                    }
                    double TrimChannel(double channel)
                    {
                        if (channel < 0) return 0;

                        if (channel > 1) return 1;

                        return channel;
                    }
                    return new Pixel(ContrastFunction(p.R, parameters.Coefficient), ContrastFunction(p.G, parameters.Coefficient), ContrastFunction(p.B, parameters.Coefficient));

                }
                ));

            mainForm.AddFilter(new PixelFilter<HueParameters>(
                "Оттенок",
                (p, parameters) =>
                {
                    var q = Convertors.RGBToHSL(p);

                    var hue = q.H * 360 + parameters.Shift;

                    if (hue >= 360)
                        hue -= 360;

                    return Convertors.HSLToRGB(new PixelHSL(hue / 360, q.S, q.L));
                }
                ));

            mainForm.AddFilter(new TransformFilter(
                "Отражение по горизонтали",
                size => size,
                (point, size) => new Point(size.Width - point.X - 1, point.Y)
                ));

            mainForm.AddFilter(new TransformFilter(
                "Поворот на 90° против ч.с.",
                size => new Size(size.Height, size.Width),
                (point, size) => new Point(size.Width - point.Y - 1, point.X)
                ));

            mainForm.AddFilter(new TransformFilter<RotationParameters>(
                "Поворот на произвольный угол", new RotationTransformer()));

            Application.Run(mainForm);
        }
    }
}
