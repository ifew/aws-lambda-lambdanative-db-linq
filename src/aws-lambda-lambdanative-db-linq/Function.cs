using System;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Common;
using LinqToDB.Mapping;
using LinqToDB.Configuration;
using System.Linq;
using LambdaNative;

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

            using (var db = new DBConnection())
            {
                Console.WriteLine("Log: After new Lab()");

                var query = from m in db.Member
                            orderby m.Id descending
                            select m;

                Console.WriteLine("Log: After Linq Query");

                List<Member> members = query.ToList();

                Console.WriteLine("Log: After query and get list");

                Console.WriteLine("Log: Count: " + members.Count);
                
                return members;
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

        public string DefaultConfiguration => "Mysql";
        public string DefaultDataProvider => "Mysql";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSettings
                    {
                        Name = "DBConnection",
                        ProviderName = "MySql",
                        ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
                    };
            }
        }
    }

    public partial class DBConnection : LinqToDB.Data.DataConnection
    {
        public DBConnection()
		{
			InitDataContext();
		}

		public DBConnection(string configuration)
			: base(configuration)
		{
			InitDataContext();
		}

		partial void InitDataContext();

        public ITable<Member> Member => GetTable<Member>();
    }
}
