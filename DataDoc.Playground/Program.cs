using DataDoc.Infra;

namespace DataDoc.Playground
{
	class Program
	{
		static void Main(string[] args)
		{
			var connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=Employee;Integrated Security=true;";
			var database = new SqlServerDatabases().Read(connectionString);
			//new FileDatabases(@"C:\Users\42Codelab\Desktop\").Save(database);
			//var database = new FileDatabases(@"C:\").Read("Employee");
		}	
	}
}
