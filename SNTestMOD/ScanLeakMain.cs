using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SNTestMOD
{
    public class ScanLeakMain : MonoBehaviour
    {
        LeakingRadiation main;

        public void Start()
        {
            main = LeakingRadiation.main;
            InvokeRepeating("DebugLeakingRadiation", 0, 3);
            //main.damagePlayerInRadius.doDebug = true;
        }

        private void DebugLeakingRadiation()
        {
            /*
            print($"currentRadius: {main.currentRadius}");            
            print($"kGrowRate: {main.kGrowRate}");
            print($"kMaxRadius: {main.kMaxRadius}");
            print($"kNaturalDissipation: {main.kNaturalDissipation}");
            print($"kStartRadius: {main.kStartRadius}");
            */
            print($"Player.main.radiationAmount: {Player.main.radiationAmount}");
        }
    }
}
