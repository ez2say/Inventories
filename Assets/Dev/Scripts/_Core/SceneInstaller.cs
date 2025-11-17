using InventorySystem.Contracts;
using UnityEngine;
using Zenject;

namespace InventorySystem
{
    public class SceneInstaller : MonoInstaller
    {
        [Header("Configs")]
        [SerializeField] private InventoryConfig _inventoryConfig;
        [SerializeField] private ItemDatabase _itemDatabase;
        [SerializeField] private InventoryGenerationConfig _generationConfig;

        [Header("References")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private ItemTooltip _itemTooltipPrefab;

        public override void InstallBindings()
        {
            Container.Bind<InventoryConfig>().FromInstance(_inventoryConfig).AsSingle();
            Container.Bind<ItemDatabase>().FromInstance(_itemDatabase).AsSingle();
            Container.Bind<InventoryGenerationConfig>().FromInstance(_generationConfig).AsSingle();


            Container.Bind<Canvas>().FromInstance(_canvas).AsSingle();
            
            Container.Bind<IItemGeneratorService>().To<ItemGeneratorService>().AsSingle();
            
            Container.Bind<IItemService>().To<ItemService>().AsSingle();
            Container.Bind<IInventoryGenerator>().To<InventoryGenerator>().AsSingle();
            Container.Bind<IInventoryService>().To<InventoryService>().AsSingle();

            Container.Bind<IInventoryView>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IDragService>().To<DragService>().AsSingle();
            Container.Bind<IDropHandler>().To<InventoryDropHandler>().AsSingle();

            Container.Bind<IInventory>().FromMethod(CreateInventory).AsSingle();


            Container.Bind<ItemTooltip>()
                .FromComponentInNewPrefab(_itemTooltipPrefab)
                .UnderTransform(_canvas.transform)
                .AsSingle();
        }

        private IInventory CreateInventory(InjectContext context)
        {
            var inventoryService = context.Container.Resolve<IInventoryService>();
            inventoryService.Initialize();
            return inventoryService.PlayerInventory;
        }
    }
}