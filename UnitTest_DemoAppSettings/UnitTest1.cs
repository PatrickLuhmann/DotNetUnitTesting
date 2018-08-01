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

		[TestMethod]
		public void LastUsedChallenge_Basic()
		{
			Mock<IUserSettings> mockSettings = new Mock<IUserSettings>();

			// Tell Moq to return this specific value when that exact method is invoked.
			// This is emulating the value in the user setting database before the
			// class object is created.
			mockSettings.Setup(us => us.GetUserSetting("LastUsedChallenge"))
				.Returns("challenge 1");

			AppSettings TestClass = new AppSettings(mockSettings.Object);

			// Did the constructor read the user setting correctly?
			Assert.AreEqual("challenge 1", TestClass.CurrentChallenge);

			// Now try changing the setting. We will check for correct
			// behavior afterwards.
			TestClass.CurrentChallenge = "challenge 2";

			// The verify must come after the test; it is not something you
			// set up beforehand so that the framework will know to look for it.
			mockSettings.Verify(us => us.SetUserSetting("LastUsedChallenge", "challenge 2"));
			// Sanity check. This also verifies that the get accessor does not
			// go to the settings database; in that case the mock will
			// give it a different string (the old one).
			Assert.AreEqual("challenge 2", TestClass.CurrentChallenge);
		}
	}
}
