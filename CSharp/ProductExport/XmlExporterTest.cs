using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalTests.Reporters.Windows;
using Xunit;

namespace ProductExport
{
    class Thing
    {
        public int Thingish = 3;
        public String Thinger = "blue";
        public override string ToString()
        {
            return ($"{Thingish}: {Thinger}");
        }
    }

    [UseReporter(typeof(DiffReporter))]
    public class XmlExporterTest
    {
        [Fact]
        public void TestList()
        {
            var names = new[] { "Llewellyn", "James", "Dan", "Jason", "Katrina" };
            Array.Sort(names);
            Approvals.VerifyAll(names, label: "hello");
        }

        [Fact]
        public void TestObjectDictionary()
        {
            var object_dictionary = new Dictionary<int, Thing>() { 
                        {1, new Thing() { Thinger = "pop", Thingish = 10 }}, 
                        {2, new Thing() { Thinger = "pip", Thingish = 11 }} };
            Approvals.VerifyAll(object_dictionary, label: "hello");
        }

        [Fact]
        public void ExportFull()
        {
            var orders = new List<Order>() {SampleModelObjects.RecentOrder, SampleModelObjects.OldOrder};
            var xml = XmlExporter.ExportFull(orders);
            Approvals.VerifyXml(xml);
        }

        [Fact]
        public void ResharperThing()
        {
            var thing = ResharperTest(1);
            Assert.True(thing);
        }

        [Fact]
        public void ResharperThing2()
        {
            var thing = ResharperTest(4);
            Assert.True(thing);
        }

        private bool ResharperTest(int thing4)
        {
            var thing1 = false;
            var thing2 = false;
            var thing3 = true;
            var thing5 = false;

            if (thing4 > 2)
            {
                thing1 = true;
                thing2 = true;
                thing3 = false;
            }

            if (thing4 < 5)
            {
                thing1 = true;
                thing2 = false;
                thing3 = true;
            }

            if (thing4 == 9)
            {
                thing1 = true;
                thing3 = true;
            }

            if (thing1)
            {
                if (thing2)
                {
                    thing5 = true;
                }
            }

            return thing5;
        }

        [Fact]
        public void ExportTaxDetails()
        {
            var orders = new List<Order>() {SampleModelObjects.RecentOrder, SampleModelObjects.OldOrder};
            var xml = XmlExporter.ExportTaxDetails(orders);
            Approvals.VerifyXml(xml);
        }

        [Fact]
        public void ExportStore()
        {
            var store = SampleModelObjects.FlagshipStore;
            var xml = XmlExporter.ExportStore(store);
            Approvals.VerifyXml(xml);
        }

        [Fact]
        public void ExportHistory()
        {
            var orders = new List<Order>() {SampleModelObjects.RecentOrder, SampleModelObjects.OldOrder};
            var xml = XmlExporter.ExportHistory(orders);
            var regex = "createdAt=\"[^\"]+\"";
            var report = Regex.Replace(xml, regex, "createdAt=\"2018-09-20T00:00Z\"");
            Approvals.VerifyXml(report);
        }
    }
}