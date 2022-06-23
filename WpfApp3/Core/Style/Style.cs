using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.Core.Style
{
    public struct Style
    {
        public string Name { get; set; }
        public List<StyleProperty> Properties { get; set; }
    }
}
