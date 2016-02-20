using Android.Media;
using ChGK.Core.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace ChGK.Droid.Services
{
    public class AudioPlayerService : IAudioPlayerService
    {
        public void PlayShort()
        {
            Play("long_timer.mp3");
        }

        public void PlayLong()
        {
            Play("long_timer.mp3");
        }

        private void Play(string filename)
        {
            MediaPlayer mediaPlayerLong = null;

            var assetsFileDescriptor = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.Assets.OpenFd(filename);
            if (assetsFileDescriptor != null)
            {
                mediaPlayerLong = new MediaPlayer();
                mediaPlayerLong.SetDataSource(assetsFileDescriptor.FileDescriptor, assetsFileDescriptor.StartOffset,
                    assetsFileDescriptor.Length);
            }

            mediaPlayerLong?.Prepare();
            mediaPlayerLong?.Start();
        }
    }
}