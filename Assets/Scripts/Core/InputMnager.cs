using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    // 定义输入事件，让其他脚本订阅
    public event Action OnAttackInput;
    public event Action OnJumpInput;

    void Awake() => Instance = this;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            OnAttackInput?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInput?.Invoke();
        }
    }
}
