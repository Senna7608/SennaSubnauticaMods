using Harmony;

namespace CyclopsLaserCannonModule.Patch
{
    [HarmonyPatch(typeof(PDAScanner))]
    [HarmonyPatch("Unlock")]
    public static class PDAScanner_Unlock_Patch
    {
        public static bool Prefix(PDAScanner.EntryData entryData)
        {
            if (entryData.key == TechType.PrecursorPrisonArtifact7)
            {
                if (!KnownTech.Contains(Main.techTypeID))
                {
                    KnownTech.Add(Main.techTypeID);
                    ErrorMessage.AddMessage(CannonConfig.language_settings["Item_Unlock_Message"]);
                }
            }
            return true;
        }
    }
}
