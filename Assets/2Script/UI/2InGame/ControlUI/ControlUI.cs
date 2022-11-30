using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour
{
    [SerializeField] private JoyStick joystick = null;
    [SerializeField] private Button useButton = null;

    public JoyStick JoyStick { get { return joystick; } }
    public Button UseButton { get { return useButton; } }
}
