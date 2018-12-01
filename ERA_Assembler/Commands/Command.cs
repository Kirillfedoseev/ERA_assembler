using System;

namespace ERA_Assembler.Commands
{

    public abstract class Word
    {
        public abstract byte[] GetBytes();
    }

    /// <summary>
    /// Constant of type
    /// 0..0
    ///  32
    /// </summary>
    public class Constant:Word
    {
        protected int Value;
        public Constant(int value = 0) => Value = value;

        public override byte[] GetBytes() => BitConverter.GetBytes(Value);
    }


    /// <summary>
    /// Label Address as Constant
    /// 0..0
    ///  32
    /// </summary>
    public class LabelAddress : Word
    {
        protected Label RefLabel;
        public LabelAddress(Label label) => RefLabel = label;

        public void SetLabel(Label label) => RefLabel = label;

        public override byte[] GetBytes() => RefLabel.GetBytes();
    }


    /// <summary>
    /// Label of type
    /// 0..0
    ///  32
    /// </summary>
    public class Label : Word
    {
        public string Name;
        protected int Address;
        protected int CodeOffset;
        public Label(string name, int address)
        {
            Name = name;
            Address = address;
        }

        public void MapOnMemory(int codeOffset)
        {
            try
            {
                checked
                {
                    int a = codeOffset + Address;
                }
                CodeOffset = codeOffset;
            }
            catch (OverflowException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override byte[] GetBytes() => BitConverter.GetBytes(Address + CodeOffset);



    }



    /// <summary>
    /// Any meaningful command
    /// </summary>
    public abstract class Command:Word
    {
        protected readonly int CmdNum;
        protected readonly int Format;

        protected Command(int cmdNum, int format)
        {    
            CmdNum = cmdNum;
            Format = format;
        }
    }


    /// <summary>
    /// Command of type like
    /// 00 0000 00000 00000 0..0
    /// f  cmd   r1    r2    16
    /// </summary>
    public abstract class BinaryCommand : Command
    {
        protected int Register1;
        protected int Register2;

        protected BinaryCommand(byte register1, byte register2, int cmdNum, int format) : base(cmdNum, format)
        {
            if (register1 >= 32) throw new Exception("Register 1 out of bound: " + register1);
            if (register2 >= 32) throw new Exception("Register 2 out of bound: " + register2);

            Register1 = register1;
            Register2 = register2;
        }

        public override byte[] GetBytes()
        {
            int a = (Format << 29) + (CmdNum << 25) + (Register1 << 21) + (Register2 << 16);
            return BitConverter.GetBytes(a);
        }

    }



    /// <summary>
    /// NOP
    /// skip;
    /// skip 0..1023;
    /// </summary>
    public class StopCommand : Command
    {
        protected int Value;

        public StopCommand(int value = 0) : base(0, 1)
        {
            if(Value >= 1024 || Value < 0) throw new Exception("Stop constant overflow: " + value);
            Value = value;
        }

        public override byte[] GetBytes()
        {
            int a = (Format << 29) + (CmdNum << 25) + (Value << 16);
            return BitConverter.GetBytes(a);
        }
    }


    /// <summary>
    /// LD i j
    /// Rj := *Ri;
    /// </summary>
    public class LoadFromMemoryCommand : BinaryCommand
    {
        public LoadFromMemoryCommand(byte register1, byte register2) : base(register1, register2, 1, 3) { }
    }

    /// <summary>
    /// LDA i j
    /// Rj := Ri + Constant;
    /// </summary>
    public class AddNextConstCommand : BinaryCommand
    {
        public AddNextConstCommand(byte register1, byte register2) : base(register1, register2, 2, 3) { }
    }

    /// <summary>
    /// LDC Constant j
    /// Rj := Constant;
    /// Constant = [0;31]
    /// </summary>
    public class AssignConstCommand : BinaryCommand
    {
        //todo understand why the same command number as in LDA
        public AssignConstCommand(byte register1, byte register2) : base(register1, register2, 2, 3) { }
    }

    /// <summary>
    /// ST i j
    /// *Rj := Ri;
    /// </summary>
    public class CopyRegisterToMemoryCommand : BinaryCommand
    {
        public CopyRegisterToMemoryCommand(byte register1, byte register2) : base(register1, register2, 3, 3) { }
    }

    /// <summary>
    /// MOV i j
    /// Rj := Ri;
    /// </summary>
    public class CopyRegisterToRegisterCommand : BinaryCommand
    {
        public CopyRegisterToRegisterCommand(byte register1, byte register2, Formate formate = Formate.None) : base(register1, register2, 4, (int) formate) { }
    }



    /// <summary>
    /// ADD i j
    /// Rj += Ri
    /// </summary>
    public class AddCommand : BinaryCommand
    {
        public AddCommand(byte register1, byte register2, Formate formate = Formate.None) : base(register1, register2, 5, (int)formate) { }
    }

    /// <summary>
    /// SUB i j
    /// Rj -= Ri
    /// </summary>
    public class SubCommand : BinaryCommand
    {
        public SubCommand(byte register1, byte register2, Formate formate = Formate.None) : base(register1, register2, 6, (int)formate) { }
    }


    /// <summary>
    /// ASR i j
    /// Rj >>= Ri
    /// </summary>
    public class ArithmeticRightShiftCommand : BinaryCommand
    {
        public ArithmeticRightShiftCommand(byte register1, byte register2, Formate formate = Formate.None) : base(register1, register2, 7, (int)formate) { }
    }

    /// <summary>
    /// ASL i j
    ///  Ri leftleft= Rj
    /// </summary>
    public class ArithmeticLeftShiftCommand : BinaryCommand
    {
        public ArithmeticLeftShiftCommand(byte register1, byte register2, Formate formate = Formate.None) : base(register1, register2, 8, (int)formate) { }
    }
    
    
    /// <summary>
    /// OR i j
    ///  Ri |= Rj
    /// </summary>
    public class LogicOrCommand : BinaryCommand
    {
        public LogicOrCommand(byte register1, byte register2, Formate formate = Formate.None) : base(register1, register2, 9, (int)formate) { }
    }


    /// <summary>
    /// AND i j
    ///  Ri &= Rj
    /// </summary>
    public class LogicAndCommand : BinaryCommand
    {
        public LogicAndCommand(byte register1, byte register2, Formate formate = Formate.None) : base(register1, register2, 10, (int)formate) { }
    }

    /// <summary>
    /// XOR i j
    ///  Ri ^= Rj
    /// </summary>
    public class LogicXorCommand : BinaryCommand
    {
        public LogicXorCommand(byte register1, byte register2, Formate formate = Formate.None) : base(register1, register2, 11, (int)formate) { }
    }

    /// <summary>
    /// LSL i j
    ///  Ri left= Rj
    /// </summary>
    public class LogicLeftShiftCommand : BinaryCommand
    {
        public LogicLeftShiftCommand(byte register1, byte register2, Formate formate = Formate.None) : base(register1, register2, 12, (int)formate) { }
    }

    /// <summary>
    /// LSR i j
    ///  Ri >= Rj
    /// </summary>
    public class LogicRightShiftCommand : BinaryCommand
    {
        public LogicRightShiftCommand(byte register1, byte register2, Formate formate = Formate.None) : base(register1, register2, 13, (int)formate) { }
    }

    /// <summary>
    /// CND i j
    ///  Ri ?= Rj
    /// </summary>
    public class ArithmeticCompareCommand : BinaryCommand
    {
        public ArithmeticCompareCommand(byte register1, byte register2, Formate formate = Formate.None) : base(register1, register2, 14, (int)formate) { }
    }


    /// <summary>
    /// CBR i j
    /// if Ri goto Rj
    /// </summary>
    public class GotoCommand : BinaryCommand
    {
        public GotoCommand(byte register1, byte register2) : base(register1, register2, 15, 3) { }
    }



}
