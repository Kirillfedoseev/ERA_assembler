using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ERA_Assembler;

namespace ERA_Tests
{
    [TestClass]
    public class InterpretationTest
    {
        public void AreEqual(string expected, string actual)
        {
            Console.WriteLine(actual);
            expected = expected.Replace(" ", "").Replace("\n", "");
            actual = actual.Replace(" ", "").Replace("\n", "").Replace("\r", "");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LDATest()
        {
            string expected = "00 00 00 00 00 00 00 00 08 41 00 00 00 05 00 00 00 00";

            string result = Executer.Execute("R1 := R2 + 5;");
            AreEqual(expected, result);
        }

        [TestMethod]
        public void LDCTest()
        {
            string expected = "00 00 00 00 00 00 00 00 C8 A1 00 00 00 00";

            string result = Executer.Execute("R1 := 5;");
            AreEqual(expected, result);
        }

        [TestMethod]
        public void LDTest()
        {
            string expected = "00 00 00 00 00 00 00 00 C4 22 00 00 00 00";

            string result = Executer.Execute("R1 := *R2;");
            AreEqual(expected, result);
        }


        [TestMethod]
        public void STTest()
        {
            string expected = "00 00 00 00 00 00 00 00 CC 22 00 00 00 00";

            string result = Executer.Execute("*R1 := R2;");
            AreEqual(expected, result);
        }


        [TestMethod]
        public void MOVTest()
        {
            string expected = "00 00 00 00 00 00 00 00 D0 22 00 00 00 00";

            string result = Executer.Execute("R1 := R2;");
            AreEqual(expected, result);
            
        }


        [TestMethod]
        public void AddTest()
        {
            string expected = "00 00 00 00 00 00 00 00 D4 22 00 00 00 00";

            string result = Executer.Execute("R1 += R2;");
            AreEqual(expected, result);

        }


        [TestMethod]
        public void ASRest()
        {
            string expected = "00 00 00 00 00 00 00 00 DC 22 00 00 00 00";

            string result = Executer.Execute("R1 >>= R2;");
            AreEqual(expected, result);
        }


        [TestMethod]
        public void ASLTest()
        {
            string expected = "00 00 00 00 00 00 00 00 E0 22 00 00 00 00";

            string result = Executer.Execute("R1 <<= R2;");
            AreEqual(expected, result);

        }


        [TestMethod]
        public void ORTest()
        {
            string expected = "00 00 00 00 00 00 00 00 E4 22 00 00 00 00";

            string result = Executer.Execute("R1 |= R2;");
            AreEqual(expected, result);

        }


        [TestMethod]
        public void ANDTest()
        {
            string expected = "00 00 00 00 00 00 00 00 E8 22 00 00 00 00";

            string result = Executer.Execute("R1 &= R2;");
            AreEqual(expected, result);

        }


        [TestMethod]
        public void XORTest()
        {
            string expected = "00 00 00 00 00 00 00 00 EC 22 00 00 00 00";

            string result = Executer.Execute("R1 ^= R2;");
            AreEqual(expected, result);

        }


        [TestMethod]
        public void LSLTest()
        {
            string expected = "00 00 00 00 00 00 00 00 F0 22 00 00 00 00";

            string result = Executer.Execute("R1 <= R2;");
            AreEqual(expected, result);

        }


        [TestMethod]
        public void LSRTest()
        {
            string expected = "00 00 00 00 00 00 00 00 F4 22 00 00 00 00";

            string result = Executer.Execute("R1 >= R2;");
            AreEqual(expected, result);

        }


        [TestMethod]
        public void CNDTest()
        {
            string expected = "00 00 00 00 00 00 00 00 F8 22 00 00 00 00";

            string result = Executer.Execute("R1 ?= R2;");
            AreEqual(expected, result);

        }


        [TestMethod]
        public void CBRTest()
        {
            string expected = "00 00 00 00 00 00 00 00 FC 22 00 00 00 00";

            string result = Executer.Execute("if R1 goto R2;");
            AreEqual(expected, result);

        }


        [TestMethod]
        public void NOPTest()
        {
            string expected = "00 00 00 00 00 00 00 00 40 00 00 00 00 00";

            string result = Executer.Execute("nop;");
            AreEqual(expected, result);

        }


        [TestMethod]
        public void STOPTest()
        {
            string expected = "00 00 00 00 00 00 00 00 00 00 00 00 00 00";

            string result = Executer.Execute("stop;");
            AreEqual(expected, result);

        }
    }
}
