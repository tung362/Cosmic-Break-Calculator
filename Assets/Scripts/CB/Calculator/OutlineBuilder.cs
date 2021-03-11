using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using CB.Utils;

namespace CB.Calculator
{
    /// <summary>
    /// Handles creation, editing and loading of the outline hierarchy
    /// </summary>
    public class OutlineBuilder : MonoBehaviour
    {
        public RectTransform Content;
        public RectTransform Template;
        public FolderItemSlot FolderItemTemplate;
        public FileItemSlot FileItemTemplate;
        public Vector2 SlotOffset = new Vector2(0, -22.35f);
        public Vector2 ChildLineOffset = new Vector2(-2, 22.35f);
        public float ScrollBarOffset = 7.0f;

        /*Slot to edit*/
        public FileItemSlot SelectedSlot;

        /*Cache*/
        private Dictionary<string, IDirectoryFile> Items = new Dictionary<string, IDirectoryFile>();
        private List<DirectoryWatcher> Watchers = new List<DirectoryWatcher>();

        void Start()
        {

        }

        void OnDestroy()
        {
            UnsetListeners();
        }

        #region Listeners
        void OnDirectoryChanged(string path, string root)
        {
            if (path == root)
            {
                //Create root folders
                string alternativeName = null;
                if (Path.GetFullPath(Calculator.PartPath) == root ||
                    Path.GetFullPath(Calculator.TunePath) == root ||
                    Path.GetFullPath(Calculator.CartridgePath) == root)
                {
                    alternativeName = "Data Bundle";
                }
                if (Path.GetFullPath(Calculator.CustomContraptionPath) == root ||
                    Path.GetFullPath(Calculator.CustomPartPath) == root ||
                    Path.GetFullPath(Calculator.CustomTunePath) == root ||
                    Path.GetFullPath(Calculator.CustomCartridgePath) == root)
                {
                    alternativeName = "Custom Data Bundle";
                }
                CreateFolder(Template.GetComponent<FolderItemSlot>(), path, alternativeName);
                RecalculateContent(Template);
            }
        }

        void OnFileChanged(string path, string root)
        {
            if (!Items.ContainsKey(path))
            {
                //Create folders and files
                CreateDirectory(path, new List<string>(), -1);
                CreateFile((FolderItemSlot)Items[Directory.GetParent(path).FullName], path);
                RecalculateContent(Template);
            }
        }

        void OnDeleted(string path, string root)
        {
            if (Items.ContainsKey(path))
            {
                if (Items[path].GetType() == typeof(FolderItemSlot)) RemoveFolder(path);
                else RemoveFile(path);
                RecalculateContent(Template);
            }
        }
        #endregion

        #region Creation And Removal
        public void AddBranch(FolderItemSlot itemSlot, int count, int startIndex, bool skipFirstPlacement = false)
        {
            if(!skipFirstPlacement)
            {
                //Update positions
                for (int i = startIndex; i < itemSlot.SubItems.Count; i++)
                {
                    RectTransform childTransform = ((Component)itemSlot.SubItems[i]).GetComponent<RectTransform>();
                    childTransform.anchoredPosition = new Vector2(childTransform.anchoredPosition.x, childTransform.anchoredPosition.y + (SlotOffset.y * count));
                }
            }
            if(itemSlot.Show)
            {
                //Update branch count
                itemSlot.BranchCount += count;
                //Update child line
                if (itemSlot.ChildLine) itemSlot.ChildLine.sizeDelta = new Vector2(itemSlot.ChildLine.sizeDelta.x, ChildLineOffset.y * (itemSlot.BranchCount - 1));
                //Recursive branching
                if (itemSlot.Parent != null) AddBranch((FolderItemSlot)itemSlot.Parent, count, itemSlot.BranchIndex + 1);
            }
        }

        public void RemoveBranch(FolderItemSlot itemSlot, int count, int startIndex, bool skipFirstPlacement = false)
        {
            if(!skipFirstPlacement)
            {
                //Update positions
                for (int i = startIndex; i < itemSlot.SubItems.Count; i++)
                {
                    RectTransform childTransform = ((Component)itemSlot.SubItems[i]).GetComponent<RectTransform>();
                    childTransform.anchoredPosition = new Vector2(childTransform.anchoredPosition.x, childTransform.anchoredPosition.y - (SlotOffset.y * count));
                    //Update index
                    itemSlot.SubItems[i].BranchIndex = i;
                }
            }
            if (itemSlot.Show)
            {
                //Update branch count
                itemSlot.BranchCount -= count;
                //Update child line
                if (itemSlot.ChildLine) itemSlot.ChildLine.sizeDelta = new Vector2(itemSlot.ChildLine.sizeDelta.x, ChildLineOffset.y * (itemSlot.BranchCount - 1));
                //Recursive branching
                if (itemSlot.Parent != null) RemoveBranch((FolderItemSlot)itemSlot.Parent, count, itemSlot.BranchIndex + 1);
            }
        }

        public void UnloadSlot()
        {
            if (SelectedSlot) SelectedSlot.SelectButton.interactable = true;
            SelectedSlot = null;
        }

        public void LoadSlot<T>(FileItemSlot fileItemSlot, IBuilder<T> builder)
        {
            if (Serializer.Load(fileItemSlot.Path, out T result) && result != null)
            {
                if (SelectedSlot) SelectedSlot.SelectButton.interactable = true;
                SelectedSlot = fileItemSlot;
                SelectedSlot.SelectButton.interactable = false;

                builder.Load(result);
                Calculator.instance.SaveLocations[Calculator.instance.SaveState] = SelectedSlot.Path;
                Calculator.instance.FileNameInputField.text = Path.GetFileNameWithoutExtension(Calculator.instance.SaveLocations[Calculator.instance.SaveState]);
            }
        }

        public void CreateFolder(FolderItemSlot folderItemSlot, string path, string alternativeName = null)
        {
            if (!Items.ContainsKey(path))
            {
                FolderItemSlot folderSlotChild = Instantiate(FolderItemTemplate, folderItemSlot.FileSlotOrigin);
                folderSlotChild.gameObject.SetActive(true);
                folderSlotChild.Path = path;
                folderSlotChild.NameText.text = alternativeName != null ? alternativeName : new DirectoryInfo(folderSlotChild.Path).Name;
                folderSlotChild.Parent = folderItemSlot;
                folderSlotChild.BranchIndex = folderItemSlot.SubItems.Count;
                folderSlotChild.GetComponent<RectTransform>().anchoredPosition = new Vector2(SlotOffset.x, SlotOffset.y * (folderItemSlot.BranchCount - 1));
                if (folderItemSlot.Show)
                {
                    folderItemSlot.BranchCount += 1;
                    if (folderItemSlot.ChildLine) folderItemSlot.ChildLine.sizeDelta = new Vector2(folderItemSlot.ChildLine.sizeDelta.x, ChildLineOffset.y * (folderItemSlot.BranchCount - 1));
                    if (folderItemSlot.Parent != null) AddBranch((FolderItemSlot)folderItemSlot.Parent, 1, folderItemSlot.BranchIndex + 1);
                }
                else
                {
                    int branchCount = 0;
                    for(int i = 0; i < folderItemSlot.SubItems.Count; i++) branchCount += folderItemSlot.SubItems[i].BranchCount;
                    folderSlotChild.GetComponent<RectTransform>().anchoredPosition = new Vector2(SlotOffset.x, SlotOffset.y * branchCount);
                }
                folderItemSlot.SubItems.Add(folderSlotChild);
                Items.Add(path, folderSlotChild);
            }
        }

        public void CreateFile(FolderItemSlot folderItemSlot, string path)
        {
            if (!Items.ContainsKey(path))
            {
                FileItemSlot fileSlotChild = Instantiate(FileItemTemplate, folderItemSlot.FileSlotOrigin);
                fileSlotChild.gameObject.SetActive(true);
                fileSlotChild.Path = path;
                fileSlotChild.NameText.text = Path.GetFileNameWithoutExtension(fileSlotChild.Path);
                fileSlotChild.Parent = folderItemSlot;
                fileSlotChild.BranchIndex = folderItemSlot.SubItems.Count;
                fileSlotChild.GetComponent<RectTransform>().anchoredPosition = new Vector2(SlotOffset.x, SlotOffset.y * (folderItemSlot.BranchCount - 1));
                if (folderItemSlot.Show)
                {
                    folderItemSlot.BranchCount += 1;
                    if (folderItemSlot.ChildLine) folderItemSlot.ChildLine.sizeDelta = new Vector2(folderItemSlot.ChildLine.sizeDelta.x, ChildLineOffset.y * (folderItemSlot.BranchCount - 1));
                    if (folderItemSlot.Parent != null) AddBranch((FolderItemSlot)folderItemSlot.Parent, 1, folderItemSlot.BranchIndex + 1);
                }
                else
                {
                    int branchCount = 0;
                    for (int i = 0; i < folderItemSlot.SubItems.Count; i++) branchCount += folderItemSlot.SubItems[i].BranchCount;
                    fileSlotChild.GetComponent<RectTransform>().anchoredPosition = new Vector2(SlotOffset.x, SlotOffset.y * branchCount);
                }
                folderItemSlot.SubItems.Add(fileSlotChild);
                Items.Add(path, fileSlotChild);
            }
        }

        public void RemoveFolder(string path)
        {
            if(Items.ContainsKey(path))
            {
                FolderItemSlot folderSlot = (FolderItemSlot)Items[path];
                if (folderSlot.Parent != null)
                {
                    for (int i = 0; i < folderSlot.SubItems.Count; i++)
                    {
                        //Recursive branching
                        if (folderSlot.SubItems[i].GetType() == typeof(FolderItemSlot))
                        {
                            RemoveFolder(folderSlot.SubItems[i].Path);
                            i--;
                        }
                        else Items.Remove(folderSlot.SubItems[i].Path);
                    }
                    FolderItemSlot parentSlot = (FolderItemSlot)folderSlot.Parent;
                    parentSlot.SubItems.Remove(folderSlot);
                    if (parentSlot.Show) RemoveBranch(parentSlot, folderSlot.BranchCount, folderSlot.BranchIndex);
                    else
                    {
                        for (int i = folderSlot.BranchIndex; i < parentSlot.SubItems.Count; i++)
                        {
                            RectTransform childTransform = ((Component)parentSlot.SubItems[i]).GetComponent<RectTransform>();
                            childTransform.anchoredPosition = new Vector2(childTransform.anchoredPosition.x, childTransform.anchoredPosition.y - (SlotOffset.y * folderSlot.BranchCount));
                            parentSlot.SubItems[i].BranchIndex = i;
                        }
                    }
                    Destroy(folderSlot.gameObject);
                    Items.Remove(path);
                }
            }
        }

        public void RemoveFile(string path)
        {
            if (Items.ContainsKey(path))
            {
                FileItemSlot fileSlot = (FileItemSlot)Items[path];
                if (fileSlot.Parent != null)
                {
                    FolderItemSlot parentSlot = (FolderItemSlot)fileSlot.Parent;
                    parentSlot.SubItems.Remove(fileSlot);
                    if (parentSlot.Show) RemoveBranch(parentSlot, fileSlot.BranchCount, fileSlot.BranchIndex);
                    else
                    {
                        for (int i = fileSlot.BranchIndex; i < parentSlot.SubItems.Count; i++)
                        {
                            RectTransform childTransform = ((Component)parentSlot.SubItems[i]).GetComponent<RectTransform>();
                            childTransform.anchoredPosition = new Vector2(childTransform.anchoredPosition.x, childTransform.anchoredPosition.y - (SlotOffset.y * fileSlot.BranchCount));
                            parentSlot.SubItems[i].BranchIndex = i;
                        }
                    }
                    Destroy(fileSlot.gameObject);
                    Items.Remove(path);
                }
            }
        }

        public void Show(string path)
        {
            if (Items.ContainsKey(path))
            {
                FolderItemSlot folderSlot = (FolderItemSlot)Items[path];
                folderSlot.BranchCount = 1;
                for (int i = 0; i < folderSlot.SubItems.Count; i++) AddBranch(folderSlot, folderSlot.SubItems[i].BranchCount, folderSlot.SubItems[i].BranchIndex + 1, true);
            }
        }

        public void Hide(string path)
        {
            if (Items.ContainsKey(path))
            {
                FolderItemSlot folderSlot = (FolderItemSlot)Items[path];
                RemoveBranch((FolderItemSlot)folderSlot.Parent, folderSlot.BranchCount - 1, folderSlot.BranchIndex + 1);
                folderSlot.BranchCount = 1;
            }
        }

        public void RecalculateContent(RectTransform slot)
        {
            Bounds combinedBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(Content.transform, slot);
            Content.sizeDelta = new Vector2(Mathf.Abs(combinedBounds.max.x) + ScrollBarOffset, Mathf.Abs(combinedBounds.min.y) + ScrollBarOffset);
        }
        #endregion

        #region Utils
        public void SetListeners(DirectoryWatcher watcher)
        {
            watcher.OnDirectoryChanged += OnDirectoryChanged;
            watcher.OnFileChanged += OnFileChanged;
            watcher.OnDeleted += OnDeleted;
            Watchers.Add(watcher);
        }

        public void UnsetListeners()
        {
            for(int i = 0; i < Watchers.Count; i++)
            {
                Watchers[i].OnDirectoryChanged -= OnDirectoryChanged;
                Watchers[i].OnFileChanged -= OnFileChanged;
                Watchers[i].OnDeleted -= OnDeleted;
            }
        }

        public void CreateDirectory(string path, List<string> previousPaths, int currentIndex)
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
                        CreateFolder((FolderItemSlot)Items[parentFolder.FullName], path);
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
        #endregion
    }
}
