using System.Collections.Generic;

namespace DataDoc.Domain
{
	public class Table : Entity
	{
		public string VersionCreated { get; set; }
		public string LastVersionModified { get; set; }
		IEnumerable<Column> Columns { get; set; }
	}
}
