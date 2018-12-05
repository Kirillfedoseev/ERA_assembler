using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ERA_Assembler.Words;

namespace ERA_Assembler
{            
    //todo mapped commands to memory
    public class Mapper
    {

        private int _memoryLength;

        private int _codeOffset;

        public Hashtable Labels;

        public List<KeyValuePair<string, LabelAddress>> Unreferenced;

        public Mapper()
        {
            Labels = new Hashtable();
            Unreferenced = new List<KeyValuePair<string, LabelAddress>>();
        }

        /// <summary>
        /// Map labels with offset on memory
        /// </summary>
        /// <param name="program">words of program</param>
        /// <param name="data">words of data</param>
        /// <returns></returns>
        public List<byte[]> Map(ref List<Word> program, ref List<Word> data)
        {
            _memoryLength = data.Count * 2;
            _codeOffset = _memoryLength + 4;

            ResolveUnreferenced();
            ResolveLabelsAddresses();

            List<byte[]> bytesList = new List<byte[]>(1 + program.Count + data.Count);


            List<byte> header = new List<byte>();
            header.Add(0);//version
            header.Add(0);//padding
            header.AddRange(BitConverter.GetBytes(_memoryLength).Reverse()); //data length

            bytesList.Add(header.ToArray());

            foreach (Word word in data)
                bytesList.Add(word.GetBytes().Reverse().ToArray());

            foreach (Word word in program)
                bytesList.Add(word.GetBytes().Reverse().ToArray());

            return bytesList;

        }

        private void ResolveLabelsAddresses()
        {
            foreach (Label label in Labels.Values)           
                label.MapOnMemory(_codeOffset);
            
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
