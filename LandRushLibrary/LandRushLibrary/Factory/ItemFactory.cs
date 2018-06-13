﻿using System;
using System.Collections.Generic;
using LandRushLibrary.Items;
using LandRushLibrary.Repository;
using LandRushLibrary.Utilities;
using System.Linq;

namespace LandRushLibrary.Factory
{
    public class ItemFactory
    {
        #region Singleton
        private static ItemFactory _instace;

        public static ItemFactory Instance
        {
            get
            {
                if (_instace == null)
                    _instace = new ItemFactory();

                return _instace;
            }
        }

        private readonly Dictionary<ItemID, GameItem> _items;

        private ItemFactory()
        {
            _items = ItemSerializer.Instance.Deseriailize();
        }
        #endregion

        #region Methods

        public GameItem Create(ItemID itemId)
        {
            GameItem clone = _items[itemId].Clone();

            return clone;
        }

        public T Create<T>(ItemID itemId) where T : GameItem
        {
            GameItem clone = _items[itemId].Clone();

            return clone as T;
        }

        public List<T> FindItemListId<T> (Func<T, bool> predicate) where T : GameItem
        {
            var items = (from x in _items
                         where x.Value is T
                        select x.Value as T);

            return items.Where(predicate).ToList();

        }

        #endregion
    }
}
