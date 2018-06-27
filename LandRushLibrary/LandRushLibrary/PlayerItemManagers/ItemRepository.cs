﻿using LandRushLibrary.Items;
using LandRushLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using LandRushLibrary.Factory;
using LandRushLibrary.Interfaces;

namespace LandRushLibrary.PlayerItemManagers
{
    public abstract class ItemRepository
    {
        #region Fields

        public GameItem[,] Items { get; set; }
        public int Row { get; protected set; }= 4;
        public int Column { get; protected set; }= 3;

        protected ItemRepository()
        {
            Items = new GameItem[Row, Column];
        }

        #endregion

        #region Methods


        public void ClearInventory()
        {
            Items = new GameItem[Row, Column];
        }

        public GameItem GetSlotItemInfo(int row, int column)
        {
            return Items[row, column];
        }

        public bool AddGameItem(GameItem gameItem)
        {

            foreach (var item in Items)
            {
                if (item == null)
                {
                    if (gameItem.Amount == 0)
                    {
                        OnInventoryItemChanged(Items);
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (item.ItemId == gameItem.ItemId)
                {
                    gameItem.Amount = item.AddAmount(gameItem.Amount);

                }

            }

            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    if (Items[i, j] == null)
                    {
                        Items[i, j] = gameItem;
                        OnInventoryItemChanged(Items);
                        return true;
                    }
                }
            }

            OnInventoryIsFullForOut(gameItem);
            return false;

        }

        public void SwapItem(int sourceRow, int sourceColumn, int targetRow, int tagetColum)
        {
            GameItem temp = Items[ sourceRow, sourceColumn];
            Items[sourceRow, sourceColumn] = Items[targetRow, tagetColum];
            Items[targetRow, tagetColum] = temp;

            OnInventoryItemChanged(Items);
        }

        public void RemoveItem(ItemID itemId, int amount)
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    if (Items[i, j] == null)
                        continue;

                    if (Items[i, j].ItemId == itemId)
                    {
                        Items[i, j].Amount -= amount;

                        if (Items[i, j].Amount < 0)
                        {
                            amount = Items[i, j].Amount * (-1);
                            Items[i, j] = null;
                        }
                        else
                            break;

                    }
                }
            }

            foreach (var item in Items)
            {

            }

            OnInventoryItemChanged(Items);
        }

        public int GetAmountForId(ItemID itemId)
        {
            int amount = 0;

            foreach(var item in Items)
            {
                if (item == null)
                    continue;

                if (item.ItemId == itemId)
                    amount += item.Amount;
            }

            return amount;

        }


        public GameItem this[int row, int column]
        {
            get { return Items[row - 1, column - 1]; }
        }

        #endregion

        #region Events

        #region InventoryItemChanged
        public event EventHandler<InventoryItemChangedEventArgs> InventoryItemChanged;

        protected virtual void OnInventoryItemChanged(InventoryItemChangedEventArgs e)
        {
            if (InventoryItemChanged != null)
                InventoryItemChanged(this, e);
        }

        private InventoryItemChangedEventArgs OnInventoryItemChanged(GameItem[,] GameItems)
        {
            InventoryItemChangedEventArgs args = new InventoryItemChangedEventArgs(GameItems);
            OnInventoryItemChanged(args);

            return args;
        }

        private InventoryItemChangedEventArgs OnInventoryItemChangedForOut()
        {
            InventoryItemChangedEventArgs args = new InventoryItemChangedEventArgs();
            OnInventoryItemChanged(args);

            return args;
        }

        public class InventoryItemChangedEventArgs : EventArgs
        {
            public GameItem[,] GameItems { get; set; }

            public InventoryItemChangedEventArgs()
            {
            }

            public InventoryItemChangedEventArgs(GameItem[,] gameItems)
            {
                GameItems = gameItems;
            }
        }

        #endregion

        #region InventoryIsFull
        public event EventHandler<InventoryIsFullEventArgs> InventoryIsFull;

        protected virtual void OnInventoryIsFull(InventoryIsFullEventArgs e)
        {
            if (InventoryIsFull != null)
                InventoryIsFull(this, e);
        }

        private InventoryIsFullEventArgs OnInventoryIsFull(GameItem remainItem)
        {
            InventoryIsFullEventArgs args = new InventoryIsFullEventArgs(remainItem);
            OnInventoryIsFull(args);

            return args;
        }

        private InventoryIsFullEventArgs OnInventoryIsFullForOut(GameItem remainItem)
        {
            InventoryIsFullEventArgs args = new InventoryIsFullEventArgs(remainItem);
            OnInventoryIsFull(args);

            return args;
        }

        public class InventoryIsFullEventArgs : EventArgs
        {

            GameItem RemainItem { get; set; }

            public InventoryIsFullEventArgs(GameItem remainItem)
            {
                RemainItem = remainItem;
            }

        }

        #endregion

        #endregion
    }
}