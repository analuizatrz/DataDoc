using System.Collections.Generic;

namespace DataDoc.Domain
{
	public class Database : Entity
	{
		public IEnumerable<Table> Tables { get; set; }
	}
}
