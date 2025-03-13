using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class Camera
    {
        [XmlAttribute]
        public Guid CameraId;

        public Camera()
        {
            CameraId = Guid.Empty;
        }
    }
}
