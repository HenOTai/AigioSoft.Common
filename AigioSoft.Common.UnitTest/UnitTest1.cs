using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AigioSoft.Common;
using System.Linq;

namespace AigioSoft.Common.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void LinqExtension_TestMethod()
        {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.AsQueryable().OrderByDescending(x => x).Paging(1, 5);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(data));
        }

        [TestMethod]
        public void HashsHelper_TestMethod()
        {
            var temp1 = Helpers.Hashs.MD5("123");
            var temp2 = Helpers.Hashs.MD5("123", false);
            var temp3 = Helpers.Hashs.SHA1("123");
            var temp4 = Helpers.Hashs.SHA256("123");
            var temp5 = Helpers.Hashs.SHA512("123");
            Console.WriteLine("OK");
        }
    }
}
