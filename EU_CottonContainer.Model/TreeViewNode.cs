using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU_CottonContainer.Model
{
    public class TreeViewNode
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public State state { get; set; }
    }

    public class State
    {
        public bool selected { get; set; }
    }
}
