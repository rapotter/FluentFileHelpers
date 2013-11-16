using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFileHelpers
{
    public abstract class SubClassMap<T> : ClassMapBase<T> where T : new()
    {
        public string Discriminator { get; set; }

        public SubClassMap()
            : base()
        {

        }

        public void DiscriminatorValue(string Value)
        {
            Discriminator = Value;
        }

    }

}
