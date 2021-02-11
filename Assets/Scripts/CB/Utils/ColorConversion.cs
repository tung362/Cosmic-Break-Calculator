using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace CB.Utils
{
    /// <summary>
    /// Color conversion calculation utilities
    /// </summary>
    public static class ColorConversion
    {
        #region Format
        public struct HSVColor
        {
            public double H; //0 - 360
            public double S; //0 - 1
            public double V; //0 - 1

            public float NormalizedH
            {
                get
                {
                    return (float)H / 360f;
                }

                set
                {
                    H = (double)value * 360;
                }
            }

            public float NormalizedS
            {
                get
                {
                    return (float)S;
                }
                set
                {
                    S = (double)value;
                }
            }

            public float NormalizedV
            {
                get
                {
                    return (float)V;
                }
                set
                {
                    V = (double)value;
                }
            }

            public HSVColor(double h, double s, double v)
            {
                this.H = h;
                this.S = s;
                this.V = v;
            }
        }
        #endregion

        #region Utils
        public static HSVColor ConvertRGBToHSV(Color color)
        {
            return ConvertRGBToHSV((int)(color.r * 255), (int)(color.g * 255), (int)(color.b * 255));
        }

        public static HSVColor ConvertRGBToHSV(double r, double b, double g)
        {
            double delta, min;
            double h = 0, s, v;

            min = Math.Min(Math.Min(r, g), b);
            v = Math.Max(Math.Max(r, g), b);
            delta = v - min;

            if (v.Equals(0)) s = 0;
            else s = delta / v;

            if (s.Equals(0)) h = 360;
            else
            {
                if (r.Equals(v)) h = (g - b) / delta;
                else if (g.Equals(v)) h = 2 + (b - r) / delta;
                else if (b.Equals(v)) h = 4 + (r - g) / delta;

                h *= 60;
                if (h <= 0.0) h += 360;
            }

            HSVColor hsvColor = new HSVColor
            {
                H = 360 - h,
                S = s,
                V = v / 255
            };
            return hsvColor;

        }

        public static Color ConvertHSVToRGB(double h, double s, double v, float alpha)
        {
            double r;
            double g;
            double b;

            if (s.Equals(0))
            {
                r = v;
                g = v;
                b = v;
            }
            else
            {
                int i;
                double f;
                double p;
                double q;
                double t;

                if (h.Equals(360)) h = 0;
                else h = h / 60;

                i = (int)(h);
                f = h - i;

                p = v * (1.0 - s);
                q = v * (1.0 - (s * f));
                t = v * (1.0 - (s * (1.0f - f)));

                switch (i)
                {
                    case 0:
                        r = v;
                        g = t;
                        b = p;
                        break;
                    case 1:
                        r = q;
                        g = v;
                        b = p;
                        break;
                    case 2:
                        r = p;
                        g = v;
                        b = t;
                        break;
                    case 3:
                        r = p;
                        g = q;
                        b = v;
                        break;
                    case 4:
                        r = t;
                        g = p;
                        b = v;
                        break;
                    default:
                        r = v;
                        g = p;
                        b = q;
                        break;
                }
            }
            return new Color((float)r, (float)g, (float)b, alpha);
        }
        #endregion
    }
}
