using Dapper;
using DataDoc.Domain;
using System.Data.SqlClient;

namespace DataDoc.Infra
{
	public class SqlServerDatabases
	{
		DatabaseModelParser DatabaseModelParser { get; }
		public SqlServerDatabases(DatabaseModelParser databaseModelParser)
		{
			DatabaseModelParser = databaseModelParser;
		}

		public Database Read(string connectionString)
		{
			var query = @"  
			SELECT 
				   ColumnsInfo.TABLE_CATALOG TableCatalog ,
				   ColumnsInfo.TABLE_NAME TableName,
				   ColumnsInfo.COLUMN_NAME ColumnName,
				   ColumnsInfo.ORDINAL_POSITION OrdinalPosition,
				   ColumnsInfo.COLUMN_DEFAULT ColumnDefault,
				   ColumnsInfo.DATA_TYPE DataType,
				   ColumnsInfo.CHARACTER_MAXIMUM_LENGTH CharacterMaximumLength,
				   ColumnsInfo.NUMERIC_PRECISION NumericPrecision,
				   ColumnsInfo.NUMERIC_PRECISION_RADIX NumericPrecisionRadix,
				   ColumnsInfo.NUMERIC_SCALE NumericScale,
				   ColumnsInfo.DATETIME_PRECISION DatetimePrecision,
				   ConstrainColumn.CONSTRAINT_NAME ConstraintName,
				   ConstrainType.CONSTRAINT_TYPE ConstrainType,
				   ForeignPrimaryKeyMap.UNIQUE_CONSTRAINT_NAME PrimaryKeyReferenced
			FROM INFORMATION_SCHEMA.COLUMNS ColumnsInfo
			LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ConstrainColumn 
				ON ColumnsInfo.TABLE_NAME = ConstrainColumn.TABLE_NAME AND ColumnsInfo.COLUMN_NAME = ConstrainColumn.COLUMN_NAME
			LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS ConstrainType
				ON ConstrainColumn.CONSTRAINT_NAME = ConstrainType.CONSTRAINT_NAME
			LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS ForeignPrimaryKeyMap
				ON ConstrainColumn.CONSTRAINT_NAME = ForeignPrimaryKeyMap.CONSTRAINT_NAME";
			
			using (var connection = new SqlConnection(connectionString))
			{
				var dtos = connection.Query<DatabaseModel>(query);
				return DatabaseModelParser.From(dtos);
			}
		}
	}
}
