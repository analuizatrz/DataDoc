using DataDoc.Domain;
using Newtonsoft.Json;
using System.IO;

namespace DataDoc.Infra
{
	public class FileDatabases : IRepository<Database>
	{
		public string FolderPath { get; }
		public FileDatabases(string folderPath)
		{
			FolderPath = folderPath;
		}
		public void Save(Database database)
		{
			var jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
			var json = JsonConvert.SerializeObject(database, Formatting.Indented, jsonSettings);
			File.WriteAllText(Path.Combine(FolderPath, $"{database.Name}.json"), json);
		}
		public Database Read(string databaseName)
		{
			var json = File.ReadAllText(Path.Combine(FolderPath, $"{databaseName}.json"));
			return JsonConvert.DeserializeObject<Database>(json);
		}
	}
}
