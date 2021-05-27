using DataDoc.Infra;
using Newtonsoft.Json;
using System.IO;

namespace DataDoc.Playground
{
	class Program
	{
		static void Main(string[] args)
		{
			var connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=Employee;Integrated Security=true;";
			var database = new SqlServerDatabases().Get(connectionString);
			new FileDatabases(@"C:\Users\42Codelab\Desktop\").Save(database);
		}
	}
}
