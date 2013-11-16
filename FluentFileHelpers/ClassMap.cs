using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FluentFileHelpers
{
    public abstract class ClassMap<T> : ClassMapBase<T> where T : new()
    {
        public SubClassDiscriminator Discriminator { get; set; }
        public Dictionary<string, dynamic> SubClassMaps = new Dictionary<string, dynamic>();

        public ClassMap()
            : base()
        {

        }

        public void DiscriminateSubClassesOn(int Offset, int Length)
        {
            Discriminator = new SubClassDiscriminator(Offset, Length);
        }

    }
}
