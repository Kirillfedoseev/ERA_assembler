using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ERA_Assembler.Commands;

namespace ERA_Assembler
{            
    //todo mapped commands to memory
    public class Mapper
    {

        private int MemoryLenght;

        private int CodeOffset;

        public Hashtable Labels;

        public List<KeyValuePair<string, LabelAddress>> Unreferenced;

        public Mapper()
        {
            Labels = new Hashtable();
            Unreferenced = new List<KeyValuePair<string, LabelAddress>>();
        }


        public List<byte[]> Map(ref List<Word> program, ref List<Word> data)
        {
            MemoryLenght = data.Count * 2;
            CodeOffset = MemoryLenght + 4;

            ResolveUnreferenced();
            ResolveLabelsAddresses();

            List<byte[]> bytesList = new List<byte[]>(1 + program.Count + data.Count);


            List<byte> header = new List<byte>();
            header.Add(0);//bersion
            header.Add(0);//padding
            header.AddRange(BitConverter.GetBytes(MemoryLenght).Reverse()); //data length

            bytesList.Add(header.ToArray());

            foreach (Word word in data)
                bytesList.Add(word.GetBytes());

            foreach (Word word in program)
                bytesList.Add(word.GetBytes());

            return bytesList;

        }

        private void ResolveLabelsAddresses()
        {
            foreach (Label label in Labels.Values)           
                label.MapOnMemory(CodeOffset);
            
        }

        private void ResolveUnreferenced()
        {
            foreach (KeyValuePair<string, LabelAddress> pair in Unreferenced)
            {
                if (!Labels.Contains(pair.Key)) throw new Exception("No such label: " + pair.Key);
                {
                    pair.Value.SetLabel((Label)Labels[pair.Key]);
                }
            }
        }

    }

     
}
