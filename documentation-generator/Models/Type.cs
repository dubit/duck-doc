using System.Collections.Generic;

namespace DuckDoc.Models
{
	public class Type
	{
		public string FullName { get; private set; }
		public string ShortName { get; private set; }
		public string Summary { get; private set; }

		public Dictionary<string, Member> Members { get; private set; }

		public Type(string fullName, string shortName, string summary)
		{
			FullName = fullName;
			ShortName = shortName;
			Summary = summary;

			Members = new Dictionary<string, Member>();
		}
	}
}