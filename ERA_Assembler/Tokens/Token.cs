
namespace ERA_Assembler.Tokens
{
    /// <summary>
    /// Storage class for tokens
    /// </summary>
    public class Token
    {
        public TokenType Type { get; set; }

        public int Line { get; set; }

        public int Position { get; set; }

        public string Value { get; set; }

        public string Lexeme { get; set; }

        public Token(TokenType type, int line, int position, string value = "", string lexeme = null)
        {
            Type = type;
            Line = line;
            Position = position;
            Value = value;
            Lexeme = lexeme;
        }

        public override string ToString()
        {
            return $"{Type.ToString(),-14} {Line,-5} {Value}";
        }
    }
}