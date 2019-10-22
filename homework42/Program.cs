using System;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace _42
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "D:";
            string filter = "config.json";
            bool exit = false;
            Configuration configuration = new Configuration();
            ConfigurationReader configurationReader = new ConfigurationReader((path + "\\" + filter));
            Work work = new Work();
            FileWatcher fileWatcher = new FileWatcher(path, filter, configuration, work, configurationReader);

            configurationReader.Deserializer();

            Console.WriteLine("Показать конфигурацию S, выйти Q");
            while (!exit)
            {
                switch (Console.ReadLine())
                {
                    case "s":
                        work.ShowConfig();
                        break;
                    case "q":
                        exit = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    [DataContract]
    public class Configuration
    {
        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                ConfigutaionLoaded?.Invoke();
            }
        }
        [DataMember] public int Age { get => _age; set => _age = value; }
        [DataMember] public int Money { get => _money; set => _money = value; }

        public event Action ConfigutaionLoaded;

        private string _name;
        private int _age;
        private int _money;
    }

    class Work
    {
        public Configuration Configuration { set => _configuration = value; }

        private Configuration _configuration;

        public void ShowConfig()
        {
            Console.WriteLine(_configuration.Name + " " + _configuration.Money + " " + _configuration.Age);
        }
    }

    class ConfigurationReader
    {
        public Configuration Configuration { get => _configuration; }

        public event Action ConfigurationReaded;

        private DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Configuration));
        private Configuration _configuration;
        private string _path;

        public ConfigurationReader(string path)
        {
            _path = path;
        }

        public void Deserializer()
        {
            using (FileStream file = new FileStream(_path, FileMode.Open))
            {
                var json = JsonReaderWriterFactory.CreateJsonReader(file, Encoding.GetEncoding("utf-8"), XmlDictionaryReaderQuotas.Max, null);
                _configuration = (Configuration)jsonFormatter.ReadObject(json);
                file.Close();
                ConfigurationReaded?.Invoke();
            }
        }

        public void Serializer()
        {
            using (FileStream file = File.Open(_path, FileMode.Create))
            {
                var json = JsonReaderWriterFactory.CreateJsonWriter(file);
                jsonFormatter.WriteObject(json, Configuration);
                json.Flush();
                file.Close();
            }
        }
    }

    class FileWatcher
    {
        FileSystemWatcher _fileSystemWatcher;
        Configuration _configuration;
        Work _work;
        ConfigurationReader _configurationReader;

        public FileWatcher(string path, string filter, Configuration configuration, Work work, ConfigurationReader configurationReader)
        {
            _fileSystemWatcher = new FileSystemWatcher(path, filter);
            _fileSystemWatcher.Changed += new FileSystemEventHandler(_fileSystemWatcherChanged);
            _fileSystemWatcher.EnableRaisingEvents = true;

            _configuration = configuration;
            _configuration.ConfigutaionLoaded += ApplyConfiguration;

            _work = work;

            _configurationReader = configurationReader;
            _configurationReader.ConfigurationReaded += LoadConfiguration;
        }

        private void ApplyConfiguration()
        {
            _work.Configuration = _configuration;
        }

        private void LoadConfiguration()
        {
            _configuration.Name = _configurationReader.Configuration.Name;
            _configuration.Money = _configurationReader.Configuration.Money;
            _configuration.Age = _configurationReader.Configuration.Age;
        }

        private void _fileSystemWatcherChanged(object sender, FileSystemEventArgs e)
        {
            _configurationReader.Deserializer();
            Console.WriteLine("File changed!");
        }
    }
}
