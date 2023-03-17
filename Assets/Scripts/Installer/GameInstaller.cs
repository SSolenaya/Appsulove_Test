using Assets.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MovementLogic>().AsSingle();
            Container.BindInterfacesAndSelfTo<CirclesController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SquaresController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CountdownTimer>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerDataManager>().AsSingle().NonLazy();

            Container.Bind<EngineManager>()
                .To<EngineManager>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("TimeManager")
                .AsSingle()
                .NonLazy();
        }
    }
}