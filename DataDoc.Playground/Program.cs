using DataDoc.Infra;
using System.IO;

namespace DataDoc.Playground
{
	class Program
	{
		static void Main(string[] args)
		{
			var connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=Employee;Integrated Security=true;";
			var database = new SqlServerDatabases().Get(connectionString);
		}
	}
}
