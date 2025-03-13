using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TMLGen.Models.Global
{
    public class TimelineQuestionHoldAutomationSettings
    {
        [XmlAttribute]
        public bool OverrideGlobalSettings;
        [XmlAttribute]
        public bool IsQuestionHoldAutomationEnabled;
        [XmlAttribute]
        public float QuestionHoldAutomationCycleSpeed;
        [XmlAttribute]
        public float QuestionHoldAutomationCycleSpeedDeviation;
        [XmlAttribute]
        public float QuestionHoldAutomationStartOffset;
        [XmlAttribute]
        public float QuestionHoldAutomationStartOffsetDeviation;

        public TimelineQuestionHoldAutomationSettings()
        {
            this.OverrideGlobalSettings = false;
            this.QuestionHoldAutomationCycleSpeed = -1.0f;
            this.IsQuestionHoldAutomationEnabled = true;
            this.QuestionHoldAutomationCycleSpeedDeviation = 1.0f;
            this.QuestionHoldAutomationStartOffset = 0.0f;
            this.QuestionHoldAutomationStartOffsetDeviation = 0.0f;
        }
    }
}
