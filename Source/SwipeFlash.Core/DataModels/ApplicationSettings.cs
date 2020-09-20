using System;

namespace SwipeFlash.Core
{
    /// <summary>
    /// A struct containing the settings for the application
    /// </summary>
    [Serializable()]
    public struct ApplicationSettings
    {
        #region Public Properties

        /// <summary>
        /// Whether the flashcard illustrations are enabled
        /// </summary>
        public bool IllustrationsEnabled; 

        #endregion

        #region Constructor

        /// <summary>
        /// The default constructor
        /// </summary>s
        public ApplicationSettings(bool illustrationsEnabled)
        {
            IllustrationsEnabled = illustrationsEnabled;
        }

        #endregion
    }
}
