using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using CB.Utils;
using CB.Calculator.Utils;

namespace CB.Calculator
{
    /// <summary>
    /// Handles creation, editing and loading of the outline hierarchy
    /// </summary>
    public class OutlineBuilder : MonoBehaviour
    {
        public RectTransform Template;
        public RectTransform Content;
        public RectTransform Container;
        public OutlinePoolItem PoolTemplate;
        public Calculator.SaveType SaveState = Calculator.SaveType.None;
        public Vector2 SlotOffset = new Vector2(11.0f, 22.35f);
        public float ScrollBarOffset = 7.0f;

        /*Slot to edit*/
        public string SelectedSlot = "";

        /*Cache*/
        private List<IPool<string>> Pool = new List<IPool<string>>();
        private Dictionary<string, (bool, bool, List<string>, int)> Items = new Dictionary<string, (bool, bool, List<string>, int)>(); //isFolder, flagged, items, depth
        private (HashSet<string>, List<string>) SortedItems = (new HashSet<string>(), new List<string>());
        private int TopIndex = 0;

        public void Init()
        {
            List<string> rootPaths = new List<string>();
            switch (SaveState)
            {
                case Calculator.SaveType.Build:
                    rootPaths.Add(Path.GetFullPath(Calculator.CustomContraptionPath));
                    break;
                case Calculator.SaveType.Part:
                    rootPaths.Add(Path.GetFullPath(Calculator.PartPath));
                    rootPaths.Add(Path.GetFullPath(Calculator.CustomPartPath));
                    break;
                case Calculator.SaveType.Tune:
                    rootPaths.Add(Path.GetFullPath(Calculator.TunePath));
                    rootPaths.Add(Path.GetFullPath(Calculator.CustomTunePath));
                    break;
                case Calculator.SaveType.Cartridge:
                    rootPaths.Add(Path.GetFullPath(Calculator.CartridgePath));
                    rootPaths.Add(Path.GetFullPath(Calculator.CustomCartridgePath));
                    break;
                default:
                    break;
            }
            for(int i = 0; i < rootPaths.Count; i++)
            {
                if(!Items.ContainsKey(rootPaths[i])) Items.Add(rootPaths[i], (true, true, new List<string>(), 0));
                if (!SortedItems.Item1.Contains(rootPaths[i]))
                {
                    SortedItems.Item1.Add(rootPaths[i]);
                    SortedItems.Item2.Add(rootPaths[i]);
                }
            }
            RecalculatePool();
        }

        #region Listeners
        void Show(string path, bool toggle)
        {
            if(SortedItems.Item1.Contains(path))
            {
                if (Items.ContainsKey(path))
                {
                    Items[path] = (Items[path].Item1, toggle, Items[path].Item3, Items[path].Item4);

                    if (Items[path].Item3 != null)
                    {
                        if (toggle) AddBranch(Items[path].Item3, SortedItems.Item2.IndexOf(path));
                        else RemoveBranch(Items[path].Item3);
                    }
                    UpdatePool();
                    UpdateContent();
                }
            }
        }

        void LoadSlot<T>(string path, IBuilder<T> builder)
        {
            if (Serializer.Load(path, out T result) && result != null)
            {
                if (Items.ContainsKey(SelectedSlot)) Items[SelectedSlot] = (Items[SelectedSlot].Item1, true, Items[SelectedSlot].Item3, Items[SelectedSlot].Item4);
                if (Items.ContainsKey(path))
                {
                    Items[path] = (Items[path].Item1, false, Items[path].Item3, Items[path].Item4);
                    SelectedSlot = path;
                }
                UpdatePool();

                builder.Load(result);
                Calculator.instance.SaveLocations[Calculator.instance.SaveState] = SelectedSlot;
                Calculator.instance.FileNameInputField.text = Path.GetFileNameWithoutExtension(Calculator.instance.SaveLocations[Calculator.instance.SaveState]);
            }
        }
        #endregion

        #region Creation And Removal
        public void OnChange(string path, bool changed)
        {
            if (changed) CreateFile(path);
            else RemoveFile(path);
        }

        public void OnFinish()
        {
            UpdatePool();
            UpdateContent();
        }

        public void RecalculatePool()
        {
            int poolCount = Mathf.CeilToInt(Template.rect.height / Mathf.Abs(SlotOffset.y)) + 1;
            int difference = (poolCount < 0 ? 0 : poolCount) - Pool.Count;

            if (difference >= 0)
            {
                for (int i = 0; i < difference; i++)
                {
                    OutlinePoolItem poolItem = Instantiate(PoolTemplate, Container);
                    poolItem.gameObject.SetActive(true);
                    poolItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -SlotOffset.y * Pool.Count);
                    Pool.Add(poolItem);
                }
            }
            else
            {
                for (int i = 0; i < -difference; i++)
                {
                    int lastIndex = Pool.Count - 1;
                    Pool[lastIndex].Destroy();
                    Pool.RemoveAt(lastIndex);
                }
            }
            UpdatePool();
        }

        public void UpdateScroll()
        {
            int currentTopIndex = Mathf.FloorToInt(Content.anchoredPosition.y / SlotOffset.y);
            if (currentTopIndex < 0) currentTopIndex = 0;
            if (currentTopIndex != TopIndex)
            {
                Container.anchoredPosition = new Vector2(Container.anchoredPosition.x, -SlotOffset.y * currentTopIndex);
                TopIndex = currentTopIndex;
                UpdatePool();
            }
        }

        public void UpdatePool()
        {
            for (int i = 0; i < Pool.Count; i++)
            {
                OutlinePoolItem poolItem = (OutlinePoolItem)Pool[i];

                int index = TopIndex + i;
                if (index >= SortedItems.Item2.Count)
                {
                    poolItem.SetActive(false);
                    poolItem.FolderToggle.onValueChanged.RemoveAllListeners();
                    poolItem.FileButton.onClick.RemoveAllListeners();
                }
                else
                {
                    (bool, bool, List<string>, int) itemData = Items[SortedItems.Item2[index]];
                    poolItem.SetActive(true);
                    poolItem.FolderToggle.onValueChanged.RemoveAllListeners();
                    poolItem.FileButton.onClick.RemoveAllListeners();
                    poolItem.UpdateItem(SortedItems.Item2[index], itemData.Item1, itemData.Item2);

                    if (itemData.Item1) poolItem.FolderToggle.onValueChanged.AddListener(toggle => Show(SortedItems.Item2[index], toggle));
                    else
                    {
                        dynamic builder = null;
                        switch (SaveState)
                        {
                            case Calculator.SaveType.Build:
                                builder = Calculator.instance.CustomBuildBuilder;
                                break;
                            case Calculator.SaveType.Part:
                                builder = Calculator.instance.CustomPartBuilder;
                                break;
                            case Calculator.SaveType.Tune:
                                builder = Calculator.instance.CustomTuneBuilder;
                                break;
                            case Calculator.SaveType.Cartridge:
                                builder = Calculator.instance.CustomCartridgeBuilder;
                                break;
                            default:
                                break;
                        }
                        poolItem.FileButton.onClick.AddListener(() => LoadSlot(SortedItems.Item2[index], builder));
                    }
                    poolItem.PoolTransform.anchoredPosition = new Vector2(SlotOffset.x * itemData.Item4, poolItem.PoolTransform.anchoredPosition.y);
                }
            }
        }

        void CreateFolder(string path, int depth)
        {
            if(!Items.ContainsKey(path))
            {
                //Get parent folder
                DirectoryInfo parentFolder = Directory.GetParent(path);

                Items.Add(path, (true, true, new List<string>(), depth));

                //If parent folder is not collapsed
                if (CanShow(path))
                {
                    if (!SortedItems.Item1.Contains(path))
                    {
                        int parentIndex = SortedItems.Item2.IndexOf(parentFolder.FullName);

                        SortedItems.Item2.Insert(parentIndex + 1, path);
                        SortedItems.Item1.Add(path);
                    }
                }
                Items[parentFolder.FullName].Item3.Insert(0, path);
            }
        }

        void CreateFile(string path)
        {
            if (!Items.ContainsKey(path))
            {
                //Create folders if they don't exist
                CreateDirectory(path, new List<string>(), -1);

                //Get parent folder
                DirectoryInfo parentFolder = Directory.GetParent(path);

                //Add
                Items.Add(path, (false, true, null, Items[parentFolder.FullName].Item4 + 1));
                //If parent folders is not collapsed
                if (CanShow(path))
                {
                    if (!SortedItems.Item1.Contains(path))
                    {
                        int lastIndex = LastIndexBranch(parentFolder.FullName);
                        SortedItems.Item2.Insert(lastIndex + 1, path);
                        SortedItems.Item1.Add(path);
                    }
                }
                Items[parentFolder.FullName].Item3.Add(path);
            }
        }

        void RemoveFolder(string path)
        {
            //Get parent folder
            DirectoryInfo parentFolder = Directory.GetParent(path);

            //If is not root
            if(Items.ContainsKey(parentFolder.FullName))
            {
                if (Items.ContainsKey(path))
                {
                    //Remove
                    if (SortedItems.Item1.Contains(path))
                    {
                        SortedItems.Item2.Remove(path);
                        SortedItems.Item1.Remove(path);
                    }
                    Items[parentFolder.FullName].Item3.Remove(path);
                    Items.Remove(path);

                    //Remove folder if empty
                    if (Items[parentFolder.FullName].Item3.Count <= 0) RemoveFolder(parentFolder.FullName);
                }
            }
        }

        void RemoveFile(string path)
        {
            if (Items.ContainsKey(path))
            {
                //Get parent folder
                DirectoryInfo parentFolder = Directory.GetParent(path);

                //Remove
                if (SortedItems.Item1.Contains(path))
                {
                    SortedItems.Item2.Remove(path);
                    SortedItems.Item1.Remove(path);
                }
                Items[parentFolder.FullName].Item3.Remove(path);
                Items.Remove(path);

                //Remove folder if empty
                if (Items[parentFolder.FullName].Item3.Count <= 0) RemoveFolder(parentFolder.FullName);

                //Update
                UpdatePool();
                UpdateContent();
            }
        }

        public void UnloadSlot()
        {
            if (Items.ContainsKey(SelectedSlot)) Items[SelectedSlot] = (Items[SelectedSlot].Item1, true, Items[SelectedSlot].Item3, Items[SelectedSlot].Item4);
            SelectedSlot = "";
            UpdatePool();
        }

        public void UpdateContent()
        {
            Bounds combinedBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(Content.transform, Container);
            Content.sizeDelta = new Vector2(Mathf.Abs(combinedBounds.max.x) + ScrollBarOffset, (SlotOffset.y * SortedItems.Item2.Count) + ScrollBarOffset);
        }
        #endregion

        #region Utils
        void CreateDirectory(string path, List<string> previousPaths, int currentIndex)
        {
            //If parent folder exists
            DirectoryInfo parentFolder = Directory.GetParent(path);
            if (Items.ContainsKey(parentFolder.FullName))
            {
                if (currentIndex >= 0)
                {
                    //If folder doesn't exist
                    if (!Items.ContainsKey(path))
                    {
                        CreateFolder(path, Items[parentFolder.FullName].Item4 + 1);
                        //Recursive branching forward
                        if (currentIndex >= 0) CreateDirectory(previousPaths[currentIndex], previousPaths, currentIndex - 1);
                    }
                }
                return;
            }
            //Recursive branching backwards
            previousPaths.Add(path);
            CreateDirectory(parentFolder.FullName, previousPaths, currentIndex + 1);
        }

        bool CanShow(string path)
        {
            DirectoryInfo parentFolder = Directory.GetParent(path);
            if (Items.ContainsKey(parentFolder.FullName))
            {
                //Collapsed
                if (!Items[path].Item2) return false;
                else return CanShow(parentFolder.FullName);
            }
            //Root
            else
            {
                //Collapsed
                if (!Items[path].Item2) return false;
                return true;
            }
        }

        int LastIndexBranch(string folderItem)
        {
            if (Items[folderItem].Item2 && Items[folderItem].Item3 != null)
            {
                if (Items[folderItem].Item3.Count > 0)
                {
                    return LastIndexBranch(Items[folderItem].Item3[Items[folderItem].Item3.Count - 1]);
                }
            }
            return SortedItems.Item2.IndexOf(folderItem);
        }

        void AddBranch(List<string> items, int index)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                SortedItems.Item1.Add(items[i]);
                SortedItems.Item2.Insert(index + 1, items[i]);

                if (Items.ContainsKey(items[i]))
                {
                    //Recursive branching
                    if (Items[items[i]].Item3 != null && Items[items[i]].Item2) AddBranch(Items[items[i]].Item3, SortedItems.Item2.IndexOf(items[i]));
                }
            }
        }

        void RemoveBranch(List<string> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (SortedItems.Item1.Contains(items[i]))
                {
                    //Recursive branching
                    if (Items.ContainsKey(items[i]))
                    {
                        if(Items[items[i]].Item3 != null) RemoveBranch(Items[items[i]].Item3);
                    }
                    SortedItems.Item1.Remove(items[i]);
                    SortedItems.Item2.Remove(items[i]);
                }
            }
        }
        #endregion
    }
}
