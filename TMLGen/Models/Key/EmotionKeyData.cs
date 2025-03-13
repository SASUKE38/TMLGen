using System.Xml.Serialization;
using System;

namespace TMLGen.Models.Key
{
    public class EmotionKeyData
    {
        [XmlAttribute]
        public string Emotion;
        [XmlAttribute]
        public int Variation;
        [XmlAttribute]
        public bool AppliesMaterials;
        [XmlAttribute]
        public bool IsSustainedEmotion;

        public EmotionKeyData()
        {
            Emotion = Enum.GetName(typeof(EmotionType), EmotionType.Neutral);
            Variation = 0;
            AppliesMaterials = true;
            IsSustainedEmotion = true;
        }
    }
}
