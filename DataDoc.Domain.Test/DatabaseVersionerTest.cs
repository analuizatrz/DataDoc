using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DataDoc.Domain.Test
{
	public class DatabaseVersionerTest
	{
		IList<IEnumerable<Table>> Tables = new List<IEnumerable<Table>>
		{
			new List<Table>
			{
				NameAndListOfColumns("Table A"),
				NameAndListOfColumns("Table B", "B"),
			},
			new List<Table>
			{
				NameAndListOfColumns("Table A", "A"),
				NameAndListOfColumns("Table B", "B"),
			},
			new List<Table>
			{
				NameAndListOfColumns("Table A", "A", "C"),
				NameAndListOfColumns("Table B"),
			},
			new List<Table>
			{
				NameAndListOfColumns("Table B"),
				NameAndListOfColumns("Table C", "A", "C"),
			},
		};
		IList<IEnumerable<Database>> Datas = new List<IEnumerable<Database>>
		{
			ListOfTables(),
			ListOfTables("A", "B"),
			ListOfTables("B", "C" ),
			ListOfTables("A"),
		};
		[Theory]
		[InlineData(0, 1, 1)]
		[InlineData(0, 0, 1)]
		[InlineData(2, 1, 0)]
		[InlineData(1, 1, 2)]
		[InlineData(1, 2, 1)]
		[InlineData(0, 3, 1)]
		[InlineData(1, 1, 3)]
		[InlineData(1, 3, 2)]
		[InlineData(2, 2, 3)]
		public void WhenComparingListstheDifferenceCountShouldBe(int expected, int listOne, int listTwo)
		{
			var source = Datas[listOne];
			var target = Datas[listTwo];

			var actual = new DatabaseVersioner().Difference(source, target).Count();
			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData(0, 1, 1, "Table A")]
		[InlineData(0, 1, 1, "Table B")]

		[InlineData(0, 0, 1, "Table A")]
		[InlineData(0, 0, 1, "Table B")]
		[InlineData(1, 1, 0, "Table A")]
		[InlineData(0, 1, 0, "Table B")]

		[InlineData(0, 0, 2, "Table A")]
		[InlineData(1, 0, 2, "Table B")]
		[InlineData(2, 2, 0, "Table A")]
		[InlineData(0, 2, 0, "Table B")]

		[InlineData(0, 1, 2, "Table A")]
		[InlineData(1, 1, 2, "Table B")]
		[InlineData(1, 2, 1, "Table A")]
		[InlineData(0, 2, 1, "Table B")]

		[InlineData(0, 3, 2, "Table A")]
		[InlineData(0, 3, 2, "Table B")]
		[InlineData(2, 3, 2, "Table C")]
		[InlineData(2, 2, 3, "Table A")]
		[InlineData(0, 2, 3, "Table B")]
		[InlineData(0, 2, 3, "Table C")]
		public void WhenComparingListstheDifferenceColumnsCountShouldBe(int expected, int listOne, int listTwo, string tableName)
		{
			var source = Tables[listOne];
			var target = Tables[listTwo];
			var actual = 0;

			var result = new DatabaseVersioner().DifferenceColumns(source, target);
			if (result.TryGetValue(tableName, out var columns))
				actual = columns.Count();

			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData("A", 1, 0, "Table A")]
		[InlineData("B", 0, 2, "Table B")]
		[InlineData("A", 2, 0, "Table A")]
		[InlineData("C", 2, 0, "Table A")]
		[InlineData("B", 1, 2, "Table B")]
		[InlineData("C", 2, 1, "Table A")]
		[InlineData("A", 3, 2, "Table C")]
		[InlineData("C", 3, 2, "Table C")]
		[InlineData("A", 2, 3, "Table A")]
		[InlineData("C", 2, 3, "Table A")]
		public void WhenComparingListstheDifferenceColumnsShouldBe(string expected, int listOne, int listTwo, string tableName)
		{
			var source = Tables[listOne];
			var target = Tables[listTwo];

			var result = new DatabaseVersioner().DifferenceColumns(source, target)[tableName];
	
			Assert.Contains(result, x => x.Name == expected);
		}
		[Theory]
		[InlineData(false, 1, 1, "Table A")]
		[InlineData(false, 1, 1, "Table B")]
		[InlineData(false, 0, 1, "Table A")]
		[InlineData(false, 0, 1, "Table B")]
		[InlineData(false, 1, 0, "Table A")]
		[InlineData(false, 1, 0, "Table B")]
		[InlineData(false, 0, 2, "Table A")]
		[InlineData(false, 0, 2, "Table B")]
		[InlineData(false, 2, 0, "Table A")]
		[InlineData(false, 2, 0, "Table B")]
		[InlineData(false, 1, 2, "Table A")]
		[InlineData(false, 1, 2, "Table B")]
		[InlineData(false, 2, 1, "Table A")]
		[InlineData(false, 2, 1, "Table B")]
		[InlineData(false, 3, 2, "Table A")]
		[InlineData(false, 3, 2, "Table B")]
		[InlineData(true, 3, 2, "Table C")]
		[InlineData(true, 2, 3, "Table A")]
		[InlineData(false, 2, 3, "Table B")]
		[InlineData(false, 2, 3, "Table C")]
		public void WhenVerifyingChangesNewTablesShouldBe(bool expected, int listOne, int listTwo, string tableName)
		{
			var source = new Database { Tables = Tables[listOne] };
			var target = new Database { Tables = Tables[listTwo] };

			var result = new DatabaseVersioner().Changes(source, target).NewTables;
			var actual = result.Any(x => x.Name == tableName);

			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData(false, 1, 1, "Table A")]
		[InlineData(false, 1, 1, "Table B")]
		[InlineData(false, 0, 1, "Table A")]
		[InlineData(false, 0, 1, "Table B")]
		[InlineData(false, 1, 0, "Table A")]
		[InlineData(false, 1, 0, "Table B")]
		[InlineData(false, 0, 2, "Table A")]
		[InlineData(false, 0, 2, "Table B")]
		[InlineData(false, 2, 0, "Table A")]
		[InlineData(false, 2, 0, "Table B")]
		[InlineData(false, 1, 2, "Table A")]
		[InlineData(false, 1, 2, "Table B")]
		[InlineData(false, 2, 1, "Table A")]
		[InlineData(false, 2, 1, "Table B")]
		[InlineData(true, 3, 2, "Table A")]
		[InlineData(false, 3, 2, "Table B")]
		[InlineData(false, 3, 2, "Table C")]
		[InlineData(false, 2, 3, "Table A")]
		[InlineData(false, 2, 3, "Table B")]
		[InlineData(true, 2, 3, "Table C")]
		public void WhenVerifyingChangesDeletedTablesShouldBe(bool expected, int listOne, int listTwo, string tableName)
		{
			var source = new Database { Tables = Tables[listOne] };
			var target = new Database { Tables = Tables[listTwo] };

			var result = new DatabaseVersioner().Changes(source, target).DeletedTables;
			var actual = result.Any(x => x.Name == tableName);

			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData(false, 1, 1, "Table A", "A")]
		[InlineData(false, 1, 1, "Table A", "B")]
		[InlineData(false, 1, 1, "Table B", "A")]
		[InlineData(false, 1, 1, "Table B", "B")]
		[InlineData(false, 0, 1, "Table A", "A")]
		[InlineData(false, 0, 1, "Table A", "B")]
		[InlineData(false, 0, 1, "Table B", "A")]
		[InlineData(false, 0, 1, "Table B", "B")]
		[InlineData(true, 1, 0, "Table A", "A")]
		[InlineData(false, 1, 0, "Table A", "B")]
		[InlineData(false, 1, 0, "Table B", "A")]
		[InlineData(false, 1, 0, "Table B", "B")]
		[InlineData(false, 0, 2, "Table A", "A")]
		[InlineData(false, 0, 2, "Table A", "B")]
		[InlineData(false, 0, 2, "Table B", "A")]
		[InlineData(true, 0, 2, "Table B", "B")]
		[InlineData(true, 2, 0, "Table A", "A")]
		[InlineData(false, 2, 0, "Table A", "B")]
		[InlineData(true, 2, 0, "Table A", "C")]
		[InlineData(false, 2, 0, "Table B", "A")]
		[InlineData(false, 2, 0, "Table B", "B")]
		[InlineData(false, 1, 2, "Table A", "A")]
		[InlineData(false, 1, 2, "Table A", "B")]
		[InlineData(false, 1, 2, "Table B", "A")]
		[InlineData(true, 1, 2, "Table B", "B")]
		[InlineData(false, 2, 1, "Table A", "A")]
		[InlineData(false, 2, 1, "Table A", "B")]
		[InlineData(true, 2, 1, "Table A", "C")]
		[InlineData(false, 2, 1, "Table B", "A")]
		[InlineData(false, 2, 1, "Table B", "B")]
		[InlineData(false, 3, 2, "Table A", "A")]
		[InlineData(false, 3, 2, "Table A", "B")]
		[InlineData(false, 3, 2, "Table A", "C")]
		[InlineData(false, 3, 2, "Table B", "A")]
		[InlineData(false, 3, 2, "Table B", "B")]
		[InlineData(false, 3, 2, "Table B", "C")]
		[InlineData(true, 3, 2, "Table C", "A")]
		[InlineData(false, 3, 2, "Table C", "B")]
		[InlineData(true, 3, 2, "Table C", "C")]
		[InlineData(true, 2, 3, "Table A", "A")]
		[InlineData(false, 2, 3, "Table A", "B")]
		[InlineData(true, 2, 3, "Table A", "C")]
		[InlineData(false, 2, 3, "Table B", "A")]
		[InlineData(false, 2, 3, "Table B", "B")]
		[InlineData(false, 2, 3, "Table B", "C")]
		[InlineData(false, 2, 3, "Table C", "A")]
		[InlineData(false, 2, 3, "Table C", "B")]
		[InlineData(false, 2, 3, "Table C", "C")]
		public void WhenVerifyingChangesNewColumnsShouldBe(bool expected, int listOne, int listTwo, string tableName, string columnName)
		{
			var source = new Database { Tables = Tables[listOne] };
			var target = new Database { Tables = Tables[listTwo] };
			var actual = false;

			var result = new DatabaseVersioner().Changes(source, target).NewColumns;
			if (result.TryGetValue(tableName, out var columns))
				actual = columns.Any(x => x.Name == columnName);

			Assert.Equal(expected, actual);
		}
		[Theory]
		[InlineData(false, 1, 1, "Table A", "A")]
		[InlineData(false, 1, 1, "Table A", "B")]
		[InlineData(false, 1, 1, "Table B", "A")]
		[InlineData(false, 1, 1, "Table B", "B")]
		[InlineData(false, 0, 1, "Table A", "A")]
		[InlineData(false, 0, 1, "Table A", "B")]
		[InlineData(false, 0, 1, "Table B", "A")]
		[InlineData(false, 0, 1, "Table B", "B")]
		[InlineData(true, 1, 0, "Table A", "A")]
		[InlineData(false, 1, 0, "Table A", "B")]
		[InlineData(false, 1, 0, "Table B", "A")]
		[InlineData(false, 1, 0, "Table B", "B")]
		[InlineData(false, 0, 2, "Table A", "A")]
		[InlineData(false, 0, 2, "Table A", "B")]
		[InlineData(false, 0, 2, "Table B", "A")]
		[InlineData(true, 0, 2, "Table B", "B")]
		[InlineData(true, 2, 0, "Table A", "A")]
		[InlineData(false, 2, 0, "Table A", "B")]
		[InlineData(true, 2, 0, "Table A", "C")]
		[InlineData(false, 2, 0, "Table B", "A")]
		[InlineData(false, 2, 0, "Table B", "B")]
		[InlineData(false, 1, 2, "Table A", "A")]
		[InlineData(false, 1, 2, "Table A", "B")]
		[InlineData(false, 1, 2, "Table B", "A")]
		[InlineData(true, 1, 2, "Table B", "B")]
		[InlineData(false, 2, 1, "Table A", "A")]
		[InlineData(false, 2, 1, "Table A", "B")]
		[InlineData(true, 2, 1, "Table A", "C")]
		[InlineData(false, 2, 1, "Table B", "A")]
		[InlineData(false, 2, 1, "Table B", "B")]
		[InlineData(false, 3, 2, "Table A", "A")]
		[InlineData(false, 3, 2, "Table A", "B")]
		[InlineData(false, 3, 2, "Table A", "C")]
		[InlineData(false, 3, 2, "Table B", "A")]
		[InlineData(false, 3, 2, "Table B", "B")]
		[InlineData(false, 3, 2, "Table B", "C")]
		[InlineData(true, 3, 2, "Table C", "A")]
		[InlineData(false, 3, 2, "Table C", "B")]
		[InlineData(true, 3, 2, "Table C", "C")]
		[InlineData(true, 2, 3, "Table A", "A")]
		[InlineData(false, 2, 3, "Table A", "B")]
		[InlineData(true, 2, 3, "Table A", "C")]
		[InlineData(false, 2, 3, "Table B", "A")]
		[InlineData(false, 2, 3, "Table B", "B")]
		[InlineData(false, 2, 3, "Table B", "C")]
		[InlineData(false, 2, 3, "Table C", "A")]
		[InlineData(false, 2, 3, "Table C", "B")]
		[InlineData(false, 2, 3, "Table C", "C")]
		public void WhenVerifyingChangesDeletedColumnsShouldBe(bool expected, int listTwo, int listOne, string tableName, string columnName)
		{
			var source = new Database { Tables = Tables[listOne] };
			var target = new Database { Tables = Tables[listTwo] };
			var actual = false;

			var result = new DatabaseVersioner().Changes(source, target).DeletedColumns;
			if (result.TryGetValue(tableName, out var columns))
				actual = columns.Any(x => x.Name == columnName);

			Assert.Equal(expected, actual);
		}

		public static IEnumerable<Database> ListOfTables(params string[] items)
		{
			return items.Select(x => new Database { Name = x }).ToList();
		}
		public static Table NameAndListOfColumns(params string[] items)
		{
			var name = items[0];
			var columns = items.Skip(1).Select(x => new Column { Name = x}).ToList();
			return new Table { Name = name, Columns = columns };
		}
	}
}
