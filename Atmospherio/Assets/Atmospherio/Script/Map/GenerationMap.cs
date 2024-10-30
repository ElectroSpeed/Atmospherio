using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GenerationMap))]
public class GenerationMapEditor : Editor
{
    private int _seed;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Map"))
        {
            _seed = Random.Range(-10000, 10000);
            (target as GenerationMap).GenerateMap(_seed);
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

    [Header("Spawn Block Iron")]
    [SerializeField] private float _spawnBlockIronMin;
    [SerializeField] private float _spawnBlockIronMax;

    [Header("Spawn Block Coal")]
    [SerializeField] private float _spawnBlockCoalMin;
    [SerializeField] private float _spawnBlockCoalMax;

    [Header("Spawn Block Coal")]
    [SerializeField] private float _spawnBlockCopperMin;
    [SerializeField] private float _spawnBlockCopperMax;




    public void GenerateMap(int seed)
    {
        for (int z = 0; z < _height; z++)
        {
            for (int x = 0; x < _width; x++)
            {
                float perlin = Mathf.PerlinNoise(x / 10f + seed, z / 10f + seed);

                if (perlin > _spawnBlockIronMin && perlin < _spawnBlockIronMax)
                {
                    Instantiate(_blockIron, new Vector3(x, 0, z), _blockIron.transform.rotation, gameObject.transform);
                }

                else if (perlin > _spawnBlockCoalMin && perlin < _spawnBlockCoalMax)
                {
                    Instantiate(_blockCoal, new Vector3(x, 0, z), _blockCoal.transform.rotation, gameObject.transform);
                }

                else if (perlin > _spawnBlockCopperMin && perlin < _spawnBlockCopperMax)
                {
                    Instantiate(_blockCopper, new Vector3(x, 0, z), _blockCopper.transform.rotation, gameObject.transform);
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

        foreach(Transform child in childs.ToList())
        {
            if(child.gameObject == gameObject) continue;
            DestroyImmediate(child.gameObject);
        }
    }

}