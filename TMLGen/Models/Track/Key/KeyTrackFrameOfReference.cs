using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Track.Key
{
    public class KeyTrackFrameOfReference : KeyTrackBase
    {
        [XmlAttribute]
        public string AttachmentType;

        public KeyTrackFrameOfReference()
        {
            AttachmentType = Enum.GetName(typeof(FrameOfReferenceAttachmentType), FrameOfReferenceAttachmentType.DummyAttachmentBone);
            Type = "KeyTrackFrameOfReference";
        }
    }
}
