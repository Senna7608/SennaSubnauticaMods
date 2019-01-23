using System;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UnityEngine;

namespace Common
{
    ///<summary>
    ///Based on Craftable Class from PrimeSonic/Upgraded Vehicles mod
    ///Original code found on GitHub: https://github.com/PrimeSonic/PrimeSonicSubnauticaMods/blob/master/UpgradedVehicles/Craftables/Craftable.cs
    ///</summary>

    internal abstract class Craftable : ModPrefab
    {
        public readonly string NameID;
        public readonly string FriendlyName;
        public readonly string Description;
        public GameObject prefab { get; private set; }

        protected readonly TechType PrefabTemplate;
        protected readonly CraftTree.Type FabricatorType;
        protected readonly string FabricatorTab;
        protected readonly TechType RequiredForUnlock;
        protected readonly TechGroup GroupForPDA;
        protected readonly TechCategory CategoryForPDA;

        internal readonly string GameResourceFileName;        
        //internal readonly Type[] ComponentsToAdd;

        protected Craftable(
            string nameID,
            string friendlyName,
            string description,            
            TechType template,
            CraftTree.Type fabricatorType,
            string fabricatorTab,
            TechType requiredAnalysis,
            TechGroup groupForPDA,
            TechCategory categoryForPDA,
            string gamerResourceFileName            
            //Type[] componentsToAdd
            )
            : base(nameID, $"{nameID}Prefab")
        {
            NameID = nameID;
            FriendlyName = friendlyName;
            Description = description;            
            PrefabTemplate = template;
            FabricatorType = fabricatorType;
            FabricatorTab = fabricatorTab;
            RequiredForUnlock = requiredAnalysis;
            GroupForPDA = groupForPDA;
            CategoryForPDA = categoryForPDA;
            GameResourceFileName = gamerResourceFileName;            
            //ComponentsToAdd = componentsToAdd;
        }

        public virtual void Patch()
        {
            TechType = TechTypeHandler.AddTechType(NameID, FriendlyName, Description, ImageUtils.LoadSpriteFromFile($"./QMods/{NameID}/Assets/{NameID}.png"), false);

            CraftTreeHandler.AddCraftingNode(FabricatorType, TechType, FabricatorTab);
            CraftDataHandler.SetTechData(TechType, GetRecipe());
            CraftDataHandler.AddToGroup(GroupForPDA, CategoryForPDA, TechType);

            PrefabHandler.RegisterPrefab(this);

            KnownTechHandler.SetAnalysisTechEntry(RequiredForUnlock, new TechType[1] { TechType }, $"{FriendlyName} blueprint discovered!");
            
        }

        protected abstract TechData GetRecipe();

        public override GameObject GetGameObject()
        {
            if (GameResourceFileName == null)
                prefab = CraftData.GetPrefabForTechType(PrefabTemplate);
            else
                prefab = Resources.Load<GameObject>(GameResourceFileName);

            /*
            var obj = UnityEngine.Object.Instantiate(prefab);            

            if (ComponentsToAdd != null)
            {
                foreach (Type component in ComponentsToAdd)
                {
                  bool result = prefab.AddIfNeedComponent(component);
                    if (result)
                        Debug.Log($"[Craftable] Log: Component: [{component.Name}] added to prefab: [{prefab.name}]");
                }
            }
            */
            return UnityEngine.Object.Instantiate(prefab);
        }        
    }
}
