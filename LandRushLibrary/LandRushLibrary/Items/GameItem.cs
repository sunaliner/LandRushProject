﻿using LandRushLibrary.Repository;
using Newtonsoft.Json;

namespace LandRushLibrary.Items
{
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class GameItem
    {
        public ItemID ItemId { get; set; }
        public string Name { get; set; }
        public string IconName { get; set; }
        public string PrefabName { get; set; }
        public ItemType Type { get; set; }
        public abstract GameItem Clone();
        
        protected virtual void SetBasicCloneItem(GameItem clone)
        {
            clone.ItemId = ItemId;
            clone.Name = Name;
            clone.IconName = IconName;
            clone.PrefabName = PrefabName;
            clone.Type = Type;
        }

    }
}
