using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CB.Calculator
{
    /// <summary>
    /// Stats for parts
    /// </summary>
    //[System.Serializable]
    public class Stats
    {
        /*Configuration*/
        public int COST = 0;
        public int CAPA = 0;
        public int HP = 0;
        public int STR = 0;
        public int TEC = 0;
        public int WLK = 0;
        public int FLY = 0;
        public int TGH = 0;

        public Stats()
        {
        }

        public Stats(int cost, int capa, int hp, int str, int tec, int wlk, int fly, int tgh)
        {
            COST = cost;
            CAPA = capa;
            HP = hp;
            STR = str;
            TEC = tec;
            WLK = wlk;
            FLY = fly;
            TGH = tgh;
        }

        public static Stats operator +(Stats s1, Stats s2)
        {
            return new Stats(s1.COST + s2.COST, s1.CAPA + s2.CAPA, s1.HP + s2.HP, s1.STR + s2.STR, s1.TEC + s2.TEC, s1.WLK + s2.WLK, s1.FLY + s2.FLY, s1.TGH + s2.TGH);
        }

        public static Stats operator -(Stats s1, Stats s2)
        {
            return new Stats(s1.COST - s2.COST, s1.CAPA - s2.CAPA, s1.HP - s2.HP, s1.STR - s2.STR, s1.TEC - s2.TEC, s1.WLK - s2.WLK, s1.FLY - s2.FLY, s1.TGH - s2.TGH);
        }
    }
}
