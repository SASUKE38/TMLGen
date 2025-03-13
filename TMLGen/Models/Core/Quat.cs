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
            Vector3 angles = new();

            // roll / x
            double sinr_cosp = 2 * (q.w * q.x + q.y * q.z);
            double cosr_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
            angles.x = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // pitch / y
            double sinp = 2 * (q.w * q.y - q.z * q.x);
            if (Math.Abs(sinp) >= 1)
            {
                angles.y = (float)Math.CopySign(Math.PI / 2, sinp);
            }
            else
            {
                angles.y = (float)Math.Asin(sinp);
            }

            // yaw / z
            double siny_cosp = 2 * (q.w * q.z + q.x * q.y);
            double cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
            angles.z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            return angles;
        }

        public string ToAnimationString()
        {
            return x + "," + y + "," + z + "," + w;
        }
    }
}
