using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ERA_Assembler;
using ERA_Assembler.Tokens;

namespace ERA_Tests
{
    [TestClass]
    public class LexerTests
    {

        public string Execute(string input)
        {
            Lexer lexer = new Lexer();
            List<Token> tokens = lexer.Scan(input);
            StringBuilder sb = new StringBuilder();
            foreach (Token token in tokens)
            {
                sb.AppendLine(token.ToString());
            }

            return sb.ToString();
        }


        public void AreEqual(string expected, string actual)
        {
            Console.WriteLine(actual);
            expected = expected.Replace(" ", "").Replace("\n", "");
            actual = actual.Replace(" ", "").Replace("\n", "").Replace("\r", "");

            Assert.AreEqual(expected, actual);
        }



        //todo tests for commands lexer
        [TestMethod]
        public void AddCmdTest()
        {
            string expected = "Register 0 1\n" +
                                "Operator 0 +=\n" +
                                "Register 0 2\n" +
                                "Semicolon 0\n" +
                                "EndOfInput 1";

            string result = Execute("R1 += R2;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void AssignTest()
        {
            string expected = "Register 0 1\n" +
                                "Operator 0 :=\n" +
                                "Register 0 2\n" +
                                "Semicolon 0\n" +
                                "EndOfInput 1";

            string result = Execute("R1 := R2;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void SubTest()
        {
            string expected = "Register 0 1\n" +
                                "Operator 0 -=\n" +
                                "Register 0 2\n" +
                                "Semicolon 0\n" +
                                "EndOfInput 1";

            string result = Execute("R1 -= R2;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void AsrTest()
        {
            string expected = "Register 0 1\n" +
                                "Operator 0 >>=\n" +
                                "Register 0 2\n" +
                                "Semicolon 0\n" +
                                "EndOfInput 1";

            string result = Execute("R1 >>= R2;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void AslTest()
        {
            string expected = "Register 0 1\n" +
                                "Operator 0 <<=\n" +
                                "Register 0 2\n" +
                                "Semicolon 0\n" +
                                "EndOfInput 1";

            string result = Execute("R1 <<= R2;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void OrTest()
        {
            string expected = "Register 0 1\n" +
                                "Operator 0 |=\n" +
                                "Register 0 2\n" +
                                "Semicolon 0\n" +
                                "EndOfInput 1";

            string result = Execute("R1 |= R2;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void AndTest()
        {
            string expected = "Register 0 1\n" +
                                "Operator 0 &=\n" +
                                "Register 0 2\n" +
                                "Semicolon 0\n" +
                                "EndOfInput 1";

            string result = Execute("R1 &= R2;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void XorTest()
        {
            string expected = "Register 0 1\n" +
                                "Operator 0 ^=\n" +
                                "Register 0 2\n" +
                                "Semicolon 0\n" +
                                "EndOfInput 1";

            string result = Execute("R1 ^= R2;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void LslTest()
        {
            string expected = "Register 0 1\n" +
                                "Operator 0 <=\n" +
                                "Register 0 2\n" +
                                "Semicolon 0\n" +
                                "EndOfInput 1";

            string result = Execute("R1 <= R2;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void LsrTest()
        {
            string expected = "Register 0 1\n" +
                                "Operator 0 >=\n" +
                                "Register 0 2\n" +
                                "Semicolon 0\n" +
                                "EndOfInput 1";

            string result = Execute("R1 >= R2;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void CndTest()
        {
            string expected = "Register 0 1\n" +
                                "Operator 0 ?=\n" +
                                "Register 0 2\n" +
                                "Semicolon 0\n" +
                                "EndOfInput 1";

            string result = Execute("R1 ?= R2;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void GotoTest()
        {
            string expected = "If 0\n" +
                                "Register 0 12\n" +
                                "Goto 0\n" +
                                "Register 0 14\n" +
                                "Semicolon 0\n" +
                                "EndOfInput 1";

            string result = Execute("if R12 goto R14;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void StopTest()
        {
            string expected = "Stop 0\n" +
                                "EndOfInput 1";

            string result = Execute("stop");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void NopTest()
        {
            string expected = "Nop 0\n" +
                              "Semicolon 0\n" +
                              "EndOfInput 1";

            string result = Execute("nop;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void Test1()
        {
            string expected = "Register 0 0\n" +
                                "Operator 0 :=\n" +
                                "Literal 0 2\n" +
                                "Semicolon 0\n" +
                                "Register 1 0\n" +
                                "Operator 1 +=\n" +
                                "Register 1 31\n" +
                                "Semicolon 1\n" +
                                "Operator 2 *\n" +
                                "Register 2 0\n" +  
                                "Operator 2 :=\n" +
                                "Register 2 1\n" +
                                "Semicolon 2\n" +
                                "EndOfInput 3";

            string result = Execute("R0 := 2;\n" +
                                                "R0 += R31;\n" +
                                                 "*R0 := R1;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void Test2()
        {
            string expected = "Register 0 0\n" +
                                "Operator 0 :=\n" +
                                "Literal 0 2\n" +
                                "Semicolon 0\n" +
                                "Register 1 0\n" +
                                "Operator 1 +=\n" +
                                "Register 1 31\n" + 
                                "Semicolon 1\n" +
                                "Operator 2 *\n" +
                                "Register 2 31\n" +  
                                "Operator 2 =\n" +
                                "Register 2 31\n" +
                                "Semicolon 2\n" +
                                "Operator 3 *\n" +
                                "Register 3 0\n" +  
                                "Operator 3 :=\n" +
                                "Register 3 0\n" +
                                "Semicolon 3\n" +
                                "If 4\n" +
                                "Register 4 0\n" +
                                "Goto 4\n" +
                                "Register 4 0\n" +
                                "Semicolon 4\n" +
                                "EndOfInput 5";

            string result = Execute("R0 := 2;\n" +
                                                "R0 += R31;\n" + 
                                                "*R31 = R31;\n" +  
                                                "*R0 := R0;\n" + 
                                                "if R0 goto R0;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void Test3()
        {
            string expected = "Register 0 1\n" +
                                "Operator 0 :=\n" +
                                "Literal 0 1\n" +
                                "Semicolon 0\n" +
                                "Label 1 LoopOuter\n" +
                                "Register 2 3\n" +
                                "Operator 2 :=\n" +
                                "String 2 Size\n" + 
                                "Semicolon 2\n" +
                                "Register 3 3\n" +
                                "Operator 3 :=\n" +
                                "Operator 3 *\n" +
                                "Register 3 3\n" + 
                                "Semicolon 3\n" +
                                "Register 4 4\n" +
                                "Operator 4 :=\n" +
                                "Literal 4 1\n" +
                                "Semicolon 4\n" +
                                "Register 5 3\n" +
                                "Operator 5 -=\n" +
                                "Register 5 4\n" +
                                "Semicolon 5\n" +
                                "Register 6 3\n" +
                                "Operator 6 ?=\n" +
                                "Register 6 1\n" +
                                "Semicolon 6\n" +
                                "Register 7 4\n" +
                                "Operator 7 &=\n" +
                                "Register 7 3\n" +
                                "Semicolon 7\n" +
                                "Register 8 3\n" +
                                "Operator 8 :=\n" +
                                "String 8 OutOuter\n" + 
                                "Semicolon 8\n" +
                                "If 9\n" +
                                "Register 9 4\n" +
                                "Goto 9\n" +
                                "Register 9 3\n" +
                                "Semicolon 9\n" +
                                "EndOfInput 10";

            string result = Execute("R1 := 1;\n" + 
                                                "<LoopOuter>\n" +
                                                "R3 := Size;\n" +
                                                "R3 := *R3;\n" +
                                                "R4 := 1;\n" +
                                                "R3 -= R4;\n" +
                                                "R3 ?= R1;\n" +
                                                "R4 &= R3;\n" +
                                                "R3 := OutOuter;\n" +
                                                "if R4 goto R3;");
            AreEqual(expected, result);
            

        }
         [TestMethod]
        public void Test4()
        {
            string expected = "Label 0 OutOuter\n" +
                                "Register 1 15\n" +
                                "Operator 1 :=\n" +
                                "String 1 Size\n" +
                                "Semicolon 1\n" +
                                "Register 2 15\n" +
                                "Operator 2 :=\n" +
                                "Operator 2 *\n" +
                                "Register 2 15\n" +
                                "Semicolon 2\n" + 
                                "Register 3 16\n" +
                                "Operator 3 :=\n" +
                                "String 3 Array\n" +
                                "Semicolon 3\n" +
                                "Stop 4\n" +
                                "Semicolon 4\n" +
                                "Nop 4\n" +
                                "Semicolon 4\n"+
                                "EndOfInput 5";

            string result = Execute("<OutOuter>\n" +
                                                "R15 := Size;\n" +
                                                "R15 := *R15;\n" +
                                                "R16 := Array;\n" +
                                                "STOP; NOP;");
            AreEqual(expected, result);

        }

        [TestMethod]
        public void Test5()
        {
            string expected = "Label 0 Size\n" +
                                "Data 1\n" +
                                "Literal 1 20\n" +
                                "Label 2 Array\n" +
                                "Data 3\n" +
                                "Literal 3 537\n" +
                                "Comma 3\n" +
                                "Literal 3 242\n" +
                                "Comma 3\n" +
                                "Literal 3 114\n" +
                                "Comma 3\n" +
                                "Literal 3 436\n" +
                                "Comma 3\n" +
                                "Literal 3 337\n" +
                                "Comma 3\n" +
                                "Literal 3 296\n" +
                                "Comma 3\n" +
                                "Literal 3 285\n" +
                                "Comma 3\n" +
                                "Literal 3 655\n" +
                                "Comma 3\n" +
                                "Literal 3 639\n" +
                                "Comma 3\n" +
                                "Literal 3 436\n" +
                                "Data 4\n" +
                                "Literal 4 912\n" +
                                "Comma 4\n" +
                                "Literal 4 520\n" +
                                "Comma 4\n" +
                                "Literal 4 624\n" +
                                "Comma 4\n" +
                                "Literal 4 551\n" +
                                "Comma 4\n" +
                                "Literal 4 600\n" +
                                "Comma 4\n" +
                                "Literal 4 741\n" +
                                "Comma 4\n" +
                                "Literal 4 612\n" +
                                "Comma 4\n" +
                                "Literal 4 943\n" +
                                "Comma 4\n" +
                                "Literal 4 871\n" +
                                "Comma 4\n" +
                                "Literal 4 735\n" +
                                "EndOfInput 5";

            string result = Execute("<Size>\n" +
                                                "DATA 20\n" +
                                                "<Array>\n" +
                                                "DATA 537, 242, 114, 436, 337, 296, 285, 655, 639, 436\n" +
                                                "DATA 912, 520, 624, 551, 600, 741, 612, 943, 871, 735");
            AreEqual(expected, result);

        }
    }
}