using UnityEngine;

namespace Common
{
    public static class GameHelper
    {
        public static void ExecuteCommand(object message, object command)
        {
            if (message != null)
            {
                ErrorMessage.AddMessage(message.ToString());
            }

            if (command != null)
            {
                SNLogger.Log((string)command);
                DevConsole.SendConsoleCommand(command.ToString());
            }
        }

        public static bool IsPlayerInVehicle()
        {
            return Player.main.inSeamoth || Player.main.inExosuit ? true : false;
        }

        public static Vector3 ConvertStringPosToVector3(string target)
        {
            string[] numbers = target.Split(' ');

            return numbers.Length != 3 ? Vector3.zero : new Vector3(float.Parse(numbers[0]), float.Parse(numbers[1]), float.Parse(numbers[2]));
        }
               
        public static string Teleport(string targetName, string vector3string)
        {
            Vector3 currentWorldPos = Player.main.transform.position;

            string prevCwPos = string.Format("{0:D} {1:D} {2:D}", (int)currentWorldPos.x, (int)currentWorldPos.y, (int)currentWorldPos.z);

            if (IsPlayerInVehicle())
            {
                Player.main.GetVehicle().TeleportVehicle(ConvertStringPosToVector3(vector3string), Quaternion.identity);
                Player.main.CompleteTeleportation();
                ErrorMessage.AddMessage($"Vehicle and Player Warped to: {targetName}\n({vector3string})");
            }
            else
            {
                Player.main.SetPosition(ConvertStringPosToVector3(vector3string));
                Player.main.OnPlayerPositionCheat();
                ErrorMessage.AddMessage($"Player Warped to: {targetName}\n({vector3string})");
            }

            return prevCwPos;
        }

        public static Vector3 Teleport(string targetName, Vector3 targetPos)
        {
            Vector3 currentWorldPos = Player.main.transform.position;            

            if (IsPlayerInVehicle())
            {
                Player.main.GetVehicle().TeleportVehicle(targetPos, Quaternion.identity);
                Player.main.CompleteTeleportation();
                ErrorMessage.AddMessage($"Vehicle and Player Warped to: {targetName}\n({targetPos.ToString()})");
            }
            else
            {
                Player.main.SetPosition(targetPos);
                Player.main.OnPlayerPositionCheat();
                ErrorMessage.AddMessage($"Player Warped to: {targetName}\n({targetPos.ToString()})");
            }

            return currentWorldPos;
        }

        public static void SetProgressColor(Color color)
        {
            HandReticle.main.progressText.color = color;
            HandReticle.main.progressImage.color = color;
        }

        public static void SetInteractColor(Color color, bool isSetSecondary = true)
        {
            HandReticle.main.interactPrimaryText.color = color;

            if (isSetSecondary)
                HandReticle.main.interactSecondaryText.color = color;
        }
    }
}
