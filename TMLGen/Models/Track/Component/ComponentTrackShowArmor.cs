using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMLGen.Models.Track.Key;

namespace TMLGen.Models.Track.Component
{
    public class ComponentTrackShowArmor : ComponentTrackBase
    {
        [XmlAttribute]
        public string ClothingType;

        [XmlIgnore]
        public Dictionary<string, KeyTrackShowClothingBoolean> showClothingTrackDict;

        public ComponentTrackShowArmor()
        {
            Name = "Show Clothing (All";
            Type = "ComponentTrackShowArmor";
            ClothingType = "All";

            showClothingTrackDict = [];

            KeyTrackBooleanWrapper wrapperTrack = new() { Name = "All" };
            
            foreach (string name in Enum.GetNames(typeof(CharacterSlot)))
            {
                KeyTrackShowClothingBoolean newTrack = new() { Name = name, SlotData = name };
                wrapperTrack.Tracks.Add(newTrack);
                showClothingTrackDict.Add(name, newTrack);
            }
            Tracks.Add(wrapperTrack);
        }
    }
}
