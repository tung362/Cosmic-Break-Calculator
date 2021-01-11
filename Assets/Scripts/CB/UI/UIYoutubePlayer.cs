using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using Cysharp.Threading.Tasks;
using CB.Utils;

using Debug = UnityEngine.Debug;

namespace CB.UI
{
    /// <summary>
    /// UI effects for playing youtube videos
    /// </summary>
    public class UIYoutubePlayer : MonoBehaviour
    {
        public string YoutubeDLExePath = "";
        public VideoPlayer VideoPlayerBind;
        public List<string> Tracks = new List<string>();

        /*Events*/
        public UnityEvent OnVideoStart = new UnityEvent();
        public UnityEvent OnVideoEnd = new UnityEvent();
        public UnityEvent OnVideoError = new UnityEvent();

        /*Cache*/
        private int TrackIndex = 0;
        private bool ForceStop = true; //Ensure async stops
        private bool PlayRequestPending = false; //Spam prevention

        void Start()
        {
            //Set listener
            VideoPlayerBind.started += OnPlay;
            VideoPlayerBind.loopPointReached += OnEnd;
            VideoPlayerBind.errorReceived += OnError;
        }

        void OnDestroy()
        {
            //Unset listener
            VideoPlayerBind.started -= OnPlay;
            VideoPlayerBind.loopPointReached -= OnEnd;
            VideoPlayerBind.errorReceived -= OnError;
        }

        #region Listeners
        void OnPlay(VideoPlayer source)
        {
            OnVideoStart.Invoke();
        }

        void OnEnd(VideoPlayer source)
        {
            OnVideoEnd.Invoke();
        }

        void OnError(VideoPlayer source, string message)
        {
            OnVideoError.Invoke();
        }
        #endregion

        #region Utils
        public void Play()
        {
            ForceStop = false;
            if (Tracks.Count == 0 || PlayRequestPending) return;

            PlayRequestPending = true;
            TrackIndex = ValidIndex(TrackIndex);
            RequestPlay(Tracks[TrackIndex]).Forget();
        }

        public void Stop()
        {
            ForceStop = true;
            VideoPlayerBind.Stop();
        }

        public void Pause()
        {
            VideoPlayerBind.Pause();
        }

        public void Resume()
        {
            ForceStop = false;
            VideoPlayerBind.Play();
        }

        public void Next()
        {
            ++TrackIndex;
            Play();
        }

        public void Back()
        {
            --TrackIndex;
            Play();
        }

        public void Volume(float num)
        {
            VideoPlayerBind.SetDirectAudioVolume(0, num);
        }

        public void SetTracks(List<string> tracks)
        {
            Tracks = tracks;
        }
        #endregion

        #region Functions
        async UniTask<string> GetYoutubeDirectUrl(string path)
        {
            ProcessStartInfo youtubeDLInfo = new ProcessStartInfo
            {
                FileName = Application.dataPath + YoutubeDLExePath,
                Arguments = $"--get-url -f best {path}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process youtubeDL = Process.Start(youtubeDLInfo);
            await youtubeDL.WaitForExitAsync();
            return youtubeDL.StandardOutput.ReadLine();
        }

        async UniTaskVoid RequestPlay(string path)
        {
            string output = await GetYoutubeDirectUrl(path);
            await UniTask.SwitchToMainThread();
            if (Application.isPlaying && !ForceStop)
            {
                VideoPlayerBind.url = output;
                VideoPlayerBind.Play();
            }
            PlayRequestPending = false;
        }

        int ValidIndex(int value)
        {
            int num = value;
            if (num < 0) num = Tracks.Count - 1;
            if (num >= Tracks.Count) num = 0;
            return num;
        }

        public bool IsForceStop()
        {
            return ForceStop;
        }

        public bool IsPlayRequestPending()
        {
            return PlayRequestPending;
        }

        public bool IsPlaying()
        {
            return VideoPlayerBind.isPlaying;
        }
        #endregion
    }
}
