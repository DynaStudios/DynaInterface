using System;
using DynaStudios.UI.Screens;

namespace DynaStudios.UI
{
    public class ScreenFactory : IScreenFactory
    {
        public GameScreen CreateScreen(Type screenType)
        {
            return Activator.CreateInstance(screenType) as GameScreen;
        }
    }
}