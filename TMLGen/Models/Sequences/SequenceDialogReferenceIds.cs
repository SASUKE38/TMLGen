using System;
using System.Xml.Serialization;

namespace TMLGen.Models.Sequences
{
    public class SequenceDialogReferenceIds
    {
        [XmlAttribute]
        public Guid DialogNodeId;
        [XmlAttribute]
        public Guid ReferenceId;

        public SequenceDialogReferenceIds()
        {
            DialogNodeId = Guid.Empty;
            ReferenceId = Guid.Empty;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(SequenceDialogReferenceIds a, SequenceDialogReferenceIds b)
        {
            return (a.DialogNodeId == b.DialogNodeId);
        }

        public static bool operator !=(SequenceDialogReferenceIds a, SequenceDialogReferenceIds b)
        {
            return (a.DialogNodeId != b.DialogNodeId);
        }
    }
}
