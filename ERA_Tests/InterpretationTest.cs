using Microsoft.VisualStudio.TestTools.UnitTesting;
using ERA_Assembler;

namespace ERA_Tests
{
    [TestClass]
    public class InterpretationTest
    {            
        //todo tests for commands interpretation
        [TestMethod]
        public void TestMethod1()
        {
            string expected = "D4 22 00 00";

            string result = Executer.Execute("R1 += R2;");
            Assert.AreEqual(expected, result);

        }
    }
}
