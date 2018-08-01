using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DemoAppSettings;
using Moq;

namespace UnitTest_DemoAppSettings
{
	[TestClass]
	public class UnitTest_AppSettings
	{
		[TestMethod]
		public void GetRunCount_MoqReturnCallback()
		{
			// Create the mock user settings interface, and configure it
			// to increment the return value each time it is invoked.
			Mock<IUserSettings> mockSettings = new Mock<IUserSettings>();
			int mCount = 0; // must be initialized before Setup().
			mockSettings.Setup(us => us.GetUserSetting("RunCount"))
				.Returns(() => mCount)
				.Callback(() => mCount++);
			AppSettings TestClass = new AppSettings(mockSettings.Object);

			int count;

			mCount = 28; // starting run count
			count = TestClass.GetRunCount();
			Assert.AreEqual(-1, count);

			TestClass.StartRun();
			count = TestClass.GetRunCount();
			Assert.AreEqual(mCount - 1, count); // mCount is incremented twice using this technique

			TestClass.EndRun();
			count = TestClass.GetRunCount();
			Assert.AreEqual(-1, count);
		}

		[TestMethod]
		public void GetRunCount_MoqReturnLazyEval()
		{
			// Create the mock user settings interface, and configure it
			// to use "lazy evaluation" for the return value.
			Mock<IUserSettings> mockSettings = new Mock<IUserSettings>();
			int mCount = 0; // must be initialized before Setup().
			mockSettings.Setup(us => us.GetUserSetting("RunCount"))
				.Returns(() => mCount);

			AppSettings TestClass = new AppSettings(mockSettings.Object);

			int count;

			mCount = 17; // starting run count
			count = TestClass.GetRunCount();
			Assert.AreEqual(-1, count);

			TestClass.StartRun();

			mCount++; // inc run count
			count = TestClass.GetRunCount();

			Assert.AreEqual(mCount, count);

			TestClass.EndRun();
			count = TestClass.GetRunCount();
			Assert.AreEqual(-1, count);
		}

	}
}
