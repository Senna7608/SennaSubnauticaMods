using System.Collections.Generic;

namespace Common.Helpers.SMLHelpers
{
    public static class EncyHelper
    {
        private static readonly Dictionary<EncyNode, string> EncyNodes = new Dictionary<EncyNode, string>()
        {
            { EncyNode.Welcome,           "Welcome" },
            { EncyNode.StartGear,         "Welcome/StartGear" },

            { EncyNode.Advanced,          "Advanced" },            

            { EncyNode.Sea,               "Lifeforms/Flora/Sea" },
            { EncyNode.Land,              "Lifeforms/Flora/Land" },
            { EncyNode.SmallHerbivores,   "Lifeforms/Fauna/SmallHerbivores" },
            { EncyNode.LargeHerbivores,   "Lifeforms/Fauna/LargeHerbivores" },
            { EncyNode.Carnivores,        "Lifeforms/Fauna/Carnivores" },
            { EncyNode.Deceased,          "Lifeforms/Fauna/Deceased" },
            { EncyNode.Scavengers,        "Lifeforms/Fauna/Scavengers" },
            { EncyNode.Leviathans,        "Lifeforms/Fauna/Leviathans" },
            
            { EncyNode.Coral,             "Lifeforms/Coral" },
            { EncyNode.Exploitable,       "Lifeforms/Flora/Exploitable" },

            { EncyNode.Equipment,         "Tech/Equipment" },
            { EncyNode.Habitats,          "Tech/Habitats" },
            { EncyNode.Vehicles,          "Tech/Vehicles" },
            { EncyNode.Power,             "Tech/Power" },

            { EncyNode.PlanetaryGeology,  "PlanetaryGeology" },

            { EncyNode.BeforeCrash,       "DownloadedData/BeforeCrash" },
            { EncyNode.Degasi,            "DownloadedData/Degasi" },
            { EncyNode.Orders,            "DownloadedData/Degasi/Orders" },
            { EncyNode.AuroraSurvivors,   "DownloadedData/AuroraSurvivors" },
            { EncyNode.PublicDocs,        "DownloadedData/PublicDocs" },           
            { EncyNode.Terminal,          "DownloadedData/Precursor/Terminal" },
            { EncyNode.Artifacts,         "DownloadedData/Precursor/Artifacts" },
            { EncyNode.Scan,              "DownloadedData/Precursor/Scan" },
            { EncyNode.Codes,             "DownloadedData/Codes" },            
        };

        public static string[] GetEncyNodes(EncyNode node)
        {
            return EncyNodes[node].Split('/');
        }

        public static string GetEncyPath(EncyNode node)
        {
            return EncyNodes[node];
        }
    }

    public enum EncyNode
    {
        Welcome,
        StartGear,
        Advanced,
        Sea,
        Land,
        SmallHerbivores,
        LargeHerbivores,
        Carnivores,
        Deceased,
        Scavengers,
        Leviathans,
        Coral,
        Exploitable,
        Equipment,
        Habitats,
        Vehicles,
        Power,
        PlanetaryGeology,
        BeforeCrash,
        Degasi,
        Orders,
        AuroraSurvivors,
        PublicDocs,
        Terminal,
        Artifacts,
        Scan,
        Codes
    }
}
