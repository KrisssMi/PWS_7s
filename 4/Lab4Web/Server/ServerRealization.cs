using Lab4_ASMX.SimplexServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Lab4_Client_.Server
{
    public class ServerRealization : Simplex
    {
        [return: XmlElement("AddMessageResult")]
        public override int Add(int x, int y)
        {
            return x + y;
        }

        [return: XmlElement("ConcatMessageResult")]
        public override string Concat(string s, double d)
        {
            return string.Concat(s,d);
        }

        [return: XmlElement("SumMessageResult")]
        public override A Sum(A a1, A a2)
        {
            A result = new A();

            result.s = string.Concat(a1.s, a2.s);
            result.k = a1.k + a2.k;
            result.f = a1.f + a2.f;

            return result;
        }
    }
}