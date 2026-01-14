using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LunaBehaviourNew : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    public float speed = 7f;
    public float maxhealth = 100f;//最大生命值
    public float currenthealth;//当前生命值

    // 冲刺设置（平面2D，不再使用跳跃）
    public float sprintMultiplier = 1.8f;

    // 内部状态（在 Update 捕获输入，在 FixedUpdate 执行物理）
    private Vector2 moveInput;
    private bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        rigidbody2D = GetComponent<Rigidbody2D>();
        currenthealth = maxhealth - 20;
        if (rigidbody2D != null)
        {
            // 平面2D：确保刚体不受重力影响
            rigidbody2D.gravityScale = 0f;
            rigidbody2D.freezeRotation = true;
        }
    }

    // Update 捕获按键
    void Update()
    {
        // 平面 2D 使用水平和垂直轴
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        //Debug.Log($"Move: {moveInput}, Sprint: {isSprinting}");
    }

    // FixedUpdate 用于物理操作
    void FixedUpdate()
    {
        float currentSpeed = speed * (isSprinting ? sprintMultiplier : 1f);

        // 防止对角速度过大：归一化输入向量
        Vector2 desired = moveInput;
        if (desired.sqrMagnitude > 1f) desired = desired.normalized;

        // 直接设置速度以获得平面移动
        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = desired * currentSpeed;
        }
    }

    public void ChangeHealth(int amount)
    {
        currenthealth = Mathf.Clamp(currenthealth + amount, 0, maxhealth);
        Debug.Log(currenthealth + '/' + maxhealth);
    }
    // 平面2D中总是视为“着地”，原跳跃检测保留以兼容旧代码
    private bool IsGrounded()
    {
        return true;
    }
}