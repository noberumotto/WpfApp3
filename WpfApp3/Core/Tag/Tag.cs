using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.core.Tags
{
    public struct Tag
    {
        public string Name { get; set; }
        public List<TagProperty> Properties { get; set; }

        public object Content { get; set; }
    }
}
