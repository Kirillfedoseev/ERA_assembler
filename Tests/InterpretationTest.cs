using Microsoft.VisualStudio.TestTools.UnitTesting;
using ERA_Assembler;
namespace Tests
{
    [TestClass]
    public class InterpretationTest
    {            
        //todo tests for commands interpretation
        [TestMethod]
        public void TestMethod1()
        {
            string result = Executer.Execute("R1 += R2;");
            Assert.AreNotEqual(result,"D4 22 00 00");

        }
    }
}