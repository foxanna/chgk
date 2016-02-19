﻿using Android.Content.Res;
using ChGK.Core.Services;
using ChGK.Droid.Controls;
using MvvmCross.Platform.Droid.Platform;

namespace ChGK.Droid.Services
{
	public class AudioPlayerService : IAudioPlayerService
	{
		public void PlayShort ()
		{		
			Play ("long_timer.mp3");
		}

		public void PlayLong ()
		{ 
			Play ("long_timer.mp3");
		}

		void Play (string filename)
		{
			Android.Media.MediaPlayer _mediaPlayerLong = null;

			var assetsFileDescriptor = MvvmCross.Platform.Mvx.Resolve<IMvxAndroidCurrentTopActivity> ().Activity.Assets.OpenFd (filename);
			if (assetsFileDescriptor != null) {
				_mediaPlayerLong = new Android.Media.MediaPlayer ();
				_mediaPlayerLong.SetDataSource (assetsFileDescriptor.FileDescriptor, assetsFileDescriptor.StartOffset,
					assetsFileDescriptor.Length);
			}

			_mediaPlayerLong.Prepare ();
			_mediaPlayerLong.Start ();
		}
	}
}

