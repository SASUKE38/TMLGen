using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using TMLGen.Models.Core;
using TMLGen.Models.Global;
using TMLGen.Models.Track.Actor;

namespace TMLGen.Generation.Collectors
{
    public abstract class CollectorBase
    {

        protected XDocument doc;
        protected XDocument gdtDoc;
        protected Timeline timeline;

        public readonly Dictionary<Guid, Guid> actorTrackMapping;
        public readonly Dictionary<Guid, ActorTrackBase> trackMapping;

        protected CollectorBase(XDocument doc, XDocument gdtDoc, Timeline timeline, Dictionary<Guid, Guid> actorTrackMapping, Dictionary<Guid, ActorTrackBase> trackMapping)
        {
            this.doc = doc;
            this.gdtDoc = gdtDoc;
            this.timeline = timeline;
            this.actorTrackMapping = actorTrackMapping;
            this.trackMapping = trackMapping;
        }

        protected CollectorBase(XDocument doc, XDocument gdtDoc, Timeline timeline)
        {
            this.doc = doc;
            this.gdtDoc = gdtDoc;
            this.timeline = timeline;
            this.actorTrackMapping = [];
            this.trackMapping = [];
        }

        public abstract void Collect();

        protected List<GlobalSoundEvent> CollectGlobalSoundEvents(IEnumerable<XElement> events)
        {
            List<GlobalSoundEvent> res = [];

            foreach (XElement ev in events)
            {
                GlobalSoundEvent eventToAdd = new()
                {
                    Event = Guid.Parse(ev.XPathSelectElement("./attribute[@id='EventID']").Attribute("value").Value),
                    SoundType = Enum.GetName(typeof(SoundType), int.Parse(ev.XPathSelectElement("./attribute[@id='SoundType']").Attribute("value").Value))
                };

                res.Add(eventToAdd);
            }

            return res;
        }

        protected static float? ExtractFloat(XElement element) => element == null ? null : (float?)element.Attribute("value");

        protected static double? ExtractDouble(XElement element) => element == null ? null : (double?)element.Attribute("value");

        protected static int? ExtractInt(XElement element) => element == null ? null : (int?)element.Attribute("value");
        
        protected static uint? ExtractUint(XElement element) => element == null ? null : (uint?)element.Attribute("value");

        protected static Guid? ExtractGuid(XElement element) => element == null ? null : (Guid?)element.Attribute("value");

        protected static string ExtractString(XElement element)
        {
            if (element != null)
            {
                return (string) element.Attribute("value");
            }
            return null;
        }

        protected static bool? ExtractBool(XElement element)
        {
            if (element != null)
            {
                bool.TryParse(element.Attribute("value").Value.ToLower(), out bool res);
                return res;
            }

            return null;
        }

        protected static Vector3 ExtractVector3(XElement element)
        {
            string vecString = ExtractString(element);
            if (vecString != null)
            {
                string[] vecSub = vecString.Split();
                return new Vector3(float.Parse(vecSub[0]), float.Parse(vecSub[1]), float.Parse(vecSub[2]));
            }
            return null;
        }

        protected static Vector2 ExtractVector2(XElement element)
        {
            string vecString = ExtractString(element);
            if (vecString != null)
            {
                string[] vecSub = vecString.Split();
                return new Vector2(float.Parse(vecSub[0]), float.Parse(vecSub[1]));
            }
            return null;
        }

        protected static Quat ExtractQuat(XElement element)
        {
            string quatString = ExtractString(element);
            if (quatString != null)
            {
                string[] vecSub = quatString.Split();
                return new Quat(float.Parse(vecSub[0]), float.Parse(vecSub[1]), float.Parse(vecSub[2]), float.Parse(vecSub[3]));
            }
            return null;
        }

        protected static string GetNameFromEnum<EnumType>(XElement keyData, string attribute)
        {
            int? value = ExtractInt(keyData.XPathSelectElement("./attribute[@id='" + attribute + "']"));
            return value.HasValue ? Enum.GetName(typeof(EnumType), value) : null;
        }
    }
}
