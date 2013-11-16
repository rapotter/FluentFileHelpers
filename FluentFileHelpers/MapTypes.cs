using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFileHelpers
{
    public class MapTypes
    {
        public Type ClassMapType { get; set; }
        public Type ClassMapGenericType { get; set; }
        public dynamic ClassMap { get; set; }
        public IList<MapTypes> SubClassMapTypes { get; set; }
    }
}
