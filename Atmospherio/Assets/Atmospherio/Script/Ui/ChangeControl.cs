using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class ChangeControl : MonoBehaviour
{
    public string _control = "";
    private PlayerInput _playerInput;
    public int _indexAction;
    public int _indexBinding;
    private void Start()
    {
        _playerInput = GetComponentInParent<GetButton>()._playerInput;
        _control = _playerInput.actions.actionMaps[0].actions[_indexAction].bindings[_indexBinding].path.ToString();
        _control = _control[11..];
        ChangeQwerty();
        GetComponentInChildren<TextMeshProUGUI>().text = _control.ToUpper();
    }
    public void Change()
    {
        _control = GetButton._text;
        if (EventSystem.current.currentSelectedGameObject != gameObject)
            return;
        List<string> list = GetComponentInParent<GetButton>()._listControl;
        if (list.Any(t => t == "<Keyboard>/" + _control))
        {
            return;
        }
        _playerInput.actions.actionMaps[0].actions[_indexAction].ChangeBinding(_indexBinding).WithPath("<Keyboard>/" + _control);
        ChangeQwerty();
        GetComponentInChildren<TextMeshProUGUI>().text = _control.ToUpper();
        GetComponentInParent<GetButton>().SetListControl();
    }
    private void ChangeQwerty()
    {
        _control = _control switch
        {
            "q" => "a",
            "a" => "q",
            "w" => "z",
            "z" => "w",
            "semicolon" => "m",
            _ => _control
        };
    }
}
