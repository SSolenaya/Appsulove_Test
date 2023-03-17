using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<EngineManager>()
                .To<EngineManager>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("TimeManager")
                .AsSingle()
                .NonLazy();

            Container.Bind<PlayerDataManager>().AsSingle().NonLazy();
            Container.Bind<SquaresController>().AsSingle().NonLazy();
            Container.Bind<CirclesController>().AsSingle().NonLazy();
        }
    }
}