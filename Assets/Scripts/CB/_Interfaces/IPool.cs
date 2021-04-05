using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB
{
    public interface IPool<T>
    {
        void SetActive(bool toggle);
        void UpdateItem(T path);
        void Destroy();
    }
}
