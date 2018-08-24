using System.Collections.Generic;
using Common.EasyMarkup;

namespace SlotExtender
{
    //Original code found on PrimeSonic's MoreCyclopsUpgrades MOD 
    public class EmModuleSaveData : EmPropertyCollection
    {
        private const string ItemIDKey = "ID";
        private const string KeyName = "MDS";

        private EmProperty<int> _itemID;        

        public int ItemID
        {
            get => _itemID.Value;
            set => _itemID.Value = value;
        }
        
        private static ICollection<EmProperty> GetDefinitions
        {
            get => new List<EmProperty>()
            {
                new EmProperty<int>(ItemIDKey, 0)                
            };
        }

        public EmModuleSaveData() : this(KeyName)
        {
        }

        public EmModuleSaveData(string keyName) : base(keyName, GetDefinitions)
        {
            _itemID = (EmProperty<int>)Properties[ItemIDKey];            
        }

        internal override EmProperty Copy() => new EmModuleSaveData();
    }
}
