using System.Collections.Generic;

namespace llang.Core.virtualmachine
{
    public class OutputFormatter
    {
        List<StyleValue> Values;
        List<string> Parameters;
        string MainText;
        public OutputFormatter(string[] values, string mainText)
        {
            MainText = mainText;
            Parameters = new List<string>(values);
            Values = new List<StyleValue>();
            Generate();
        }

        public static string GetOnlyTag(string txt) => "<p>" + txt + "</p>";

        public override string ToString()
        {

            string returnValue = "<p" + " style = \" ";
            foreach (var item in Values)
            {
                returnValue += item.ToString();
            }
            returnValue += ( "\" >" + MainText + "</p> ");

            return returnValue;
        }

        public void Generate()
        {
            for (int i = 0; i < Parameters.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        Values.Add(new StyleValue(Parameters[i], "font-size"));
                        break;
                    case 1:
                        Values.Add(new StyleValue(Parameters[i], "color"));
                        break;
                    case 2:
                        Values.Add(new StyleValue(Parameters[i], "top"));
                        break;
                    case 3:
                        Values.Add(new StyleValue(Parameters[i], "left"));
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public class StyleValue
    {
        public string Value;
        public string Type;

        public override string ToString() => Type + ": " + Value + ";";

        public StyleValue(string value, string type) { Value = value; Type = type; }
    }
}