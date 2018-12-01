using System;
using System.Collections;
using System.Collections.Generic;
using ERA_Assembler.Commands;

namespace ERA_Assembler
{            
    //todo mapped commands to memory
    public class Mapper
    {

        public const int MemoryOffset = 0x001;
        public const int CodeOffset = 0x00ff;

        public Hashtable Labels;

        public List<KeyValuePair<string, LabelAddress>> Unreferenced;

        public Mapper()
        {
            Labels = new Hashtable();
            Unreferenced = new List<KeyValuePair<string, LabelAddress>>();
        }


        public void Map()
        {
            ResolveUnreferenced();
            ResolveLabelsAddresses();
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
