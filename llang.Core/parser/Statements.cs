using System;
using System.Collections.Generic;
namespace llang.Core.parser
{

    public class Statements
    {
        private List<Statement> statemets;

        public int Length
        {
            get
            {
                return this.statemets.Count;
            }
        }

        public Statements()
        {
            this.statemets = new List<Statement>();
        }

        public void AddStatement(Statement st)
        {
            this.statemets.Add(st);
        }

        public void AddStatement(Statements st)
        {
            this.statemets.AddRange(st.statemets);
        }

        public Statement GetStatement(int index)
        {
            if (index < this.statemets.Count)
                return this.statemets[index];
            else
                return null;
        }

        public void Print()
        {
            for (int i = 0; i < this.statemets.Count; i++)
            {
                Console.Write("Statement " + i + ": ");
                Console.WriteLine(this.statemets[i].ToString());

            }
        }
    }

}