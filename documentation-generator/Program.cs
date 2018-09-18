using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using CommandLine;
using DuckDoc;
using Newtonsoft.Json;
using Parser = DuckDoc.Parser;

namespace DuckDock
{
	internal class Program
	{
		public class Options
		{
			[Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
			public bool Verbose { get; set; }

			[Option('x', "xmlFile", Required = true, HelpText = "The input xml file")]
			public string XmlFile { get; set; }

			[Option('a', "assembly", Required = true, HelpText = "The Assembly path")]
			public string AssemblyPath { get; set; }

			[Option('c', "configFile", Required = false, HelpText = "The configuration file")]
			public string ConfigFile { get; set; }
		}

		public static void Main(string[] args)
		{
			CommandLine.Parser.Default.ParseArguments<Options>(args)
				.WithParsed(o =>
				{
					Config config = Config.Default;

					if (!File.Exists(o.XmlFile)) throw new Exception(
						string.Format("The file could not be found: {0}", o.XmlFile));

					if (!File.Exists(o.AssemblyPath)) throw new Exception(
						string.Format("The file could not be found: {0}", o.AssemblyPath));

					if (o.ConfigFile != null && !File.Exists(o.ConfigFile)) throw new Exception(
						string.Format("The file could not be found: {0}", o.XmlFile));

					var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + o.AssemblyPath;

					Console.WriteLine("Using assembly at path " + assemblyPath);
					var assembly = Assembly.LoadFile(assemblyPath);

					if (o.ConfigFile != null)
					{
						config = JsonConvert.DeserializeObject<Config>(o.ConfigFile);
					}

					var rootElement = XElement.Load(o.XmlFile);
					var types = Parser.ParseXml(rootElement, config);

					DocGenerator.GenerateDoc(types,assembly);
				});
		}
	}
}