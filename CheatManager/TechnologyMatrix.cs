using System;
using System.Collections.Generic;
using Common;
using Common.Helpers;
using SMLExpander;

namespace CheatManager
{
    public enum TechCategory
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
        Warp,
        BaseModule,
    };    

    public class TechnologyMatrix
    {
        public class TechTypeSearch
        {
            readonly TechType _techType;

            public TechTypeSearch(TechType techType)
            {
                _techType = techType;
            }

            public bool EqualsWith(TechTypeData techTypeData)
            {
                return techTypeData.TechType == _techType;
            }
        }      
        
        public void InitTechMatrixList(ref List<TechTypeData>[] TechnologyMatrix)
        {
            int i = 0;

            foreach (KeyValuePair<TechCategory, TechType[]> kvp in baseTechMatrix)
            {
                if (Enum.IsDefined(typeof(Categories), (int)kvp.Key))
                {
                    TechnologyMatrix[i] = new List<TechTypeData>();

                    for (int j = 0; j < kvp.Value.Length; j++)
                    {
                        string name;
                        TechType techType = kvp.Value[j];
                        switch (techType)
                        {
                            case TechType.SeaEmperorBaby:
                                name = Language.main.Get(TechTypeExtensions.AsString(TechType.SeaEmperorJuvenile, false));
                                break;
                            case TechType.SeaEmperorJuvenile:
                                name = Language.main.Get(TechTypeExtensions.AsString(TechType.SeaEmperorBaby, false));
                                break;
                            default:
                                name = Language.main.Get(TechTypeExtensions.AsString(kvp.Value[j], false));
                                break;
                        }
                        TechnologyMatrix[i].Add(new TechTypeData(techType, name));
                    }

                    i++;
                }
            }
        }

        public void SortTechLists(ref List<TechTypeData>[] TechnologyMatrix)
        {
            foreach (List<TechTypeData> item in TechnologyMatrix)
            {
                item.Sort();
            }            
        }                

        public void GetModdedTechTypes(ref List<TechTypeData>[] TechnologyMatrix)
        {
            ModdedTechTypeHelper mHelper = new ModdedTechTypeHelper();

            foreach (KeyValuePair<string, TechType> kvp in mHelper.FoundModdedTechTypes)
            {
                EquipmentType equipmentType = mHelper.TypeDefCache[kvp.Value];

                switch (equipmentType)
                {
                    case EquipmentType.CyclopsModule:
                    case EquipmentType.ExosuitArm:
                    case EquipmentType.ExosuitModule:                    
                    case EquipmentType.SeamothModule:                    
                    case EquipmentType.VehicleModule:
                    case (EquipmentType)100:
                        TechnologyMatrix[(int)TechCategory.Upgrades].Add(new TechTypeData(kvp.Value, Language.main.Get(TechTypeExtensions.AsString(kvp.Value, false))));
                        break;
                    case EquipmentType.Hand:
                        TechnologyMatrix[(int)TechCategory.Tools].Add(new TechTypeData(kvp.Value, Language.main.Get(TechTypeExtensions.AsString(kvp.Value, false))));
                        break;
                    case EquipmentType.Head:
                    case EquipmentType.Body:
                    case EquipmentType.Gloves:
                    case EquipmentType.Foots:
                    case EquipmentType.Tank:
                    case EquipmentType.Chip:
                        TechnologyMatrix[(int)TechCategory.Equipment].Add(new TechTypeData(kvp.Value, Language.main.Get(TechTypeExtensions.AsString(kvp.Value, false))));
                        break;
                }
            }

            SNLogger.Debug("Modded TechTypes checked and added.");
        }       

        public readonly Dictionary<TechCategory, TechType[]> baseTechMatrix = new Dictionary<TechCategory, TechType[]>()
        {
            #region Vehicles
            {
                TechCategory.Vehicles,

                new TechType[]
                {
                    TechType.Seamoth,
                    TechType.Exosuit,
                    TechType.Cyclops,
                    TechType.RocketBase
                }
            },
            #endregion

            #region Tools
            {
                TechCategory.Tools,

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
                }
            },
            #endregion

            #region Equipment
            {
                TechCategory.Equipment,

                new TechType[]
                {
                    TechType.RadiationSuit,
                    TechType.RadiationHelmet,
                    TechType.RadiationGloves,
                    TechType.ReinforcedDiveSuit,
                    TechType.ReinforcedGloves,
                    //TechType.Stillsuit,
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
                }
            },
            #endregion

            #region Materials
            {
                TechCategory.Materials,

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
                    TechType.SeaTreaderPoop,
                    TechType.JeweledDiskPiece
                }
            },
            #endregion

            #region Electronics
            {
                TechCategory.Electronics,

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
                }
            },
            #endregion

            #region Upgrades
            {
                TechCategory.Upgrades,

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
                }
            },
            #endregion

            #region Food and Water
            {
                TechCategory.FoodAndWater,

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
                    //TechType.StillsuitWater,
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
                }
            },
            #endregion

            #region Loot and Drill
            {
                TechCategory.LootAndDrill,

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
                }
            },
            #endregion

            #region Herbivores
            {
                TechCategory.Herbivores,

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
                }
            },
            #endregion

            #region Carnivores
            {
                TechCategory.Carnivores,

                new TechType[]
                {                    
                    TechType.RockPuncher,                    
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
                }
            },
            #endregion

            #region Parasites
            {
                TechCategory.Parasites,

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
                }
            },
            #endregion

            #region Leviathan
            {
                TechCategory.Leviathan,

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
                }
            },
            #endregion

            #region Eggs
            {
                TechCategory.Eggs,

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
                }
            },
            #endregion

            #region Sea: Seed
            {
                TechCategory.SeaSeed,

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
                    TechType.SnakeMushroomSpore,
                    TechType.CreepvineSeedCluster
                }
            },
            #endregion

            #region Land: Seed
            {
                TechCategory.LandSeed,

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
                }
            },
            #endregion

            #region Flora: Item
            {
                TechCategory.FloraItem,

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
                }
            },
            #endregion

            #region Sea: Spawn
            {
                TechCategory.SeaSpawn,

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
                }
            },
            #endregion

            #region Land: Spawn
            {
                TechCategory.LandSpawn,

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
                }
            },
            #endregion

            #region Blueprints
            {
                TechCategory.Blueprints,

                new TechType[]
                {       
                    TechType.AdvancedWiringKit,
                    TechType.Aerogel,
                    TechType.AirBladder,
                    //TechType.Aquarium,
                    TechType.AramidFibers,

                    TechType.BaseBioReactor,
                    TechType.BaseBulkhead,
                    TechType.BaseFiltrationMachine,
                    TechType.BaseMapRoom,
                    TechType.BaseMoonpool,
                    TechType.BaseNuclearReactor,
                    TechType.BaseObservatory,
                    TechType.BasePlanter,
                    TechType.BaseRoom,
                    TechType.BaseUpgradeConsole,
                    TechType.BaseWaterPark,
                    TechType.BatteryCharger,
                    TechType.Bench,
                    TechType.Benzene,                    
                    
                    TechType.Compass,
                    TechType.Constructor,
                    TechType.Cyclops,
                    
                    TechType.Exosuit,
                    TechType.ExosuitDrillArmModule,
                    TechType.ExosuitGrapplingArmModule,
                    TechType.ExosuitJetUpgradeModule,
                    TechType.ExosuitPropulsionArmModule,
                    TechType.ExosuitTorpedoArmModule,

                    TechType.FarmingTray,
                    TechType.FiberMesh,                    

                    TechType.Gravsphere,

                    TechType.HeatBlade,
                    TechType.HighCapacityTank,
                    TechType.HydrochloricAcid,

                    TechType.Knife,

                    TechType.LaserCutter,
                    TechType.LEDLight,                    
                    //TechType.Locker,
                    TechType.Lubricant,                     
                                        
                    TechType.PictureFrame,
                    TechType.PlanterBox,
                    TechType.PlanterPot2,
                    TechType.PlanterPot3,
                    TechType.PlanterPot,
                    TechType.PlanterShelf,
                    TechType.PlasteelTank,
                    TechType.Polyaniline,
                    TechType.PowerCell,
                    TechType.PowerCellCharger,
                    TechType.PowerTransmitter,
                    TechType.PrecursorIonBattery,
                    TechType.PrecursorIonPowerCell,
                    TechType.PropulsionCannon,

                    TechType.RadiationSuit,
                    //TechType.Radio,
                    TechType.ReinforcedDiveSuit,
                    TechType.Rebreather,
                    TechType.RocketBaseLadder,
                    TechType.RocketStage1,
                    TechType.RocketStage2,
                    TechType.RocketStage3,
                    TechType.BaseReinforcement,
                    
                    TechType.Seaglide,
                    TechType.Seamoth,
                    //TechType.Sign,
                    //TechType.SmallLocker,
                    TechType.Spotlight,                    
                    TechType.StarshipChair,
                    TechType.StarshipDesk,
                    TechType.StasisRifle,
                    //TechType.Stillsuit,
                    TechType.SwimChargeFins,

                    TechType.Techlight,
                    TechType.Terraformer,
                    TechType.ThermalPlant,
                    TechType.Thermometer,
                    TechType.Transfuser,
                    TechType.Trashcans,
                    
                    TechType.UltraGlideFins,

                    TechType.VendingMachine,

                    TechType.Workbench                    
                }
            }
            #endregion
        };
    }
}
