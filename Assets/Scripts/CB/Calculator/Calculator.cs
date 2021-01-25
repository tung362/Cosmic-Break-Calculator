using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CB.Calculator.Database;
using CB.Utils;
using CB.UI;
using CB.Calculator.UI;
using SFB;

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
        public static string DataBundlePath { get { return RootPath + "/DataBundle/"; } }
        public static string TunePath { get { return DataBundlePath + "/Tune/"; } }
        public static string CartridgePath { get { return DataBundlePath + "/Cartridge/"; } }
        public static string BDPath { get { return DataBundlePath + "/BD/"; } }
        public static string LGPath { get { return DataBundlePath + "/LG/"; } }
        public static string HDPath { get { return DataBundlePath + "/HD/"; } }
        public static string HACPath { get { return DataBundlePath + "/HAC/"; } }
        public static string FACPath { get { return DataBundlePath + "/FAC/"; } }
        public static string AMPath { get { return DataBundlePath + "/AM/"; } }
        public static string BSPath { get { return DataBundlePath + "/BS/"; } }
        public static string WPPath { get { return DataBundlePath + "/WP/"; } }
        public static string WBPath { get { return DataBundlePath + "/WB/"; } }
        public static string DataBundleCustomPath { get { return RootPath + "/DataBundleCustom/"; } }
        public static string CustomContraptionPath { get { return DataBundleCustomPath + "/Contraption/"; } }
        public static string CustomTunePath { get { return DataBundleCustomPath + "/Tune/"; } }
        public static string CustomCartridgePath { get { return DataBundleCustomPath + "/Cartridge/"; } }
        public static string CustomBDPath { get { return DataBundleCustomPath + "/BD/"; } }
        public static string CustomLGPath { get { return DataBundleCustomPath + "/LG/"; } }
        public static string CustomHDPath { get { return DataBundleCustomPath + "/HD/"; } }
        public static string CustomHACPath { get { return DataBundleCustomPath + "/HAC/"; } }
        public static string CustomFACPath { get { return DataBundleCustomPath + "/FAC/"; } }
        public static string CustomAMPath { get { return DataBundleCustomPath + "/AM/"; } }
        public static string CustomBSPath { get { return DataBundleCustomPath + "/BS/"; } }
        public static string CustomWPPath { get { return DataBundleCustomPath + "/WP/"; } }
        public static string CustomWBPath { get { return DataBundleCustomPath + "/WB/"; } }

        /*External Locations*/
        public static string YoutubeDLPath { get { return RootPath + "/ExternalExe/youtube-dl.exe"; } }

        /*Enums*/
        public enum SaveType { None, Build, Part, Tune, Cartridge }

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
        public SaveType SaveState = SaveType.Build;
        public Dictionary<SaveType, string> SaveLocations = new Dictionary<SaveType, string>();
        public bool ShowFixedParts = false;

        /*Objects of Note*/
        [Header("Objects of Note")]
        public Canvas RootCanvas;
        public TMP_InputField VersionInputField;
        public TMP_InputField FileNameInputField;
        public PartBuilder CustomPartBuilder;
        public CartridgeBuilder CustomCartridgeBuilder;
        public UIYoutubePlayer YoutubePlayer;
        public RectTransform VideoControl;
        public GameObject AudioVisualizer;
        public RectTransform AudioVisualizerUI;

        /*Assets of Note*/
        [Header("Assets of Note")]
        public Material BlurMaterial;
        public Material VideoPlayerMaterial;
        public Material AudioVisualizerMaterial;

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

            //Apply version
            VersionInputField.text = Version;

            //Apply scale factor data
            RootCanvas.GetComponent<CanvasScaler>().scaleFactor = Settings.ScaleFactor;

            //Apply grayscale
            if(Settings.Grayscale)
            {
                VideoPlayerMaterial.SetInt("_UseGrayscale", 1);
                VideoPlayerMaterial.SetColor("_VideoColor", GrayscaleColor);
            }
            else
            {
                VideoPlayerMaterial.SetInt("_UseGrayscale", 0);
                VideoPlayerMaterial.SetColor("_VideoColor", Color.white);
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

            //Apply audio visualizer
            if (Settings.AudioVisualizer)
            {
                AudioVisualizer.gameObject.SetActive(true);
                AudioVisualizerUI.gameObject.SetActive(true);
            }
            else
            {
                AudioVisualizer.gameObject.SetActive(false);
                AudioVisualizerUI.gameObject.SetActive(false);
            }
        }

        #region Serialization
        public void NewFile()
        {
            switch (SaveState)
            {
                case SaveType.Build:
                    break;
                case SaveType.Part:
                    CustomPartBuilder.CreateNew();
                    break;
                case SaveType.Tune:
                    break;
                case SaveType.Cartridge:
                    CustomCartridgeBuilder.CreateNew();
                    break;
                default:
                    break;
            }
            SaveLocations[SaveState] = null;
            FileNameInputField.text = "Unsaved";
        }

        public void Save()
        {
            if(SaveLocations.ContainsKey(SaveState))
            {
                if (!string.IsNullOrEmpty(SaveLocations[SaveState]))
                {
                    switch (SaveState)
                    {
                        case SaveType.Build:
                            Serializer.Save(SaveLocations[SaveState], Settings);
                            break;
                        case SaveType.Part:
                            Serializer.Save(SaveLocations[SaveState], CustomPartBuilder.AssembledData);
                            break;
                        case SaveType.Tune:
                            Serializer.Save(SaveLocations[SaveState], Settings);
                            break;
                        case SaveType.Cartridge:
                            Serializer.Save(SaveLocations[SaveState], CustomCartridgeBuilder.AssembledData);
                            break;
                        default:
                            break;
                    }
                    return;
                }
            }
            SaveAs();
        }

        public void SaveAs()
        {
            object dataToSave = null;
            switch (SaveState)
            {
                case SaveType.Build:
                    SaveLocations[SaveState] = StandaloneFileBrowser.SaveFilePanel("Save Build", Calculator.CustomContraptionPath, "CustomBuild", "part");
                    dataToSave = Settings;
                    break;
                case SaveType.Part:
                    switch (CustomPartBuilder.AssembledData.Root.Joint)
                    {
                        case PartJoint.JointType.BD:
                            SaveLocations[SaveState] = StandaloneFileBrowser.SaveFilePanel("Save Part", Calculator.CustomBDPath, "CustomBD", "part");
                            break;
                        case PartJoint.JointType.LG:
                            SaveLocations[SaveState] = StandaloneFileBrowser.SaveFilePanel("Save Part", Calculator.CustomLGPath, "CustomLG", "part");
                            break;
                        case PartJoint.JointType.HD:
                            SaveLocations[SaveState] = StandaloneFileBrowser.SaveFilePanel("Save Part", Calculator.CustomHDPath, "CustomHD", "part");
                            break;
                        case PartJoint.JointType.HAC:
                            SaveLocations[SaveState] = StandaloneFileBrowser.SaveFilePanel("Save Part", Calculator.CustomHACPath, "CustomHAC", "part");
                            break;
                        case PartJoint.JointType.FAC:
                            SaveLocations[SaveState] = StandaloneFileBrowser.SaveFilePanel("Save Part", Calculator.CustomFACPath, "CustomFAC", "part");
                            break;
                        case PartJoint.JointType.AM:
                            SaveLocations[SaveState] = StandaloneFileBrowser.SaveFilePanel("Save Part", Calculator.CustomAMPath, "CustomAM", "part");
                            break;
                        case PartJoint.JointType.BS:
                            SaveLocations[SaveState] = StandaloneFileBrowser.SaveFilePanel("Save Part", Calculator.CustomBSPath, "CustomBS", "part");
                            break;
                        case PartJoint.JointType.WP:
                            SaveLocations[SaveState] = StandaloneFileBrowser.SaveFilePanel("Save Part", Calculator.CustomWPPath, "CustomWP", "part");
                            break;
                        case PartJoint.JointType.WB:
                            SaveLocations[SaveState] = StandaloneFileBrowser.SaveFilePanel("Save Part", Calculator.CustomWBPath, "CustomWB", "part");
                            break;
                        default:
                            break;
                    }
                    dataToSave = CustomPartBuilder.AssembledData;
                    break;
                case SaveType.Tune:
                    SaveLocations[SaveState] = StandaloneFileBrowser.SaveFilePanel("Save Tune", Calculator.CustomTunePath, "CustomTune", "tune");
                    dataToSave = Settings;
                    break;
                case SaveType.Cartridge:
                    SaveLocations[SaveState] = StandaloneFileBrowser.SaveFilePanel("Save Cartridge", Calculator.CustomCartridgePath, "CustomCartridge", "cartridge");
                    dataToSave = CustomCartridgeBuilder.AssembledData;
                    break;
                default:
                    break;
            }

            if (SaveLocations.ContainsKey(SaveState))
            {
                if (!string.IsNullOrEmpty(SaveLocations[SaveState]) && dataToSave != null)
                {
                    Serializer.Save(SaveLocations[SaveState], dataToSave);
                    FileNameInputField.text = Path.GetFileNameWithoutExtension(SaveLocations[SaveState]);
                }
            }
        }

        public void Load()
        {
            string[] path = new string[0];
            bool successful = false;
            switch (SaveState)
            {
                case SaveType.Build:
                    path = StandaloneFileBrowser.OpenFilePanel("Open Build", Calculator.CustomContraptionPath, "part", false);
                    break;
                case SaveType.Part:
                    path = StandaloneFileBrowser.OpenFilePanel("Open Part", Calculator.DataBundleCustomPath, "part", false);
                    if (path.Length != 0)
                    {
                        if (Serializer.Load(path[0], out Contraption result) && result != null)
                        {
                            CustomPartBuilder.Load(result);
                            successful = true;
                        }
                    }
                    break;
                case SaveType.Tune:
                    path = StandaloneFileBrowser.OpenFilePanel("Open Tune", Calculator.CustomTunePath, "tune", false);
                    break;
                case SaveType.Cartridge:
                    path = StandaloneFileBrowser.OpenFilePanel("Open Cartridge", Calculator.CustomCartridgePath, "cartridge", false);
                    if (path.Length != 0)
                    {
                        if (Serializer.Load(path[0], out Cartridge result) && result != null)
                        {
                            CustomCartridgeBuilder.Load(result);
                            successful = true;
                        }
                    }
                    break;
                default:
                    break;
            }

            if(successful)
            {
                SaveLocations[SaveState] = path[0];
                FileNameInputField.text = Path.GetFileNameWithoutExtension(SaveLocations[SaveState]);
            }
        }

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
        public void SetSaveState(int state)
        {
            SaveState = (SaveType)state;
            if (SaveLocations.ContainsKey(SaveState))
            {
                if (!string.IsNullOrEmpty(SaveLocations[SaveState]))
                {
                    FileNameInputField.text = Path.GetFileNameWithoutExtension(SaveLocations[SaveState]);
                    return;
                }
            }
            FileNameInputField.text = "Unsaved";
        }

        public void ToggleFixedParts(bool toggle)
        {
            ShowFixedParts = toggle;
        }

        public void UpdateFileName()
        {
            FileNameInputField.text = Path.GetFileNameWithoutExtension(SaveLocations[SaveState]);
        }

        public void ExitApplication()
        {
            Application.Quit();
        }
        #endregion
    }
}
