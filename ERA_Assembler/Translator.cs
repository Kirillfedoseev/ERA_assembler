﻿using System;
using System.Collections.Generic;
using ERA_Assembler.Commands;
using ERA_Assembler.Tokens;

namespace ERA_Assembler
{
    public class Translator
    {

        public Stack<Token> Tokens;

        public List<Word> Words;

        public Mapper Mapper;
      
      
        /// <summary>
        /// Parse input token Stack to AST tree
        /// </summary>
        /// <param name="tokens">input token, where head is first lexical token</param>
        /// <returns></returns>
        public List<byte[]> TranslateTokens(List<Token> tokens)
        {
            Tokens = new Stack<Token>(tokens);
            Words = new List<Word>();
            Mapper mapper = new Mapper();

            Parse();

            mapper.Map();

            List<byte[]> bytesList = new List<byte[]>(Words.Count);
            foreach (Word word in Words)          
                bytesList.Add(word.GetBytes());
            

            return bytesList;
        }

        private void Parse()
        {
            while (Tokens.Count > 0)
            {
                int size = Tokens.Count;
                LabelParse();

                GoToParse();

                StopParse();

                SimpleOperations();

                OperationsWithLeftPointer();

                OperationsWithRightPointer();

                OperationSumWithConstant();

                LabelAssign();

                ConstantAssign();

                if (size == Tokens.Count) Error("Something wrong: ", Tokens.Pop());

            }

        }


        private void LabelParse()
        {
            if (Tokens.Peek().Type == TokenType.Label)
            {
                Label label = new Label(Tokens.Pop().Value, Words.Count);
                Words.Add(label);
                Mapper.Labels.Add(label.Name,label);
            }
        }


        private void GoToParse()
        {
            bool isSuccess = false;
            Token t1 = Tokens.Pop();
            Token t2 = Tokens.Pop();
            Token t3 = Tokens.Pop();
            Token t4 = Tokens.Pop();

            if (t1.Type == TokenType.If && 
                t2.Type == TokenType.Register && 
                t3.Type == TokenType.Goto &&
                t4.Type == TokenType.Register)
            {
                if (Tokens.Peek().Type != TokenType.Semicolon) Error("No semicolon in operation", t4);
                else
                {
                    Tokens.Pop();
                    byte r1 = Convert.ToByte(t2.Value);
                    byte r2 = Convert.ToByte(t4.Value);
                    BinaryCommand cmd = new GotoCommand(r1, r2);
                    Words.Add(cmd);
                    isSuccess = true;
                }
            }

            if (isSuccess) return;

            Tokens.Push(t4);
            Tokens.Push(t3);
            Tokens.Push(t2);
            Tokens.Push(t1);
        }


        private void StopParse()
        {
            bool isSuccess = false;
            Token t1 = Tokens.Pop();
            Token t2 = Tokens.Pop();

            switch (t2.Type)
            {
                case TokenType.Semicolon when t1.Type == TokenType.Stop:
                {
                    Command cmd = new StopCommand();
                    Words.Add(cmd);
                    isSuccess = true;
                    break;
                }
                case TokenType.Literal when t1.Type == TokenType.Stop:
                {
                    if (Tokens.Peek().Type != TokenType.Semicolon) Error("No semicolon in operation", t2);
                    else
                    {
                        Tokens.Pop();
                        int value = Convert.ToInt32(t2.Value);
                        Command cmd = new StopCommand(value);
                        Words.Add(cmd);
                        isSuccess = true;
                    }
                    break;
                }
            }
                    


            if (isSuccess) return;

            Tokens.Push(t2);
            Tokens.Push(t1);
        }


        private void SimpleOperations()
        {
            bool isSuccess = false;
            Token t1 = Tokens.Pop();
            Token t2 = Tokens.Pop();
            Token t3 = Tokens.Pop();

            //simple operation of two registers
            if (t1.Type == TokenType.Register && 
                t2.Type == TokenType.Operator && 
                t3.Type == TokenType.Register)
            {
                if (Tokens.Peek().Type != TokenType.Semicolon) Error("No semicolon in operation", t3);               
                else
                {
                    Tokens.Pop();
                    BinaryCommand cmd = null;
                    byte r1 = Convert.ToByte(t1.Value);
                    byte r2 = Convert.ToByte(t3.Value);
                    switch (t2.Value)
                    {
                        case ":=":
                            cmd = new CopyRegisterToRegisterCommand(r1, r2, Formate.Int32);
                            break;
                        case "-=":
                            cmd = new SubCommand(r1, r2, Formate.Int32);
                            break;
                        case "+=":
                            cmd = new AddCommand(r1, r2, Formate.Int32);
                            break;
                        case "?=":
                            cmd = new ArithmeticCompareCommand(r1, r2, Formate.Int32);
                            break;
                        case "|=":
                            cmd = new LogicOrCommand(r1, r2, Formate.Int32);
                            break;
                        case "&=":
                            cmd = new LogicAndCommand(r1, r2, Formate.Int32);
                            break;
                        case "^=":
                            cmd = new LogicXorCommand(r1, r2, Formate.Int32);
                            break;
                        case "<<=":
                            cmd = new ArithmeticLeftShiftCommand(r1, r2, Formate.Int32);
                            break;
                        case ">>=":
                            cmd = new ArithmeticRightShiftCommand(r1, r2, Formate.Int32);
                            break;
                        case ">=":
                            cmd = new LogicRightShiftCommand(r1, r2, Formate.Int32);
                            break;
                        case "<=":
                            cmd = new LogicLeftShiftCommand(r1, r2, Formate.Int32);
                            break;
                    }

                    if (cmd != null)
                    {
                        Words.Add(cmd);
                        isSuccess = true;
                    }
                }
            }

            if (isSuccess) return;

            Tokens.Push(t3);
            Tokens.Push(t2);
            Tokens.Push(t1);
        }


        private void OperationsWithLeftPointer()
        {

            bool isSuccess = false;
            Token t1 = Tokens.Pop();
            Token t2 = Tokens.Pop();
            Token t3 = Tokens.Pop();
            Token t4 = Tokens.Pop();

           
            
            if (t1.Type == TokenType.Operator && 
                t1.Value == "*" && //first register with pointer
                t2.Type == TokenType.Register && 
                t3.Type == TokenType.Operator &&
                t4.Type == TokenType.Register)
            {
                if (Tokens.Peek().Type != TokenType.Semicolon) Error("No semicolon in operation", t4);
                else
                {
                    Tokens.Pop();
                    byte r1 = Convert.ToByte(t2.Value);
                    byte r2 = Convert.ToByte(t4.Value);
                    BinaryCommand cmd = new CopyRegisterToMemoryCommand(r1, r2);
                    Words.Add(cmd);
                    isSuccess = true;
                }
            }

            if (isSuccess) return;

            Tokens.Push(t4);
            Tokens.Push(t3);
            Tokens.Push(t2);
            Tokens.Push(t1);
        }


        private void OperationsWithRightPointer()
        {

            bool isSuccess = false;
            Token t1 = Tokens.Pop();
            Token t2 = Tokens.Pop();
            Token t3 = Tokens.Pop();
            Token t4 = Tokens.Pop();

            if (t1.Type == TokenType.Register &&
                t2.Type == TokenType.Operator &&
                t3.Type == TokenType.Operator &&
                t3.Value == "*" && //second register with pointer
                t4.Type == TokenType.Register)
            {
                if (Tokens.Peek().Type != TokenType.Semicolon) Error("No semicolon in operation", t4);
                else
                {
                    Tokens.Pop();
                    byte r1 = Convert.ToByte(t1.Value);
                    byte r2 = Convert.ToByte(t4.Value);
                    BinaryCommand cmd = new LoadFromMemoryCommand(r1, r2);
                    Words.Add(cmd);
                    isSuccess = true;
                }
            }


            if (isSuccess) return;

            Tokens.Push(t4);
            Tokens.Push(t3);
            Tokens.Push(t2);
            Tokens.Push(t1);
        }


        private void OperationSumWithConstant()
        {
            bool isSuccess = false;
            Token t1 = Tokens.Pop();
            Token t2 = Tokens.Pop();
            Token t3 = Tokens.Pop();
            Token t4 = Tokens.Pop();
            Token t5 = Tokens.Pop();

            if (t1.Type == TokenType.Register &&
                t2.Type == TokenType.Operator &&
                t2.Value == ":=" &&
                t3.Type == TokenType.Register &&
                t4.Type == TokenType.Operator &&
                t4.Value == "+" &&
                t5.Type == TokenType.Literal)
            {
                if (Tokens.Peek().Type != TokenType.Semicolon) Error("No semicolon in operation", t4);
                else
                {
                    Tokens.Pop();
                    byte r1 = Convert.ToByte(t1.Value);
                    byte r2 = Convert.ToByte(t3.Value);
                    BinaryCommand cmd = new AddNextConstCommand(r1, r2);
                    Words.Add(cmd);
                    int c = Convert.ToInt32(t5.Value);
                    Constant word = new Constant(c);
                    Words.Add(word);
                    isSuccess = true;
                }
            }


            if (isSuccess) return;

            Tokens.Push(t5);
            Tokens.Push(t4);
            Tokens.Push(t3);
            Tokens.Push(t2);
            Tokens.Push(t1);
        }


        private void LabelAssign()
        {
            bool isSuccess = false;
            Token t1 = Tokens.Pop();
            Token t2 = Tokens.Pop();
            Token t3 = Tokens.Pop();

            if (t1.Type == TokenType.Register &&
                t2.Type == TokenType.Operator &&
                t2.Value == ":=" &&
                t3.Type == TokenType.Label)
            {
                if (Tokens.Peek().Type != TokenType.Semicolon) Error("No semicolon in operation", t3);
                else
                {
                    Tokens.Pop();

                    byte r1 = Convert.ToByte(t1.Value);
                    byte r2 = Convert.ToByte(30); // by idea in that register we store address of global data
                    BinaryCommand cmd = new AddNextConstCommand(r1,r2);
                    Words.Add(cmd);

                    Label label = Mapper.Labels.Contains(t3.Value) ? (Label)Mapper.Labels[t3.Value] : null;
                    LabelAddress address = new LabelAddress(label);
                    if (label == null)
                    {
                        Mapper.Unreferenced.Add(new KeyValuePair<string, LabelAddress>(t3.Value, address));
                    }
                    Words.Add(address);

                    isSuccess = true;
                }
            }

            if (isSuccess) return;

            Tokens.Push(t3);
            Tokens.Push(t2);
            Tokens.Push(t1);
        }


        private void ConstantAssign()
        {
            bool isSuccess = false;
            Token t1 = Tokens.Pop();
            Token t2 = Tokens.Pop();
            Token t3 = Tokens.Pop();


            if (t1.Type == TokenType.Register &&
                t2.Type == TokenType.Operator &&
                t2.Value == ":=" &&
                t3.Type == TokenType.Literal)
            {
                if (Tokens.Peek().Type != TokenType.Semicolon) Error("No semicolon in operation", t3);
                else
                {
                    Tokens.Pop();
                    byte r1 = Convert.ToByte(t1.Value);
                    byte r2 = Convert.ToByte(t3.Value);
                    BinaryCommand cmd = new AddNextConstCommand(r1, r2);
                    Words.Add(cmd);
                    isSuccess = true;
                }
            }


            if (isSuccess) return;

            Tokens.Push(t3);
            Tokens.Push(t2);
            Tokens.Push(t1);
        }
     
     
        /// <summary>
        /// Err
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="tokenType">Token type</param>
        private void Error(string msg, Token token)
        {      
            //todo error(Type.Line, Type.Position, "%tokenType: Expecting '%tokenType', found '%tokenType'\n", msg, atr[tokenType].Value, atr[Type.Type].Value);
        }
    }
}