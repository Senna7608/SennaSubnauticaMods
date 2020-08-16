using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.GUIHelper;

namespace CheatManager
{
    public partial class CheatManager
    {
        protected readonly Dictionary<IntVector, string> WarpTargets_Internal = new Dictionary<IntVector, string>()
        {
            { new IntVector( 0, 0, 0),           "Safe Shallows"},
            { new IntVector( 0, 500, 0),         "Bungee Jumping (500m)"},
            { new IntVector( 0, 1000, 0),        "Bungee Jumping (1000m)"},
            { new IntVector( -710, 0, -1075 ),   "Floating Island"},
            { new IntVector( 378, 8, 1114),      "Gun Island"},
            { new IntVector( -841, -860, 418),   "Tree Cove (Lost River)"},
            { new IntVector( -594, -740, -72),   "Bones Field (Lost River)"},
            { new IntVector( 420, 132, 1185),    "Top Gun"},
            { new IntVector( 349, 158, 906),     "Hill Top"},
            { new IntVector( 0, -1930, 0),       "Underworld"},
            { new IntVector( -486, -500, 1326),  "Lifepod 2 (Blood Kelp Two)"},
            { new IntVector( -24, -15, 409),     "Lifepod 3 (Kelp Forest)"},
            { new IntVector( 710, -5, 152),      "Lifepod 4 (Crash Zone)"},
            { new IntVector( 370, -108, 306),    "Lifepod 6 (Grassy Plateaus)"},
            { new IntVector( -50, -170, -1036),  "Lifepod 7 (Crag Field)"},
            { new IntVector( 1103, -256, 562),   "Lifepod 12 (Koosh Zone)"},
            { new IntVector( -918, -166, 504),   "Lifepod 13 (Mushroom Forest)"},
            { new IntVector( -513, -83, -57),    "Lifepod 17 (Grassy Plateaus)"},
            { new IntVector( -816, -292, -877),  "Lifepod 19 (Sparse Reef Deep)"},
            { new IntVector( 285, -70, 444),     "Wreck 1 (Grassy Plateaus)"},
            { new IntVector( -690, -85, -35),    "Wreck 2 (Grassy Plateaus)"},
            { new IntVector( 929, -200, 598),    "Wreck 3 (Koosh Zone)"},
            { new IntVector( -78, -188, 847),    "Wreck 4 (Underwater Islands)"},
            { new IntVector( 740, -346, 1220),   "Wreck 5 (Mountains)"},
            { new IntVector( -1448, -332, 723),  "Wreck 6 (Dunes)"},
            { new IntVector( -1148, -166, -706), "Wreck 7 (Sea Treader Path)"},
            { new IntVector( -1223, -346, -356), "Wreck 8 (Blood Kelp Trench)"},
            { new IntVector( -672, -113, 750),   "Wreck 9 (Mushroom Forest)"},
            { new IntVector( -290, -222, -773),  "Wreck 10 (Grand Reef)"},
            { new IntVector( -865, -430, -1390), "Wreck 11 (Grand Reef)"},
            { new IntVector( -393, -112, 630),   "Wreck 12 (Grassy Plateaus)"},
            { new IntVector( -15, -96, -624),    "Wreck 13 (Grassy Plateaus)"},
            { new IntVector( -418, -94, -261),   "Wreck 14 (Grassy Plateaus)"},
            { new IntVector( -33, -25, -398),    "Wreck 15 (Safe Shallows)"},
            { new IntVector( 343, -16, -190),    "Wreck 16 (Safe Shallows)"},
            { new IntVector( 64, -24, 376),      "Wreck 17 (Kelp Forest)"},
            { new IntVector( -317, -73, 222),    "Wreck 18 (Kelp Forest)"},
            { new IntVector( 1060, -266, 1353),  "Wreck 19 (Mountains)"},
            { new IntVector( -805, -209, -718),  "Wreck 20 (Sparse Reef)"},
            { new IntVector( 1145, 0, 142),      "Aurora Entry Point"},
            { new IntVector( -772, 17, -1109),   "Degasi Base 1 (Floating Island)"},
            { new IntVector( -804, 79, -1053),   "Degasi Base 1a (Floating Island)"},
            { new IntVector( -703, 77, -1163),   "Degasi Base 1b (Floating Island)"},
            { new IntVector( 87, -262, -357),    "Degasi Base 2 (Jellyshroom Caves)"},
            { new IntVector( -632, -500, -934),  "Degasi Base 3 (Deep Grand Reef)"},
            { new IntVector( 343, 64, 902),      "Precursor Gate: Mountains to Floating Island"},
            { new IntVector( 170, -1428, -367),  "Precursor Gate: Prison to Crag Field"},
            { new IntVector( 190, -1428, -405),  "Precursor Gate: Prison to Lost River: Ghost Tree"},
            { new IntVector( 333, -1428, -276),  "Precursor Gate: Prison to Koosh Zone"},
            { new IntVector( 353, -1428, -314),  "Precursor Gate: Prison to Mushroom Forest"},
            { new IntVector( 248, -1588, -316),  "Precursor Gate: Prison Aquarium to Gun Outer"},
            { new IntVector( -35, -1212, 113),   "Precursor Gate: Lava Castle to Gun Inner"},
            { new IntVector( 386, -90, 1137),    "Precursor Gate: Gun Inner"},
            { new IntVector( -662, 5, -1077),    "Precursor Gate: Floating Island"},
            { new IntVector( -943, -620, 1019),  "Precursor Gate: Lost River: Ghost Tree"},
            { new IntVector( 1383, -298, 762),   "Precursor Gate: Koosh Zone"},
            { new IntVector( -803, -227, 408),   "Precursor Gate: Mushroom Forest"},
            { new IntVector( -80, -290, -1363),  "Precursor Gate: Crag Field"},
            { new IntVector( -222, -791, 320),   "Precursor Base (Lost River)"},
            { new IntVector( -32, -1201, 66),    "Precursor Base (Lava Castle)"},
            { new IntVector( 210, -1448, -250),  "Precursor Prison (Lava Lakes)"},
            { new IntVector( -1112, -684, -653), "Precursor Cave (Lost River: Skeleton Cave)"},
            { new IntVector( -882, -305, -786),  "Precursor Cache (Sparse Reef)"},
            { new IntVector( -1182, -378, 1128), "Precursor Cache (Dunes)"},
            { new IntVector( -596, -555, 1480),  "Precursor Cache (Blood Kelp Two)"}            
        };

        public readonly Dictionary<IntVector, string> WarpTargets_User = new Dictionary<IntVector, string>();

        public List<string> GetWarpTargetNames()
        {
            List<string> targets = new List<string>();

            foreach (string name in WarpTargets_Internal.Values)
            {
                targets.Add(name);
            }

            foreach (string name in WarpTargets_User.Values)
            {
                targets.Add(name);
            }

            return targets;
        }

        public IntVector GetIntVector(int index)
        {
            if (index >= WarpTargets_Internal.Count)
            {
                return WarpTargets_User.Keys.ElementAt(index - WarpTargets_Internal.Count);
            }
            else
            {
                return WarpTargets_Internal.Keys.ElementAt(index);
            }
        }

        public Vector3 ConvertStringPosToVector3(string target)
        {
            string[] numbers = target.Split(' ');

            return numbers.Length != 3 ? Vector3.zero : new Vector3(float.Parse(numbers[0]), float.Parse(numbers[1]), float.Parse(numbers[2]));
        }

        public string Teleport(string targetName, string vector3string)
        {
            Vector3 currentWorldPos = Player.main.transform.position;

            string prevCwPos = string.Format("{0:D} {1:D} {2:D}", (int)currentWorldPos.x, (int)currentWorldPos.y, (int)currentWorldPos.z);

            if (IsPlayerInVehicle())
            {
                Player.main.GetVehicle().TeleportVehicle(ConvertStringPosToVector3(vector3string), Quaternion.identity);
                Player.main.CompleteTeleportation();
                ErrorMessage.AddMessage($"Vehicle and Player Warped to:\n{targetName}\n({vector3string})");
            }            
            else
            {
                Player.main.SetPosition(ConvertStringPosToVector3(vector3string));
                Player.main.OnPlayerPositionCheat();
                ErrorMessage.AddMessage($"Player Warped to:\n{targetName}\n({vector3string})");
            }

            return prevCwPos;
        }


        public Vector3 Teleport(string targetName, Vector3 targetPos)
        {
            Vector3 currentWorldPos = Player.main.transform.position;

            if (IsPlayerInVehicle())
            {
                Player.main.GetVehicle().TeleportVehicle(targetPos, Quaternion.identity);
                Player.main.CompleteTeleportation();
                ErrorMessage.AddMessage($"Vehicle and Player Warped to:\n{targetName}\n({targetPos.ToString()})");
            }            
            else
            {
                Player.main.SetPosition(targetPos);
                Player.main.OnPlayerPositionCheat();
                ErrorMessage.AddMessage($"Player Warped to:\n{targetName}\n({targetPos.ToString()})");
            }

            return currentWorldPos;
        }

        public IntVector Teleport(string targetName, IntVector targetPos)
        {
            IntVector currentWorldPos = Player.main.transform.position;

            if (IsPlayerInVehicle())
            {
                Player.main.GetVehicle().TeleportVehicle(targetPos.ToVector3(), Quaternion.identity);
                Player.main.CompleteTeleportation();
                ErrorMessage.AddMessage($"Vehicle and Player Warped to:\n{targetName}\n({targetPos.ToString()})");
            }            
            else
            {
                Player.main.SetPosition(targetPos.ToVector3());
                Player.main.OnPlayerPositionCheat();
                ErrorMessage.AddMessage($"Player Warped to:\n{targetName}\n({targetPos.ToString()})");
            }

            return currentWorldPos;
        }

        public void AddToList(IntVector worldPosition)
        {
            if (WarpTargets_Internal.ContainsKey(worldPosition))
            {
                ErrorMessage.AddMessage("CheatManager message:\nThis position is already exist in internal Warp list.");
            }
            else if (WarpTargets_User.ContainsKey(worldPosition))
            {
                ErrorMessage.AddMessage("CheatManager message:\nThis position is already exist in user Warp list.");
            }
            else
            {
                string name = $"User_{WarpTargets_User.Count + 1}_{Player.main.GetBiomeString()}";

                WarpTargets_User.Add(worldPosition, name);

                scrollItemsList[tMatrix.Length].AddGuiItemToGroup(name);

                ErrorMessage.AddMessage($"CheatManager message:\nPosition added to user Warp list with name:\n{name}.");
            }
        }

        public void RemoveFormList(IntVector key)
        {
            if (WarpTargets_User.ContainsKey(key))
            {
                //print($"WarpTargets_User.Count before: {WarpTargets_User.Count}");

                WarpTargets_User.Remove(key);

                ErrorMessage.AddMessage($"CheatManager message:\nTarget position [{key}] removed from user Warp list.");

                //print($"WarpTargets_User.Count after: {WarpTargets_User.Count}");
            }
        }

        public bool IsPositionWithinRange(IntVector position, out string nearestWarpPoint)
        {
            foreach (KeyValuePair<IntVector, string> kvpInternal in WarpTargets_Internal)
            {
                if (IntVector.Distance(kvpInternal.Key, position) < 50)
                {
                    nearestWarpPoint = kvpInternal.Value;
                    return true;
                }
            }

            foreach (KeyValuePair<IntVector, string> kvpUser in WarpTargets_User)
            {
                if (IntVector.Distance(kvpUser.Key, position) < 50)
                {
                    nearestWarpPoint = kvpUser.Value;
                    return true;
                }
            }

            nearestWarpPoint = string.Empty;
            return false;
        }

    }
}
