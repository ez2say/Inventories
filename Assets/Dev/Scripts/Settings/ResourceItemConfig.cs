using UnityEngine;

namespace InventorySystem
{
    public enum ResourceType
    {
        Wood,
        Stone,
        Iron,
        Stick
    }


    [CreateAssetMenu(fileName = "ResourceConfig", menuName = "Inventory/Resource Config")]
    public class ResourceItemConfig : ItemConfig
    {
        [SerializeField] private ResourceType _resourceType;

        public ResourceType ResourceType => _resourceType;
    }
}
