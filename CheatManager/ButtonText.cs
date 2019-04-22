namespace CheatManager
{
    internal enum Commands
    {
        day,
        night,
        unlockall,
        clearinventory,
        unlockdoors,
        encyall,
        warpme,        
        BackWarp
    }

    internal enum ToggleCommands
    {
        freedom,
        creative,
        survival,
        hardcore,
        fastbuild,
        fastscan,
        fastgrow,
        fasthatch,
        filterfast,
        nocost,
        noenergy,
        nosurvival,
        oxygen,
        radiation,
        invisible,
        shotgun,
        nodamage,
        alwaysday,
        noinfect,
        overpower        
    }

    internal enum Categories
    {
        Vehicles,
        Tools,
        Equipment,
        Materials,
        Electronics,
        Upgrades,
        FoodAndWater,
        LootAndDrill,
        Herbivores,
        Carnivores,
        Parasites,
        Leviathan,
        Eggs,
        SeaSeed,
        LandSeed,
        FloraItem,
        SeaSpawn,
        LandSpawn,
        Blueprints,
        Warp,        
    }    

    internal class ButtonText
    {
        internal readonly string[] Buttons = new string[8]
        {
            "day",
            "night",
            "unlockall",
            "clearinventory",
            "unlockdoors",
            "ency all",
            "warpme",
            "BackWarp"
        };

        internal readonly string[] DayNightTab = new string[6]
        {
            "0.1",
            "0.25",
            "0.5",
            "0.75",
            "1",
            "2"
        };


        internal readonly string[] ToggleButtons = new string[20]
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
            "alwaysday",
            "noinfect",
            "overpower"
        };

        internal readonly string[] CategoriesTab = new string[20]
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
