using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(GenerationMap))]
public class GenerationMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Map"))
        {
            (target as GenerationMap).GenerateMap();
            EditorUtility.SetDirty(this);

        }
        if (GUILayout.Button("Remove Map"))
        {
            (target as GenerationMap).RemoveMap();
        }
    }
}

public class GenerationMap : MonoBehaviour
{
    [Header("Map Size")]
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [Header("Block prefabs")]
    [SerializeField] private GameObject _blockBasic;
    [SerializeField] private GameObject _blockIron;
    [SerializeField] private GameObject _blockCoal;
    [SerializeField] private GameObject _blockCopper;
    [SerializeField] private GameObject _tree;
    [SerializeField] private GameObject _blockSand;
    [SerializeField] private GameObject _blockStone;

    [Header("Spawn Block Iron")]
    [SerializeField] private float _spawnBlockIronMin;
    [SerializeField] private float _spawnBlockIronMax;

    [Header("Spawn Block Coal")]
    [SerializeField] private float _spawnBlockCoalMin;
    [SerializeField] private float _spawnBlockCoalMax;

    [Header("Spawn Block Copper")]
    [SerializeField] private float _spawnBlockCopperMin;
    [SerializeField] private float _spawnBlockCopperMax;
    
    [Header("Spawn Tree")]
    [SerializeField] private float _spawnTreeMin;
    [SerializeField] private float _spawnTreeMax;
    
    [Header("Spawn Sand")]
    [SerializeField] private float _spawnSandMin;
    [SerializeField] private float _spawnSandMax;
    
    [Header("Spawn Stone")]
    [SerializeField] private float _spawnStoneMin;
    [SerializeField] private float _spawnStoneMax;
    
    

    public void GenerateMap()
    {
        int seedIron = Random.Range(-10000, 10000);
        int seedCoal = Random.Range(-10000, 10000);
        int seedCopper = Random.Range(-10000, 10000);
        int seedTree = Random.Range(-10000, 10000);
        int seedSand = Random.Range(-10000, 10000);
        int seedStone = Random.Range(-10000, 10000);

        for (int z = 0; z < _height; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                float perlinIron = Mathf.PerlinNoise(x / 10f + seedIron, z / 10f + seedIron);
                float perlinCoal = Mathf.PerlinNoise(x / 10f + seedCoal, z / 10f + seedCoal);
                float perlinCopper = Mathf.PerlinNoise(x / 10f + seedCopper, z / 10f + seedCopper);
                float perlinTree = Mathf.PerlinNoise(x / 10f + seedTree, z / 10f + seedTree);
                float perlinSand = Mathf.PerlinNoise(x / 10f + seedSand, z / 10f + seedSand);
                float perlinStone = Mathf.PerlinNoise(x / 10f + seedStone, z / 10f + seedStone);

                if (perlinIron > _spawnBlockIronMin && perlinIron < _spawnBlockIronMax)
                {
                    Instantiate(_blockIron, new Vector3(x, 0, z), _blockIron.transform.rotation, gameObject.transform);
                }
                else if (perlinCoal > _spawnBlockCoalMin && perlinCoal < _spawnBlockCoalMax)
                {
                    Instantiate(_blockCoal, new Vector3(x, 0, z), _blockCoal.transform.rotation, gameObject.transform);
                }
                else if (perlinCopper > _spawnBlockCopperMin && perlinCopper < _spawnBlockCopperMax)
                {
                    Instantiate(_blockCopper, new Vector3(x, 0, z), _blockCopper.transform.rotation, gameObject.transform);
                }
                else if (perlinTree > _spawnTreeMin && perlinTree < _spawnTreeMax)
                {
                    Instantiate(_tree, new Vector3(x, 0, z), _tree.transform.rotation, gameObject.transform);
                }
                else if (perlinSand > _spawnSandMin && perlinSand < _spawnSandMax)
                {
                    Instantiate(_blockSand, new Vector3(x, 0, z), _blockSand.transform.rotation, gameObject.transform);
                }
                else if (perlinStone > _spawnStoneMin && perlinStone < _spawnStoneMax)
                {
                    Instantiate(_blockStone, new Vector3(x, 0, z), _blockStone.transform.rotation, gameObject.transform);
                }
                else
                {
                    Instantiate(_blockBasic, new Vector3(x, 0, z), _blockBasic.transform.rotation, gameObject.transform);
                }
            }
        }
    }
    public void RemoveMap()
    {
        List<Transform> childs = gameObject.transform.GetComponentsInChildren<Transform>().ToList();

        foreach (Transform child in childs.ToList().Where(child => child != null).Where(child => child.gameObject != gameObject))
        {
            DestroyImmediate(child.gameObject);
        }
    }
}

#endif