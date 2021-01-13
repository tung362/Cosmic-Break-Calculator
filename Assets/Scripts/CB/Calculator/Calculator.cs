using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CB.Calculator.Database;
using CB.Utils;
using CB.UI;

namespace CB.Calculator
{
    /// <summary>
    /// Calculator manager, keeps track of all databases and cached values used by the calculator
    /// </summary>
    public class Calculator : MonoBehaviour
    {
        public static Calculator instance { get; private set; }

        /*Version*/
        public static string Version = "v. 1.0.0";

        /*Save Locations*/
        public static string RootPath { get { return Application.dataPath + "/../"; } }
        public static string OptionsPath { get { return RootPath + "/Config/Options.preset"; } }
        public static string ShortcutsPath { get { return RootPath + "/Config/Shortcuts.preset"; } }
        public static string VideoUrlLibraryPath { get { return RootPath + "/Config/VideoUrlLibrary.preset"; } }

        /*External Locations*/
        public static string YoutubeDLPath { get { return RootPath + "/ExternalExe/youtube-dl.exe"; } }

        /*Fixed Databases*/
        [Header("Fixed Databases")]
        public JointIconDatabase JointIcons;

        /*Dynamic Databases*/
        public List<Contraption> BDs = new List<Contraption>();
        public List<Contraption> LGs = new List<Contraption>();
        public List<Contraption> HDs = new List<Contraption>();
        public List<Contraption> HACs = new List<Contraption>();
        public List<Contraption> FACs = new List<Contraption>();
        public List<Contraption> AMs = new List<Contraption>();
        public List<Contraption> BSs = new List<Contraption>();
        public List<Contraption> WPs = new List<Contraption>();
        public List<Contraption> WBs = new List<Contraption>();

        /*Options*/
        public Options Settings = new Options();
        public Shortcuts Controls = new Shortcuts();
        public VideoUrlLibrary UrlLibrary = new VideoUrlLibrary();

        /*Global Variables*/
        [Header("Global Variables")]
        public bool ShowFixedParts = false;

        /*Objects of Note*/
        [Header("Objects of Note")]
        public Canvas RootCanvas;
        public UIYoutubePlayer YoutubePlayer;
        public RawImage VideoImage;
        public RectTransform VideoControl;

        /*Global Cache*/
        [Header("Global Cache")]
        public Color GrayscaleColor;

        void OnEnable()
        {
            if (!instance) instance = this;
            else Debug.Log("Warning! Multiple instances of \"Calculator\"");
        }

        void Awake()
        {
            //Initial load
            LoadOptions();
            LoadShortcuts();
            LoadUrlLibrary();

            //Apply scale factor data
            RootCanvas.GetComponent<CanvasScaler>().scaleFactor = Settings.ScaleFactor;

            //Apply grayscale
            if(Settings.Grayscale)
            {
                VideoImage.material.SetInt("_UseGrayscale", 1);
                VideoImage.color = GrayscaleColor;
            }
            else
            {
                VideoImage.material.SetInt("_UseGrayscale", 0);
                VideoImage.color = Color.white;
            }

            //Apply shortcuts data

            //Apply youtube player data
            YoutubePlayer.Tracks = UrlLibrary.Urls;
            YoutubePlayer.Volume(Settings.Sound);
            if (Settings.VideoBackground)
            {
                VideoControl.gameObject.SetActive(true);
                YoutubePlayer.Play();
            }
            else VideoControl.gameObject.SetActive(false);
        }

        #region Serialization
        public void LoadOptions()
        {
            if (Serializer.Load(Calculator.OptionsPath, out Options result) && result != null) Settings = result;
        }

        public void SaveOptions()
        {
            Serializer.Save(Calculator.OptionsPath, Settings);
        }

        public void LoadShortcuts()
        {
            if (Serializer.Load(Calculator.ShortcutsPath, out Shortcuts result) && result != null) Controls = result;
        }

        public void SaveShortcuts()
        {
            Serializer.Save(Calculator.ShortcutsPath, Controls);
        }

        public void LoadUrlLibrary()
        {
            if (Serializer.Load(Calculator.VideoUrlLibraryPath, out VideoUrlLibrary result) && result != null) UrlLibrary = result;
        }

        public void SaveUrlLibrary()
        {
            Serializer.Save(Calculator.VideoUrlLibraryPath, UrlLibrary);
        }
        #endregion

        #region Utils
        public void ToggleFixedParts(bool toggle)
        {
            ShowFixedParts = toggle;
        }

        public void ExitApplication()
        {
            Application.Quit();
        }
        #endregion
    }
}
