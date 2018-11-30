using System;
using ERA_Assembler.Tokens;

namespace ERA_Assembler.Commands
{
    /// <summary>
    /// Идея класса в том, чтобы сделать таких наследников от него, которые будут репрезентацией команд ERA
    /// И мы смогли бы массив таких команд с лекостью переделывать в машинный код, достаточно вызвать метод GetML()
    /// </summary>
    public abstract class Command
    {

        protected int _register1;
        protected int _register2;

        protected readonly int CmdNum;
        protected readonly int Format;


        protected Command(byte register1, byte register2,int cmdNum, int format)
        {
            if (register1 >= 32) throw new Exception("Register 1 out of bound: " + register1);
            if (register2 >= 32) throw new Exception("Register 2 out of bound: " + register2);

            _register1 = register1;
            _register2 = register2;
            CmdNum = cmdNum;
            Format = format;
        }

        public abstract byte[] GetBytes();



    }


    /// <summary>
    /// Command of type like
    /// 00 0000 00000 00000 0..0
    /// f  cmd   r1    r2    16
    /// </summary>
    public abstract class BinaryCommand : Command
    {
            
        protected BinaryCommand(byte register1, byte register2, int cmdNum, int format) : base(register1, register2, cmdNum, format) {}

        public override byte[] GetBytes()
        {
            int a = (Format << 29) + (CmdNum << 25) + (_register1 << 21) + (_register2 << 16);
            return BitConverter.GetBytes(a);
        }

    }

    public class AddCommand : BinaryCommand
    {
        public AddCommand(byte register1, byte register2) : base(register1, register2, 10, 0) {}
        
    }


    public class CopyCommand : BinaryCommand
    {
        public CopyCommand(byte register1, byte register2) : base(register1, register2, 1, 3) { }
    }

}
