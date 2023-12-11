using MyFirstGame.Engine.Input;

namespace MyFirstGame.States.Dev;

public class DevInputCommand : BaseInputCommand
{
    // Out of Game Commands
    public class DevQuit : DevInputCommand { }
    public class DevShoot : DevInputCommand { }
    public class DevExplode : DevInputCommand { }
    public class DevMissileExplode : DevInputCommand { }
    public class DevBulletSparks : DevInputCommand { }
}
