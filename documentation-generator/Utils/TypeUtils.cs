using System.Linq;
using System.Reflection;
using DuckDoc.Models;

namespace DuckDoc.Utils
{
	public static class TypeUtils
	{
		public static MethodInfo GetMethod(System.Type type, Member member)
		{
			var args = member.ArgTypes;

			var methods = type.GetMethods();

			foreach (var m in methods)
			{
				if (m.Name == member.ShortName)
				{
					var assemblyParams = m.GetParameters();
					if (assemblyParams.Length != args.Length)
					{
						continue;
					}
					var match = true;
					for (var i = 0; i < assemblyParams.Length; i++)
					{
						var p = m.GetParameters()[i];

						var assemblyParamType = ParseUtils.DeGenerecizeParameterType(p.ParameterType.ToString());
						var xmlParamType = args[i];

						// it's a match if any of these are true
						if (assemblyParamType == xmlParamType ||
						    p.ParameterType.IsGenericParameter) continue;

						// otherwise no match, bail out
						match = false;
						break;
					}

					if (match)
					{
						return m;
					}
				}
			}

			return null;
		}
	}
}