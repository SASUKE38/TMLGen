using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMLGen.Models.Sequences;

namespace TMLGen.Models.Component
{
    public class ComponentVoice : ComponentBase
    {
        [XmlAttribute]
        public string FaceTech;
        [XmlAttribute]
        public bool IsAliased;
        [XmlAttribute]
        public double LeftBuffer;
        [XmlAttribute]
        public bool HoldMocap;
        [XmlAttribute]
        public bool DisableMocap;
        [XmlAttribute]
        public double PerformanceFade;
        [XmlAttribute]
        public double FadeIn;
        [XmlAttribute]
        public double FadeOut;
        [XmlAttribute]
        public double FadeInOffset;
        [XmlAttribute]
        public double FadeOutOffset;
        [XmlAttribute]
        public string PerformanceDrift;
        [XmlAttribute]
        public bool IsMirrored;
        [XmlAttribute]
        public double HeadPitchCorrectionDegrees;
        [XmlAttribute]
        public double HeadYawCorrectionDegrees;
        [XmlAttribute]
        public double HeadRollCorrectionDegrees;
        [XmlAttribute]
        public bool SeparateFades;

        [XmlElement]
        public SequenceDialogReferenceIds DialogNodeReference;

        public ComponentVoice()
        {
            FaceTech = Enum.GetName(typeof(FaceTech), 1);
            IsAliased = false;
            LeftBuffer = 0.0;
            HoldMocap = true;
            DisableMocap = false;
            PerformanceFade = 2.5;
            FadeIn = 0;
            FadeOut = 0;
            FadeInOffset = 0.0;
            FadeOutOffset = 0.0;
            PerformanceDrift = Enum.GetName(typeof(PerformanceDriftType), 1);
            IsMirrored = false;
            HeadPitchCorrectionDegrees = 0.0;
            HeadYawCorrectionDegrees = 0.0;
            HeadRollCorrectionDegrees = 0.0;
            SeparateFades = false;
        }
    }
}
