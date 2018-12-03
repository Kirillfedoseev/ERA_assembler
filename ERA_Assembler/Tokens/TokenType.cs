﻿namespace ERA_Assembler.Tokens
{
    public enum TokenType
    {
        EndOfInput,
        Error,
        Register,
        Operator,
        Semicolon,
        Literal,
        If,
        Goto,
        Stop,
        Nop,
        Data,
        Label,
        Spaces,
        Comment,
        String,
        Comma,
        Format
    }
}