using System;
using DynaStudios.UI.Screens;

namespace DynaStudios.UI
{
    public interface IScreenFactory
    {
        GameScreen CreateScreen(Type screenType);
    }
}