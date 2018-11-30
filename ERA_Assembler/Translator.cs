using System.Collections.Generic;
using ERA_Assembler.Commands;
using ERA_Assembler.Tokens;

namespace ERA_Assembler
{
    public class Translator
    {             
        /// <summary>
        /// Parse input token Stack to AST tree
        /// </summary>
        /// <param name="tokens">input token, where head is first lexical token</param>
        /// <returns></returns>
        public List<byte[]> TranslateTokens(List<Token[]> tokens)
        {


            //todo convert List<Token[]> to List<Commands>
            List<Command> commands = new List<Command>();


            //todo mapped commands to memory
            Mapper mapper = new Mapper();
            mapper.MapCommands(ref commands);

            //todo foreach command.GetML()
            List<byte[]> bytesList = new List<byte[]>(commands.Count);
            foreach (Command command in commands)
            {
                bytesList.Add(command.GetBytes());
            }

            return bytesList;
        }

        /// <summary>
        /// Err
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="tokenType">Token type</param>
        private void Error(string msg, TokenType tokenType)
        {      
            //todo error(Type.Line, Type.Position, "%tokenType: Expecting '%tokenType', found '%tokenType'\n", msg, atr[tokenType].Value, atr[Type.Type].Value);
        }
    }
}