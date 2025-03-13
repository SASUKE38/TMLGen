using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TMLGen.Models.Component
{
    public class ComponentCameraShot : ComponentBase
    {
        [XmlAttribute]
        public bool AreCamersEdited;
        [XmlAttribute]
        public bool IsAutomatedLighting;
        [XmlAttribute]
        public bool IsAutomatedCamera;
        [XmlAttribute]
        public bool IsConditionalStagingDisabled;
        [XmlAttribute]
        public bool IsLogicEnabled;
        [XmlAttribute]
        public float SwitchInterval;
        [XmlAttribute]
        public bool IsLooping;
        [XmlAttribute]
        public bool IsJCutEnabled;
        [XmlAttribute]
        public float JCutLength;
        [XmlAttribute]
        public Guid CompanionCameraA;
        [XmlAttribute]
        public Guid CompanionCameraB;
        [XmlAttribute]
        public Guid CompanionCameraC;

        public List<Camera> CameraContainer;

        public ComponentCameraShot()
        {
            AreCamersEdited = true;
            IsAutomatedLighting = true;
            IsAutomatedCamera = true;
            IsConditionalStagingDisabled = false;
            IsLogicEnabled = false;
            SwitchInterval = 5f;
            IsLooping = true;
            IsJCutEnabled = false;
            JCutLength = 0f;
            CameraContainer = new List<Camera>();
        }
    }
}
