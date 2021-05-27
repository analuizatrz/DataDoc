using Dapper;
using DataDoc.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DataDoc.Infra
{
	public class SqlServerDatabases
	{
		public Database Get(string connectionString)
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
				ON ColumnsInfo.TABLE_SCHEMA = ConstrainColumn.TABLE_SCHEMA AND ColumnsInfo.COLUMN_NAME = ConstrainColumn.COLUMN_NAME
			LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS ConstrainType
				ON ConstrainColumn.CONSTRAINT_NAME = ConstrainType.CONSTRAINT_NAME
			LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS ForeignPrimaryKeyMap
				ON ConstrainColumn.CONSTRAINT_NAME = ForeignPrimaryKeyMap.CONSTRAINT_NAME";
			
			using (var connection = new SqlConnection(connectionString))
			{
				var dtos = connection.Query<SqlServerDto>(query);
				return DatabaseFrom(dtos);
			}
		}
		public Database DatabaseFrom(IEnumerable<SqlServerDto> dtos)
		{
			var name = dtos.First().TableCatalog;
			var tables = dtos
							.GroupBy(x => x.TableName)
							.Select(table => TableFrom(table));
			return new Database { Name = name, Tables = tables };
		}
		public Table TableFrom(IEnumerable<SqlServerDto> dtos)
		{
			var name = dtos.First().TableName;
			var columns = dtos.Select(x => ColumnFrom(x));
			return new Table { Name = name, Columns = columns };
		}
		public Column ColumnFrom(SqlServerDto x)
		{
			return new Column {
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
