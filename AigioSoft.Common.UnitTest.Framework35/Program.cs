using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AigioSoft.Common;
using Helpers = AigioSoft.Common.Helpers;

namespace AigioSoft.Common.UnitTest.Framework35
{
    class Program
    {
        static void Main(string[] args)
        {
            var temp0 = Helpers.Hashs.MD5_16("123");
            var temp1 = Helpers.Hashs.MD5("123");
            var temp2 = Helpers.Hashs.MD5("123", false);
            var temp3 = Helpers.Hashs.SHA1("123");
            var temp4 = Helpers.Hashs.SHA256("123");
            var temp5 = Helpers.Hashs.SHA384("123");
            var temp6 = Helpers.Hashs.SHA512("123");
            var temp7 = "   ".IsNullOrWhiteSpace();
            Console.WriteLine("OK");
        }
    }
}
