using System;
using llang.Core.interpreter;
using System.Diagnostics;
using System.IO;
using llang.Core.Server;

namespace llang.Core
{
    class MainClass
    {
        public static int Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("llang Compiler , Usage : llang <File> ");
                return 1;
            }

            if (args[0] == "--help")
                Console.WriteLine("llang Compiler 0.1.0 \n" +
                "Programming language for Law/Lawyers , expandable with any library.\n" +
                "Özgür Gezici , 2021 , HTNE !\n");
            else
            {
                string code = args[0];
                int ind = code.LastIndexOf('/');
                string name = code.Substring(ind + 1);

                Console.WriteLine("[OK] Compiling : " + name);

                FileStream ostrm;
                StreamWriter writer;
                TextWriter oldOut = Console.Out;
                try
                {
                    ostrm = new FileStream("./Out.html", FileMode.OpenOrCreate, FileAccess.Write);
                    writer = new StreamWriter(ostrm);
                }
                catch (Exception e)
                {
                    Console.WriteLine("[Error] : Could not created an output file.");
                    Console.WriteLine(e.Message);
                    return 1;
                }
                Console.SetOut(writer);


                Interpreter interp = Interpreter.FromFile(code);
                interp.Init();
                Stopwatch sw = Stopwatch.StartNew();
                interp.Run();
                sw.Stop();

                Console.SetOut(oldOut);
                writer.Close();
                ostrm.Close();

                Console.WriteLine("[OK] Project < " + name + " > , compiled in " + sw.ElapsedMilliseconds + " ms\n");
            }

            ServeHTTP.StartHTTP();

            return 0;
        }
    }
}
