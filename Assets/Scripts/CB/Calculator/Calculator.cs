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
        public static string Version = "v. 1.0.0";

        /*Save Locations*/
        public static string RootPath { get { return Application.dataPath + "/../"; } }
        public static string VideoUrlLibraryPath { get { return RootPath + "/Config/VideoUrlLibrary.preset"; } }

        /*Fixed Databases*/
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

        /*Global Variables*/
        public bool ShowFixedParts = false;

        void OnEnable()
        {
            if (!instance) instance = this;
            else Debug.Log("Warning! Multiple instances of \"Calculator\"");
        }

        #region Utils
        public void ToggleFixedParts(bool toggle)
        {
            ShowFixedParts = toggle;
        }

        public void ExitApplication()
        {
            Application.Quit();
        }
        #endregion
    }
}
