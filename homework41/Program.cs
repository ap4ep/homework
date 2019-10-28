using System;
using System.IO;

namespace homework41
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger evenDayConsoleLog = new Logger(new EvenDayLog(new ConsoleLog()));
            Logger fileLog = new Logger(new FileLog());
            Logger evenDayFileLog = new Logger(new EvenDayLog(new FileLog()));
            Logger consoleFileLog = new Logger(new ConsoleLog(), new FileLog());
            Logger evenDayConsoleFileLog = new Logger(new EvenDayLog(new ConsoleLog(), new FileLog()));
            
            evenDayConsoleFileLog.ShowLog("В файл и в консоль, и в четный день");
            consoleFileLog.ShowLog("В файл и в консоль");
            evenDayFileLog.ShowLog("В файл и в четный день");
            fileLog.ShowLog("В файл");
            evenDayConsoleLog.ShowLog("В консоль и в четный день");
            Console.ReadKey();
        }
    }

    class Logger
    {
        private ILogger _logger1;
        private ILogger _logger2;

        public Logger(ILogger logger1, ILogger logger2 = null)
        {
            _logger1 = logger1;
            _logger2 = logger2;
        }

        public void ShowLog(string message)
        {
            _logger1.WriteLogMessage(message);
            if(_logger2 != null) 
                _logger2.WriteLogMessage(message); 
        }
    }

    class ConsoleLog : ILogger
    {
        public void WriteLogMessage(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] : {message}");
        }
    }

    class FileLog : ILogger
    {
        public void WriteLogMessage(string message)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine("", "Log.txt"), true))
            {
                outputFile.WriteLine($"[{DateTime.Now}] : {message}");
            }
        }
    }

    class EvenDayLog : ILogger
    {
        private ILogger _logger1;
        private ILogger _logger2;

        public EvenDayLog(ILogger logger1, ILogger logger2 = null)
        {
            _logger1 = logger1;
            _logger2 = logger2;
        }

        public void WriteLogMessage(string message)
        {
            if (DateTime.Now.Day % 2 == 0)
            {
                _logger1.WriteLogMessage(message);
                if (_logger2 != null)
                    _logger2.WriteLogMessage(message);
            }
        }
    }

    interface ILogger
    {
        void WriteLogMessage(string message);
    }
}
