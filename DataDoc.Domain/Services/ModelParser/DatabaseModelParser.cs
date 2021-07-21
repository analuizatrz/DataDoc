using System.Collections.Generic;
using System.Linq;

namespace DataDoc.Domain
{
	public class DatabaseModelParser
	{
		public static Database From(IEnumerable<DatabaseModel> dtos)
		{
			var name = dtos.First().TableCatalog;
			var tables = dtos
							.GroupBy(x => x.TableName)
							.Select(table => TableFrom(table));
			return new Database { Name = name, Tables = tables };
		}
		public static Table TableFrom(IEnumerable<DatabaseModel> dtos)
		{
			var name = dtos.First().TableName;
			var columns = dtos.Select(x => ColumnFrom(x));
			return new Table { Name = name, Columns = columns };
		}
		public static Column ColumnFrom(DatabaseModel x)
		{
			return new Column
			{
				Name = x.ColumnName,
				OrdinalPosition = x.OrdinalPosition,
				ColumnDefault = x.ColumnDefault,
				DataType = x.DataType,
				CharacterMaximumLength = x.CharacterMaximumLength,
				NumericPrecision = x.NumericPrecision,
				NumericPrecisionRadix = x.NumericPrecisionRadix,
				NumericScale = x.NumericScale,
				DatetimePrecision = x.DatetimePrecision,
				ConstraintName = x.ConstraintName,
				ConstrainType = x.ConstrainType,
				PrimaryKeyReferenced = x.PrimaryKeyReferenced,
			};
		}
	}
}
