using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class TubeViewAssetManager :  SingletonMonoBehaviour<TubeViewAssetManager>
{
    [System.Serializable]
    public struct DefinedTubePrefab 
    {
        public int designedCapacity;
        public GameObject tubeViewPrefab;
        public float cellSize;
    }
    public List<DefinedTubePrefab> definedTubePrefabs;

    protected override void Awake()
    {
        base.Awake(); 
    }
    public GameObject GetPrefabsForCapacity(int capacity)
    {
        DefinedTubePrefab mapping = definedTubePrefabs.FirstOrDefault(def => def.designedCapacity == capacity);
        return mapping.tubeViewPrefab;
    }
    public float CellSize(int capacity)
    {
        DefinedTubePrefab mapping = definedTubePrefabs.FirstOrDefault(def => def.designedCapacity == capacity);
        return mapping.cellSize;
    }
}
