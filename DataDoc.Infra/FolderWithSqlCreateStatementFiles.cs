using DataDoc.Domain;
using DataDoc.Domain.Services.CreateStatementParser;
using System.IO;
using System.Linq;

namespace DataDoc.Infra
{
	public class FolderWithSqlCreateStatementFiles
	{
		DatabaseCreateStatementParser CreateStatementParser { get; }

		public FolderWithSqlCreateStatementFiles(DatabaseCreateStatementParser createStatementParser)
		{
			CreateStatementParser = createStatementParser;
		}
		public Database Read(string folderPath)
		{
			var fileEntries = Directory.GetFiles(folderPath);

			var tables = fileEntries.Select(filename => ParseTable(filename));

			return new Database { Tables = tables };
		}
		Table ParseTable(string filePath)
		{
			string text = File.ReadAllText(filePath);
			return CreateStatementParser.ParseTableContent(text);
		}
	}
}
