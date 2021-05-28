using System.Collections.Generic;

namespace DataDoc.Domain
{
	public class Table : Entity
	{
		public string VersionCreated { get; set; }
		public string LastVersionModified { get; set; }
		public IEnumerable<Column> Columns { get; set; }
	}
}
