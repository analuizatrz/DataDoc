using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataDoc.Domain.Utils
{
	public static class MatchExtensions
	{
		public static string GetValue(this Match match, string errorMessage)
		{
			if (!match.Success || match.Groups.Count != 2)
				throw new Exception(errorMessage);

			return match.Groups[1].Value;
		}
		public static bool TryGetValue(this Match match, out string value)
		{
			value = null;
			if (match.Success || match.Groups.Count == 2)
			{
				value = match.Groups[1].Value.Split(",").Last();
				return true;
			}
			return false;
		}
	}
}
