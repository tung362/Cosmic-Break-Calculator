using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB
{
    public interface IBuilder<T>
    {
        void CreateNew();
        void Load(T loadData);
    }
}
