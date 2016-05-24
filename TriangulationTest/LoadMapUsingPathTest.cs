using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using LightControl;
using NUnit.Framework;
using System.Drawing;
using SimEnvironment;

namespace LightControlTest
{
    [TestFixture]
    class LoadMapUsingPathTest
    {
        private LoadMapUsingPath LoadMap;
        [SetUp]
        public void Init()
        {
            LoadMap = new LoadMapUsingPath();
        }

        [Test]
        public void LoadFileIntoBitMapTestArgumentException()
        {
            bool expected = false;
            bool actual = true;
            try
            {
                Bitmap b = LoadMap.LoadFileIntoBitMap("");
            }
            catch (ArgumentException exception)
            {
                actual = false;
            }
            Assert.AreEqual(expected,actual);
        }
        [Test]
        public void LoadFileIntoBitMapTestException()
        {
            bool expected = false;
            bool actual = true;
            try
            {
                Bitmap b = LoadMap.LoadFileIntoBitMap("");
            }
            catch (Exception exception)
            {
                actual = false;
            }
            Assert.AreEqual(expected, actual);
        }
    }
}
