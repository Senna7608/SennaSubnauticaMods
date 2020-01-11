using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UnityEngine;

namespace Common
{
    internal abstract class BuildableModItem : Buildable
    {
        protected readonly string _ClassID;
        protected readonly string _FriendlyName;
        protected readonly string _Description;
        protected readonly string _HandOverText;       
        protected readonly bool _UnlockedAtStart;

        protected readonly string IconFilename;
        protected readonly TechType IconTechType;
        protected readonly TechType PrefabTemplate;
        protected readonly string GameResourceFileName;

        protected GameObject _GameObject { get; private set; }

        public override TechGroup GroupForPDA { get; }
        public override TechCategory CategoryForPDA { get; }
        public override string AssetsFolder { get; }
        public override TechType RequiredForUnlock { get; }

        protected BuildableModItem(
            string _classID,
            string _friendlyName,
            string _handOverText,
            string _description,
            TechType requiredForUnlock,
            TechGroup groupForPDA,
            TechCategory categoryForPDA,
            bool _unlockedAtStart,

            string iconFileName,
            TechType iconTechType,                        
            TechType prefabTemplate,                                       
            string gamerResourceFileName
            )
            : base(_classID, _friendlyName, _description)
        {
            _ClassID = _classID;
            _FriendlyName = _friendlyName;
            _Description = _description;
            _HandOverText = _handOverText;
            RequiredForUnlock = requiredForUnlock;
            GroupForPDA = groupForPDA;
            CategoryForPDA = categoryForPDA;
            _UnlockedAtStart = _unlockedAtStart;

            IconFilename = iconFileName;
            IconTechType = iconTechType;                       
            PrefabTemplate = prefabTemplate;                                     
            GameResourceFileName = gamerResourceFileName;
        }

        public virtual void Patch_B()
        {
            Atlas.Sprite sprite = null;

            if (IconFilename != null)
            {
                try
                {
                    sprite = ImageUtils.LoadSpriteFromFile($"./QMods/{IconFilename}/Assets/{IconFilename}.png");
                }
                catch
                {
                    SNLogger.Log($"[{_ClassID}] ***ERROR! File [{IconFilename}.png] not Found! ");
                }
            }
            else if (IconTechType != TechType.None)
            {
                try
                {
                    sprite = GetResourceIcon(IconTechType);
                }
                catch
                {
                    SNLogger.Log($"[{_ClassID}] ***ERROR! Resource TechType icon [{IconTechType.ToString()}] not Found! ");
                }
            }
            else
            {
                try
                {
                    sprite = GetResourceIcon(PrefabTemplate);
                }
                catch
                {
                    SNLogger.Log($"[{_ClassID}] ***ERROR! Resource template icon [{PrefabTemplate.ToString()}] not Found! ");
                }
            }


            TechType = TechTypeHandler.AddTechType(_ClassID, _FriendlyName, _Description, sprite , false);
            SpriteHandler.RegisterSprite(TechType, sprite);           
            CraftDataHandler.SetTechData(TechType, GetRecipe());
            CraftDataHandler.AddToGroup(GroupForPDA, CategoryForPDA, TechType);            
            KnownTechHandler.SetAnalysisTechEntry(RequiredForUnlock, new TechType[1] { TechType }, $"{_FriendlyName} blueprint discovered!");

            PrefabHandler.RegisterPrefab(this);            
        }

        protected abstract TechData GetRecipe();        

        public override GameObject GetGameObject()
        {
            if (GameResourceFileName == null)
            {
                try
                {
                    _GameObject = Object.Instantiate(CraftData.GetPrefabForTechType(PrefabTemplate));
                }
                catch
                {
                    _GameObject = Object.Instantiate(CraftData.GetPrefabForTechType(TechType.Titanium));
                }
            }
            else
            {
                try
                {
                    _GameObject = Object.Instantiate(Resources.Load<GameObject>(GameResourceFileName));
                }
                catch
                {
                    _GameObject = Object.Instantiate(CraftData.GetPrefabForTechType(TechType.Titanium));
                }
            }

            _GameObject.name = _ClassID;

            //_GameObject.AddComponent<SelfDestruct>();

            return _GameObject;
        }
        
        public Atlas.Sprite GetResourceIcon(TechType techType)
        {
            return SpriteManager.Get(techType);
        }
    }
}
