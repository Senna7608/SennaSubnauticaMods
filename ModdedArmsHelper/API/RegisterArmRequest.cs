using ModdedArmsHelper.API.Interfaces;

namespace ModdedArmsHelper.API
{
    /// <summary>
    /// The required class to register the new modded arm.   
    /// </summary>
    public class RegisterArmRequest
    {
        /// <summary>
		/// The base instance of the <see cref="CraftableModdedArm"/> class.       
		/// </summary>     
        public CraftableModdedArm craftableModdedArm;

        /// <summary>
        /// The modded arm setting interface. 
        /// </summary>
        public IArmModdingRequest armModdingRequest;

        /// <summary>
        /// The basic constructor of this class. 
        /// </summary>
        public RegisterArmRequest(CraftableModdedArm craftableModdedArm, IArmModdingRequest armModdingRequest)
        {
            this.craftableModdedArm = craftableModdedArm;
            this.armModdingRequest = armModdingRequest;
        }
    }
}
