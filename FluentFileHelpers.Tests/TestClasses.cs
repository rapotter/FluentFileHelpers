using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentFileHelpers.Enum;

namespace FluentFileHelpers.Tests
{
    public class TestClass1
    {
        public int p1 { get; set; }
        public string p2 { get; set; }
        public Decimal p3 { get; set; }
    }

    public class TestClass1Map : ClassMap<TestClass1>
    {
        public TestClass1Map()
        {
            DiscriminateSubClassesOn(0, 1);
            Map(x => x.p1, 0, 10);
            Map(x => x.p2, 10, 10).Trim();
            Map(x => x.p3, 20, 10).Align(Alignment.Right).Padding('*');
        }
    }

    public class TestClass2
    {
        public int p1 { get; set; }
        public string p2 { get; set; }
        public string p3 { get; set; }
    }

    public class TestClass2Map : ClassMap<TestClass2>
    {
        public TestClass2Map()
        {
            Map(x => x.p1, 0, 10);
            Map(x => x.p2, 10, 10);
            Map(x => x.p3, 20, 10).Trim();
        }
    }

    public class TestSubClass1 : TestClass1
    {
        public Decimal p4 { get; set; }
    }

    public class TestSubClass1Map : SubClassMap<TestSubClass1>
    {
        public TestSubClass1Map()
        {
            DiscriminatorValue("0");
            Map(x => x.p4, 30, 10);
        }
    }

    public class TestSubClass2 : TestClass1
    {
        public string p4 { get; set; }
    }

    public class TestSubClass2Map : SubClassMap<TestSubClass2>
    {
        public TestSubClass2Map()
        {
            DiscriminatorValue("1");
            Map(x => x.p4, 30, 10);
        }
    }

}
