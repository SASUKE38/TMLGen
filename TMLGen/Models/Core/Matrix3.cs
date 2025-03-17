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

        public Matrix3()
        {
            m00 = 0f;
            m01 = 0f;
            m02 = 0f;
            m10 = 0f;
            m11 = 0f;
            m12 = 0f;
            m20 = 0f;
            m21 = 0f;
            m22 = 0f;
        }

        public Matrix3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
        }

        public Matrix3(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            m00 = v1.x;
            m01 = v2.x;
            m02 = v3.x;
            m10 = v1.y;
            m11 = v2.y;
            m12 = v3.y;
            m20 = v1.z;
            m21 = v2.z;
            m22 = v3.z;
        }

        public static Matrix3 Rotation(Quat q)
        {
            float x2 = q.x * 2;
            float y2 = q.y * 2;
            float z2 = q.z * 2;
            float xx2 = q.x * x2;
            float xy2 = q.x * y2;
            float xz2 = q.x * z2;
            float wx2 = q.w * x2;
            float yy2 = q.y * y2;
            float yz2 = q.y * z2;
            float wy2 = q.w * y2;
            float zz2 = q.z * z2;
            float wz2 = q.w * z2;
            return new Matrix3(new Vector3(1f - yy2 - zz2, xy2 + wz2, xz2 - wy2), new Vector3(xy2 - wz2, 1f - xx2 - zz2, yz2 + wx2), new Vector3(xz2 + wy2, yz2 - wx2, 1f - xx2 - yy2));
        }

        public Matrix4 ToMatrix4(Vector3 translate, Vector3 scale)
		{
			return new Matrix4(this.m00 * scale.x, this.m01 * scale.y, this.m02 * scale.z, translate.x, this.m10 * scale.x, this.m11 * scale.y, this.m12 * scale.z, translate.y, this.m20 * scale.x, this.m21 * scale.y, this.m22 * scale.z, translate.z, 0f, 0f, 0f, 1f);
		}
    }
}
