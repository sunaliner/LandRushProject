﻿using System;
using System.Collections.Generic;
using System.IO;
using LandRushLibrary.Factory;
using LandRushLibrary.Items;
using LandRushLibrary.Repository;
using LandRushLibrary.Units;
using LandRushLibrary.Utilities;
using Newtonsoft.Json;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitSerializer.Instance.Deseriailize();
            ItemSerializer.Instance.Serialize();
        }
    }
}
