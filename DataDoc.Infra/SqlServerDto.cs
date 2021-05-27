namespace DataDoc.Infra
{
	public class SqlServerDto
	{
		public string TableCatalog { get; set; }
		public string TableName { get; set; }
		public string ColumnName { get; set; }
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
