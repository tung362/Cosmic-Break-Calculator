using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB
{
    public interface IDirectoryFile
    {
        string Path { get; set; }
        IDirectoryFile Parent { get; set; }
        int BranchCount { get; set; }
        int BranchIndex { get; set; }
    }
}
