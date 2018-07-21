namespace CheatManager
{
    internal static class TechMatrix
    {
        public static readonly TechType[][] techMatrix = new TechType[][]
        {
            #region Vehicles

            new TechType[]
            {
                TechType.Seamoth,                
                TechType.Exosuit,
                TechType.Cyclops,
                TechType.RocketBase
            },
            #endregion

            #region Tools

            new TechType[]
            {
                TechType.Knife,
                TechType.DiamondBlade,
                TechType.HeatBlade,
                TechType.Flashlight,
                TechType.Beacon,
                TechType.Builder,
                TechType.AirBladder,
                TechType.Terraformer,
                TechType.DiveReel,
                TechType.Scanner,
                TechType.FireExtinguisher,
                TechType.PipeSurfaceFloater,
                TechType.Welder,
                TechType.Seaglide,
                TechType.Constructor,
                TechType.Transfuser,
                TechType.Flare,
                TechType.StasisRifle,
                TechType.PropulsionCannon,
                TechType.RepulsionCannon,
                TechType.Gravsphere,
                TechType.SmallStorage,
                TechType.LaserCutter,
                TechType.LEDLight
            },
            #endregion

            #region Equipment

            new TechType[]
            {
                TechType.RadiationSuit,
                TechType.RadiationHelmet,
                TechType.RadiationGloves,
                TechType.ReinforcedDiveSuit,
                TechType.ReinforcedGloves,
                TechType.Stillsuit,
                TechType.Fins,
                TechType.UltraGlideFins,
                TechType.SwimChargeFins,
                TechType.Tank,
                TechType.DoubleTank,
                TechType.PlasteelTank,
                TechType.HighCapacityTank,
                TechType.Rebreather,
                TechType.Compass,
                TechType.MapRoomHUDChip,
                TechType.WhirlpoolTorpedo,
                TechType.GasTorpedo
            },
            #endregion

            #region Materials

            new TechType[]
            {
                TechType.Quartz,
                TechType.ScrapMetal,
                TechType.FiberMesh,
                TechType.Copper,
                TechType.Lead,
                TechType.Salt,
                TechType.StalkerTooth,
                TechType.MercuryOre,
                TechType.Glass,
                TechType.Titanium,
                TechType.Silicone,
                TechType.Gold,
                TechType.Magnesium,
                TechType.Sulphur,
                TechType.Bleach,
                TechType.Silver,
                TechType.TitaniumIngot,
                TechType.CrashPowder,
                TechType.Diamond,
                TechType.Lithium,
                TechType.PlasteelIngot,
                TechType.EnameledGlass,
                TechType.Uranium,
                TechType.AluminumOxide,
                TechType.HydrochloricAcid,
                TechType.Magnetite,
                TechType.Polyaniline,
                TechType.AramidFibers,
                TechType.Aerogel,
                TechType.Benzene,
                TechType.Lubricant,
                TechType.UraniniteCrystal,
                TechType.PrecursorIonCrystal,
                TechType.Kyanite,
                TechType.Nickel,
                TechType.HatchingEnzymes,
                TechType.SeaTreaderPoop
            },
            #endregion

            #region Electronics

            new TechType[]
            {
                TechType.CopperWire,
                TechType.WiringKit,
                TechType.AdvancedWiringKit,
                TechType.ComputerChip,
                TechType.ReactorRod,
                TechType.DepletedReactorRod,
                TechType.Battery,
                TechType.PrecursorIonBattery,
                TechType.PowerCell,
                TechType.PrecursorIonPowerCell,
                TechType.PrecursorKey_Red,
                TechType.PrecursorKey_Blue,
                TechType.PrecursorKey_Orange,
                TechType.PrecursorKey_White,
                TechType.PrecursorKey_Purple
            },
            #endregion

            #region Upgrades

            new TechType[]
            {
                TechType.MapRoomUpgradeScanRange,
                TechType.MapRoomUpgradeScanSpeed,
                TechType.VehiclePowerUpgradeModule,
                TechType.VehicleStorageModule,
                TechType.VehicleArmorPlating,
                TechType.LootSensorFragment,
                TechType.VehicleHullModule1,
                TechType.VehicleHullModule2,
                TechType.VehicleHullModule3,
                TechType.SeamothSolarCharge,
                TechType.SeamothElectricalDefense,
                TechType.SeamothTorpedoModule,
                TechType.SeamothSonarModule,
                TechType.ExosuitJetUpgradeModule,
                TechType.ExosuitDrillArmModule,
                TechType.ExosuitThermalReactorModule,
                TechType.ExosuitClawArmModule,
                TechType.ExosuitPropulsionArmModule,
                TechType.ExosuitGrapplingArmModule,
                TechType.ExosuitTorpedoArmModule,
                TechType.ExoHullModule1,
                TechType.ExoHullModule2,
                TechType.PowerUpgradeModule,
                TechType.CyclopsShieldModule,
                TechType.CyclopsSonarModule,
                TechType.CyclopsSeamothRepairModule,
                TechType.CyclopsDecoyModule,
                TechType.CyclopsFireSuppressionModule,
                TechType.CyclopsThermalReactorModule,
                TechType.CyclopsHullModule1,
                TechType.CyclopsHullModule2,
                TechType.CyclopsHullModule3
            },
            #endregion
            
            #region Food & Water

            new TechType[]
            {
                TechType.NutrientBlock,
                TechType.Snack1,
                TechType.Snack2,
                TechType.Snack3,
                TechType.Coffee,
                TechType.FirstAidKit,
                TechType.FilteredWater,
                TechType.DisinfectedWater,
                TechType.StillsuitWater,
                TechType.BigFilteredWater,
                TechType.CookedPeeper,
                TechType.CookedHoleFish,
                TechType.CookedGarryFish,
                TechType.CookedReginald,
                TechType.CookedBladderfish,
                TechType.CookedHoverfish,
                TechType.CookedSpadefish,
                TechType.CookedBoomerang,
                TechType.CookedEyeye,
                TechType.CookedOculus,
                TechType.CookedHoopfish,
                TechType.CookedSpinefish,
                TechType.CookedLavaEyeye,
                TechType.CookedLavaBoomerang,
                TechType.CuredPeeper,
                TechType.CuredHoleFish,
                TechType.CuredGarryFish,
                TechType.CuredReginald,
                TechType.CuredBladderfish,
                TechType.CuredHoverfish,
                TechType.CuredSpadefish,
                TechType.CuredBoomerang,
                TechType.CuredEyeye,
                TechType.CuredOculus,
                TechType.CuredHoopfish,
                TechType.CuredSpinefish,
                TechType.CuredLavaEyeye,
                TechType.CuredLavaBoomerang
            },
            #endregion
            
            #region Loot & Drill

            new TechType[]
            {
                TechType.LimestoneChunk,
                TechType.SandstoneChunk,
                TechType.BasaltChunk,
                TechType.ShaleChunk,
                TechType.DrillableSalt,
                TechType.DrillableQuartz,
                TechType.DrillableCopper,
                TechType.DrillableTitanium,
                TechType.DrillableLead,
                TechType.DrillableSilver,
                TechType.DrillableDiamond,
                TechType.DrillableGold,
                TechType.DrillableMagnetite,
                TechType.DrillableLithium,
                TechType.DrillableMercury,
                TechType.DrillableUranium,
                TechType.DrillableAluminiumOxide,
                TechType.DrillableNickel                              
            },
            #endregion
            
            #region Herbivores

            new TechType[]
            {
                TechType.Skyray,
                TechType.HoleFish,
                TechType.Peeper,
                TechType.Oculus,
                TechType.RabbitRay,
                TechType.GarryFish,
                TechType.Boomerang,
                TechType.Eyeye,
                TechType.Bladderfish,
                TechType.Hoverfish,
                TechType.Jellyray,
                TechType.Reginald,
                TechType.Spadefish,
                TechType.Gasopod,
                TechType.Hoopfish,
                TechType.HoopfishSchool,
                TechType.Cutefish,
                TechType.Spinefish,
                TechType.LavaBoomerang,
                TechType.LavaEyeye,
                TechType.GhostRayBlue,
                TechType.GhostRayRed
            },
            #endregion
            
            #region Carnivores

            new TechType[]
            {
                TechType.Crash,
                TechType.Stalker,
                TechType.Sandshark,
                TechType.BoneShark,
                TechType.Mesmer,
                TechType.Crabsnake,
                TechType.Warper,
                TechType.Biter,
                TechType.Shocker,
                TechType.Blighter,
                TechType.CrabSquid,
                TechType.LavaLizard,
                TechType.SpineEel
            },
            #endregion
            
            #region Parasites

            new TechType[]
            {
                TechType.Jumper,
                TechType.LavaLarva,
                TechType.Floater,
                TechType.Bleeder,
                TechType.Rockgrub,
                TechType.CaveCrawler,
                TechType.Shuttlebug,
                TechType.LargeFloater,
                TechType.PrecursorDroid
            },
            #endregion
            
            #region Leviathan

            new TechType[]
            {
                TechType.ReefbackBaby,
                TechType.Reefback,
                TechType.SeaTreader,
                TechType.ReaperLeviathan,
                TechType.SeaDragon,
                TechType.SeaEmperorBaby,
                TechType.SeaEmperorJuvenile,                
                TechType.GhostLeviathanJuvenile,
                TechType.GhostLeviathan                
            },
            #endregion
            
            #region Eggs

            new TechType[]
            {
                TechType.StalkerEgg,
                TechType.ReefbackEgg,
                TechType.SpadefishEgg,
                TechType.RabbitrayEgg,
                TechType.MesmerEgg,
                TechType.JumperEgg,
                TechType.SandsharkEgg,
                TechType.JellyrayEgg,
                TechType.BonesharkEgg,
                TechType.CrabsnakeEgg,
                TechType.ShockerEgg,
                TechType.GasopodEgg,
                TechType.CrashEgg,
                TechType.CrabsquidEgg,
                TechType.CutefishEgg,
                TechType.LavaLizardEgg
            },
            #endregion
            
            #region Sea: Seed

            new TechType[]
            {
                TechType.AcidMushroomSpore,
                TechType.WhiteMushroomSpore,
                TechType.SpikePlantSeed,
                TechType.BluePalmSeed,
                TechType.PurpleFanSeed,
                TechType.SmallFanSeed,
                TechType.PurpleTentacleSeed,
                TechType.GabeSFeatherSeed,
                TechType.SeaCrownSeed,
                TechType.MembrainTreeSeed,
                TechType.EyesPlantSeed,
                TechType.RedGreenTentacleSeed,
                TechType.PurpleStalkSeed,
                TechType.RedBasketPlantSeed,
                TechType.RedBushSeed,
                TechType.RedConePlantSeed,
                TechType.ShellGrassSeed,
                TechType.SpottedLeavesPlantSeed,
                TechType.RedRollPlantSeed,
                TechType.PurpleBranchesSeed,
                TechType.SnakeMushroomSpore
            },
            #endregion
            
            #region Land: Seed

            new TechType[]
            {
                TechType.BulboTreePiece,
                TechType.OrangeMushroomSpore,
                TechType.PurpleVasePlantSeed,
                TechType.PinkMushroomSpore,
                TechType.PurpleRattleSpore,
                TechType.PurpleVegetable,
                TechType.MelonSeed,
                TechType.PinkFlowerSeed,
                TechType.FernPalmSeed,
                TechType.OrangePetalsPlantSeed
            },
            #endregion
            
            #region Flora: Item

            new TechType[]
            {
                TechType.JellyPlant,
                TechType.AcidMushroom,
                TechType.SmallFan,
                TechType.BloodOil,
                TechType.WhiteMushroom,
                TechType.JeweledDiskPiece,
                TechType.CoralChunk,
                TechType.KooshChunk,
                TechType.PurpleBrainCoralPiece,
                TechType.JellyPlantSeed,
                TechType.PinkMushroom,
                TechType.PurpleRattle,
                TechType.HangingFruit,
                TechType.SmallMelon,
                TechType.Melon
            },
            #endregion
            
            #region Sea: Spawn

            new TechType[]
            {
                TechType.SmallKoosh,
                TechType.MediumKoosh,
                TechType.LargeKoosh,
                TechType.HugeKoosh,
                TechType.MembrainTree,
                TechType.PurpleFan,
                TechType.PurpleTentacle,
                TechType.SmallFanCluster,
                TechType.BigCoralTubes,
                TechType.TreeMushroom,
                TechType.BloodRoot,
                TechType.BloodVine,
                TechType.BluePalm,
                TechType.GabeSFeather,
                TechType.SeaCrown,
                TechType.EyesPlant,
                TechType.RedGreenTentacle,
                TechType.PurpleStalk,
                TechType.RedBasketPlant,
                TechType.RedBush,
                TechType.RedConePlant,
                TechType.ShellGrass,
                TechType.SpottedLeavesPlant,
                TechType.RedRollPlant,
                TechType.PurpleBranches,
                TechType.SnakeMushroom,
                TechType.GenericJeweledDisk,
                TechType.PurpleBrainCoral,
                TechType.SpikePlant,
                TechType.BallClusters,
                TechType.BarnacleSuckers,
                TechType.BlueBarnacle,
                TechType.BlueBarnacleCluster,
                TechType.BlueCoralTubes,
                TechType.RedGrass,
                TechType.GreenGrass,
                TechType.Mohawk,
                TechType.GreenReeds,
                TechType.RedSeaweed,
                TechType.CoralShellPlate,
                TechType.BlueCluster,
                TechType.BrownTubes,
                TechType.BloodGrass,
                TechType.FloatingStone,
                TechType.BlueAmoeba,
                TechType.RedTipRockThings,
                TechType.BlueTipLostRiverPlant,
                TechType.BlueLostRiverLilly,
                TechType.HangingStinger,
                TechType.BrainCoral,
                TechType.CoveTree
            },
            #endregion
            
            #region Land: Spawn

            new TechType[]
            {
                TechType.PinkFlower,
                TechType.BulboTree,
                TechType.PurpleVasePlant,
                TechType.OrangeMushroom,
                TechType.FernPalm,
                TechType.HangingFruitTree,
                TechType.PurpleVegetablePlant,
                TechType.MelonPlant,
                TechType.OrangePetalsPlant
            },
            #endregion
            
            #region Blueprints

            new TechType[]
            {
                TechType.RocketBaseLadder,
                TechType.RocketStage1,
                TechType.RocketStage2,
                TechType.RocketStage3
            }
            #endregion
        };

        /* old matrix
        private static readonly int[][] techMatrix = new int[][]
        {
            new int[] { 2000, 2001, 2003, 5900 },
            new int[] {  505,  800,  801,  507,  508,  509,  513,  514,  517,  523,  524,  526,  750,  751,  752,  753,  754,  755,  757,  807,  758,  759,  761,  762 },
            new int[] {  519,  520,  521,  522,  529,  808,  502,  805,  806,  503,  528,  803,  804,  518,  512,  525, 2111, 2119 },
            new int[] {    1,    2,    3,    7,    8,    9, 3504,   12,   15,   16,   17,   21,   22,   23,   27,   28,   30,   35,   36,   40,   41,   42,   51,   52,   53,   54,   56,   57,   59,   61,   62,   63,   66,   68,   69 , 1547, 3063 },
            new int[] {   32,   33,   34,   44,   64,   65,  504, 4210,   43, 4209, 4200, 4201, 4202, 4203, 4204 },
            new int[] { 2250, 2251, 2101, 2103, 2105, 2108, 2112, 2113, 2114, 2102, 2104, 2109, 2110, 2115, 2116, 2117, 2118, 2120, 2121, 2122, 2128, 2129, 1516, 1551, 1552, 1553, 1554, 1555, 1557, 1537, 1538, 1558  },
            new int[] { 4513, 4520, 4521, 4522, 4523, 4514, 4500, 4501, 4515, 4516, 4502, 4503, 4504, 4505, 4506, 4507, 4508, 4509, 4510, 4511, 4512, 4517, 4518, 4519, 4600, 4601, 4602, 4603, 4604, 4605, 4606, 4607, 4608, 4609, 4610, 4611, 4612, 4613 },
            new int[] {    4,   31,   37,   38,   70,   71,   72,   73,   74,   75,   76,   77,   78,   79,   80,   81,   82,   83 },
            new int[] { 2542, 2501, 2504, 2505, 2506, 2507, 2510, 2513, 2515, 2516, 2517, 2519, 2520, 2523, 2531, 2532, 2538, 2546, 2554, 2555, 2558, 2559 },
            new int[] { 2509, 2512, 2524, 2534, 2535, 2539, 2549, 2543, 2545, 2548, 2550, 2551, 2552 },
            new int[] { 2502, 2511, 2522, 2526, 2527, 2541, 2547, 3070, 2561 },
            new int[] { 2518, 2536, 2540, 2553, 2556, 2560, 2562, 2564, 2565 },
            new int[] { 1258, 1259, 1260, 1261, 1262, 1263, 1264, 1265, 1266, 1267, 1268, 1269, 1281, 1283, 1285, 1287  },
            new int[] { 3509, 3510, 3519, 3520, 3521, 3522, 3523, 3525, 3526, 3527, 3531, 3532, 3533, 3534, 3535, 3536, 3537, 3538, 3539, 3540, 3541},
            new int[] { 3506, 3507, 3508, 3511, 3512, 3514, 3517, 3528, 3529, 3530},
            new int[] { 3010, 3021, 3026, 3034, 3035, 3501, 3502, 3503, 3518, 3524, 3039, 3040, 3513, 3515, 3516 },
            new int[] { 3015, 3016, 3017, 3018, 3019, 3020, 3022, 3027, 3028, 3029, 3036, 3037, 3048, 3049, 3050, 3052, 3053, 3054, 3055, 3056, 3057, 3058, 3059, 3060, 3061, 3062, 3064, 4002, 4004, 3001, 3002, 3003, 3004, 3005, 3006, 3007, 3008, 3009, 3023, 3025, 3030, 3031, 3032, 3065, 3066, 3067, 3068, 3069, 4003, 4005, 4006 },
            new int[] { 3038, 3041, 3042, 3043, 3044, 3045, 3046, 3047, 3051 },
            new int[] { 5901, 5902, 5903, 5904 }
        };
        */
    }
}
