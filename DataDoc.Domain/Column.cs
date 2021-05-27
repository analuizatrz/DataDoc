namespace DataDoc.Domain
{
	public class Column : Entity
	{
		public string VersionCreated { get; set; }
		public string LastVersionModified { get; set; }
		public bool IsPrimaryKey { get; set; }
		public bool IsForeignKey { get; set; }
		public int OrdinalPosition { get; set; }
		public string ColumnDefault { get; set; }
		public string DataType { get; set; }
		public int? CharacterMaximumLength { get; set; }
		public int? NumericPrecision { get; set; }
		public int? NumericPrecisionRadix { get; set; }
		public int? NumericScale { get; set; }
		public int? DatetimePrecision { get; set; }
		public string ConstraintName { get; set; }
		public string ConstrainType { get; set; }
		public string PrimaryKeyReferenced { get; set; }
	}
}
