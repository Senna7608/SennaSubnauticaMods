using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using QModManager.API.ModLoading;
using APIBasedExosuitArms.Craftables;

namespace APIBasedExosuitArms
{
    [QModCore]
    public static class Main
    {
        public static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [QModPatch]
        public static void Load()
        {
            try
            {
                new APIBasedClawArm().Patch();
                new APIBasedDrillArm().Patch();
                new APIBasedPropulsionArm().Patch();
                new APIBasedGrapplingArm().Patch();
                new APIBasedTorpedoArm().Patch();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
