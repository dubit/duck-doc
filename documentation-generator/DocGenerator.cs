using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DuckDoc.Utils;
using Type = DuckDoc.Models.Type;

namespace DuckDoc
{
	public class DocGenerator
	{
		public static void GenerateDoc(Dictionary<string, Type> types, Assembly assembly)
		{
			var outputDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			foreach (var type in types)
			{
				Console.WriteLine("Type: " + type.Key);
				CreateFile(type.Value, outputDir, assembly);
			}
		}

		private static void CreateFile(Type typeValue, string outputDir, Assembly assembly)
		{
			var type = assembly.GetType(typeValue.FullName);

			var sb = new StringBuilder();

			sb.AppendLine("### Events");
			sb.AppendLine();

			sb.AppendLine("### Functions");
			sb.AppendLine();

			foreach (var member in typeValue.Members.Values)
			{
				Console.Write("\tmember: " + member.ShortName + " ... ");
				var methodInfo = TypeUtils.GetMethod(type, member);
				if (methodInfo == null)
				{
					Console.WriteLine("(unable to parse)");
					continue;
				}
				Console.WriteLine("(success)");

				var signature = methodInfo.GetSignature();

				sb.AppendLine(string.Format("#### `{0}`", signature));
				sb.AppendLine("##### Summary");
				sb.AppendLine(string.Format("{0}", member.Summary));
				sb.AppendLine();
				sb.AppendLine("##### Returns");
				sb.AppendLine(string.Format("{0}", member.Returns));
				sb.AppendLine();
				sb.AppendLine("##### Parameters");
				sb.AppendLine("| Name | Type | Description |");
				sb.AppendLine("| --- | --- | --- |");
				foreach (var keyValuePair in member.Params)
				{
					var name = keyValuePair.Key;
					var paramType = MethodInfoExtensions.TypeName(methodInfo.GetParameters().FirstOrDefault(p => p.Name == name).ParameterType);
					var description = keyValuePair.Value;
					sb.AppendLine(string.Format("| {0} | {1} | {2} |", name, paramType, description));
				}

				sb.AppendLine();
			}

			var outfile = outputDir + "/" + typeValue.ShortName + ".md";
			Console.WriteLine(string.Format("Written output to {0}", outfile));
			File.WriteAllText(outfile, sb.ToString());
		}
	}
}