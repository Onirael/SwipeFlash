using NAudio.Wave;
using SoundTouch;

namespace SwipeFlash
{
    /// <summary>
    /// An audio sample provider used to read other sample providers' data
    /// </summary>
    class AudioSampleProvider : ISampleProvider
    {
        #region Public Properties

        /// <summary>
        /// The wave format of the audio
        /// </summary>
        public WaveFormat WaveFormat => SourceProvider.WaveFormat;

        /// <summary>
        /// The pitch shift (in semitones) of the audio to play
        /// </summary>
        public double Pitch
        {
            get => soundProc.PitchSemiTones;
            set => soundProc.PitchSemiTones = value;
        }

        private ISampleProvider _sourceProvider = null;
        /// <summary>
        /// The sample source provider
        /// </summary>
        public ISampleProvider SourceProvider
        {
            get => _sourceProvider;
            set
            {
                var prevValue = _sourceProvider;
                _sourceProvider = value;

                if (prevValue != value)
                {
                    Init(value);
                }
            }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// The buffer to read from
        /// </summary>
        private float[] mSourceReadBuffer;

        /// <summary>
        /// The buffer to write to
        /// </summary>
        private float[] mSoundTouchReadBuffer;

        /// <summary>
        /// The sound processor
        /// </summary>
        private SoundTouchProcessor soundProc;

        #endregion

        #region Public Methods

        /// <summary>
        /// Reads the audio data
        /// </summary>
        /// <param name="buffer">The buffer to write to</param>
        /// <param name="offset">The read offset</param>
        /// <param name="count">The amount of samples to read</param>
        /// <returns>The amount of read samples</returns>
        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = 0;
            bool reachedEndOfSource = false;
            // While the end of the audio hasn't been reached
            while (samplesRead < count)
            {
                // If there are samples available
                if (soundProc.AvailableSamples == 0)
                {
                    // Read the samples from the source provider
                    var readFromSource = SourceProvider.Read(mSourceReadBuffer, 0, mSourceReadBuffer.Length);
                    // If there are samples to read
                    if (readFromSource > 0)
                    {
                        // Put the samples in the sound processor
                        soundProc.PutSamples(mSourceReadBuffer, readFromSource / WaveFormat.Channels);
                    }
                    else
                    {
                        // Signal that the end of the source has been reached
                        reachedEndOfSource = true;
                        // Tell SoundTouch we are done
                        soundProc.Flush();
                    }
                }

                var desiredSampleFrames = (count - samplesRead) / WaveFormat.Channels;

                // Gets the output samples from the processor
                var received = soundProc.ReceiveSamples(mSoundTouchReadBuffer, desiredSampleFrames) * WaveFormat.Channels;
                // use loop instead of Array.Copy due to WaveBuffer
                for (int n = 0; n < received; n++)
                {
                    buffer[offset + samplesRead++] = mSoundTouchReadBuffer[n];
                }
                if (received == 0 && reachedEndOfSource) break;
            }

            // Returns the amount of samples that were read
            return samplesRead;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Initializes the buffers and the sound processor given a sample provider
        /// </summary>
        /// <param name="sourceProvider">The sample provider to use</param>
        private void Init(ISampleProvider sourceProvider)
        {
            // Creates the buffers
            mSourceReadBuffer = new float[(WaveFormat.SampleRate * WaveFormat.Channels * 100) / 1000];
            mSoundTouchReadBuffer = new float[mSourceReadBuffer.Length * 10];

            // Creates a new sound processor
            soundProc = new SoundTouchProcessor()
            {
                SampleRate = WaveFormat.SampleRate,
                Channels = WaveFormat.Channels,
            };
        }

        #endregion
    }
}