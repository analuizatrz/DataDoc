using System.Collections.Generic;

namespace DataDoc.Domain
{
	public class DatabaseChanges
	{
		public IEnumerable<Table> NewTables { get; set; }
		public IEnumerable<Table> DeletedTables { get; set; }
		public IDictionary<string, IEnumerable<Column>> NewColumns { get; set; }
		public IDictionary<string, IEnumerable<Column>> DeletedColumns { get; set; }
	}
}
