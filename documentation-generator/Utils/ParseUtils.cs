using System;
using System.Linq;

namespace DuckDoc.Utils
{
	public static class ParseUtils
	{
		public static string GetType(string value)
		{
			var colonIndex = value.IndexOf(':');
			return value.Substring(0, colonIndex).ToLower();
		}

		public static string GetShortMemberName(string value)
		{
			var result = value;

			var lastOpeningBracketIndex = value.LastIndexOf('(');
			if (lastOpeningBracketIndex != -1)
			{
				result = value.Substring(0, lastOpeningBracketIndex);
			}

			var lastDotIndex = result.LastIndexOf('.');
			result = result.Substring(lastDotIndex + 1);
			return result
				.Replace("``1", "")
				.Replace("`1", "");
		}

		public static string RemoveType(string value)
		{
			var colonIndex = value.IndexOf(':');
			return value.Substring(colonIndex + 1);
		}

		public static string[] ParseParamTypes(string value)
		{
			// EG:
			// DUCK.Utils.CollectionExtensions.Random``1(System.Collections.Generic.IEnumerable{``0})"
			// DUCK.Utils.Instantiator.Instantiate``1(``0,UnityEngine.Transform,System.Boolean)

			var openBracketIndex = value.IndexOf('(');
			if (openBracketIndex == -1)
			{
				return new string[0];
			}
			var closeBracketIndex = value.IndexOf(')');
			var length = closeBracketIndex - openBracketIndex;
			var allArgsString = value.Substring(openBracketIndex + 1, length - 1);
			var allArgs = allArgsString.Split(',');

			return allArgs.Select(a => a.Replace("{``0}", "`1").Replace("``0", "T")).ToArray();
		}

		public static string DeGenerecizeParameterType(string value)
		{
			var openBracketIndex = value.IndexOf('[');
			if (openBracketIndex != -1)
			{
				return value.Substring(0, openBracketIndex);
			}

			return value;
		}

		public static string FormatSummary(string summary)
		{
			string[] lines = summary.Trim().Split(
				new[] { "\r\n", "\r", "\n" },
				StringSplitOptions.None
			);

			return string.Join(Environment.NewLine, lines.Select(s => s.Trim()));
		}
	}
}