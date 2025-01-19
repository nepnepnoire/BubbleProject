using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;
using UnityEngine.InputSystem;

public class NeededControl : MonoBehaviour
{
    private bool isControlEnabled; // 控制是否启用
    public Rigidbody2D rb;
    public Vector2 inputDirection;
    [Header("物理材质")]
    public PhysicsMaterial2D defaultMaterial; // 默认物理材质
    public PhysicsMaterial2D slipperyMaterial; // 滑溜物理材质
    [Header("基本参数")]
    public float speed;
    public float jumpSpeed;
    public float currentSpeed;//当前速度
    public int maxHealth = 10;
    public int currentHealth = 10;
    public float speedMultiplier = 1.0f; // 速度倍数，用于连按加速
    public float timeSinceLastPress = 0f; // 记录上一次按键的时间
    public float timeThreshold = 0.5f; // 时间阈值，在该时间内连按会加速
    public int pressCount = 0; // 连按次数
    [Header("人物状态")]
    public bool isAttacking = false;//是否正在攻击
    public bool isGrounded = false;//是否接触地面
    public bool isLeftWalled = false;//是否贴在左墙壁
    public bool isRightWalled = false;//是否贴在右边墙壁

    private Vector2 checkpointPosition; // 存储检查点位置
    private bool isDead = false; // 玩家是否死亡


    public void Awake()
    {

    }

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //checkpointPosition = transform.position; // 初始化检查点为当前玩家位置
        isControlEnabled = true;
        isGrounded = true;
        isLeftWalled = false;
        isRightWalled = false;
        // 设置初始物理材质
        rb.sharedMaterial = defaultMaterial;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 当进入触发器时，如果触发器的标签是 "Ground"，表示角色在地面上
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        // 当进入触发器时，如果触发器的标签是 "LeftWall"，表示角色接触左墙上
        if (collision.gameObject.CompareTag("LeftWall"))
        {
            isLeftWalled = true;
        }
        // 当进入触发器时，如果触发器的标签是 "RightWall"，表示角色接触右墙上
        else if (collision.gameObject.CompareTag("RightWall"))
        {
            isRightWalled = true;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 当进入触发器时，如果触发器的标签是 "Ground"，表示角色在地面上
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 当进入触发器时，如果触发器的标签是 "Ground"，表示角色在地面上
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        // 当进入触发器时，如果触发器的标签是 "LeftWall"，表示角色接触左墙上
        if (collision.gameObject.CompareTag("LeftWall"))
        {
            isLeftWalled = false;
        }
        // 当进入触发器时，如果触发器的标签是 "RightWall"，表示角色接触右墙上
        else if (collision.gameObject.CompareTag("RightWall"))
        {
            isRightWalled = false;
        }
    }
    public void DisableControls()
    {
        isControlEnabled = false; // 禁用控制
    }

    public void EnableControls()
    {
        isControlEnabled = true; // 启用控制
    }

    void Update()

    {
        if (isControlEnabled && !isDead) // 只有在控制启用且未死亡时才处理输入
        {
        }
        if (isControlEnabled)
        {
            HandleJump();
            if (currentHealth <= 0)
            {

                Debug.Log("Game Over!");
                isDead = true;
            }
        }
    }
    private void FixedUpdate()
    {
        if (isControlEnabled)
        {
            HandleMovement();
        }

    }
    private void HandleMovement()
    {
        float moveInput = 0;
        if (Input.GetKey("a") && !isLeftWalled) moveInput = -1; // 向左
        if (Input.GetKey("d") && !isRightWalled) moveInput = 1;  // 向右

        // 检测按键按下瞬间
        if (Input.GetKeyDown("a") || Input.GetKeyDown("d"))
        {
            float currentTime = Time.time;
            if (currentTime - timeSinceLastPress < timeThreshold)
            {
                pressCount++;
            }
            else
            {
                pressCount = 1;
            }
            timeSinceLastPress = currentTime;
        }

        // 根据连按次数调整速度倍数
        if (pressCount >= 2)
        {
            speedMultiplier = 2.0f; // 连按两次及以上，速度加倍，可根据需要调整倍数
        }
        else
        {
            speedMultiplier = 1.0f;
        }

        if (moveInput != 0)
        {
            currentSpeed = speed * moveInput * speedMultiplier;
            rb.velocity = new Vector2(currentSpeed * Time.deltaTime, rb.velocity.y);
        }
        else
        {
            currentSpeed = 0;
        }

        // 人物反转
        int faceDir = (int)transform.localScale.x;

        if (moveInput > 0)
        {
            faceDir = 1;
        }
        else if (moveInput < 0)
        {
            faceDir = -1;
        }
        transform.localScale = new Vector3(faceDir, 1, 1);
    }
    public void HandleJump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)//跳起来之后
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                //dashingCondition = true;
                isGrounded = false;
            }
        }
    }
}


