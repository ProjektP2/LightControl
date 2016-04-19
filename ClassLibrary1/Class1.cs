using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NUnit.Framework;

namespace ClassLibrary1
{    
    [TestFixture]
    public class Class1
    {
        [Test]
        public void lord()
        {
            int actual = 2, expected = 2;
            Assert.AreEqual(actual, expected);
        }
    }
}
