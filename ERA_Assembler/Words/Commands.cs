﻿using System;

namespace ERA_Assembler.Words
{
 
    /// <summary>
    /// Any meaningful command
    /// </summary>
    public abstract class Command : Word
    {
        protected readonly int CmdNum;

        protected readonly int Format;

        protected Command(int cmdNum, int format)
        {    
            CmdNum = cmdNum;
            Format = format;
        }
    }


    #region single commands

    /// <summary>
    /// Command of type like
    /// 00 0000 0000000000 0..0
    /// f  cmd    const     16
    /// </summary>
    public abstract class SingleCommand : Command
    {
        protected int Value;

        protected SingleCommand(int cmd, int format, int value = 0) : base(cmd, format)
        {
            if (Value >= 1024 || Value < 0) throw new Exception("Single constant overflow: " + value);
            Value = value;
        }

        public override byte[] GetBytes()
        {
            int a = (Format << 14) + (CmdNum << 10) + Value;
            return BitConverter.GetBytes(a);
        }
    }


    /// <summary>
    /// SKIP
    /// Nop;
    /// NOP 0..1023;
    /// </summary>
    public class SkipCommand : SingleCommand
    {
        public SkipCommand(int value = 0) : base(0, 1, value){}
    }


    /// <summary>
    /// STOP
    /// Stop;
    /// STOP 0..1023;
    /// </summary>
    public class StopCommand : SingleCommand
    {
        public StopCommand(int value = 0) : base(0, 0, value) { }
    }

    #endregion


    #region Binary commands

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
            int a = (Format << 14) + (CmdNum << 10) + (Register1 << 5) + Register2;
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
        public AddNextConstCommand(byte register1, byte register2) : base(register1, register2, 2, 0) { }
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
        public CopyRegisterToRegisterCommand(byte register1, byte register2, Format format = Words.Format.Int32) : base(register1, register2, 4, (int)format) { }
    }


    /// <summary>
    /// ADD i j
    /// Rj += Ri
    /// </summary>
    public class AddCommand : BinaryCommand
    {
        public AddCommand(byte register1, byte register2, Format format = Words.Format.Int32) : base(register1, register2, 5, (int)format) { }
    }


    /// <summary>
    /// SUB i j
    /// Rj -= Ri
    /// </summary>
    public class SubCommand : BinaryCommand
    {
        public SubCommand(byte register1, byte register2, Format format = Words.Format.Int32) : base(register1, register2, 6, (int)format) { }
    }


    /// <summary>
    /// ASR i j
    /// Rj >>= Ri
    /// </summary>
    public class ArithmeticRightShiftCommand : BinaryCommand
    {
        public ArithmeticRightShiftCommand(byte register1, byte register2, Format format = Words.Format.None) : base(register1, register2, 7, (int)format) { }
    }


    /// <summary>
    /// ASL i j
    ///  Ri leftleft= Rj
    /// </summary>
    public class ArithmeticLeftShiftCommand : BinaryCommand
    {
        public ArithmeticLeftShiftCommand(byte register1, byte register2, Format format = Words.Format.Int32) : base(register1, register2, 8, (int)format) { }
    }
    
    
    /// <summary>
    /// OR i j
    ///  Ri |= Rj
    /// </summary>
    public class LogicOrCommand : BinaryCommand
    {
        public LogicOrCommand(byte register1, byte register2, Format format = Words.Format.Int32) : base(register1, register2, 9, (int)format) { }
    }


    /// <summary>
    /// AND i j
    ///  Ri &= Rj
    /// </summary>
    public class LogicAndCommand : BinaryCommand
    {
        public LogicAndCommand(byte register1, byte register2, Format format = Words.Format.Int32) : base(register1, register2, 10, (int)format) { }
    }


    /// <summary>
    /// XOR i j
    ///  Ri ^= Rj
    /// </summary>
    public class LogicXorCommand : BinaryCommand
    {
        public LogicXorCommand(byte register1, byte register2, Format format = Words.Format.Int32) : base(register1, register2, 11, (int)format) { }
    }


    /// <summary>
    /// LSL i j
    ///  Ri left= Rj
    /// </summary>
    public class LogicLeftShiftCommand : BinaryCommand
    {
        public LogicLeftShiftCommand(byte register1, byte register2, Format format = Words.Format.Int32) : base(register1, register2, 12, (int)format) { }
    }


    /// <summary>
    /// LSR i j
    ///  Ri >= Rj
    /// </summary>
    public class LogicRightShiftCommand : BinaryCommand
    {
        public LogicRightShiftCommand(byte register1, byte register2, Format format = Words.Format.Int32) : base(register1, register2, 13, (int)format) { }
    }


    /// <summary>
    /// CND i j
    ///  Ri ?= Rj
    /// </summary>
    public class ArithmeticCompareCommand : BinaryCommand
    {
        public ArithmeticCompareCommand(byte register1, byte register2, Format format = Words.Format.Int32) : base(register1, register2, 14, (int)format) { }
    }


    /// <summary>
    /// CBR i j
    /// if Ri goto Rj
    /// </summary>
    public class GotoCommand : BinaryCommand
    {
        public GotoCommand(byte register1, byte register2) : base(register1, register2, 15, 3) { }
    }

#endregion

}
