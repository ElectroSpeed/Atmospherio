using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class GetButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> _listButton;
    public PlayerInput _playerInput;
    [HideInInspector] public List<string> _listControl = new();
    private List<ChangeControl> _changeControls = new();
    private int _indexButton;
    public static string _text = "99";
    private void Start()
    {
        for (int i = 0; i < _listButton.Count; i++)
        {
            _listControl.Add("");
            _changeControls.Add(_listButton[i].GetComponent<ChangeControl>());
        }
        SetListControl();
    }
    public void SetListControl()
    {
        for (int i = 0; i < _changeControls.Count; i++)
        {
            _listControl[i] = _playerInput.actions.actionMaps[0].actions[_changeControls[i]._indexAction].bindings[_changeControls[i]._indexBinding].path.ToString();
        }
    }

    private void OnEnable()
    {
        _playerInput.actions.actionMaps[0].Disable();
        _playerInput.actions.actionMaps[1].Enable();
    }
    private void OnDisable()
    {
        _playerInput.actions.actionMaps[0].Enable();
        _playerInput.actions.actionMaps[1].Disable();
    }
    public void Any(InputAction.CallbackContext ctx)
    {
        InputSystem.onAnyButtonPress.CallOnce(ctrl => _text = ctrl.name);
        if ((_text.Length < 2 || _text == "space" || _text == "leftShift" || _text == "semicolon" || _text == "tab") && _text != "escape")
        {
            _listButton[_indexButton].GetComponent<ChangeControl>().Change();
        }
            
            
    }
    public void OnClick(int index)
    {
        _indexButton = index;
    }
}
