﻿using LandRushLibrary.Factory;
using LandRushLibrary.Items;
using LandRushLibrary.PlayerItemManagers;
using LandRushLibrary.Repository;
using LandRushLibrary.Upgrade;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LandRushLibraryTests
{
    [TestClass]
    public class UpgradeTest
    {
        private bool stop = true;

        [TestMethod]
        public void 업그레이드_시도()
        {
            Sword sword = ItemFactory.Instance.Create<Sword>(ItemID.OldSword);

            Upgrader upgrader = Upgrader.Instance;

            Inventory inven = Inventory.Instance;

            inven.ClearInventory();

            Assert.AreEqual(false, upgrader.UpgradePossibility(sword));

            IngredientItem stone = ItemFactory.Instance.Create<IngredientItem>(ItemID.Stone);
            stone.Amount = 30;

            IngredientItem wood = ItemFactory.Instance.Create<IngredientItem>(ItemID.Wood);
            wood.Amount = 30;

            IngredientItem iron = ItemFactory.Instance.Create<IngredientItem>(ItemID.Iron);
            iron.Amount = 30;

            inven.AddGameItem(stone);
            inven.AddGameItem(wood);
            inven.AddGameItem(iron);

            Assert.AreEqual(true, upgrader.UpgradePossibility(sword));

            upgrader.UpgradeTried += OnUpgradeTried;

            while(stop)
            {
                upgrader.Upgrade<Sword>(ref sword);
            }

            Assert.AreEqual("ShortSword", sword.Name);
            Assert.AreEqual(2, sword.Grade);
        }

        public void OnUpgradeTried(object sender, Upgrader.UpgradeTriedEventArgs e)
        {
            if( e.Success )
            {
                stop = false;
            }
        }
    }
}
