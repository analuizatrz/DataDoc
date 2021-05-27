using DataDoc.Domain;
using Newtonsoft.Json;
using System.IO;

namespace DataDoc.Infra
{
	public class FileDatabases
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
	}
}
