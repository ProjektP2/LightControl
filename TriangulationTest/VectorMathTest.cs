using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography;
using LightControl;
using NUnit.Framework;
using Triangulering;
namespace LightControlTest
{
    [TestFixture]
    class VectorMathTest
    {
        private Coords p1;
        private Coords p2;
        [SetUp]
        public void Init()
        {
            p1 = new Coords(0,0);
            p2 = new Coords(10,0);
        }

        [Test]
        public void CalculateVectorTest()
        {
            Coords Expectet = new Coords(10,0);
           Coords result = VectorMath.CalculateVector(p1, p2);
            Assert.AreEqual(Expectet.x, result.x);
            Assert.AreEqual(Expectet.y, result.y);
        }
        [Test]
        public void LengthOfVectorTest()
        {
            double Expectet = 10;
            double result = VectorMath.LengthOfVector(p2);
            Assert.AreEqual(Expectet, result);
        }

        //[Test]
        public void NormalizeVectorTest()
        {
            //VectorMath.NormalizeVector();
        }
        [Test]
        public void SubtractVectorsTest()
        {
            Coords Expectet = new Coords(10,0);
            Coords result = VectorMath.SubtractVectors(p2, p1);
            Assert.AreEqual(Expectet.x, result.x);
            Assert.AreEqual(Expectet.y, result.y);
        }
        //[test]
        public void AngleBetweenVectorsTest()
        {
            
        }
        //[test]
        public void DotProductTest()
        {
            
        }
        [Test]
        public void ScaleVectorTest()
        {
            Coords Expectet = new Coords(30, 0);
            Coords result = VectorMath.ScaleVector(p2, 3);
            Assert.AreEqual(Expectet.x, result.x);
            Assert.AreEqual(Expectet.y, result.y);
        }
        //[test]
        public void PerpendicularVectorBetweenPointAndVectorTest()
        {
            
        }
        [Test]
        public void projectionLengthTest()
        {
            Coords light = new Coords(1,1);
            Coords occupant = new Coords(0,0);
            Coords scalledVector = new Coords(2,2);
            double Expectet = ((light.x*scalledVector.x) + light.y*scalledVector.y)/
                              Math.Pow(Math.Sqrt(Math.Pow(scalledVector.x, 2)+Math.Pow(scalledVector.y,2)),2);
            Coords result =  VectorMath.projectionLength(light, scalledVector, occupant);
            Assert.AreEqual(Expectet*scalledVector.x ,result.x);
            Assert.AreEqual(Expectet*scalledVector.y, result.y);
        }

    }
}
