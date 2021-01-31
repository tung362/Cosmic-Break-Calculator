using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace CB.Calculator
{
    /// <summary>
    /// Handles creation, editing and loading of the outline hierarchy
    /// </summary>
    public class OutlineBuilder : MonoBehaviour
    {
        public RectTransform Content;
        public FolderItemSlot FolderItemTemplate;
        public FileItemSlot FileItemTemplate;
        public Vector2 SlotOffset = new Vector2(0, -22.35f);
        public float ScrollBarOffset = 7.0f;

        /*Slot to edit*/
        public Button SelectedSlot;

        /*Cache*/
        public Dictionary<string, IDirectoryFile> Items = new Dictionary<string, IDirectoryFile>();

        void Start()
        {

        }

        void OnDestroy()
        {
            UnsetListeners();
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                string path = @"C:\Users\Tung Nguyen\Desktop\Dev\VisualStudios\Cosmic Break Calculator\DataBundleCustom\Cartridge\NewF\a.cartridge";
                //Debug.Log(Directory.GetParent(path).FullName + Path.DirectorySeparatorChar);
                //Debug.Log(path);
                CreateDirectory(path, new List<string>(), -1);
                Debug.Log("BOOP");
                Debug.Log(Items[@"C:\Users\Tung Nguyen\Desktop\Dev\VisualStudios\Cosmic Break Calculator\DataBundleCustom\Cartridge\"]);
            }
        }

        #region Listeners
        void OnDirectoryChanged(string path, string root)
        {
            if (path == root)
            {
                //Create root folders
                CreateFolder(Content.GetComponent<FolderItemSlot>(), path);
                RecalculateContent(Content);
            }
        }

        void OnFileChanged(string path, string root)
        {
            if (!Items.ContainsKey(path))
            {
                //Debug.Log(path + " | " + root);
                //Create folders and files
                //Debug.Log("BOOP");
                //Debug.Log(Items[@"C:\Users\Tung Nguyen\Desktop\Dev\VisualStudios\Cosmic Break Calculator\DataBundleCustom\Cartridge\"]);
                CreateDirectory(path, new List<string>(), -1);
                CreateFile((FolderItemSlot)Items[Directory.GetParent(path).FullName + Path.DirectorySeparatorChar], path);
                RecalculateContent(Content);
            }
        }

        public string UKA(string path)
        {
            return Items[path].ToString();
        }

        void OnDeleted(string path, string root)
        {
            //if(Items.ContainsKey(path))
            //{
            //    if (Items[path].GetType() == typeof(FolderItemSlot)) RemoveFolder(path);
            //    if (Items[path].GetType() == typeof(FileItemSlot)) RemoveFile(path);
            //    Items.Remove(path);
            //    RecalculateContent(Content);
            //}
        }
        #endregion

        #region Creation And Removal
        void AddBranch(FolderItemSlot itemSlot, int startIndex)
        {
            //Update branch count
            itemSlot.BranchCount += 1;
            //Update positions
            for (int i = startIndex; i < itemSlot.SubItems.Count; i++)
            {
                RectTransform childTransform = ((Component)itemSlot.SubItems[i]).GetComponent<RectTransform>();
                childTransform.anchoredPosition = new Vector2(childTransform.anchoredPosition.x, childTransform.anchoredPosition.y + SlotOffset.y);
            }
            //Recursive branching
            if (itemSlot.Parent != null) AddBranch((FolderItemSlot)itemSlot.Parent, itemSlot.BranchIndex + 1);
        }

        public void LoadSlot(Button slot)
        {
            if (SelectedSlot) SelectedSlot.interactable = true;
             SelectedSlot = slot;
        }

        void CreateFolder(FolderItemSlot folderItemSlot, string path)
        {
            if (!Items.ContainsKey(path))
            {
                FolderItemSlot folderSlotChild = Instantiate(FolderItemTemplate, folderItemSlot.FileSlotOrigin); //Dead end
                folderSlotChild.gameObject.SetActive(true);
                folderSlotChild.Path = path;
                folderSlotChild.NameText.text = new DirectoryInfo(folderSlotChild.Path).Name;
                folderSlotChild.Parent = folderItemSlot;
                folderSlotChild.BranchIndex = folderItemSlot.SubItems.Count;
                folderSlotChild.GetComponent<RectTransform>().anchoredPosition = new Vector2(SlotOffset.x, SlotOffset.y * (folderItemSlot.BranchCount - 1));
                folderItemSlot.SubItems.Add(folderSlotChild);
                Items.Add(path, folderSlotChild);
                folderItemSlot.BranchCount += 1;
                if (folderItemSlot.Parent != null) AddBranch((FolderItemSlot)folderItemSlot.Parent, folderItemSlot.BranchIndex + 1);
            }
        }

        void CreateFile(FolderItemSlot folderItemSlot, string path)
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
                folderItemSlot.SubItems.Add(fileSlotChild);
                Items.Add(path, fileSlotChild);
                folderItemSlot.BranchCount += 1;
                if (folderItemSlot.Parent != null) AddBranch((FolderItemSlot)folderItemSlot.Parent, folderItemSlot.BranchIndex + 1);
            }
        }

        void RemoveFolder(string path)
        {
            Items.Remove(path);
        }

        void RemoveFile(string path)
        {
            Items.Remove(path);
        }

        public void RecalculateContent(RectTransform slot)
        {
            Bounds combinedBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(Content.transform, slot);
            Content.sizeDelta = new Vector2(Mathf.Abs(combinedBounds.max.x) + ScrollBarOffset, Mathf.Abs(combinedBounds.min.y) + ScrollBarOffset);
        }
        #endregion

        #region Editing
        #endregion

        #region Utils
        public void SetListeners()
        {
            Calculator.instance.CartridgeWatcher.OnDirectoryChanged += OnDirectoryChanged;
            Calculator.instance.CartridgeWatcher.OnFileChanged += OnFileChanged;
            Calculator.instance.CartridgeWatcher.OnDeleted += OnDeleted;
        }

        public void UnsetListeners()
        {
            Calculator.instance.CartridgeWatcher.OnDirectoryChanged -= OnDirectoryChanged;
            Calculator.instance.CartridgeWatcher.OnFileChanged -= OnFileChanged;
            Calculator.instance.CartridgeWatcher.OnDeleted -= OnDeleted;
        }

        public void CreateDirectory(string path, List<string> previousPaths, int currentIndex)
        {
            //If parent folder exists
            DirectoryInfo parentFolder = Directory.GetParent(path);
            if (Items.ContainsKey(parentFolder.FullName + Path.DirectorySeparatorChar))
            {
                if (currentIndex >= 0)
                {
                    //If folder doesn't exist
                    if (!Items.ContainsKey(path + Path.DirectorySeparatorChar))
                    {
                        CreateFolder((FolderItemSlot)Items[parentFolder.FullName + Path.DirectorySeparatorChar], path + Path.DirectorySeparatorChar);
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
