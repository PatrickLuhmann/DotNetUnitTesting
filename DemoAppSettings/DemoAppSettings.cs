using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAppSettings
{
	public interface IUserSettings
	{
		object GetUserSetting(string name);
		void SetUserSetting(string name, object value);
	}

	public class StandardUserSettings : IUserSettings
	{
		public object GetUserSetting(string name)
		{
			// Access the user.config file for the requested value.
			return Properties.Settings.Default[name];
		}

		public void SetUserSetting(string name, object value)
		{
			Properties.Settings.Default[name] = value;
		}
	}

	public class AppSettings
    {
		private IUserSettings _user_config;
		private bool RunInProgress = false;

		private string currentChallenge;
		public string CurrentChallenge
		{
			get
			{
				return currentChallenge;
			}

			set
			{
				currentChallenge = value;
				_user_config.SetUserSetting("LastUsedChallenge", currentChallenge);
			}
		}

		public AppSettings(IUserSettings UserConfig)
		{
			_user_config = UserConfig;

			CurrentChallenge = (string)_user_config.GetUserSetting("LastUsedChallenge");
		}

		public void StartRun()
		{
			// Increment run count every time a run is started.
			//Properties.Settings.Default.RunCount++;
			int count = (int)_user_config.GetUserSetting("RunCount");
			count++;
			_user_config.SetUserSetting("RunCount", count);

			RunInProgress = true;
		}

		public void EndRun()
		{
			RunInProgress = false;

			// User settings must be saved explicitly in C#.
			Properties.Settings.Default.Save();
		}

		public int GetRunCount()
		{
			if (RunInProgress)
				//				return (int)Properties.Settings.Default["RunCount"];
				return (int)_user_config.GetUserSetting("RunCount");
			else
				return -1;
		}
    }
}
