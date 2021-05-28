using DataDoc.Domain;
using Microsoft.SqlServer.Dac.Model;
using System.Linq;

namespace DataDoc.Infra
{
	public class DacFileDatabases
	{
		public string FolderPath { get; }
		public DacFileDatabases(string folderPath)
		{
			FolderPath = folderPath;
		}
		public Database Read(string path)
		{
			using (TSqlModel model = new TSqlModel(path))
			{
				var allTables = model
								.GetObjects(DacQueryScopes.All, ModelSchema.Table)
								.Select(x => ReadTables(x));

				return new Database { Tables = allTables.ToList() };
			}
		}
		public Domain.Table ReadTables(TSqlObject table)
		{
			var columns = table
							.GetChildren()
							.Where(child => child.ObjectType.Name == "Column")
							.Select(x => new Domain.Column { Name = x.Name.ToString() });

			return new Domain.Table { Name = table.Name.ToString(), Columns = columns.ToList() };
		}
	}
}


