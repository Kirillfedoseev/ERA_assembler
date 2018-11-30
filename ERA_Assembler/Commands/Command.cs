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
       
        //todo Implement structure
        private Token command;


        public abstract byte[] GetBytes();



    }

    public class AddCommand : Command
    {
        private int _register1;
        private int _register2;

        private const int cmd_num = 10;

        public AddCommand(byte register1, byte register2)
        {
            if (register1 >= 32) throw new Exception("Register 1 out of bound: " + register1);
            if (register2 >= 32) throw new Exception("Register 2 out of bound: " + register2);

            _register1 = register1;
            _register2 = register2;
        }

        public override byte[] GetBytes()
        {

            int a = (3 << 29) + (cmd_num << 25) + (_register1 << 21) + (_register2 << 16);
            return BitConverter.GetBytes(a);
        }
    }
}
