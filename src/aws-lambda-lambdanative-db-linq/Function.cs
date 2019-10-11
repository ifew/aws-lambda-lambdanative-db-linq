using System;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using LambdaNative;
using MySql.Data.MySqlClient;
using System.Data;
using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Mapping;
using LinqToDB.Configuration;
using System.Linq;
using LinqToDB.Data;

namespace aws_lambda_lambdanative
{
    public class Handler : IHandler<string, List<Member>>
    {
        public ILambdaSerializer Serializer => new Amazon.Lambda.Serialization.Json.JsonSerializer();

        public List<Member> Handle(string name, ILambdaContext context)
        {
            Console.WriteLine("Log: Start Connection");

            DataConnection.DefaultSettings = new MySettings();

            Console.WriteLine("Log: After Connection");

            using (var db = new lab())
            {
                Console.WriteLine("Log: After new Lab()");

                var query = from m in db.Member
                            orderby m.Id descending
                            select m;

                Console.WriteLine("Log: After Linq Query");

                List<Member> result = query.ToList();

                Console.WriteLine("Log: After query and get list");

                Console.WriteLine("Log: Count: " + result.Count);
                
                return result;
            }
        }
    }

    [Table(Name = "test_member")]
    public class Member
    {
        [PrimaryKey, Identity]
        [Column(Name = "id"), NotNull]
        public int Id { get; set; }

        [Column(Name = "firstname"), NotNull]
        public string Firstname { get; set; }

        [Column(Name = "lastname"), NotNull]
        public string Lastname { get; set; }

    }

    public class ConnectionStringSettings : IConnectionStringSettings
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => false;
    }

    public class MySettings : ILinqToDBSettings
    {
        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

        public string DefaultConfiguration => "MySQL";
        public string DefaultDataProvider => "MySQL";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSettings
                    {
                        Name = "MySQL",
                        ProviderName = "MySQL",
                        ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
                    };
            }
        }
    }

    public class lab : LinqToDB.Data.DataConnection
    {
        public lab() : base("lab") { }

        public ITable<Member> Member => GetTable<Member>();
    }
}
