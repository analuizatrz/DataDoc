using DataDoc.Domain.Services.CreateStatementParser;
using System.Linq;
using Xunit;

namespace DataDoc.Domain.Test
{
	public class DatabaseCreateStatementParserTest
	{
		DatabaseCreateStatementParser CreateStatementParser = new DatabaseCreateStatementParser();
		string CreateStatementText = @"
			CREATE TABLE [dbo].[ TableName ] ( --  (Subtitle)  :   table description.
				[ID]             BIGINT         NOT NULL,
				[Name]           NVARCHAR (200) NOT NULL, --Name description.
				[PropertyBIGINT] BIGINT         NULL,     --Property which is a bigint.
				CONSTRAINT [PK_TableName] PRIMARY KEY CLUSTERED ([ID] ASC),
				CONSTRAINT [FK_TableName_PropertyBIGINT] FOREIGN KEY ([TableName]) REFERENCES [dbo].[PropertyBIGINT] ([ID])
			);";

		public DatabaseCreateStatementParserTest()
		{

		}
		[Fact]
		public void WhenParsingTableFromCreateStatementTableNameShouldBe()
		{
			var expected = "TableName";
			var table = CreateStatementParser.ParseTableContent(CreateStatementText);
			var actual = table.Name;

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void WhenParsingTableFromCreateStatementSubtitleShouldBe()
		{
			var expected = "(Subtitle)";
			var table = CreateStatementParser.ParseTableContent(CreateStatementText);
			var actual = table.Subtitle;

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void WhenParsingTableFromCreateStatementTableDescriptionShouldBe()
		{
			var expected = "table description.";
			var table = CreateStatementParser.ParseTableContent(CreateStatementText);
			var actual = table.Description;

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void WhenParsingTableFromCreateStatementShouldNotContainPropertyWithoutComment()
		{
			var propertyToBeLeftOut = "ID";
			var table = CreateStatementParser.ParseTableContent(CreateStatementText);
			var hasIDColumn = table.Columns.Any(x => x.Name == propertyToBeLeftOut);

			Assert.False(hasIDColumn);
		}

		[Theory]
		[InlineData("Name", 0)]
		[InlineData("PropertyBIGINT", 1)]
		public void WhenParsingTableFromCreateStatementTablePropertyNameShouldBe(string expected, int propertyPosition)
		{
			var table = CreateStatementParser.ParseTableContent(CreateStatementText);
			var column = table.Columns.ToList()[propertyPosition];
			var actual = column.Name;

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("Texto Max 200 caracteres", 0)]
		[InlineData("Inteiro", 1)]
		public void WhenParsingTableFromCreateStatementTablePropertyTypeShouldBe(string expected, int propertyPosition)
		{
			var table = CreateStatementParser.ParseTableContent(CreateStatementText);
			var column = table.Columns.ToList()[propertyPosition];
			var actual = column.DataType;

			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("Texto Max 200 caracteres", 0)]
		[InlineData("Inteiro", 1)]
		public void WhenParsingTableFromCreateStatementTablePropertyDescriptionShouldBe(string expected, int propertyPosition)
		{
			var table = CreateStatementParser.ParseTableContent(CreateStatementText);
			var column = table.Columns.ToList()[propertyPosition];
			var actual = column.DataType;

			Assert.Equal(expected, actual);
		}

	}
}
