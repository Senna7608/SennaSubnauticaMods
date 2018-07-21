namespace CheatManager
{
    internal static class ButtonText
    {        
        public static readonly string[] Buttons = new string[]
        {
            "day",
            "night",
            "unlockall",
            "Clear Inventory",
            "unlockdoors",
            "ency all",
            "warpme",
            "BackWarp"
        };

        public static readonly string[] DayNightTab = new string[]
        {
            "0.1",
            "0.25",
            "0.5",
            "0.75",
            "1",
            "2"
        };


        public static readonly string[] ToggleButtons = new string[]
        {
            "freedom",
            "creative",
            "survival",
            "hardcore",
            "fastbuild",
            "fastscan",
            "fastgrow",
            "fasthatch",
            "filterfast",
            "nocost",
            "noenergy",
            "nosurvival",
            "oxygen",
            "radiation",
            "invisible",
            "shotgun",
            "nodamage",            
            "alwaysDay",
            "noInfect",
#if DEBUG
            "OverPower"
#endif
        };

        public static readonly string[] CategoriesTab = new string[]
        {
            "Vehicles",
            "Tools",
            "Equipment",
            "Materials",
            "Electronics",
            "Upgrades",
            "Food & Water",
            "Loot & Drill",
            "Herbivores",
            "Carnivores",
            "Parasites",
            "Leviathan",
            "Eggs",
            "Sea: Seed",
            "Land: Seed",
            "Flora: Item",
            "Sea: Spawn",
            "Land: Spawn",
            "Blueprints",
            "Warp"
        };
    }
}
