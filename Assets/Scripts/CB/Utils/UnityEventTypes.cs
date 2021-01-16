using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CB.Utils
{
    [System.Serializable]
    public class BitMaskEvent : UnityEvent<Bitmask> { }

    [System.Serializable]
    public class ColorEvent : UnityEvent<Color> { }

    [System.Serializable]
    public class Vector2Event : UnityEvent<Vector2> { }

    [System.Serializable]
    public class Vector3Event : UnityEvent<Vector3> { }

    [System.Serializable]
    public class Vector4Event : UnityEvent<Vector4> { }

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    [System.Serializable]
    public class IntEvent : UnityEvent<int> { }

    [System.Serializable]
    public class FloatEvent : UnityEvent<float> { }

    [System.Serializable]
    public class DoubleEvent : UnityEvent<double> { }

    [System.Serializable]
    public class DecimalEvent : UnityEvent<decimal> { }

    [System.Serializable]
    public class LongEvent : UnityEvent<long> { }

    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }
}
