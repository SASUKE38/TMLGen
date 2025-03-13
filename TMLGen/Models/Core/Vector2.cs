namespace TMLGen.Models.Core
{
    public class Vector2
    {
        public float x;
        public float y;

        public Vector2()
        {
            x = 0f;
            y = 0f;
        }

        public Vector2(float f)
        {
            x = f;
            y = f;
        }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return x + "; " + y;
        }
    }
}
