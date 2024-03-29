﻿#define Raudio

using Raylib_CsLo;
using Trinkit.Debug;

namespace Trinkit
{
    public abstract class TrinkitApp : IDisposable
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static TrinkitApp Instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// The application name.
        /// </summary>
        public string Name = "";

        /// <summary>
        /// The current scene the application is running.
        /// </summary>
        public Scene? CurrentScene;

        private Image _icon;

        /// <summary>
        /// All components in the application, destroyed once the game is exited.
        /// </summary>
        public List<Component> AllComponents = new List<Component>();

        public TrinkitApp(string title, int width, int height, bool resizable = false)
        {
            Instance = this;

            Name = title;
            ConfigFlags flags = ConfigFlags.FLAG_WINDOW_ALWAYS_RUN | ConfigFlags.FLAG_VSYNC_HINT;
            if (resizable) flags |= ConfigFlags.FLAG_WINDOW_RESIZABLE;

            Raylib.SetConfigFlags(flags);
            Raylib.SetTraceLogLevel((int)rlTraceLogLevel.RL_LOG_ERROR);
            Raylib.InitWindow(width, height, Name);

            _icon = Raylib.LoadImage("Resources/icon.png");
            Raylib.SetWindowIcon(_icon);

#if Raudio
            Raylib_CsLo.Raylib.InitAudioDevice();
#endif
        }

        public void Run()
        {
            OnStart();

            while (!Raylib.WindowShouldClose())
            {
                Counters.Reset();

                OnUpdate();
                Raylib.BeginDrawing();
                OnDraw();
                Raylib.EndDrawing();
            }
        }

        public abstract void OnStart();
        public abstract void OnUpdate();
        public abstract void OnDraw();
        public abstract void OnQuit();

        public void Dispose()
        {
            OnQuit();

            Raylib.UnloadImage(_icon);

            for (int i = 0; i < AllComponents.Count; i++)
            {
                AllComponents[i].Dispose();
            }
            AllComponents.Clear();

#if Raudio
            Raylib_CsLo.Raylib.CloseAudioDevice();
#endif


            Raylib.CloseWindow();
        }
    }
}
