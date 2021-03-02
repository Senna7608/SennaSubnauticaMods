using UnityEngine;

namespace Common.Helpers.SMLHelpers
{
    /// <summary>
    /// Used to set up the encyclopedia data.
    /// </summary>
    public class EncyData
    {
        /// <summary>
        /// The title of the encyclopedia.
        /// </summary>
        public string title;

        /// <summary>
        /// The description of the encyclopedia.
        /// </summary>
        public string description;

        /// <summary>
        /// The encyclopedia class of the item.
        /// </summary>
        /// <remarks>
        /// See also <see cref="EncyNode"/>.
        /// </remarks>
        public EncyNode node;

        /// <summary>
        /// The encyclopedia image of the item.
        /// </summary>
        public Texture2D image;

        /// <summary>
        /// The EncyData constructor.
        /// </summary>
        public EncyData()
        {
        }

        /// <summary>
        /// The EncyData constructor with parameters.
        /// </summary>
        public EncyData(string encyTitle, string encyText, EncyNode encyNode, Texture2D encyPic)
        {
            title = encyTitle;
            description = encyText;
            node = encyNode;
            image = encyPic;
        }
    }
}