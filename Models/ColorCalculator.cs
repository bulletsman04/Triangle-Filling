using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ColorCalculator
    {
        public System.Drawing.Color CalculateColor(int x, int y, Triangle triangle, Settings _settings)
        {
            Vector3 L;
            if (_settings.IsLightMouse || _settings.IsLightAnimation)
            {
                L = new Vector3(_settings.LightPoint.X - x, _settings.LightPoint.Y + y,
                     _settings.LightPoint.Z);
            }
            else
            {
                L = new Vector3(0, 0, 1);
            }

            L = Vector3.Normalize(L);

            Vector3 IL = _settings.LightColor;

            Vector3 IO;
            if (triangle.TriangleSettings.IsColor)
            {
                IO = triangle.TriangleSettings.PickedColor;
            }
            else
            {
                // ToDo: tutaj trzeba rozpatrzeć że modulo wychodzi ujemne
                IO = triangle.TriangleSettings.PickedTriangleTexture[
                    (x - triangle.MoveVector.Coordinates.X) % triangle.TriangleSettings.PickedTriangleTexture.GetLength(0),
                    (y - triangle.MoveVector.Coordinates.Y) % triangle.TriangleSettings.PickedTriangleTexture.GetLength(1)];
            }

            Vector3 N = _settings.NMap[x, y];

            double cosVR = 0;

            if (_settings.IsPhong)
            {
                Vector3 V = new Vector3(0, 0, 1);
                Vector3 RV = Vector3.Normalize(2 * N - L);

                cosVR = Math.Pow(V.X * RV.X + V.Y * RV.Y + V.Z * RV.Z, _settings.MPhong);
                if (cosVR < 0) cosVR = 0;
            }

            double cosLN = N.X * L.X + N.Y * L.Y + N.Z * L.Z;
            double R = IL.X * (_settings.LambertRate * IO.X * cosLN + _settings.PhongRate * cosVR);
            double G = IL.Y * (_settings.LambertRate * IO.Y * cosLN + _settings.PhongRate * cosVR);
            double B = IL.Z * (_settings.LambertRate * IO.Z * cosLN + _settings.PhongRate * cosVR);

            if (R < 0) R = 0;
            if (R > 1) R = 1;
            if (G < 0) G = 0;
            if (G > 1) G = 1;
            if (B < 0) B = 0;
            if (B > 1) B = 1;
            return Color.FromArgb((byte)Math.Round(255 * R), (byte)Math.Round(255 * G), (byte)Math.Round(255 * B));

        }
    }
}
