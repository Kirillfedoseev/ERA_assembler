using ERA_Assembler.Tokens;
using System;
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

        /// <summary>
        /// Adds operator token from the beginning of the code if finds one
        /// </summary>
        /// <param name="lineN"></param> number of the line
        /// <param name="lastTokenEnd"></param> end index of the last token 
        /// <param name="sourceCode"></param> the line of the source code to search in
        /// <param name="tokens"></param> list of token to add operator token to
        private void PutOperatorToken(int lineN, int lastTokenEnd, string sourceCode, List<Token> tokens)
        {
            var operatorsNames = new string[]
            {
                "+=",
                "-=",
                "&=",
                "*=",
                "|=",
                "^=",
                "?=",
                ":=",
                "<<=",
                ">>=",
                "<=",
                ">=",
                "=",
                "+",
                "-",
                "&",
                "*",
                "|",
                "^",
                "?",
            };
            foreach (string operatorName in operatorsNames)
            {
                Regex regex = new Regex("^" + Regex.Escape(operatorName));
                var match = regex.Match(sourceCode);
                if (match.Success)
                {
                    tokens.Add(new Token(TokenType.Operator, lineN, lastTokenEnd + 1, match.Value, match.Value));
                    return;
                }
            }
        }

        /// <summary>
        /// Adds spaces token from the beginning of the code if finds one
        /// </summary>
        /// <param name="lineN"></param> number of the line
        /// <param name="lastTokenEnd"></param> end index of the last token 
        /// <param name="sourceCode"></param> the line of the source code to search in
        /// <param name="tokens"></param> list of token to add operator token to
        private void PutSpacesToken(int lineN, int lastTokenEnd, string sourceCode, List<Token> tokens)
        {
            Regex regex = new Regex("^ +");
            var match = regex.Match(sourceCode);
            if (match.Success)
            {
                tokens.Add(new Token(TokenType.Spaces, lineN, lastTokenEnd + match.Value.Length, match.Value, match.Value));
            }
        }


        /// <summary>
        /// Adds register token from the beginning of the code if finds one
        /// </summary>
        /// <param name="lineN"></param> number of the line
        /// <param name="lastTokenEnd"></param> end index of the last token 
        /// <param name="sourceCode"></param> the line of the source code to search in
        /// <param name="tokens"></param> list of token to add operator token to
        private void PutRegisterToken(int lineN, int lastTokenEnd, string sourceCode, List<Token> tokens)
        {
            Regex regex = new Regex("^R\\d+");
            var match = regex.Match(sourceCode);
            if (match.Success)
            {
                tokens.Add(new Token(TokenType.Register, lineN, lastTokenEnd + 1, match.Value.Substring(1), match.Value));
            }
        }


        /// <summary>
        /// Adds punctuation token from the beginning of the code if finds one
        /// </summary>
        /// <param name="lineN"></param> number of the line
        /// <param name="lastTokenEnd"></param> end index of the last token 
        /// <param name="sourceCode"></param> the line of the source code to search in
        /// <param name="tokens"></param> list of token to add operator token to
        private void PutPunctuationToken(int lineN, int lastTokenEnd, string sourceCode, List<Token> tokens)
        {
            var puntuationNames = new string[]
            {
                ",", ";"
            };
            foreach (string punctuationName in puntuationNames)
            {
                Regex regex = new Regex("^" + Regex.Escape(punctuationName));
                var match = regex.Match(sourceCode);
                if (match.Success)
                {
                    switch (match.Value) {
                        case ",":
                            tokens.Add(new Token(TokenType.Comma, lineN, lastTokenEnd + 1, "", match.Value));
                            break;
                        case ";":
                            tokens.Add(new Token(TokenType.Semicolon, lineN, lastTokenEnd + 1, "", match.Value));
                            break;
                    }
                    return;
                }
            }
        }


        /// <summary>
        /// Adds literal token from the beginning of the code if finds one
        /// </summary>
        /// <param name="lineN"></param> number of the line
        /// <param name="lastTokenEnd"></param> end index of the last token 
        /// <param name="sourceCode"></param> the line of the source code to search in
        /// <param name="tokens"></param> list of token to add operator token to
        private void PutLiteralToken(int lineN, int lastTokenEnd, string sourceCode, List<Token> tokens)
        {
            Regex regex = new Regex("^\\d+");
            var match = regex.Match(sourceCode);
            if (match.Success)
            {
                tokens.Add(new Token(TokenType.Literal, lineN, lastTokenEnd + 1, match.Value, match.Value));
            }
        }


        /// <summary>
        /// Adds reserved word token from the beginning of the code if finds one
        /// </summary>
        /// <param name="lineN"></param> number of the line
        /// <param name="lastTokenEnd"></param> end index of the last token 
        /// <param name="sourceCode"></param> the line of the source code to search in
        /// <param name="tokens"></param> list of token to add operator token to
        private void PutReservedWordToken(int lineN, int lastTokenEnd, string sourceCode, List<Token> tokens)
        {
            var reservedWordsNames = new string[]
            {
                "if",
                "goto",
                "format",
                "stop",
                "nop",
                "data"
            };
            foreach (string reservedWordName in reservedWordsNames)
            {
                Regex regex = new Regex("^" + Regex.Escape(reservedWordName));
                var match = regex.Match(sourceCode.ToLower());
                if (match.Success)
                {
                    switch (match.Value)
                    {
                        case "if":
                            tokens.Add(new Token(TokenType.If, lineN, lastTokenEnd + 1, "", match.Value));
                            break;
                        case "goto":
                            tokens.Add(new Token(TokenType.Goto, lineN, lastTokenEnd + 1, "", match.Value));
                            break;
                        case "format":
                            tokens.Add(new Token(TokenType.Format, lineN, lastTokenEnd + 1, "", match.Value));
                            break;
                        case "stop":
                            tokens.Add(new Token(TokenType.Stop, lineN, lastTokenEnd + 1, "", match.Value));
                            break;
                        case "nop":
                            tokens.Add(new Token(TokenType.Nop, lineN, lastTokenEnd + 1, "", match.Value));
                            break;
                        case "data":
                            tokens.Add(new Token(TokenType.Data, lineN, lastTokenEnd + 1, "", match.Value));
                            break;
                    }
                    return;
                }
            }
        }


        /// <summary>
        /// Adds label token from the beginning of the code if finds one
        /// </summary>
        /// <param name="lineN"></param> number of the line
        /// <param name="lastTokenEnd"></param> end index of the last token 
        /// <param name="sourceCode"></param> the line of the source code to search in
        /// <param name="tokens"></param> list of token to add operator token to
        private void PutLabelToken(int lineN, int lastTokenEnd, string sourceCode, List<Token> tokens)
        {
            Regex regex = new Regex("^< *[a-zA-Z]+ *>");
            Regex valueRegex = new Regex("[a-zA-Z]+");
            var match = regex.Match(sourceCode);
            if (match.Success)
            {
                tokens.Add(new Token(TokenType.Label, lineN, lastTokenEnd + 1, valueRegex.Match(match.Value).Value, match.Value));
            }
        }


        /// <summary>
        /// Adds string token from the beginning of the code if finds one
        /// </summary>
        /// <param name="lineN"></param> number of the line
        /// <param name="lastTokenEnd"></param> end index of the last token 
        /// <param name="sourceCode"></param> the line of the source code to search in
        /// <param name="tokens"></param> list of token to add operator token to
        private void PutStringToken(int lineN, int lastTokenEnd, string sourceCode, List<Token> tokens)
        {
            Regex regex = new Regex("^[a-zA-Z]+");
            var match = regex.Match(sourceCode);
            if (match.Success)
            {
                tokens.Add(new Token(TokenType.String, lineN, lastTokenEnd + 1, match.Value, match.Value));
            }
        }


        /// <summary>
        /// Adds comment token from the beginning of the code if finds one
        /// </summary>
        /// <param name="lineN"></param> number of the line
        /// <param name="lastTokenEnd"></param> end index of the last token 
        /// <param name="sourceCode"></param> the line of the source code to search in
        /// <param name="tokens"></param> list of token to add operator token to
        private void PutCommentToken(int lineN, int lastTokenEnd, string sourceCode, List<Token> tokens)
        {
            Regex regex = new Regex("^\\/\\/");
            var match = regex.Match(sourceCode);
            if (match.Success)
            {
                tokens.Add(new Token(TokenType.Comment, lineN, lastTokenEnd + 1, sourceCode, sourceCode));
            }
        }


        /// <summary>
        /// Removes unnecessary tokens like comments and spaces from tokens list
        /// </summary>
        /// <param name="tokens"></param> list of tokes to remove from
        private void RemoveUnnecessaryTokens(List<Token> tokens)
        {
            HashSet<TokenType> unnecessaryTokens = new HashSet<TokenType>
            {
                TokenType.Comment,
                TokenType.Spaces
            };

            for (int tokenIndex = 0; tokenIndex < tokens.Count; tokenIndex++)
            {
                if (unnecessaryTokens.Contains(tokens[tokenIndex].Type))
                {
                    tokens.RemoveAt(tokenIndex);
                }
            }
        }


        /// <summary>
        /// Scanning input code and create massive of tokens
        /// </summary>
        /// <param name="sourceCode">assembly code</param>
        public List<Token> Scan(string sourceCode)
        {
            List<Token> tokens = new List<Token>();
            string[] lines = sourceCode.Split('\n');
            int lineN;
            int deltaTokensN = 0;
            int oldTokensN = 0;
            for (lineN = 0; lineN < lines.Length; lineN++)
            {
                var line = lines[lineN];
                int lastTokenPosition = 1;
                int lastTokenEnd = lastTokenPosition;
                List<Action> list = new List<Action>();
                list.Add(() => PutSpacesToken(lineN, lastTokenEnd, line, tokens));
                list.Add(() => PutLabelToken(lineN, lastTokenEnd, line, tokens));
                list.Add(() => PutOperatorToken(lineN, lastTokenEnd, line, tokens));
                list.Add(() => PutPunctuationToken(lineN, lastTokenEnd, line, tokens));
                list.Add(() => PutLiteralToken(lineN, lastTokenEnd, line, tokens));
                list.Add(() => PutRegisterToken(lineN, lastTokenEnd, line, tokens));
                list.Add(() => PutReservedWordToken(lineN, lastTokenEnd, line, tokens));
                list.Add(() => PutStringToken(lineN, lastTokenEnd, line, tokens));
                list.Add(() => PutCommentToken(lineN, lastTokenEnd, line, tokens));
                while (line.Length > 0)
                {
                    foreach (Action a in list)
                    {
                        a.Invoke();
                        deltaTokensN = tokens.Count - oldTokensN;
                        oldTokensN = tokens.Count;
                        if (deltaTokensN > 0)
                        {
                            Token lastToken = tokens[tokens.Count - 1];
                            lastTokenPosition = lastToken.Position;
                            line = line.Substring(lastToken.Lexeme.Length);
                            break;
                        } 
                    }
                    if (deltaTokensN == 0 && line.Length > 0)
                    {
                        tokens.Add(new Token(TokenType.Error, lineN, lastTokenPosition));
                        RemoveUnnecessaryTokens(tokens);
                        return tokens;
                    }
                }
            }
            RemoveUnnecessaryTokens(tokens);
            tokens.Add(new Token(TokenType.EndOfInput, lineN, 1));
            return tokens;
        }
    }
}