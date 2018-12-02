using System;
using System.Collections.Generic;
using System.Text;
using ERA_Assembler.Tokens;

namespace ERA_Assembler
{
    public class Executer
    {

        static void Main(string[] args)
        {
            //string code = File.ReadAllText("in.txt");
            //File.WriteAllText("out.txt", Execute(code));

            List<Token> tokens = new List<Token>();
            tokens.Add(new Token(TokenType.Register,0,0,"1"));
            tokens.Add(new Token(TokenType.Operator, 0, 1, "+="));
            tokens.Add(new Token(TokenType.Register, 0, 4, "2"));
            tokens.Add(new Token(TokenType.Semicolon, 0, 5));

            tokens.Add(new Token(TokenType.Register, 0, 0, "1"));
            tokens.Add(new Token(TokenType.Operator, 0, 1, ":="));
            tokens.Add(new Token(TokenType.String, 0, 4, "Data"));
            tokens.Add(new Token(TokenType.Semicolon, 0, 5));



            tokens.Add(new Token(TokenType.Label, 0, 4, "Data"));

            tokens.Add(new Token(TokenType.Data, 1, 0));
            tokens.Add(new Token(TokenType.Literal, 1, 0, "13"));
            tokens.Add(new Token(TokenType.Semicolon, 1, 0));

            tokens.Add(new Token(TokenType.EndOfInput, 2, 1));

            Translator translator = new Translator();
            List<byte[]> result = translator.TranslateTokens(tokens);



            Console.WriteLine(MachineCodeToReadableFormat(result));
            Console.ReadLine();
        }

        public static string Execute(string code)
        {
            //// strip windows line endings out
            code = code.Replace("\r", "");

            Lexer lexer = new Lexer();
            List<Token> tokens = lexer.Scan(code);

            Translator translator = new Translator();
            List<byte[]> result = translator.TranslateTokens(tokens);
            return MachineCodeToReadableFormat(result);
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

