﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LandRushLibrary.Factory;
using LandRushLibrary.PlayerItemManagers;
using LandRushLibrary.Items;
using LandRushLibrary.Repository;
using LandRushLibrary.Units;
using LandRushLibrary.Upgrade;
using LandRushLibrary.Utilities;
using    LandRushLibrary.Drop;
using Newtonsoft.Json;

namespace Game
{
    class Program
    {
        static private int[] _probability = {0, 0, 0, 0, 0, 0};
        static int _dropTry = 1000000;
        static int _upTry = 100000;
        static int _upSuceess = 0;

        public static void OnUpTried(object sender, UpgradeTriedEventArgs e)
        {
            Console.WriteLine(e.Success);

            if (e.Success)
                _upSuceess++;
        }

        static void Main(string[] args)
        {
            Inventory inven = Inventory.Instance;
            UpgradeManager upgrader = UpgradeManager.Instance;

            upgrader.UpgradeTried += OnUpTried;

            for (int i = 0; i < _upTry; i++)
            {
                Sword sword = ItemFactory.Instance.Create<Sword>(ItemID.STEEL_SWORD);

                UpgradeCost cost = upgrader.GetUpgradeCost(sword);

                foreach (var item in cost.RequireIngredients)
                {
                    for (int j = 0; j < item.Value; j++)
                    {
                        IngredientItem ingredient = ItemFactory.Instance.Create<IngredientItem>(item.Key);

                        inven.AddInvenItem(ingredient.ItemId);
                    }
                }

                upgrader.Upgrade<Sword>(ref sword);
            }

            Console.WriteLine($"{(double) _upSuceess / _upTry * 100:N2}%");
        }

        static void foo()
        {
            Player player = Player.Instance;
            PlayerEquipment equipmentManager = PlayerEquipment.Instance;

//            equipmentManager.SetEquipmentToSlot(1, ItemFactory.Instance.Create<Sword>(ItemID.OLD_SWORD));
//            equipmentManager.SetEquipmentToSlot(2, ItemFactory.Instance.Create<Bow>(ItemID.OLD_BOW));
//            equipmentManager.SetEquipmentToSlot(3, ItemFactory.Instance.Create<Shield>(ItemID.OLD_SHIELD));
//            equipmentManager.SetEquipmentToSlot(4, ItemFactory.Instance.Create<Quiver>(ItemID.OLD_QUIVER));

            equipmentManager.EquipCurrentPair();

            Monster orc;
            Monster orcLord;

            for (int i = 0; i < _dropTry; i++)
            {
                orc = MonsterFactory.Instance.Create(MonsterID.ORC);
                orc.ItemDropped += OnItemDropped;

                player.Attack(orc, ((Sword) player.RightItem).AttackPower);
                player.Attack(orc, ((Sword) player.RightItem).AttackPower);

                //orcLord = MonsterFactory.Instance.Create(MonsterID.ORC_LORD);
                //orcLord.ItemDropped += OnItemDropped;

                //for (int j = 0; j < 12; j++)
                //{
                //    player.Attack(orcLord, ((Sword)player.RightItem).AttackPower);
                //}
                

            }

            Console.WriteLine("==================================================================");

            foreach (var item in _probability)
            {
                Console.WriteLine($"{(double) item / _dropTry * 100:N2}%");
            }
        }

        static void OnItemDropped(object sender, ItemDroppedEventArgs e)
        {
            Inventory inventoryManager = Inventory.Instance;

            List<DroppedItems> dropItemses = e.DropItems;

            foreach (DroppedItems dropItem in dropItemses)
            {
                inventoryManager.AddInvenItem(dropItem.ItemId, dropItem.Amount);
            }
        }
    }
}