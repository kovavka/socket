using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Domain;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Infrastructure
{
    public class DataBaseHelper
    {
        private static ISessionFactory sessionFactory;

        private static readonly string server = @"Server= .; ";

        private static FluentConfiguration defaultConfiguration = Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2012.ConnectionString(
                    server + @"Initial Catalog=EventsDb; Integrated Security=SSPI;").ShowSql()
            )
            .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true));

        private static FluentConfiguration reBiuldConfiguration = Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2012.ConnectionString(
                    server + @"Integrated Security=SSPI;").ShowSql()
            )
            .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true));


        public static ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                    sessionFactory = defaultConfiguration.BuildSessionFactory();

                return sessionFactory;
            }
        }

        public static void CreateDB()
        {
            var factory = reBiuldConfiguration.BuildSessionFactory();

            Run("Create database EventsDb;", factory);
            factory.Close();

            UpdateDB(SessionFactory);
        }

        private static void Run(string queryString, ISessionFactory sessionFactory)
        {
            using (ISession session = sessionFactory.OpenSession())
            {
                ISQLQuery query = session.CreateSQLQuery(queryString);
                query.SetTimeout(600);
                query.UniqueResult();
            }
        }

        private static void UpdateDB(ISessionFactory sessionFactory)
        {
            var scripts = GetScripts();
            foreach (var script in scripts)
            {
                script.Value.RunScript(sessionFactory);
            }
        }

        private static SortedDictionary<int, ScriptItem> GetScripts()
        {
            var dir = @".\Sql";
            var result = new SortedDictionary<int, ScriptItem>();


            var files = Directory.GetFiles(dir, "*.sql");

            foreach (var name in files)
            {
                int number;
                int.TryParse(Path.GetFileNameWithoutExtension(name), out number);

                //if (result.ContainsKey(number))
                //    Log.Error(new Exception(string.Format("скрипт с номером {0} уже существует", number)));

                var script = File.ReadAllText(name, Encoding.UTF8);

                result.Add(number, new ScriptItem(script));
            }

            return result;
        }

        public static void ClearDB()
        {
            var factory = defaultConfiguration.BuildSessionFactory();

            Run(@"delete 
FROM [EventsDb].[dbo].[Event]; 

delete 
FROM [EventsDb].[dbo].[Address]; 

delete 
FROM [EventsDb].[dbo].Street; 

delete 
FROM [EventsDb].[dbo].City; 

delete 
FROM [EventsDb].[dbo].CityType; 

delete 
FROM [EventsDb].[dbo].Region; 

delete 
FROM [EventsDb].[dbo].Country;", factory);
            factory.Close();
        }

        public static IEnumerable<EventDto> GetDtos()
        {
            var connection =
                new System.Data.OleDb.OleDbConnection(
                    @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\gikap\Documents\events.accdb");
            var command = connection.CreateCommand();
            command.CommandText = "Select Execution,EventName,EventComment,EventInfo,Country,Region,CityType,City,Street,House From Events";
            connection.Open();
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var dto = new EventDto()
                {
                    Execution = Convert.ToDateTime(reader.GetValue(0)),
                    EventName = reader.GetString(1),
                    EventComment = reader.GetString(2),
                    EventInfo = reader.GetString(3),
                    Country = reader.GetString(4),
                    Region = reader.GetString(5),
                    CityType = reader.GetString(6),
                    City = reader.GetString(7),
                    Street = reader.GetString(8),
                    House = reader.GetString(9),
                };
                yield return dto;
            }

            reader.Close();
        }
    }
    
    public class ScriptItem
    {
        public readonly string Script;

        public ScriptItem(string script)
        {
            Script = script;
        }
        
        public void RunScript(ISessionFactory sessionFactory)
        {
            string[] parts = Script.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            using (ISession session = sessionFactory.OpenSession())
            {
                foreach(var queryString in parts)
                {
                    if (string.IsNullOrEmpty(queryString.Trim()))
                        continue;
                    
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        ISQLQuery query = session.CreateSQLQuery(queryString);
                        query.SetTimeout(600);
                        query.UniqueResult();

                        tx.Commit();
                    }
                }
            }
        }
        
    }
}
