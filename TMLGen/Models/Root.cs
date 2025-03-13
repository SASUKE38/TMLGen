using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TMLGen.Models.Global;

namespace TMLGen.Models
{
    public class Root
    {
        public Timeline Timeline;

        public Root()
        {
            this.Timeline = new Timeline();
        }
    }
}
