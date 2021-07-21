using DataDoc.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataDoc.Domain.Services.CreateStatementParser
{
	public class DatabaseCreateStatementParser
	{
		static readonly string TypeSizeTag = "@TypeSize";
		readonly IDictionary<string, string> types = new Dictionary<string, string>
		{
			["INT"] = "Inteiro",
			["BIGINT"] = "Inteiro",
			["NVARCHAR"] = $"Texto Max {TypeSizeTag} caracteres",
			["DATE"] = "Data",
			["DATETIME"] = "Data & Hora",
			["BIT"] = "Binário 0 - Não 1 - Sim",
			["DECIMAL"] = $"Decimal precisão {TypeSizeTag}",
			["NCHAR"] = $"Texto Max {TypeSizeTag} caracteres",
		};
		public Table ParseTableContent(string text)
		{
			var lines = text
							.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
							.Where(x => x.Contains("--"))
							.Select(x => SplitCodeComment(x));
			if (lines.Any())
			{
				var (headerCode, headerComment) = lines.First();

				var table = ParseTableNameAndDescription(headerCode, headerComment);
				table.Columns = lines.Skip(1).Select(x => ParseProperty(x.code, x.comment));

				return table;
			}
			return null;
		}
		Table ParseTableNameAndDescription(string code, string comment)
		{
			var tableName = GetSqlTableName(code);
			var (title, description) = GetTitleAndDescription(comment);
			return new Table { Name = tableName.Trim(), Subtitle = title.Trim(), Description = description.Trim() };
		}
		Column ParseProperty(string code, string comment)
		{
			var name = GetSqlFieldName(code);
			var type = GetTypeName(code);
			return new Column { Name = name.Trim(), Description = comment.Trim(), DataType = type.Trim() };
		}
		(string code, string comment) SplitCodeComment(string line)
		{
			var splitted = line.Split("--");
			return (splitted[0], splitted[1]);
		}
		(string title, string description) GetTitleAndDescription(string comment)
		{
			var splitted = comment.Split(":");
			return (splitted[0], splitted[1]);
		}
		string GetTypeName(string code)
		{
			var sqlType = GetSqlTypeName(code);
			var typeSize = GetTypeSizeName(code);

			return types[sqlType.ToUpper()].Replace(TypeSizeTag, typeSize);
		}
		string GetSqlTableName(string code) => Regex.Match(code, @"\.\[(.*)\]", RegexOptions.IgnoreCase).GetValue("Nome da tabela não reconhecido");
		string GetSqlFieldName(string code) => Regex.Match(code, @"\[(.*)\]", RegexOptions.IgnoreCase).GetValue("Nome de campo não reconhecido");
		string GetSqlTypeName(string code) => Regex.Match(code, @".*\]\s*([^\s]*)", RegexOptions.IgnoreCase).GetValue("Tipo não reconhecido");
		string GetTypeSizeName(string code)
		{
			Regex
				.Match(code, @"\((.*)\)", RegexOptions.IgnoreCase)
				.TryGetValue(out var result);

			return result;
		}
	}
}
