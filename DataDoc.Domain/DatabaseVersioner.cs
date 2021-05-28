using System.Collections.Generic;
using System.Linq;

namespace DataDoc.Domain
{
	public class DatabaseVersioner
	{
		public DatabaseChanges Changes(Database currentVersion, Database lastVersion)
		{
			return new DatabaseChanges
			{
				NewTables = Difference(currentVersion.Tables, lastVersion.Tables),
				DeletedTables = Difference(lastVersion.Tables, currentVersion.Tables),
				NewColumns = DifferenceColumns(currentVersion.Tables, lastVersion.Tables),
				DeletedColumns = DifferenceColumns(lastVersion.Tables, currentVersion.Tables),
			};
		}
		public IDictionary<string, IEnumerable<Column>> DifferenceColumns(IEnumerable<Table> source, IEnumerable<Table> target)
		{
			var result = new Dictionary<string, IEnumerable<Column>>();
			foreach (var table in source)
			{
				var matchingTable = target.Where(x => x.Name == table.Name);
				if(matchingTable.Any())
				{
					var columns = Difference(table.Columns, matchingTable.First().Columns);
					if (columns.Any())
						result.Add(table.Name, columns);
				}
				else
				{
					result.Add(table.Name, table.Columns);
				}
			}
			return result;
		}
		public IEnumerable<T> Difference<T>(IEnumerable<T> source, IEnumerable<T> target) where T : Entity
		{
			foreach (var table in source)
			{
				var tableExists = target.Any(x => x.Name == table.Name);
				if (!tableExists)
					yield return table;
			}
		}
	}
}
