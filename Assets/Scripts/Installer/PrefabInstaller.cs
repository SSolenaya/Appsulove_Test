using Assets.Scripts;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PrefabInstaller", menuName = "ScriptableObjects/PrefabInstaller", order = 1)]
public class PrefabInstaller : ScriptableObjectInstaller
{
    [SerializeField] private Factory _factory;
    [SerializeField] private MainCanvas _mainCanvas;
    [SerializeField] private Field _field;
    [SerializeField] private UIController _uiController;
    [SerializeField] private Settings _settings;
    
    public override void InstallBindings()
    {
        Container.Bind<Factory>().FromInstance(_factory).AsSingle().NonLazy(); 
        Container.Bind<Settings>().FromInstance(_settings).NonLazy();

        Container.Bind<MainCanvas>()
            .FromComponentInNewPrefab(_mainCanvas)
            .WithGameObjectName("MainCanvas")
            .AsSingle()
            .NonLazy();

        Container.Bind<Field>()
            .FromComponentInNewPrefab(_field)
            .WithGameObjectName("Field")
            .AsSingle()
            .NonLazy();

        Container.Bind<UIController>()
            .FromComponentInNewPrefab(_uiController)
            .WithGameObjectName("UIController")
            .AsSingle()
            .NonLazy();
    }
}