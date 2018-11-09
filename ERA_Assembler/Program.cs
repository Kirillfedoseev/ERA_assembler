using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ERA_Assembler.Tokens;

namespace ERA_Assembler
{
    class Program
    {

        static void Main(string[] args)
        {
            string code = File.ReadAllText("in.txt");

            //// strip windows line endings out
            code = code.Replace("\r", "");

            Lexer lexer = new Lexer();
            List<Token[]> tokens = lexer.Scan(code);

            Translator translator = new Translator();
            List<byte[]> result = translator.TranslateTokens(tokens);

            File.WriteAllText("out.txt", MachineCodeToReadableFormat(result));

        }

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

