using System;
using llang.Core.lexer;
using llang.Core.parser;
using llang.Core.virtualmachine;
using System.IO;

namespace llang.Core.interpreter
{
    public class Interpreter
    {
        private Lexer lexer;
        private Parser parser;
        private VirtualMachine vm;
        private Program program;

        public Interpreter()
        {
            this.parser = new Parser();
            this.vm = new VirtualMachine();
        }

        private Interpreter(Lexer l) : this()
        {
            this.lexer = l;
        }

        public Interpreter(string scode) : this()
        {
            this.lexer = new Lexer(scode);
        }

        public void GetNextInput(string scode)
        {
            this.lexer = new Lexer(scode);
        }

        public static Interpreter FromFile(string path)
        {
            int fIdx = path.IndexOf('/'),
                lIdx = path.LastIndexOf('/');

            string dirPath;

            if (fIdx == 0)
            {
                dirPath = path.Substring(0, lIdx);
                path = path.Substring(lIdx + 1);

            }
            else if (lIdx != -1)
            {
                dirPath = Directory.GetCurrentDirectory() + "/" + path.Substring(0, lIdx);
                path = path.Substring(lIdx + 1);

            }
            else
            {
                dirPath = Directory.GetCurrentDirectory();
            }

            Directory.SetCurrentDirectory(dirPath);

            int liofp = path.LastIndexOf('.');
            int len = path.Length;

            return new Interpreter(Lexer.FromFile(path));
        }

        public void Init()
        {
            this.lexer.Tokenize();
            this.program = this.parser.Parse(this.lexer.Tokens);

            if (this.program == null)
                Console.WriteLine("Error : Parsing could not completed successfuly.");
        }

        public void Run()
        {
            this.vm.Execute(this.program);
        }

        public ExpressionValue RunAsModule()
        {
            this.vm.Environment.Modify("exports", new ExpressionValue(ExpressionValueType.OBJECT));
            this.vm.Execute(this.program);
            return this.vm.Environment.Get("exports") as ExpressionValue;
        }
    }
}