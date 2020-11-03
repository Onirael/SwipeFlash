using SwipeFlash.Core;
using System.IO;
using SoundTouch;
using NAudio.Wave;
using System.Threading.Tasks;

namespace SwipeFlash
{
    /// <summary>
    /// The audio player interface
    /// </summary>
    public interface IAudioPlayer
    {
        void PlaySound(object sender, SoundType soundType);
    }

    /// <summary>
    /// A class handling the audio of the application
    /// </summary>
    public class SoundManager : IAudioPlayer
    {
        #region Private Properties

        /// <summary>
        /// The sound processor
        /// </summary>
        private readonly SoundTouchProcessor mSoundProc = new SoundTouchProcessor();

        /// <summary>
        /// The audio samples provider, 
        /// used to read the audio stream
        /// </summary>
        private AudioSampleProvider sampleProvider;

        /// <summary>
        /// The audio player
        /// </summary>
        private readonly IWavePlayer wavePlayer;

        /// <summary>
        /// The consecutive successes
        /// </summary>
        private int mSuccessCombo = 0;

        #endregion

        #region Constructor

        public SoundManager()
        {
            // Gets the sound service
            var soundService = IoC.Get<SoundService>();

            // Hooks the play sound method to the event
            soundService.OnPlaySound += PlaySound;
            
            // Initializes the base audio objects
            wavePlayer = new WaveOutEvent();
            sampleProvider = new AudioSampleProvider();
            sampleProvider.SourceProvider = new WaveFileReader(Properties.Resources.Success).ToSampleProvider();
            wavePlayer.Init(sampleProvider);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Plays a sound of the given type
        /// </summary>
        /// <param name="soundType">The type of sound to play</param>
        public void PlaySound(object sender, SoundType soundType)
        {
            // Asynchronously plays the sound
            Task.Run(() =>
            {
                // The file stream to read
                Stream fileStream = null;
                // Whether a pitch modifier should be applied
                bool applyPitch = false;
                
                switch (soundType)
                {
                    // Success sound
                    case SoundType.Success:
                        fileStream = Properties.Resources.Success;
                        applyPitch = true;
                        // Increments the combo
                        mSuccessCombo++;
                        break;
                    // Fail sound
                    case SoundType.Fail:
                        fileStream = Properties.Resources.Wrong;
                        // Resets the combo
                        mSuccessCombo = 0;
                        break;
                    // No sound
                    default:
                        break;
                }

                // Creates a new stream reader
                var streamReader = new WaveFileReader(fileStream);
                // Changes the sample provider
                sampleProvider.SourceProvider = streamReader.ToSampleProvider();
                // If the pitch modifier should be applied
                if (applyPitch)
                    // Moves the pitch up by a semi-tone for each consecutive success
                    // The pitch moves up by a maximum of 10 semi-tones
                    sampleProvider.Pitch = mSuccessCombo % 10;
                // Plays the sound
                wavePlayer.Play(); 
            });
        }


        #endregion
    }
}
