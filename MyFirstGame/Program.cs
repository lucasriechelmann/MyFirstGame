using MyFirstGame.Engine;
using MyFirstGame.States.Splash;

using var game = new MainGame(1280, 720, new SplashState());
game.Run();
