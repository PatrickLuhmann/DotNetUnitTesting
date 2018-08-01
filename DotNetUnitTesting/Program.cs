using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoAppSettings;

namespace DotNetUnitTesting
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Welcome to DotNetUnitTesting!");

			StandardUserSettings settings = new StandardUserSettings();
			AppSettings MyInfo = new AppSettings(settings);

			MyInfo.StartRun();
			int count = MyInfo.GetRunCount();
			Console.WriteLine("This is run number {0}", count);
			MyInfo.EndRun();
		}
	}
}
