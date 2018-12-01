using Microsoft.VisualStudio.TestTools.UnitTesting;
using ERA_Assembler;

namespace ERA_Tests
{
    [TestClass]
    public class InterpretationTest
    {            
        //todo tests for commands interpretation
        [TestMethod]
        public void LDATest()
        {
            string expected = "08 2F 00 00 00 00 00 05";

            string result = Executer.Execute("R1 := R2 + 5;");
            Assert.AreEqual(expected, result)
        }

        public void LDCTest()
        {
            string expected = "C8 25 00 00";

            string result = Executer.Execute("R1 := 5;");
            Assert.AreEqual(expected, result)
        }

        public void LDTest()
        {
            string expected = "C4 22 00 00";

            string result = Executer.Execute("R1 := *R2;");
            Assert.AreEqual(expected, result)
        }

        public void STTest()
        {
            string expected = "CC 22 00 00";

            string result = Executer.Execute("*R1 := R2;");
            Assert.AreEqual(expected, result)
        }

        public void MOVTest()
        {
            string expected = "D0 22 00 00";

            string result = Executer.Execute("R1 := R2;");
            Assert.AreEqual(expected, result)
        }

        public void AddTest()
        {
            string expected = "D4 22 00 00";

            string result = Executer.Execute("R1 += R2;");
            Assert.AreEqual(expected, result);

        }

        public void ASRest()
        {
            string expected = "DC 22 00 00";

            string result = Executer.Execute("R1 >>= R2;");
            Assert.AreEqual(expected, result)
        }

        public void ASLTest()
        {
            string expected = "E0 22 00 00";

            string result = Executer.Execute("R1 <<= R2;");
            Assert.AreEqual(expected, result);

        }

        public void ORTest()
        {
            string expected = "E4 22 00 00";

            string result = Executer.Execute("R1 |= R2;");
            Assert.AreEqual(expected, result);

        }

        public void ANDTest()
        {
            string expected = "E8 22 00 00";

            string result = Executer.Execute("R1 &= R2;");
            Assert.AreEqual(expected, result);

        }

        public void XORTest()
        {
            string expected = "EC 22 00 00";

            string result = Executer.Execute("R1 ^= R2;");
            Assert.AreEqual(expected, result);

        }

        public void LSLTest()
        {
            string expected = "F0 22 00 00";

            string result = Executer.Execute("R1 <= R2;");
            Assert.AreEqual(expected, result);

        }

        public void LSRTest()
        {
            string expected = "F4 22 00 00";

            string result = Executer.Execute("R1 >= R2;");
            Assert.AreEqual(expected, result);

        }

        public void CNDTest()
        {
            string expected = "F8 22 00 00";

            string result = Executer.Execute("R1 ?= R2;");
            Assert.AreEqual(expected, result);

        }

        public void CBRTest()
        {
            string expected = "FC 22 00 00";

            string result = Executer.Execute("if R1 goto R2;");
            Assert.AreEqual(expected, result);

        }

        public void NOPTest()
        {
            string expected = "40 00 00 00";

            string result = Executer.Execute("skip");
            Assert.AreEqual(expected, result);

        }

        public void STOPTest()
        {
            string expected = "00 00 00 00";

            string result = Executer.Execute("stop");
            Assert.AreEqual(expected, result);

        }
    }
}
