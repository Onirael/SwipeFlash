using System;
using System.ComponentModel;

namespace SwipeFlash.Core
{
    /// <summary>
    /// An interface used to manage sounds
    /// </summary>
    public interface ISoundService
    {
        void PlaySound(SoundType soundType);
    }

    /// <summary>
    /// A class containing helper methods to manage sounds
    /// </summary>
    public class SoundService : ISoundService
    {
        /// <summary>
        /// Whether the sounds are enabled 
        /// </summary>
        private bool mSoundsEnabled = true;

        #region Event Handlers

        /// <summary>
        /// An event handler fired when a window is being created
        /// </summary>
        public EventHandler<SoundType> OnPlaySound { get; set; }

        #endregion

        #region Constructor

        public SoundService()
        {
            // Gets the value of the sounds enabled settings
            mSoundsEnabled = Properties.Settings.Default.SoundsEnabled;

            // Hooks the settings changed method to the event
            Properties.Settings.Default.PropertyChanged += OnSettingsChanged;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Plays a sound of the given type
        /// </summary>
        /// <param name="soundType"></param>
        public void PlaySound(SoundType soundType)
        {
            // If sounds are disabled, quit
            if (!mSoundsEnabled)
                return;

            // Play the sound
            OnPlaySound?.Invoke(this, soundType);
        }

        #endregion
        
        #region Private Helpers

        /// <summary>
        /// Called when the settings have been modified by the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Properties.Settings.Default.SoundsEnabled):
                    // Updates the local value
                    mSoundsEnabled = Properties.Settings.Default.SoundsEnabled;
                    return;
            }
        }

        #endregion
    }
}
