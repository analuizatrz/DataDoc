using DataDoc.Infra;

namespace DataDoc.Playground
{
	class Program
	{
		static void DacpacFileRead()
		{
			var path = @"C:\data.dacpac";
			var database = new DacFileDatabases(path).Read(path);
		}
		static void SqlServerRead()
		{
			//var database = new FileDatabases(@"C:\Users\42Codelab\Desktop").Read("BeyoungDoc");
			var connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=Employee;Integrated Security=true;";
			var database = new SqlServerDatabases(new Domain.DatabaseModelParser()).Read(connectionString);
			//new FileDatabases(@"C:\").Save(database);
		}
		static void Main(string[] args)
		{
			DacpacFileRead();
		}	
	}
}
