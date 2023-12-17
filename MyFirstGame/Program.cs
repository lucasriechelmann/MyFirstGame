using MyFirstGame.Engine;
using MyFirstGame.States.Splash;
using System;

using var game = new MainGame(1280, 720, new SplashState());
//using var game = new MainGame(1920, 1080, new SplashState());

//Used to force run at 60FPS
//game.IsFixedTimeStep = true;
//game.TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 60);
game.Run();
