using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AigioSoft.Common.UnitTest.Framework
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void HashsHelper_TestMethod()
        {
            var temp0 = Helpers.Hashs.MD5_16("123");
            var temp1 = Helpers.Hashs.MD5("123");
            var temp2 = Helpers.Hashs.MD5("123", false);
            var temp3 = Helpers.Hashs.SHA1("123");
            var temp4 = Helpers.Hashs.SHA256("123");
            var temp5 = Helpers.Hashs.SHA384("123");
            var temp6 = Helpers.Hashs.SHA512("123");
            Console.WriteLine("OK");
        }
    }
}
