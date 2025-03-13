using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMLGen.Models.Core
{
    public class Transform
    {
        public Quat rotate;
        public float scale;
        public Vector3 translate;

        public Transform()
        {
            rotate = new Quat();
            scale = 1.0f;
            translate = new Vector3();
        }

        public Transform(Quat rotate, float scale, Vector3 translate)
        {
            this.rotate = rotate;
            this.scale = scale;
            this.translate = translate;
        }

        public Matrix4 getMatrix()
        {
            Matrix3 rotationMatrix = Matrix3.Rotation(rotate);
            return rotationMatrix.ToMatrix4(translate, new Vector3(scale));
        }
    }
}
