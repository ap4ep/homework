using System;

namespace homework43
{
    class Program
    {
        static void Main(string[] args)
        {
            string row = "Lo#rem i#ps#um# dolo#r si#t am#et, conse#ctet#uer# adipi#scing e#lit.";
            string row2 = "asdasdasdas*123asdasdas6;";
            Parser sharpParser = new Parser(new SharpParser(), row);
            Parser asteriksParser = new Parser(new AsteriskParser(), row2);

            sharpParser.ShowParsedRow();
            asteriksParser.ShowParsedRow();

            Console.ReadKey();
        }
    }

    class Parser
    {
        private ISymbolParser _symbolPaser;
        private string _row;

        public Parser(ISymbolParser symbolParser, string row)
        {
            _symbolPaser = symbolParser;
            _row = row;
        }

        public void ShowParsedRow()
        {
            Console.WriteLine(_symbolPaser.SymbolParser(_row));
        }
    }

    class SharpParser : ISymbolParser
    {
        public string SymbolParser(string row)
        {
            string clearRow = "";
            char[] array = row.ToCharArray();
            foreach (var symbol in array)
            {
                if (symbol != '#')
                    clearRow += symbol;
            }
            return clearRow;
        }
    }

    class AsteriskParser : ISymbolParser
    {
        public string SymbolParser(string row)
        {
            int startPoint = 0;
            int endPoint = 0;
            int result = 0;
            char[] array = row.ToCharArray();

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == '*') 
                    startPoint = i;
                if (array[i] == ';')
                { 
                    endPoint = i;
                    for (int j = startPoint; j <= endPoint; j++)
                    { 
                        if (Int32.TryParse(array[j].ToString(), out _))
                        {  
                            result += Convert.ToInt32(array[j].ToString());
                        }
                    }
                    break;
                }
            }

            if (result % 2 == 0)
                return "Сумма найденых обьектов " + result;
            else
                return "Сумма не четная" + result;
        }
    }

    internal interface ISymbolParser
    {
        public string SymbolParser(string row);
    }
}
