using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CraftReciepe : MonoBehaviour
{
    public Item _itemCraft;
    public List<ComponentCraft> _componentCraftList = new();
    public int _itemCraftCount;
}
