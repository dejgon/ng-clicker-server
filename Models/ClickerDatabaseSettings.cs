﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClickerAPI.Models
{
    public class ClickerDatabaseSettings: IClickerDatabaseSettings
    {
        public string UsersCollectionName { get; set; }
        public string UpgradesCollectionName { get; set; }
        public string StatisticsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
