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

        /// <summary>
        /// Scanning input code and create massive of tokens
        /// </summary>
        /// <param name="sourceCode">assembly code</param>
        public List<Token> Scan(string sourceCode)
        {
            List<Token> tokens = new List<Token>();
            string[] lines = sourceCode.Replace("\r","").Replace("\t","").Split('\n');
            int lineN;
            int deltaTokensN = 0;
            int oldTokensN = 0;
            for (lineN = 0; lineN < lines.Length; lineN++)
            {
                var line = lines[lineN];
                int lastTokenPosition = 1;
                int lastTokenEnd = lastTokenPosition;
                List<Action> list = new List<Action>
                {
                    () => PutSpacesToken(lineN, lastTokenEnd, line, tokens),
                    () => PutLabelToken(lineN, lastTokenEnd, line, tokens),
                    () => PutOperatorToken(lineN, lastTokenEnd, line, tokens),
                    () => PutPunctuationToken(lineN, lastTokenEnd, line, tokens),
                    () => PutLiteralToken(lineN, lastTokenEnd, line, tokens),
                    () => PutRegisterToken(lineN, lastTokenEnd, line, tokens),
                    () => PutReservedWordToken(lineN, lastTokenEnd, line, tokens),
                    () => PutStringToken(lineN, lastTokenEnd, line, tokens),
                    () => PutCommentToken(lineN, lastTokenEnd, line, tokens)
                };
                while (line.Length > 0)
                {
                    foreach (Action a in list)
                    {
                        a.Invoke();
                        deltaTokensN = tokens.Count - oldTokensN;
                        oldTokensN = tokens.Count;

                        if (deltaTokensN <= 0) continue;

                        Token lastToken = tokens[tokens.Count - 1];
                        lastTokenPosition = lastToken.Position;
                        line = line.Substring(lastToken.Lexeme.Length);
                        break;
                    }

                    if (deltaTokensN != 0 || line.Length <= 0) continue;

                    tokens.Add(new Token(TokenType.Error, lineN, lastTokenPosition));
                    RemoveUnnecessaryTokens(tokens);
                    return tokens;
                }
            }
            RemoveUnnecessaryTokens(tokens);
            tokens.Add(new Token(TokenType.EndOfInput, lineN, 1));
            return tokens;
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
                if (!match.Success) continue;
                tokens.Add(new Token(TokenType.Operator, lineN, lastTokenEnd + 1, match.Value, match.Value));
                return;
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
            var punctuationNames = new string[]
            {
                ",", ";"
            };
            foreach (string punctuationName in punctuationNames)
            {
                Regex regex = new Regex("^" + Regex.Escape(punctuationName));
                var match = regex.Match(sourceCode);
                if (match.Success)
                {
                    switch (match.Value)
                    {
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
            Regex regex = new Regex("^\\/\\/[ a-zA-Z]*");
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
                if (!unnecessaryTokens.Contains(tokens[tokenIndex].Type)) continue;

                tokens.RemoveAt(tokenIndex);
                tokenIndex--;
            }
        }

    }
}