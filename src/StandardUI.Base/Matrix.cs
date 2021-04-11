using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.StandardUI
{
    public struct Matrix : IEquatable<Matrix>
    {
        // | m11 m12 0 |
        // | m21 m22 0 |
        // | m31 m32 1 |
        public float m11;
        public float m12;
        public float m21;
        public float m22;
        public float m31;
        public float m32;

        public Matrix(float m11, float m12, float m21, float m22, float m31, float m32)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m21 = m21;
            this.m22 = m22;
            this.m31 = m31;
            this.m32 = m32;
        }

        public static Matrix Identity =>
            new Matrix(1, 0, 0, 1, 0, 0);

        public static Matrix Translate(float offsetX, float offsetY) =>
            new Matrix(1, 0, 0, 1, offsetX, offsetY);

        public static Matrix Translate(Point offset) => Translate(offset.X, offset.Y);

        public static Matrix Scale(float scaleX, float scaleY) =>
            new Matrix(scaleX, 0, 0, scaleY, 0, 0);

        public bool Equals(Matrix o) =>
            m11 == o.m11 &&
            m12 == o.m12 &&
            m21 == o.m21 &&
            m22 == o.m22 &&
            m31 == o.m31 &&
            m32 == o.m32;

        public override bool Equals(object? obj)
        {
            return obj is Matrix m && Equals(m);
        }

        public static bool operator ==(Matrix left, Matrix right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Matrix left, Matrix right)
        {
            return !(left == right);
        }

        public static Matrix operator *(Matrix left, Matrix right) =>
            new Matrix(
                left.m11 * right.m11 + left.m12 * right.m21,
                left.m11 * right.m12 + left.m12 * right.m22,
                left.m21 * right.m11 + left.m22 * right.m21,
                left.m21 * right.m12 + left.m22 * right.m22,
                left.m31 * right.m11 + left.m32 * right.m21 + right.m31,
                left.m31 * right.m12 + left.m32 * right.m22 + right.m32);

        public override int GetHashCode()
        {
#if NETCOREAPP
            return HashCode.Combine(m11, m12, m21, m22, m31, m32);
#else
            return m11.GetHashCode() ^ m12.GetHashCode() ^ m21.GetHashCode() ^ m22.GetHashCode() ^ m31.GetHashCode() ^ m32.GetHashCode();
#endif
        }
    }
}
