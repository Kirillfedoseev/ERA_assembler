using System.Runtime.InteropServices;
using ERA_Assembler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ERA_Tests
{
    [TestClass]
    public class MappingTests
    {
        //todo tests for mapping code to memory structure and jumps

        [TestMethod]
        public void TestRegister1()
        {
            string result =
                Executer.Execute("R3 := 1;\n" +
                                 "R2 := R3");
            string expected = "R3 1\n" +
                              "R2 1";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestRegister2()
        {
            string result = Executer.Execute("R3 := 1\n" +
                                             "R2 := 5\n" +
                                             "R2 += R3");
            string expected = "R2 = 6\n" +
                              "R3 = 1";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestRegister3()
        {
            //will cause an error because x is not defines
            string result = Executer.Execute("R3 := *x");
        }

        [TestMethod]
        public void TestRegister4()
        {
            string result = Executer.Execute("x: R1\n" +
                                             "R1 := 1" +
                                             "R3 := *x");
            string expected = "R1 1\n" +
                              "R3 1";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestRegister5()
        {
            string result = Executer.Execute("x: R1\n" +
                                             "R1 := 1\n" +
                                             "R3 := 6\n" +
                                             "R3 -= *x");
            string expected = "R1 1\n" +
                              "R3 5";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestRegisterAndLabel()
            //should not crash because goto &a is normal
        {
            string result = Executer.Execute("a: R1\n" +
                                             "R2 := 2\n" +
                                             "goto &a");
            string expected = "R1 0\n" +
                              "R2 2";
        }

        [TestMethod]
        public void TestLabel1()
        {
            //will cause an error
            string result = Executer.Execute("label L;\n" +
                                             "a:= &L");
        }

        [TestMethod]
        public void TestLabel2()
        {
            string result = Executer.Execute("R1 := L1;\n" +
                                             "if R0 goto R1; // Если условие ложно, переход на L1\n" +
                                             "<Statements1>\n" +
                                             "R1 := L2;\n" +
                                             "if R1 goto R1; // Безусловный переход на L2 \n" +
                                             "<L1> <Statements2> <L2>");

            string expected = "R0 1";

            Assert.AreEqual(expected, result);
        }
    }
}