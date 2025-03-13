using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Track.Key
{
    public class KeyTrackShowClothingBoolean : KeyTrackBase
    {
        [XmlAttribute]
        public bool FallbackValue;
        [XmlAttribute]
        public string SlotData;
        [XmlAttribute]
        public string ClothingTypeData;

        public KeyTrackShowClothingBoolean()
        {
            Type = "KeyTrackShowClothingBoolean";
            FallbackValue = true;
            SlotData = Enum.GetName(typeof(CharacterSlot), CharacterSlot.Helmet);
            ClothingTypeData = Enum.GetName(typeof(ClothingType), ClothingType.All);
        }
    }
}
