using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TMLGen.Models.Global
{
    public class TimelineFoleySettings
    {
        [XmlAttribute]
        public bool ShouldPlay;
        [XmlAttribute]
        public bool CameraCullingEnabled;
        [XmlAttribute]
        public float MinVelocityThreshold;
        [XmlAttribute]
        public float MaxVelocityThreshold;
        [XmlAttribute]
        public float VelocitySpikeThreshold;
        [XmlAttribute]
        public float VelocityResumeSpikeDetectionThreshold;
        [XmlAttribute]
        public float MinRotationThreshold;
        [XmlAttribute]
        public float MaxRotationThreshold;
        [XmlAttribute]
        public float RotationSpikeThreshold;
        [XmlAttribute]
        public float RotationResumeSpikeDetectionThreshold;
        [XmlAttribute]
        public string ArmorType;

        public TimelineFoleySettings()
        {
            this.ShouldPlay = true;
            this.CameraCullingEnabled = true;
            this.MinVelocityThreshold = 0.2f;
            this.MaxVelocityThreshold = 8.0f;
            this.VelocitySpikeThreshold = 3.25f;
            this.VelocityResumeSpikeDetectionThreshold = 3.25f;
            this.MinRotationThreshold = 0.2f;
            this.MaxRotationThreshold = 10.0f;
            this.RotationSpikeThreshold = 2.25f;
            this.RotationResumeSpikeDetectionThreshold = 2.25f;
            this.ArmorType = "Cloth";
        }

    }
}
