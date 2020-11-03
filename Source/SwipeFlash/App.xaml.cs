﻿using System.Windows;
using SwipeFlash.Core;

namespace SwipeFlash
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The application window manager
        /// </summary>
        private WindowManager mWindowManager;

        /// <summary>
        /// The application audio manager
        /// </summary>
        private SoundManager mSoundManager;

        /// <summary>
        /// Custom startup so we load our IoC immediately before anything else
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Let the base application do what it needs
            base.OnStartup(e);

            // Setup Ioc
            IoC.Setup();

            // Creates the managers
            mWindowManager = new WindowManager();
            mSoundManager = new SoundManager();

            // Show the main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }
    }
}
