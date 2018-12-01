using ERA_Assembler.Tokens;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ERA_Assembler
{

    /// <summary>
    /// Lexer for ERA Assembler
    /// </summary>
    public class Lexer
    {
        //todo need refactor
        #region Constants and Constructors
        // character classes 
        //todo use if needed

        private const string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_";

        private const string Numbers = "0123456789";

        private const string Identifier = Letters + Numbers;

        private const string Whitespace = " \t\n\r";

        // mappings from string keywords to token type 
        private readonly Dictionary<string, TokenType> _keywordTokenTypeMap;

        //todo use if needed
        private int _line = 1;
        private int _position = 1;
        private string _input;

        public Lexer()
        {
            _keywordTokenTypeMap = new Dictionary<string, TokenType>();
        }

        public List<Token[]> Tokens { get; } = new List<Token[]>();
        
        #endregion 

        /// <summary>
        /// Scanning input code and create massive of tokens
        /// where each Token[] is line of 3 or two parts: command, param1, param2, for example
        /// add r1, r2
        /// where r1 and r2 are registers
        /// </summary>
        /// <param name="input">assembly code</param>
        public List<Token> Scan(string input)
        {
            //todo formatted code
            //todo tokenize all strings
            //todo return error if problems
            //todo return List<Token[]> where Token[] is exact one part of input assembler code delimited by ';'
            return null;
        }


        /// <summary>
        /// match string numbers
        /// todo use only regex
        /// </summary>
        private void MatchNumber()
        {

            Regex regex_float = new Regex("^[0-9]*[.][0-9]+");
            Regex regex_int = new Regex("^[0-9][0-9]*");

            Match match = regex_float.Match(_input);
            string s = match.Value;
            if (match.Success)
            {
            }


        }


        /// <summary>
        /// match string literals
        /// todo use only regex
        /// </summary>
        private void MatchCharacter()
        {
            Regex regex = new Regex("^\'[\\S|\\s]\'|^(\'\\\\n\')|^(\'\\\\\\\\\')");    
            Match match = regex.Match(_input);
            if (!match.Success) return;

            string s = match.Value;
            //Tokens.Add(new Token(TokenType.Char, _line, _position, s));
            
        }


        /// <summary>
        /// substitute all comments to empty string
        /// </summary>
        private void MatchComments(string input)
        { 
            Regex regex = new Regex(@"^(\/\/[^\n]*\n)");
            regex.Replace(input, "");
        }


        /// <summary>
        /// Outputs Error message to the console and exits 
        /// </summary>
        /// <param name="message">Error message to display to user</param>
        /// <param name="line">Line Error occurred on</param>
        /// <param name="position">Line column that the Error occurred at</param>
        private void Error(string message, int line, int position)
        {
            //todo error system
            // output Error to the console and exit
            //Tokens.Add(new []{new Token(TokenType.Error, line, position - 1, message)});
        }

    }
}