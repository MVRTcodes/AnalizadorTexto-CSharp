using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjecteM06UF1
{
    class Program
    {
        
        static void Main(string[] args)
        {

            string YourDir;

            Escritorio(out YourDir);

            AbsTool(ref YourDir);

            Analizadortext newone = new Analizadortext(YourDir);
            newone.Iniciar();

        }
        public static void Escritorio(out string YourDir)
        {
            YourDir = "C:\\Users\\" + Environment.UserName + "\\Desktop\\";
        }

        public static void AbsTool(ref string YourDir)
        {
            YourDir += "AbstractTool\\";
        }
    }
}
