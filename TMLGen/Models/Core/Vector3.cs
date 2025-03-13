namespace TMLGen.Models.Core
{
    public class Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Vector3(float f)
        {
            x = f;
            y = f;
            z = f;
        }

        public Vector3 (float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return x + "; " + y + "; " + z;
        }

        public string ToAnimationString()
        {
            return x + "," + y + "," + z;
        }
    }
}
