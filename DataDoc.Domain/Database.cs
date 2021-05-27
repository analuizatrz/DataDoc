using System.Collections.Generic;

namespace DataDoc.Domain
{
	public class Database : Entity
	{
		IEnumerable<Table> Tables { get; set; }
	}
}
