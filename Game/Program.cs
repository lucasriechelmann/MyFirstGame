using Engine2D;
using Game.Content;
using Game.States.Splash;
using System;
using System.Globalization;

const bool DEBUG = true;

const int WIDTH = 1280;
const int HEIGHT = 720;

const string ENGLISH = "en";
const string FRENCH = "fr";
const string JAPANESE = "ja";

Strings.Culture = CultureInfo.CurrentCulture;
//Strings.Culture = CultureInfo.GetCultureInfo(JAPANESE);

using (var game = new MainGame(WIDTH, HEIGHT, new SplashState(), DEBUG))
//using (var game = new MainGame(WIDTH, HEIGHT, new TestCameraState(), DEBUG))
{
    game.IsFixedTimeStep = true;
    game.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 50);
    game.Run();
}
