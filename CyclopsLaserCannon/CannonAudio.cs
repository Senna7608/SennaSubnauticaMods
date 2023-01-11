using FMOD;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using UnityEngine;

namespace CyclopsLaserCannonModule
{
    public static class CannonAudio
    {
        public static AssetBundle assetBundle;
        public static AudioSource audioSource;

        public const string sennaSFXBus = "bus:/senna/SFX";

        public const MODE k3DSoundModes = MODE.DEFAULT | MODE._3D | MODE.ACCURATETIME | MODE._3D_LINEARSQUAREROLLOFF;        

        public static void Patch()
        {
            assetBundle = AssetBundle.LoadFromFile($"{Main.modFolder}/Assets/laser_sounds");

            GameObject laser_sound = Object.Instantiate(assetBundle.LoadAsset<GameObject>("turret_sound"));

            audioSource = laser_sound.GetComponent<AudioSource>();

            
            /*
            Sound sound = AudioUtils.CreateSound(audioSource.clip, k3DSoundModes);

            sound.set3DMinMaxDistance(0, 50);
            
            CustomSoundHandler.RegisterCustomSound("turret_sound", sound, AudioUtils.BusPaths.UnderwaterAmbient);

            //Object.DestroyImmediate(laser_sound);
            */
        }        
    }
}
