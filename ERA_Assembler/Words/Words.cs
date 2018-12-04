using System;

namespace ERA_Assembler.Words
{
    /// <summary>
    /// Abstract class of memory unit
    /// 0..0
    ///  32
    /// </summary>
    public abstract class Word
    {
        public abstract byte[] GetBytes();
    }


    /// <summary>
    /// Constant of type
    /// 0..0
    ///  32
    /// </summary>
    public class Constant : Word
    {
        protected int Value;

        public Constant(int value = 0) => Value = value;

        public override byte[] GetBytes() => BitConverter.GetBytes(Value);
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
}