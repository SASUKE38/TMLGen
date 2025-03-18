using System;

namespace TMLGen.Models.Core
{
    public class Quat
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Quat(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Quat()
        {
            this.x = 0f;
            this.y = 0f;
            this.z = 0f;
            this.w = 1f;
        }

        // https://stackoverflow.com/questions/70462758/c-sharp-how-to-convert-quaternions-to-euler-angles-xyz
        public static Quat ToQuaternion(Vector3 v)
        {

            float cy = (float)Math.Cos(v.z * 0.5);
            float sy = (float)Math.Sin(v.z * 0.5);
            float cp = (float)Math.Cos(v.y * 0.5);
            float sp = (float)Math.Sin(v.y * 0.5);
            float cr = (float)Math.Cos(v.x * 0.5);
            float sr = (float)Math.Sin(v.x * 0.5);

            return new Quat
            {
                w = (cr * cp * cy + sr * sp * sy),
                x = (sr * cp * cy - cr * sp * sy),
                y = (cr * sp * cy + sr * cp * sy),
                z = (cr * cp * sy - sr * sp * cy)
            };

        }

        public static Vector3 ToEulerAngles(Quat q)
        {
            Vector3 res = new();
            float positiveBound = 0.49999995f;
            float negativeBound = -0.49999995f;
            float singularityTest = q.z * q.x - q.w * q.y;
            if (singularityTest > positiveBound || singularityTest < negativeBound)
            {
                res.x = BoundAngle((float) (2 * Math.Atan2(q.x, q.w)));
                res.y = (float)((singularityTest < negativeBound ? Math.PI : -Math.PI) / 2);
                res.z = 0f;
            }
            else
            {
                float xsq = q.x * q.x;
                float ysq = q.y * q.y;
                float zsq = q.z * q.z;
                res.x = -(float) Math.Atan2((-2f * (q.w * q.x + q.y * q.z)), 1f - 2f * (xsq + ysq));
                res.y = -(float) Math.Asin(2f * singularityTest);
                res.z = (float) Math.Atan2(2f * (q.w * q.z + q.x * q.y), 1f - 2f * (ysq + zsq));
                float xBound = BoundAngle((float)(res.x - Math.PI));
                float yBound = BoundAngle((float)(Math.PI - res.y));
                float zBound = BoundAngle((float)(res.z - Math.PI));
                if (xBound * xBound + yBound * yBound + zBound * zBound < res.x * res.x + res.y * res.y + res.z * res.z)
                {
                    res.x = xBound;
                    res.y = yBound;
                    res.z = zBound;
                }
            }
            return res;
        }

        private static float BoundAngle(float angle)
        {
            float pi2 = (float)(2 * Math.PI);
            if (angle < -Math.PI) angle += pi2;
            else if (angle > Math.PI) angle -= pi2;
            return angle;
        }

        public string ToAnimationString()
        {
            return x + "," + y + "," + z + "," + w;
        }
    }
}
