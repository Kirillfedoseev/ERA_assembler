using ERA_Assembler.Tokens;

namespace ERA_Assembler.Commands
{
    /// <summary>
    /// Идея класса в том, чтобы сделать таких наследников от него, которые будут репрезентацией команд ERA
    /// И мы смогли бы массив таких команд с лекостью переделывать в машинный код, достаточно вызвать метод GetML()
    /// </summary>
    class Command
    {


       
        //todo Implement structure
        private Token command;


        public byte[] GetML()
        {

            return new byte[4];
            return new byte[8];
        }

    }
}
