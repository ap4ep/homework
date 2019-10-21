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
            Logger consoleFileLog = new Logger(new ConsoleLog(new FileLog()));
            Logger evenDayConsoleFileLog = new Logger(new EvenDayLog(new ConsoleLog(new FileLog())));

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
        private ILogger _log;

        public Logger(ILogger log)
        {
            _log = log;
        }

        public void ShowLog(string message)
        {
            _log.WriteLogMessage(message);
        }
    }

    class ConsoleLog : ILogger
    {
        private ILogger _logger;

        public ConsoleLog(ILogger logger = null)
        {
            _logger = logger;
        }

        public void WriteLogMessage(string message)
        {
            if (_logger != null)
                _logger.WriteLogMessage(message);

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
        private ILogger _logger;

        public EvenDayLog(ILogger logger)
        {
            _logger = logger;
        }

        public void WriteLogMessage(string message)
        {
            if (DateTime.Now.Day % 2 == 0)
            {
                _logger.WriteLogMessage(message);
            }
        }
    }

    interface ILogger
    {
        void WriteLogMessage(string message);
    }
}
