using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using DuckDoc.Models;
using DuckDoc.Utils;
using Type = DuckDoc.Models.Type;

namespace DuckDoc
{
	public class Parser
	{
		public static Dictionary<string, Type> ParseXml(XElement rootElement, Config config)
		{
			var types = new Dictionary<string, Type>();

			var members = rootElement.Element("members");

			foreach (var memberElement in members.Elements())
			{
				var value = memberElement.Attribute("name").Value;

				try
				{
					var type = ParseUtils.GetType(value);
					var fullNameWithoutType = ParseUtils.RemoveType(value);
					var shortName = ParseUtils.GetShortMemberName(value);
					var summary = memberElement.Element("summary").Value;


//					Console.WriteLine("Processing: " + value);
//					Console.WriteLine("as Type: " + type);
//					Console.WriteLine("");

					switch (type)
					{
						case "t": // Type
							types.Add(fullNameWithoutType, new Type(fullNameWithoutType, shortName, summary));
							break;
						case "m": // Method
						case "p": // Property
						case "e": // Event
							foreach (var fullTypeName in types.Keys)
							{
								//Console.WriteLine("Checking if it belongs to: " + fullTypeName);
								if (fullNameWithoutType.StartsWith(fullTypeName))
								{
									//Console.WriteLine("it does: " + fullTypeName);
									var member = new Member(fullNameWithoutType, shortName, summary, type);
									types[fullTypeName].Members.Add(fullNameWithoutType, member);

									var returnsElement = memberElement.Element("returns");
									var returns = returnsElement != null ? returnsElement.Value : null;
									member.Returns = returns;

									foreach (var paramElement in memberElement.Elements("param"))
									{
										var paramName = paramElement.Attribute("name").Value;
										var paramDescription = paramElement.Value;
										member.Params.Add(paramName, paramDescription);
									}

									break;
								}
							}
							break;
					}
				}
				catch (Exception e)
				{
					Console.WriteLine("Error with: " + value);
					Console.WriteLine(e);
				}
			}

			return types;

			foreach (var type in types)
			{
				Console.WriteLine("##############################");
				Console.WriteLine(type.Key);
				Console.WriteLine("Short name: " + type.Value.ShortName);
				Console.WriteLine("Summary: " + type.Value.Summary);
				Console.WriteLine("Members: ");

				foreach (var member in type.Value.Members.Values)
				{
					Console.WriteLine("\t" + member.ShortName);
					Console.WriteLine("\t\t" + member.Summary);
					Console.WriteLine("\t\t---------------" );
				}
			}
		}


	}
}