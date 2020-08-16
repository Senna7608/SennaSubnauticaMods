using HarmonyLib;

namespace AncientSword
{
    [HarmonyPatch(typeof(PDAScanner))]
    [HarmonyPatch("Unlock")]
    public static class PDAScannerUnlockPatch
    {
        public static bool Prefix(PDAScanner.EntryData entryData)
        {
            if (entryData.key == TechType.PrecursorPrisonArtifact8)
            {
                if (!KnownTech.Contains(SwordPrefab.TechTypeID))
                {
                    KnownTech.Add(SwordPrefab.TechTypeID);
                    ErrorMessage.AddMessage("Added blueprint for Ancient Sword fabrication to database");
                }
            }
            return true;
        }
    }
}
