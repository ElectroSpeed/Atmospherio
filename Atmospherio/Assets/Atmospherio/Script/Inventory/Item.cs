using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    public Sprite _icon;
    public string _itemName;
    public string _itemDescription;
    public GameObject _itemBuilding;
    public int _maxStack;
    public Item _ressourceCook;
    public float _timeCook;
}
