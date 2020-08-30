using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CB.Calculator.Database;

namespace CB.Calculator
{
    /// <summary>
    /// Calculator manager, keeps track of all databases and cached values used by the calculator
    /// </summary>
    public class Calculator : MonoBehaviour
    {
        public static Calculator instance { get; private set; }

        /*Version*/
        public const string Version = "v. 1.0.0";

        /*Fixed Databases*/
        public JointIconDatabase JointIcons;

        /*Dynamic Databases*/
        public List<Part> BDs = new List<Part>();
        public List<Part> LGs = new List<Part>();
        public List<Part> HDs = new List<Part>();
        public List<Part> HACs = new List<Part>();
        public List<Part> FACs = new List<Part>();
        public List<Part> AMs = new List<Part>();
        public List<Part> BSs = new List<Part>();
        public List<Part> WPs = new List<Part>();
        public List<Part> WBs = new List<Part>();

        void OnEnable()
        {
            if (!instance) instance = this;
            else Debug.Log("Warning! Multiple instances of \"Calculator\"");
        }
    }
}
