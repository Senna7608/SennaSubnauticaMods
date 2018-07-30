using System;
using System.Collections.Generic;
using UnityEngine;
using SMLHelper.V2.Handlers;

namespace CheatManager
{
    public class TechMatrix
    {
        public class TechTypeData : IComparable<TechTypeData>
        {
            public TechType TechType { get; set; }
            public string Name { get; set; }            

            public TechTypeData (TechType techType, string name)
            {
                TechType = techType;
                Name = name;
            }

            public int CompareTo(TechTypeData other)
            {
                return String.Compare(Name, other.Name);
            }

            public string GetTechName()
            {
                return Name;
            }           
        }


        public class TechTypeSearch
        {
            readonly TechType _techType;

            public TechTypeSearch(TechType s)
            {
                _techType = s;
            }

            public bool EqualsWith(TechTypeData techTypeData)
            {
                return techTypeData.TechType == _techType;
            }
        }


        public static List<TechType> addedTechTypes = new List<TechType>();

        public static void InitTechMatrixList(ref List<TechTypeData>[] TechnologyMatrix)
        {
            for (int i = 0; i < techMatrix.Length; i++)
            {
                TechnologyMatrix[i] = new List<TechTypeData>();

                for (int j = 0; j < techMatrix[i].Length; j++)
                {
                    TechnologyMatrix[i].Add(new TechTypeData(techMatrix[i][j], Language.main.Get(TechTypeExtensions.AsString(techMatrix[i][j], false))));
                }                
            }            
        }
        
        public static void SortTechLists(ref List<TechTypeData>[] TechnologyMatrix)
        {
            foreach (List<TechTypeData> item in TechnologyMatrix)
            {
                item.Sort();
            }            
        }

        public static void IsExistsModdersTechTypes(ref List<TechTypeData>[] TechnologyMatrix, Dictionary<string, CATEGORY> dictionary)
        {
            foreach (KeyValuePair<string, CATEGORY> pair in dictionary)
            {
                if (pair.Value == CATEGORY.BaseModule)
                {
                    continue;
                }

                if (TechTypeHandler.TryGetModdedTechType(pair.Key, out TechType techType))
                {
                    if (TechTypeExtensions.AsString(techType, false) == pair.Key)
                    {
                        TechnologyMatrix[(int)pair.Value].Add(new TechTypeData(techType, Language.main.Get(TechTypeExtensions.AsString(techType, false))));

                        addedTechTypes.Add(techType);

                        Debug.Log($"[CheatManager]:\n '{pair.Key}' found in TechTypeExtensions and added to TechMatrix.");
                    }
                }
            }
        } 
        

        public enum CATEGORY
        {
            Vehicles,
            Tools,
            Equipment,
            Materials,
            Electronics,
            Upgrades,
            FoodAndWater,
            LootAndDrill,
            Herbivores,
            Carnivores,
            Parasites,
            Leviathan,
            Eggs,
            SeaSeed,
            LandSeed,
            FloraItem,
            SeaSpawn,
            LandSpawn,
            Blueprints,
            BaseModule,
        };

        public static readonly Dictionary<string, CATEGORY> Known_AHK1221_TechTypes = new Dictionary<string, CATEGORY>
        {
            { "SeamothHullModule4", CATEGORY.Upgrades },
            { "SeamothHullModule5", CATEGORY.Upgrades },
            { "SeamothDrillModule", CATEGORY.Upgrades },
            { "SeamothThermalModule", CATEGORY.Upgrades },
        };


        public static readonly Dictionary<string, CATEGORY> Known_PrimeSonic_TechTypes = new Dictionary<string, CATEGORY>
        {
            { "SeaMothMk2", CATEGORY.Vehicles },
            { "SeaMothMk3", CATEGORY.Vehicles },
            { "ExosuitMk2", CATEGORY.Vehicles },
            { "SpeedModule", CATEGORY.Upgrades },
            { "VehiclePowerCore", CATEGORY.Electronics },
            { "CyclopsThermalChargerMk2", CATEGORY.Upgrades },
            { "CyclopsSolarCharger", CATEGORY.Upgrades },
            { "CyclopsSolarChargerMk2", CATEGORY.Upgrades },
            { "PowerUpgradeModuleMk2", CATEGORY.Upgrades },
            { "PowerUpgradeModuleMk3", CATEGORY.Upgrades },
            { "CyclopsSpeedModule", CATEGORY.Upgrades },
            { "CyclopsNuclearModule", CATEGORY.Upgrades },
            { "DepletedCyclopsNuclearModule", CATEGORY.Upgrades },
            { "CyclopsNuclearModuleRefil", CATEGORY.Upgrades },
            { "NuclearFabricator", CATEGORY.BaseModule },
            { "AuxCyUpgradeConsole", CATEGORY.BaseModule },
            { "VModFabricator", CATEGORY.BaseModule }
        };

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
                TechType.LithiumIonBattery,
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
    }
}
