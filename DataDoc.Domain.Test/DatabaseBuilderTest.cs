using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DataDoc.Domain.Test
{
	public class DatabaseBuilderTest
	{
		DatabaseModel model;
		IEnumerable<DatabaseModel> modelsOfOneTable;
		IEnumerable<DatabaseModel> modelsOfAnyTable;
		IList<IEnumerable<DatabaseModel>> modelsSets;
		public DatabaseBuilderTest()
		{
			model = new DatabaseModel
			{
				TableCatalog = "Employee",
				TableName = "Countries",
				ColumnName = "Region",
				OrdinalPosition = 3,
				ColumnDefault = null,
				DataType = "int",
				CharacterMaximumLength = null,
				NumericPrecision = 10,
				NumericPrecisionRadix = 10,
				NumericScale = 0,
				DatetimePrecision = null,
				ConstraintName = "FK_Countries_Region",
				ConstrainType = "FOREIGN KEY",
				PrimaryKeyReferenced = "PK_region"
			};
			modelsOfOneTable = new List<DatabaseModel>
			{
				new DatabaseModel
				{
					TableName = "Country",
					ColumnName = "Id"
				},
				new DatabaseModel
				{
					TableName = "Country",
					ColumnName = "Name"
				},
				new DatabaseModel
				{
					TableName = "Country",
					ColumnName = "Region"
				},
			};
			modelsOfAnyTable = new List<DatabaseModel>
			{
				new DatabaseModel
				{
					TableName = "A",
					ColumnName = "a"
				},
				new DatabaseModel
				{
					TableName = "A",
					ColumnName = "aa"
				},
				new DatabaseModel
				{
					TableName = "B",
					ColumnName = "b"
				},
			};
			modelsSets = new List<IEnumerable<DatabaseModel>>
			{
				modelsOfOneTable,
				modelsOfAnyTable
			};
		}
		[Theory]
		[InlineData("Employee")]
		[InlineData("Costumer")]
		public void WhenBuildingFromModelsNameShouldBe(string expected)
		{
			var models = modelsOfOneTable.Select(x => new DatabaseModel { TableCatalog = expected, ColumnName = x.ColumnName });

			var actual = DatabaseBuilder.From(models).Name;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData(1,0)]
		[InlineData(2,1)]
		public void WhenBuildingFromModelsTableCountShouldBe(int expected, int index)
		{
			var models = modelsSets[index];

			var actual = DatabaseBuilder.From(models).Tables.Count();
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData("Country", 0, 0)]
		[InlineData("A", 1, 0)]
		[InlineData("B", 1, 1)]
		public void WhenBuildingFromModelsTableNameShouldBe(string expected, int modelsIndex, int tableIndex)
		{
			var models = modelsSets[modelsIndex];

			var actual = DatabaseBuilder.From(models).Tables.ToList()[tableIndex].Name;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData(3, 0, 0)]
		[InlineData(2, 1, 0)]
		[InlineData(1, 1, 1)]
		public void WhenBuildingFromModelsTableColumnsCountShouldBe(int expected, int modelsIndex, int tableIndex)
		{
			var models = modelsSets[modelsIndex];

			var actual = DatabaseBuilder.From(models).Tables.ToList()[tableIndex].Columns.Count();
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData("City")]
		[InlineData("Country")]
		public void WhenBuildingTableFromModelsNameShouldBe(string expected)
		{
			var models = modelsOfOneTable.Select(x => new DatabaseModel { TableName = expected, ColumnName = x.ColumnName });

			var actual = DatabaseBuilder.TableFrom(models).Name;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public void WhenBuildingTableFromModelsColumnsCountShouldBe(int expected)
		{
			var models = modelsOfOneTable.Take(expected);

			var actual = DatabaseBuilder.TableFrom(models).Columns.Count();
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData("Id", 0)]
		[InlineData("Name", 1)]
		[InlineData("Region", 2)]
		public void WhenBuildingTableFromModelsColumnNameShouldBe(string expected, int index)
		{
			var models = modelsOfOneTable;

			var actual = DatabaseBuilder.TableFrom(models).Columns.ToList()[index].Name;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData("NameOne")]
		[InlineData("NameTwo")]
		public void WhenBuildingFromModelNameShouldBe(string expected)
		{
			model.ColumnName = expected;
			var actual = DatabaseBuilder.ColumnFrom(model).Name;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		public void WhenBuildingFromModelOrdinalPositionShouldBe(int expected)
		{
			model.OrdinalPosition = expected;
			var actual = DatabaseBuilder.ColumnFrom(model).OrdinalPosition;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData("(NULL)")]
		[InlineData("2")]
		[InlineData(null)]
		public void WhenBuildingFromModelColumnDefaultShouldBe(string expected)
		{
			model.ColumnDefault = expected;
			var actual = DatabaseBuilder.ColumnFrom(model).ColumnDefault;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData("int")]
		[InlineData("nvarchar")]
		public void WhenBuildingFromModelDataTypeShouldBe(string expected)
		{
			model.DataType = expected;
			var actual = DatabaseBuilder.ColumnFrom(model).DataType;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(null)]
		public void WhenBuildingFromModelCharacterMaximumLengthShouldBe(int? expected)
		{
			model.CharacterMaximumLength = expected;
			var actual = DatabaseBuilder.ColumnFrom(model).CharacterMaximumLength;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(null)]
		public void WhenBuildingFromModelNumericPrecisionShouldBe(int? expected)
		{
			model.NumericPrecision = expected;
			var actual = DatabaseBuilder.ColumnFrom(model).NumericPrecision;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(null)]
		public void WhenBuildingFromModelNumericPrecisionRadixShouldBe(int? expected)
		{
			model.NumericPrecisionRadix = expected;
			var actual = DatabaseBuilder.ColumnFrom(model).NumericPrecisionRadix;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(null)]
		public void WhenBuildingFromModelNumericScaleShouldBe(int? expected)
		{
			model.NumericScale = expected;
			var actual = DatabaseBuilder.ColumnFrom(model).NumericScale;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(null)]
		public void WhenBuildingFromModelDatetimePrecisionShouldBe(int? expected)
		{
			model.DatetimePrecision = expected;
			var actual = DatabaseBuilder.ColumnFrom(model).DatetimePrecision;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData("FK_City_Country")]
		[InlineData("PK_Countries")]
		[InlineData(null)]
		public void WhenBuildingFromModelConstraintNameShouldBe(string expected)
		{
			model.ConstraintName = expected;
			var actual = DatabaseBuilder.ColumnFrom(model).ConstraintName;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData("FOREIGN KEY")]
		[InlineData("PRIMARY KEY")]
		[InlineData(null)]
		public void WhenBuildingFromModelConstrainTypeShouldBe(string expected)
		{
			model.ConstrainType = expected;
			var actual = DatabaseBuilder.ColumnFrom(model).ConstrainType;
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData("PK_city")]
		[InlineData(null)]
		public void WhenBuildingFromModelPrimaryKeyReferencedShouldBe(string expected)
		{
			model.PrimaryKeyReferenced = expected;
			var actual = DatabaseBuilder.ColumnFrom(model).PrimaryKeyReferenced;
			Assert.Equal(expected, actual);
		}
	}
}
