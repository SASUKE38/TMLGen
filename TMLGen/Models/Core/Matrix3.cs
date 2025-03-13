using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLGen.Models.Core
{
    public class Matrix3
    {
        private readonly float m00;
        private readonly float m01;
        private readonly float m02;
        private readonly float m10;
        private readonly float m11;
        private readonly float m12;
        private readonly float m20;
        private readonly float m21;
        private readonly float m22;

        public Matrix3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
        {
            this.m00 = 0f;
            this.m01 = 0f;
            this.m02 = 0f;
            this.m10 = 0f;
            this.m11 = 0f;
            this.m12 = 0f;
            this.m20 = 0f;
            this.m21 = 0f;
            this.m22 = 0f;
        }

        public Matrix3(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            this.m00 = v1.x;
            this.m01 = v2.x;
            this.m02 = v3.x;
            this.m10 = v1.y;
            this.m11 = v2.y;
            this.m12 = v3.y;
            this.m20 = v1.z;
            this.m21 = v2.z;
            this.m22 = v3.z;
        }

        public static Matrix3 Rotation(Quat q)
        {
            float num = q.x + q.x;
            float num2 = q.y + q.y;
            float num3 = q.z + q.z;
            float num4 = q.x * num;
            float num5 = q.x * num2;
            float num6 = q.x * num3;
            float num7 = q.w * num;
            float num8 = q.y * num2;
            float num9 = q.y * num3;
            float num10 = q.w * num2;
            float num11 = q.z * num3;
            float num12 = q.w * num3;
            return new Matrix3(new Vector3(1f - num8 - num11, num5 + num12, num6 - num10), new Vector3(num5 - num12, 1f - num4 - num11, num9 + num7), new Vector3(num6 + num10, num9 - num7, 1f - num4 - num8));
        }

        public Matrix4 ToMatrix4(Vector3 translate, Vector3 scale)
		{
			return new Matrix4(this.m00 * scale.x, this.m01 * scale.y, this.m02 * scale.z, translate.x, this.m10 * scale.x, this.m11 * scale.y, this.m12 * scale.z, translate.y, this.m20 * scale.x, this.m21 * scale.y, this.m22 * scale.z, translate.z, 0f, 0f, 0f, 1f);
		}
    }
}
