namespace DataDoc.Domain
{
	public class Column : Entity
	{
		public string VersionCreated { get; set; }
		public string LastVersionModified { get; set; }
		public bool IsPrimaryKey { get; set; }
		public bool IsForeignKey { get; set; }
	}
}
