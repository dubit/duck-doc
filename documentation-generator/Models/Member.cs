using System.Collections.Generic;
using DuckDoc.Utils;

namespace DuckDoc.Models
{
	public class Member
	{
		public string FullName { get; private set; }
		public string ShortName { get; private set; }
		public string Summary { get; private set; }
		public string MemberType { get; private set; }
		public string[] ArgTypes { get; private set; }
		public string Returns { get; set; }
		public Dictionary<string, string> Params { get; private set; }

		public Member(string fullName, string shortName, string summary, string memberType)
		{
			FullName = fullName;
			ShortName = shortName;
			Summary = ParseUtils.FormatSummary(summary);
			MemberType = memberType;
			if (memberType == "m")
			{
				ArgTypes = ParseUtils.ParseParamTypes(fullName);
			}

			Params = new Dictionary<string, string>();
		}
	}
}