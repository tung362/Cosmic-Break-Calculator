using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using CB.Calculator.Database;
using CB.Utils;
using CB.UI;
using CB.Calculator.UI;
using CB.Calculator.Utils;
using SFB;
using Cysharp.Threading.Tasks;

namespace CB.Calculator
{
    /// <summary>
    /// Calculator manager, keeps track of all databases and cached values used by the calculator
    /// </summary>
    public class Calculator : MonoBehaviour
    {
        public static Calculator instance { get; private set; }

        /*Version*/
        public static string Version = "v. 0.7.5";

        /*Save Locations*/
        public static string RootPath { get { return Application.dataPath + "/../"; } }
        public static string TunePalettePath { get { return RootPath + "/Config/TunePalette.preset"; } }
        public static string OptionsPath { get { return RootPath + "/Config/Options.preset"; } }
        public static string ShortcutsPath { get { return RootPath + "/Config/Shortcuts.preset"; } }
        public static string VideoUrlLibraryPath { get { return RootPath + "/Config/VideoUrlLibrary.preset"; } }
        public static string DataBundlePath { get { return RootPath + "/DataBundle"; } }
        public static string TunePath { get { return DataBundlePath + "/Tune"; } }
        public static string CartridgePath { get { return DataBundlePath + "/Cartridge"; } }
        public static string PartPath { get { return DataBundlePath + "/Part"; } }
        public static string BDPath { get { return PartPath + "/BD"; } }
        public static string LGPath { get { return PartPath + "/LG"; } }
        public static string HDPath { get { return PartPath + "/HD"; } }
        public static string HACPath { get { return PartPath + "/HAC"; } }
        public static string FACPath { get { return PartPath + "/FAC"; } }
        public static string AMPath { get { return PartPath + "/AM"; } }
        public static string BSPath { get { return PartPath + "/BS"; } }
        public static string WPPath { get { return PartPath + "/WP"; } }
        public static string WBPath { get { return PartPath + "/WB"; } }
        public static string DataBundleCustomPath { get { return RootPath + "/DataBundleCustom"; } }
        public static string CustomContraptionPath { get { return DataBundleCustomPath + "/Contraption"; } }
        public static string CustomTunePath { get { return DataBundleCustomPath + "/Tune"; } }
        public static string CustomCartridgePath { get { return DataBundleCustomPath + "/Cartridge"; } }
        public static string CustomPartPath { get { return DataBundleCustomPath + "/Part"; } }
        public static string CustomBDPath { get { return CustomPartPath + "/BD"; } }
        public static string CustomLGPath { get { return CustomPartPath + "/LG"; } }
        public static string CustomHDPath { get { return CustomPartPath + "/HD"; } }
        public static string CustomHACPath { get { return CustomPartPath + "/HAC"; } }
        public static string CustomFACPath { get { return CustomPartPath + "/FAC"; } }
        public static string CustomAMPath { get { return CustomPartPath + "/AM"; } }
        public static string CustomBSPath { get { return CustomPartPath + "/BS"; } }
        public static string CustomWPPath { get { return CustomPartPath + "/WP"; } }
        public static string CustomWBPath { get { return CustomPartPath + "/WB"; } }

        /*External Locations*/
        public static string YoutubeDLPath { get { return RootPath + "/ExternalExe/youtube-dl.exe"; } }

        /*Enums*/
        public enum SaveType { None, Build, Part, Tune, Cartridge }

        /*Fixed Databases*/
        [Header("Fixed Databases")]
        public JointIconDatabase JointIcons;

        /*Dynamic Databases*/
        public Dictionary<string, Contraption> Builds = new Dictionary<string, Contraption>();
        public Dictionary<string, Contraption> Parts = new Dictionary<string, Contraption>();
        public Dictionary<string, Tune> Tunes = new Dictionary<string, Tune>();
        public Dictionary<string, Cartridge> Cartridges = new Dictionary<string, Cartridge>();

        /*Options*/
        public TunePalette TuneColorPalette = new TunePalette();
        public Options Settings = new Options();
        public Shortcuts Controls = new Shortcuts();
        public VideoUrlLibrary UrlLibrary = new VideoUrlLibrary();

        /*Global Variables*/
        [Header("Global Variables")]
        public SaveType SaveState = SaveType.Build;
        public Dictionary<SaveType, string> SaveLocations = new Dictionary<SaveType, string>();
        public bool ShowFixedParts = false;
        public DirectoryWatcher BuildWatcher;
        public DirectoryWatcher PartWatcher;
        public DirectoryWatcher TuneWatcher;
        public DirectoryWatcher CartridgeWatcher;

        /*Objects of Note*/
        [Header("Objects of Note")]
        public Canvas RootCanvas;
        public TMP_InputField VersionInputField;
        public TMP_InputField FileNameInputField;
        public PartListView PartLibrary;
        public PartBuilder CustomBuildBuilder;
        public PartBuilder CustomPartBuilder;
        public TuneBuilder CustomTuneBuilder;
        public CartridgeBuilder CustomCartridgeBuilder;
        public OutlineBuilder BuildOutline;
        public OutlineBuilder PartOutline;
        public OutlineBuilder TuneOutline;
        public OutlineBuilder CartridgeOutline;
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

        /*Callbacks*/
        [Header("Callbacks")]
        public StringBoolEvent OnBuildsChange;
        public UnityEvent OnBuildsFinish;
        public StringBoolEvent OnPartsChange;
        public UnityEvent OnPartsFinish;
        public StringBoolEvent OnTunesChange;
        public UnityEvent OnTunesFinish;
        public StringBoolEvent OnCartridgesChange;
        public UnityEvent OnCartridgesFinish;

        /*Cache*/
        private HashSet<string> BuildsFileQueue = new HashSet<string>();
        private HashSet<string> PartsFileQueue = new HashSet<string>();
        private HashSet<string> TunesFileQueue = new HashSet<string>();
        private HashSet<string> CartridgesFileQueue = new HashSet<string>();

        void OnEnable()
        {
            if (!instance) instance = this;
            else Debug.Log("Warning! Multiple instances of \"Calculator\"");
        }

        void Awake()
        {
            /*Variable Assignment*/
            //Watcher
            BuildWatcher = new DirectoryWatcher(new string[] { CustomContraptionPath }, new string[] { "*.part" });
            PartWatcher = new DirectoryWatcher(new string[] { PartPath, CustomPartPath }, new string[] { "*.part" });
            TuneWatcher = new DirectoryWatcher(new string[] { TunePath, CustomTunePath }, new string[] { "*.tune" });
            CartridgeWatcher = new DirectoryWatcher(new string[] { CartridgePath, CustomCartridgePath }, new string[] { "*.cartridge" });

            /*Initial library loader*/
            BuildsFileLoaderAsync().Forget();
            PartsFileLoaderAsync().Forget();
            TunesFileLoaderAsync().Forget();
            CartridgesFileLoaderAsync().Forget();

            /*Initial config Load*/
            LoadTunePalette();
            LoadOptions();
            LoadShortcuts();
            LoadUrlLibrary();

            /*Apply config Load*/
            //Apply version
            VersionInputField.text = Version;

            //Apply scale factor data
            RootCanvas.GetComponent<CanvasScaler>().scaleFactor = Settings.ScaleFactor;

            //Apply grayscale
            if (Settings.Grayscale)
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

        void Start()
        {
            /*Init*/
            //Proxy
            SetListeners();
            //List view
            PartLibrary.Init();
            PartLibrary.SetListeners(OnPartsChange, OnPartsFinish);
            //Outline
            //BuildOutline.SetListeners(BuildWatcher);
            //PartOutline.SetListeners(PartWatcher);
            //TuneOutline.SetListeners(TuneWatcher);
            //CartridgeOutline.SetListeners(CartridgeWatcher);
            //Watcher
            BuildWatcher.Load();
            BuildWatcher.Watch();
            PartWatcher.Load();
            PartWatcher.Watch();
            TuneWatcher.Load();
            TuneWatcher.Watch();
            CartridgeWatcher.Load();
            CartridgeWatcher.Watch();
        }

        void OnDestroy()
        {
            UnsetListeners();
            BuildWatcher.Dispose();
            PartWatcher.Dispose();
            TuneWatcher.Dispose();
            CartridgeWatcher.Dispose();
        }

        #region Listeners
        async UniTaskVoid OnBuildFileChanged(string path, string root)
        {
            await UniTask.Yield(PlayerLoopTiming.EarlyUpdate);
            await UniTask.SwitchToMainThread();
            BuildsFileQueue.Add(path);
        }

        async UniTaskVoid OnBuildDeleted(string path, string root)
        {
            await UniTask.Yield(PlayerLoopTiming.EarlyUpdate);
            await UniTask.SwitchToMainThread();

            if (BuildsFileQueue.Contains(path)) BuildsFileQueue.Remove(path);

            if (Builds.ContainsKey(path))
            {
                Builds.Remove(path);
                OnBuildsChange.Invoke(path, false);
            }
        }

        async UniTaskVoid OnPartFileChanged(string path, string root)
        {
            await UniTask.Yield(PlayerLoopTiming.EarlyUpdate);
            await UniTask.SwitchToMainThread();
            PartsFileQueue.Add(path);
        }

        async UniTaskVoid OnPartDeleted(string path, string root)
        {
            await UniTask.Yield(PlayerLoopTiming.EarlyUpdate);
            await UniTask.SwitchToMainThread();

            if (PartsFileQueue.Contains(path)) PartsFileQueue.Remove(path);

            if (Parts.ContainsKey(path))
            {
                Parts.Remove(path);
                OnPartsChange.Invoke(path, false);
            }
        }

        async UniTaskVoid OnTuneFileChanged(string path, string root)
        {
            await UniTask.Yield(PlayerLoopTiming.EarlyUpdate);
            await UniTask.SwitchToMainThread();
            TunesFileQueue.Add(path);
        }

        async UniTaskVoid OnTuneDeleted(string path, string root)
        {
            await UniTask.Yield(PlayerLoopTiming.EarlyUpdate);
            await UniTask.SwitchToMainThread();

            if (TunesFileQueue.Contains(path)) TunesFileQueue.Remove(path);

            if (Tunes.ContainsKey(path))
            {
                Tunes.Remove(path);
                OnTunesChange.Invoke(path, false);
            }
        }

        async UniTaskVoid OnCartridgeFileChanged(string path, string root)
        {
            await UniTask.Yield(PlayerLoopTiming.EarlyUpdate);
            await UniTask.SwitchToMainThread();
            CartridgesFileQueue.Add(path);
        }

        async UniTaskVoid OnCartridgeDeleted(string path, string root)
        {
            await UniTask.Yield(PlayerLoopTiming.EarlyUpdate);
            await UniTask.SwitchToMainThread();

            if (CartridgesFileQueue.Contains(path)) CartridgesFileQueue.Remove(path);

            if (Cartridges.ContainsKey(path))
            {
                Cartridges.Remove(path);
                OnCartridgesChange.Invoke(path, false);
            }
        }
        #endregion

        #region Serialization
        async UniTaskVoid BuildsFileLoaderAsync()
        {
            while (true)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                await UniTask.SwitchToMainThread();

                if (BuildsFileQueue.Count > 0)
                {
                    string path = BuildsFileQueue.First();
                    if (!Builds.ContainsKey(path))
                    {
                        (bool, Contraption) result = await Serializer.LoadAsync<Contraption>(path);
                        await UniTask.SwitchToMainThread();
                        if (BuildsFileQueue.Contains(path))
                        {
                            if (result.Item1 && result.Item2 != null)
                            {
                                Builds.Add(path, result.Item2);
                                OnBuildsChange.Invoke(path, true);
                                if (BuildsFileQueue.Count <= 1) OnBuildsFinish.Invoke();
                            }
                        }
                    }
                    BuildsFileQueue.Remove(path);
                }
            }
        }

        async UniTaskVoid PartsFileLoaderAsync()
        {
            while(true)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                await UniTask.SwitchToMainThread();

                if(PartsFileQueue.Count > 0)
                {
                    string path = PartsFileQueue.First();
                    if (!Parts.ContainsKey(path))
                    {
                        (bool, Contraption) result = await Serializer.LoadAsync<Contraption>(path);
                        await UniTask.SwitchToMainThread();
                        if (PartsFileQueue.Contains(path))
                        {
                            if (result.Item1 && result.Item2 != null)
                            {
                                Parts.Add(path, result.Item2);
                                OnPartsChange.Invoke(path, true);
                                if (PartsFileQueue.Count <= 1) OnPartsFinish.Invoke();
                            }
                        }
                    }
                    PartsFileQueue.Remove(path);
                }
            }
        }

        async UniTaskVoid TunesFileLoaderAsync()
        {
            while (true)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                await UniTask.SwitchToMainThread();

                if (TunesFileQueue.Count > 0)
                {
                    string path = TunesFileQueue.First();
                    if (!Tunes.ContainsKey(path))
                    {
                        (bool, Tune) result = await Serializer.LoadAsync<Tune>(path);
                        await UniTask.SwitchToMainThread();
                        if (TunesFileQueue.Contains(path))
                        {
                            if (result.Item1 && result.Item2 != null)
                            {
                                Tunes.Add(path, result.Item2);
                                OnTunesChange.Invoke(path, true);
                                if (TunesFileQueue.Count <= 1) OnTunesFinish.Invoke();
                            }
                        }
                    }
                    TunesFileQueue.Remove(path);
                }
            }
        }

        async UniTaskVoid CartridgesFileLoaderAsync()
        {
            while (true)
            {
                await UniTask.Yield(PlayerLoopTiming.Update);
                await UniTask.SwitchToMainThread();

                if (CartridgesFileQueue.Count > 0)
                {
                    string path = CartridgesFileQueue.First();
                    if (!Cartridges.ContainsKey(path))
                    {
                        (bool, Cartridge) result = await Serializer.LoadAsync<Cartridge>(path);
                        await UniTask.SwitchToMainThread();
                        if (CartridgesFileQueue.Contains(path))
                        {
                            if (result.Item1 && result.Item2 != null)
                            {
                                Cartridges.Add(path, result.Item2);
                                OnCartridgesChange.Invoke(path, true);
                                if (CartridgesFileQueue.Count <= 1) OnCartridgesFinish.Invoke();
                            }
                        }
                    }
                    CartridgesFileQueue.Remove(path);
                }
            }
        }

        public void NewFile()
        {
            switch (SaveState)
            {
                case SaveType.Build:
                    CustomBuildBuilder.CreateNew();
                    BuildOutline.UnloadSlot();
                    break;
                case SaveType.Part:
                    CustomPartBuilder.CreateNew();
                    PartOutline.UnloadSlot();
                    break;
                case SaveType.Tune:
                    CustomTuneBuilder.CreateNew();
                    TuneOutline.UnloadSlot();
                    break;
                case SaveType.Cartridge:
                    CustomCartridgeBuilder.CreateNew();
                    CartridgeOutline.UnloadSlot();
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
                            Serializer.Save(SaveLocations[SaveState], CustomBuildBuilder.AssembledData);
                            break;
                        case SaveType.Part:
                            Serializer.Save(SaveLocations[SaveState], CustomPartBuilder.AssembledData);
                            break;
                        case SaveType.Tune:
                            Serializer.Save(SaveLocations[SaveState], CustomTuneBuilder.AssembledData);
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
            string path = null;
            object dataToSave = null;
            switch (SaveState)
            {
                case SaveType.Build:
                    path = StandaloneFileBrowser.SaveFilePanel("Save Build", CustomContraptionPath, "CustomBuild", "part");
                    dataToSave = CustomBuildBuilder.AssembledData;
                    break;
                case SaveType.Part:
                    switch (CustomPartBuilder.AssembledData.Root.Joint)
                    {
                        case PartJoint.JointType.BD:
                            path = StandaloneFileBrowser.SaveFilePanel("Save Part", CustomBDPath, "CustomBD", "part");
                            break;
                        case PartJoint.JointType.LG:
                            path = StandaloneFileBrowser.SaveFilePanel("Save Part", CustomLGPath, "CustomLG", "part");
                            break;
                        case PartJoint.JointType.HD:
                            path = StandaloneFileBrowser.SaveFilePanel("Save Part", CustomHDPath, "CustomHD", "part");
                            break;
                        case PartJoint.JointType.HAC:
                            path = StandaloneFileBrowser.SaveFilePanel("Save Part", CustomHACPath, "CustomHAC", "part");
                            break;
                        case PartJoint.JointType.FAC:
                            path = StandaloneFileBrowser.SaveFilePanel("Save Part", CustomFACPath, "CustomFAC", "part");
                            break;
                        case PartJoint.JointType.AM:
                            path = StandaloneFileBrowser.SaveFilePanel("Save Part", CustomAMPath, "CustomAM", "part");
                            break;
                        case PartJoint.JointType.BS:
                            path = StandaloneFileBrowser.SaveFilePanel("Save Part", CustomBSPath, "CustomBS", "part");
                            break;
                        case PartJoint.JointType.WP:
                            path = StandaloneFileBrowser.SaveFilePanel("Save Part", CustomWPPath, "CustomWP", "part");
                            break;
                        case PartJoint.JointType.WB:
                            path = StandaloneFileBrowser.SaveFilePanel("Save Part", CustomWBPath, "CustomWB", "part");
                            break;
                        default:
                            break;
                    }
                    dataToSave = CustomPartBuilder.AssembledData;
                    break;
                case SaveType.Tune:
                    path = StandaloneFileBrowser.SaveFilePanel("Save Tune", CustomTunePath, "CustomTune", "tune");
                    dataToSave = CustomTuneBuilder.AssembledData;
                    break;
                case SaveType.Cartridge:
                    path = StandaloneFileBrowser.SaveFilePanel("Save Cartridge", CustomCartridgePath, "CustomCartridge", "cartridge");
                    dataToSave = CustomCartridgeBuilder.AssembledData;
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(path) && dataToSave != null)
            {
                SaveLocations[SaveState] = path;
                Serializer.Save(SaveLocations[SaveState], dataToSave);
                FileNameInputField.text = Path.GetFileNameWithoutExtension(SaveLocations[SaveState]);
            }
        }

        public void Load()
        {
            string[] path = new string[0];
            bool successful = false;
            switch (SaveState)
            {
                case SaveType.Build:
                    path = StandaloneFileBrowser.OpenFilePanel("Open Build", CustomContraptionPath, "part", false);
                    if (path.Length != 0)
                    {
                        if (Serializer.Load(path[0], out Contraption result) && result != null)
                        {
                            CustomBuildBuilder.Load(result);
                            successful = true;
                        }
                    }
                    break;
                case SaveType.Part:
                    path = StandaloneFileBrowser.OpenFilePanel("Open Part", CustomPartPath, "part", false);
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
                    path = StandaloneFileBrowser.OpenFilePanel("Open Tune", CustomTunePath, "tune", false);
                    if (path.Length != 0)
                    {
                        if (Serializer.Load(path[0], out Tune result) && result != null)
                        {
                            CustomTuneBuilder.Load(result);
                            successful = true;
                        }
                    }
                    break;
                case SaveType.Cartridge:
                    path = StandaloneFileBrowser.OpenFilePanel("Open Cartridge", CustomCartridgePath, "cartridge", false);
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

        public void LoadTunePalette()
        {
            if (Serializer.Load(TunePalettePath, out TunePalette result) && result != null) TuneColorPalette = result;
        }

        public void SaveTunePalette()
        {
            Serializer.Save(TunePalettePath, TuneColorPalette);
        }

        public void LoadOptions()
        {
            if (Serializer.Load(OptionsPath, out Options result) && result != null) Settings = result;
        }

        public void SaveOptions()
        {
            Serializer.Save(OptionsPath, Settings);
        }

        public void LoadShortcuts()
        {
            if (Serializer.Load(ShortcutsPath, out Shortcuts result) && result != null) Controls = result;
        }

        public void SaveShortcuts()
        {
            Serializer.Save(ShortcutsPath, Controls);
        }

        public void LoadUrlLibrary()
        {
            if (Serializer.Load(VideoUrlLibraryPath, out VideoUrlLibrary result) && result != null) UrlLibrary = result;
        }

        public void SaveUrlLibrary()
        {
            Serializer.Save(VideoUrlLibraryPath, UrlLibrary);
        }
        #endregion

        #region Utils
        public void SetListeners()
        {
            BuildWatcher.OnFileChanged += (path, root) => OnBuildFileChanged(path, root).Forget();
            BuildWatcher.OnDeleted += (path, root) => OnBuildDeleted(path, root).Forget();
            PartWatcher.OnFileChanged += (path, root) => OnPartFileChanged(path, root).Forget();
            PartWatcher.OnDeleted += (path, root) => OnPartDeleted(path, root).Forget();
            TuneWatcher.OnFileChanged += (path, root) => OnTuneFileChanged(path, root).Forget();
            TuneWatcher.OnDeleted += (path, root) => OnTuneDeleted(path, root).Forget();
            CartridgeWatcher.OnFileChanged += (path, root) => OnCartridgeFileChanged(path, root).Forget();
            CartridgeWatcher.OnDeleted += (path, root) => OnCartridgeDeleted(path, root).Forget();
        }

        public void UnsetListeners()
        {
            BuildWatcher.OnFileChanged -= (path, root) => OnBuildFileChanged(path, root).Forget();
            BuildWatcher.OnDeleted -= (path, root) => OnBuildDeleted(path, root).Forget();
            PartWatcher.OnFileChanged -= (path, root) => OnPartFileChanged(path, root).Forget();
            PartWatcher.OnDeleted -= (path, root) => OnPartDeleted(path, root).Forget();
            TuneWatcher.OnFileChanged -= (path, root) => OnTuneFileChanged(path, root).Forget();
            TuneWatcher.OnDeleted -= (path, root) => OnTuneDeleted(path, root).Forget();
            CartridgeWatcher.OnFileChanged -= (path, root) => OnCartridgeFileChanged(path, root).Forget();
            CartridgeWatcher.OnDeleted -= (path, root) => OnCartridgeDeleted(path, root).Forget();
        }

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
