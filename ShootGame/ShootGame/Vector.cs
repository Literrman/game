using System;
using System.Drawing;

namespace ShootGame
{
    public class Vector
    {
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector(Vector other)
        {
            X = other.X;
            Y = other.Y;
        }

        public readonly double X;
        public readonly double Y;
        public double Length => Math.Sqrt(X * X + Y * Y);
        public double Angle => Math.Atan2(Y, X);
        public static Vector Zero = new Vector(0, 0);
        private double v;

        public override string ToString() => $"X: {X}, Y: {Y}";
        private bool Equals(Vector other) => X.Equals(other.X) && Y.Equals(other.Y);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Vector)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (X.GetHashCode() * 397) ^ Y.GetHashCode(); }
        }

        public static Vector operator -(Vector a, Vector b) => new Vector(a.X - b.X, a.Y - b.Y);
        public static Vector operator *(Vector a, double k) => new Vector(a.X * k, a.Y * k);
        public static Vector operator /(Vector a, double k) => new Vector(a.X / k, a.Y / k);
        public static Vector operator +(Vector a, Vector b)  => new Vector(a.X + b.X, a.Y + b.Y);
        public static Vector operator *(double k, Vector a) => a * k;

        public Vector Normalize() => Length > 0 ? this * (1 / Length) : this;
        public Vector Rotate(double angle) => new Vector(X * Math.Cos(angle) - Y * Math.Sin(angle), X * Math.Sin(angle) + Y * Math.Cos(angle));
        public Vector BoundTo(Size size) => new Vector(Math.Max(0, Math.Min(size.Width, X)), Math.Max(0, Math.Min(size.Height, Y)));
    }
}