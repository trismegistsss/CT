using UnityEngine;
using Zenject;


namespace CT.Core.Installer
{
    [CreateAssetMenu(fileName = "CtSettingsInstaller", menuName = "Zenject/Installers/CtSettingsInstaller")]
    public class GameInstallerSettings : ScriptableObjectInstaller<GameInstallerSettings>
    {
        public GameInstaller.SystemModules SystemModules;
        public GameInstaller.GamePlayItems GamePlayItems;
        public GameInstaller.ParticlesItems ParticlesItems;

        public override void InstallBindings()
        {
            Container.BindInstance(SystemModules);
            Container.BindInstance(GamePlayItems);
            Container.BindInstance(ParticlesItems);          
        }
    }
}