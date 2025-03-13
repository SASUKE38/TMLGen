using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLGen.Models.Core
{
    public class Matrix4
    {
        private readonly float m00;
        private readonly float m01;
        private readonly float m02;
        private readonly float m03;
        private readonly float m10;
        private readonly float m11;
        private readonly float m12;
        private readonly float m13;
        private readonly float m20;
        private readonly float m21;
        private readonly float m22;
        private readonly float m23;
        private readonly float m30;
        private readonly float m31;
        private readonly float m32;
        private readonly float m33;

        public Matrix4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m03 = m03;
            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m30 = m30;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }

        public override string ToString()
        {
            return "{{"+m00 +","+m01+","+m02+","+m03+"}{"+m10+","+m11+","+m12+","+m13+"}{"+m20+","+m21+","+m22+","+m23+"}{"+m30+","+m31+","+m32+","+m33+"}}";
        }
    }
}
