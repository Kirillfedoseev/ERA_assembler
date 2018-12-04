using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ERA_Assembler.Tokens;

namespace ERA_Assembler
{
    public class Executer
    {

        static void Main(string[] args)
        {
            string code = File.ReadAllText("in.txt"); //input from doc
            File.WriteAllText("out.txt", Execute(code));
        }


        /// <summary>
        /// Compile ERA assembly code to machine code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Execute(string code)
        {

            Lexer lexer = new Lexer();
            List<Token> tokens = lexer.Scan(code);

            Translator translator = new Translator();
            List<byte[]> result = translator.TranslateTokens(tokens);
            return MachineCodeToReadableFormat(result);
        }


        /// <summary>
        /// Reformat binary tupple to readable bytes list
        /// </summary>
        /// <param name="bytesList"></param>
        /// <returns></returns>
        private static string MachineCodeToReadableFormat(List<byte[]> bytesList)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte[] bytes in bytesList)
            {
                string hex = BitConverter.ToString(bytes).Replace("-", " ");
                sb.AppendLine(hex);
            }

            return sb.ToString();
        }
    }
}

