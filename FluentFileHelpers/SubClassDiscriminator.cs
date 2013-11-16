using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFileHelpers
{
    public class SubClassDiscriminator
    {
        public int Offset { get; set; }
        public int Length { get; set; }

        public SubClassDiscriminator() { }

        public SubClassDiscriminator(int Offset, int Length)
        {
            this.Offset = Offset;
            this.Length = Length;
        }

        public string GetDiscriminatorValueFrom(string s)
        {
            return s.Substring(Offset, Length);
        }
    }
}
