using System;
using System.Collections.Generic;
using System.Linq;

namespace homework44
{
    class Program
    {
        static void Main(string[] args)
        {
            User i = new User(6, "__XxX822Нагибатор228XxX__", 30, true);

            User petya = new User(1, "Петя", 20, true);
            User masha = new User(2, "Маша", 30, false);
            User pasha = new User(3, "Паша", 23, true);
            User sveta = new User(4, "Света", 35, false);
            User senya = new User(5, "Сеня", 33, true);

            Statistics genderPreference = new Statistics(new GenderPreferences(), i);
            Statistics averageAge = new Statistics(new AverageAge(), i);
            Statistics topVisited = new Statistics(new TopVisitedUser(), i);

            i.VisitHistory.AddUserToHistory(masha);
            i.VisitHistory.AddUserToHistory(sveta);
            i.VisitHistory.AddUserToHistory(pasha);
            i.VisitHistory.AddUserToHistory(sveta);

            Console.WriteLine(genderPreference.Stat.GetStatistic());
            Console.WriteLine(averageAge.Stat.GetStatistic());
            Console.WriteLine(topVisited.Stat.GetStatistic());

            Console.ReadKey();
        }
    }

    class User
    {
        public int UserId { get => _userId;}
        public string Name { get => _name;}
        public int Age { get => _age;}
        public bool Gender { get => _gender;}
        internal VisitHistory VisitHistory { get => _visitHistory; set => _visitHistory = value; }

        private int _userId;
        private string _name;
        private int _age;
        private bool _gender;
        private VisitHistory _visitHistory;

        public User(int userId, string name, int age, bool gender)
        {
            _userId = userId;
            _name = name;
            _age = age;
            _gender = gender;

            _visitHistory = new VisitHistory();
            
        }
    }

    class VisitHistory
    {
        internal List<User> VisitedUsers { get => _visitedUsers; set => _visitedUsers = value; }

        private List<User> _visitedUsers = new List<User>();

        public void AddUserToHistory(User user)
        {
            _visitedUsers.Add(user);
        }
    }

    class Statistics
    {
        internal IStatistics Stat { get => _stat; }

        private User _user;
        private IStatistics _stat;

        public Statistics(IStatistics statistics, User user)
        {
            _user = user;
            _stat = statistics;
            _stat.History = user.VisitHistory.VisitedUsers;
        }

        
    }

    class TopVisitedUser : IStatistics
    {
        public List<User> History { set => _history = value; }

        private List<User> _history;

        private int[] _knowId = {0};

        public string GetStatistic()
        {
            List<User> topVisited = new List<User>();
            
            foreach(var i in _history)
            {
                if (!SearchInArray(i.UserId))
                {
                    AddToArray(i.UserId);
                    List<User> tmp = _history.FindAll(item => item.UserId == i.UserId);
                    if(tmp.Count > topVisited.Count)
                    {
                        topVisited = tmp;
                    }
                }
            }
            return "Самый посещаемый вами пользователь " + topVisited.First().Name;
        }

        private void AddToArray(int id)
        {
            int[] tmpArray = new int[_knowId.Length + 1];
            for (int i = 0; i < _knowId.Length; i++)
            {
                tmpArray[i] = _knowId[i];
            }
            tmpArray[tmpArray.Length - 1] = id;
            _knowId = tmpArray;
        }

        private bool SearchInArray(int id)
        {
            foreach(var i in _knowId)
            {
                if(i == id)
                {
                    return true;
                }
            }

            return false;
        }
    }

    class AverageAge : IStatistics
    {
        public List<User> History { set => _history = value; }

        private List<User> _history;

        public string GetStatistic()
        {
            int ages = 0;

            foreach(var i in _history)
            {
                ages += i.Age;
            }

            return (ages / _history.Count).ToString();
        }
    }

    class GenderPreferences : IStatistics
    {
        public List<User> History { set => _history = value; }

        private List<User> _history;

        public string GetStatistic()
        {
            int male = 0;
            int female = 0;

            foreach(var i in _history)
            {
                if (i.Gender == true)
                    male++;
                else
                    female++;
            }
            if (male > female)
                return "Предпочитает мужчин";
            else if (male < female)
                return "Предпочитает женщин";
            else
                return "Налопомпам";
        }
    }

    interface IStatistics
    {
        public List<User> History { set; }

        public string GetStatistic();
    }
}
