using System;
using System.Collections.Generic;
using System.Linq;
using ERA_Assembler.Tokens;
using ERA_Assembler.Words;

namespace ERA_Assembler
{
    public class Translator
    {

        public Stack<Token> Tokens;

        public List<Word> Data;

        public List<Word> Program;

        public Mapper Mapper;
      
      
        /// <summary>
        /// Parse input token Stack to AST tree
        /// </summary>
        /// <param name="tokens">input token, where head is first lexical token</param>
        /// <returns></returns>
        public List<byte[]> TranslateTokens(List<Token> tokens)
        {
            if(tokens == null) return new List<byte[]>();
            Tokens = new Stack<Token>(((IEnumerable<Token>)tokens).Reverse());

            Program = new List<Word>();
            Data = new List<Word>();
            Mapper = new Mapper();

            Parse();
                         
            return Mapper.Map(ref Program, ref Data); ;
        }


        private void Parse()
        {
            while (Tokens.Count > 0 && Tokens.Peek().Type != TokenType.EndOfInput)
            {
                int size = Tokens.Count;

                LabelParse();

                GoToParse();

                StopSkipParse();

                OperationsWithLeftPointer();

                OperationsWithRightPointer();

                OperationSumWithConstant();

                SimpleOperations();

                LabelAssign();

                ConstantAssign();

                DataParse();

                if (size == Tokens.Count) Error("Something goes wrong: ", Tokens.Pop());
            }

            if (Tokens.Pop().Type == TokenType.EndOfInput) Program.Add(new StopCommand());
            else throw new Exception("No end of input file!");
        }

       
        private void LabelParse()
        {
            if (Tokens.Count < 1) return;


            if (Tokens.Peek().Type == TokenType.Label)
            {
                Label label = new Label(Tokens.Pop().Value, Program.Count - 1);
                Mapper.Labels.Add(label.Name,label);
            }
        }


        private void GoToParse()
        {
            if (Tokens.Count < 4) return;


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
                    Program.Add(cmd);
                    isSuccess = true;
                }
            }

            if (isSuccess) return;

            Tokens.Push(t4);
            Tokens.Push(t3);
            Tokens.Push(t2);
            Tokens.Push(t1);
        }


        private void StopSkipParse()
        {
            if (Tokens.Count < 2) return;

            bool isSuccess = false;
            Token t1 = Tokens.Pop();
            Token t2 = Tokens.Pop();

            switch (t2.Type)
            {
                case TokenType.Semicolon when t1.Type == TokenType.Stop:
                {
                        Words.Commands cmd = new StopCommand();
                    Program.Add(cmd);
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
                            Words.Commands cmd = new StopCommand(value);
                        Program.Add(cmd);
                        isSuccess = true;
                    }
                    break;
                }
                case TokenType.Semicolon when t1.Type == TokenType.Nop:
                {
                        Words.Commands cmd = new SkipCommand();
                    Program.Add(cmd);
                    isSuccess = true;
                    break;
                }
                case TokenType.Literal when t1.Type == TokenType.Nop:
                {
                    if (Tokens.Peek().Type != TokenType.Semicolon) Error("No semicolon in operation", t2);
                    else
                    {
                        Tokens.Pop();
                        int value = Convert.ToInt32(t2.Value);
                            Words.Commands cmd = new SkipCommand(value);
                        Program.Add(cmd);
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
            if (Tokens.Count < 3) return;

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
                            cmd = new CopyRegisterToRegisterCommand(r1, r2, Format.Int32);
                            break;
                        case "-=":
                            cmd = new SubCommand(r1, r2, Format.Int32);
                            break;
                        case "+=":
                            cmd = new AddCommand(r1, r2, Format.Int32);
                            break;
                        case "?=":
                            cmd = new ArithmeticCompareCommand(r1, r2, Format.Int32);
                            break;
                        case "|=":
                            cmd = new LogicOrCommand(r1, r2, Format.Int32);
                            break;
                        case "&=":
                            cmd = new LogicAndCommand(r1, r2, Format.Int32);
                            break;
                        case "^=":
                            cmd = new LogicXorCommand(r1, r2, Format.Int32);
                            break;
                        case "<<=":
                            cmd = new ArithmeticLeftShiftCommand(r1, r2, Format.Int32);
                            break;
                        case ">>=":
                            cmd = new ArithmeticRightShiftCommand(r1, r2, Format.Int32);
                            break;
                        case ">=":
                            cmd = new LogicRightShiftCommand(r1, r2, Format.Int32);
                            break;
                        case "<=":
                            cmd = new LogicLeftShiftCommand(r1, r2, Format.Int32);
                            break;
                    }

                    if (cmd != null)
                    {
                        Program.Add(cmd);
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
            if(Tokens.Count < 4) return;

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
                    Program.Add(cmd);
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
            if (Tokens.Count < 4) return;

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
                    Program.Add(cmd);
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
            if (Tokens.Count < 5) return;


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
                    byte r2 = Convert.ToByte(t1.Value);
                    byte r1 = Convert.ToByte(t3.Value);
                    BinaryCommand cmd = new AddNextConstCommand(r1, r2);
                    Program.Add(cmd);
                    int c = Convert.ToInt32(t5.Value);
                    Constant word = new Constant(c);
                    Program.Add(word);
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
            if (Tokens.Count < 3) return;


            bool isSuccess = false;
            Token t1 = Tokens.Pop();
            Token t2 = Tokens.Pop();
            Token t3 = Tokens.Pop();

            if (t1.Type == TokenType.Register &&
                t2.Type == TokenType.Operator &&
                t2.Value == ":=" &&
                t3.Type == TokenType.String)
            {
                if (Tokens.Peek().Type != TokenType.Semicolon) Error("No semicolon in operation", t3);
                else
                {
                    Tokens.Pop();

                    byte r1 = Convert.ToByte(t1.Value);
                    BinaryCommand prevCmd = new AssignConstCommand(0,r1);
                    Program.Add(prevCmd);
                    BinaryCommand cmd = new AddNextConstCommand(r1,r1);
                    Program.Add(cmd);

                    Label label = Mapper.Labels.Contains(t3.Value) ? (Label)Mapper.Labels[t3.Value] : null;
                    LabelAddress address = new LabelAddress(label);
                    if (label == null)
                    {
                        Mapper.Unreferenced.Add(new KeyValuePair<string, LabelAddress>(t3.Value, address));
                    }
                    Program.Add(address);

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
            if (Tokens.Count < 3) return;


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
                    byte r2 = Convert.ToByte(t1.Value);
                    byte r1 = Convert.ToByte(t3.Value);
                    BinaryCommand cmd = new AssignConstCommand(r1, r2);
                    Program.Add(cmd);
                    isSuccess = true;
                }
            }


            if (isSuccess) return;

            Tokens.Push(t3);
            Tokens.Push(t2);
            Tokens.Push(t1);
        }


        private void DataParse()
        {
            if (Tokens.Count < 3) return;


            bool isSuccess = false;
            Token t1 = Tokens.Pop();
            Token t2 = Tokens.Pop();
            Token t3 = Tokens.Pop();

            if (t1.Type == TokenType.Data &&
                t2.Type == TokenType.Literal)
            {
                while (true)
                {
                    if (t2.Type != TokenType.Literal || t3.Type != TokenType.Comma && t3.Type != TokenType.Semicolon)
                    {
                        Error("Data format exception: ", t3);
                        break;
                    }

                    int value = Convert.ToInt32(t2.Value);
                    Word constant = new Constant(value);
                    Data.Add(constant);

                    if (t3.Type == TokenType.Semicolon)
                    {
                        isSuccess = true;
                        break;
                    }

                    if (Tokens.Count < 2) break;
                    t2 = Tokens.Pop();
                    t3 = Tokens.Pop();
                } 
            }
            

            if (isSuccess) return;

            Tokens.Push(t3);
            Tokens.Push(t2);
            Tokens.Push(t1);
        }


        /// <summary>
        /// Error
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="token">Token</param>
        private void Error(string msg, Token token)
        {      
            throw new Exception(msg + "\n"  + token);
        }
    }
}