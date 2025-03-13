using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Key
{
    public class LookAtKeyData : ImmutableTargetKeyDataBase
    {
        [XmlAttribute]
        public string TargetBone;
        [XmlAttribute]
        public string TrackingMode;
        [XmlAttribute]
        public string LookAtMode;
        [XmlAttribute]
        public string LookAtInterpMode;
        [XmlAttribute]
        public string TurnMode;
        [XmlAttribute]
        public float TurnSpeedMultiplier;
        [XmlAttribute]
        public float TorsoTurnSpeedMultiplier;
        [XmlAttribute]
        public float HeadTurnSpeedMultiplier;
        [XmlAttribute]
        public double Weight;
        [XmlAttribute]
        public double SafeZoneAngle;
        [XmlAttribute]
        public double HeadSafeZoneAngle;
        [XmlAttribute]
        public bool ResetEvent;
        [XmlAttribute]
        public string Offset;
        [XmlAttribute]
        public bool IsEyeOverrideEnabled;
        [XmlAttribute]
        public Guid EyeOverrideTargetId;
        [XmlAttribute]
        public string EyeOverrideTargetBone;
        [XmlAttribute]
        public string EyeOverrideOffset;

        public LookAtKeyData()
        {
            TargetBone = string.Empty;
            TrackingMode = Enum.GetName(typeof(LookAtTrackingMode), LookAtTrackingMode.Recenter);
            LookAtMode = Enum.GetName(typeof(LookAtLookAtMode), LookAtLookAtMode.Head);
            LookAtInterpMode = Enum.GetName(typeof(LookAtLookAtInterpMode), LookAtLookAtInterpMode.EaseInOut);
            TurnMode = Enum.GetName(typeof(LookAtTurnMode), LookAtTurnMode.Inactive);
            TurnSpeedMultiplier = 1f;
            TorsoTurnSpeedMultiplier = 1f;
            HeadTurnSpeedMultiplier = 1f;
            Weight = 0d;
            SafeZoneAngle = 22d;
            HeadSafeZoneAngle = 26d;
            ResetEvent = false;
            Offset = "0; 0; 0";
            IsEyeOverrideEnabled = false;
            EyeOverrideTargetId = Guid.Empty;
            EyeOverrideTargetBone = string.Empty;
            EyeOverrideOffset = "0; 0; 0";
        }
    }
}
