using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool isControlEnabled; // 控制是否启用
    public Rigidbody2D rb;
    public Vector2 inputDirection;
    // 引用Animator组件
    public Animator animator;
    [Header("物理材质")]
    public PhysicsMaterial2D defaultMaterial; // 默认物理材质
    public PhysicsMaterial2D slipperyMaterial; // 滑溜物理材质
    [Header("基本参数")]
    public float speed;
    public float jumpSpeed;
    public float currentSpeed;//当前速度
    public int maxHealth = 10;
    public int currentHealth = 10;
    [Header("人物状态")]
    public bool isAttacking = false;//是否正在攻击
    public bool isGrounded = false;//是否接触地面
    public bool isLeftWalled = false;//是否贴在左墙壁
    public bool isRightWalled = false;//是否贴在右边墙壁
    private Vector2 checkpointPosition; // 存储检查点位置
    private bool isDead = false; // 玩家是否死亡
    [Header("装备泡泡")]
    public float Size;  //泡泡大小倍率
    private float mousePressTime = 0f;  // 鼠标按下的时间
    private bool isPressing = false;  // 是否正在按下鼠标
    [Header("游泳参数")]
    public bool isInWater;
    public float buoyancyForce = 10.0f;  // 浮力大小
    public float waterLevel = 0.0f;   // 水位高度
    //bubble列表
    [Header("可以使用的泡泡列表")]
    public List<GameObject> bubbleList;
    public int index;
    public GameObject currentBubble;  // 当前生成的泡泡对象
    public Bubble currentBubbleScript;  // 当前泡泡的脚本引用
    [Header("使用次数")]
    public int maxTimes = 10;
    public int currentTimes = 0;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //checkpointPosition = transform.position; // 初始化检查点为当前玩家位置
        isControlEnabled = true;
        isGrounded = true;
        isLeftWalled = false;
        isRightWalled = false;
        index = 0;
        // 设置初始物理材质
        rb.sharedMaterial = defaultMaterial;
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 当进入触发器时，如果触发器的标签是 "Ground"，表示角色在地面上
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("Isjumping", false);
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
        //硬直状态无法操作
        if (isControlEnabled)
        {
            HandleJump();
            HandleBlow();
            SwitchBubble();
            Check();
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
            if (isInWater)
            {
                // 计算浮力
                float depth = waterLevel - transform.position.y;
                Vector2 buoyancy = Vector2.up * buoyancyForce * depth;
                rb.AddForce(buoyancy, ForceMode2D.Force);
            }
        }

    }
    private void HandleBlow()
    {

        if (Input.GetMouseButtonDown(0) && currentTimes < maxTimes)
        {
            // 鼠标按下时初始化按压时间
            mousePressTime = 0f;
            isPressing = true;
            // 隐藏鼠标指针
            Cursor.visible = false;
            currentTimes = currentTimes + 1;
            GenerateBubble();
        }

        // 检测鼠标左键松开
        if (Input.GetMouseButtonUp(0))
        {
            // 结束按压
            isPressing = false;
            if (currentBubble != null)
            {
                // 停止泡泡的增长，并开始销毁计时
                currentBubbleScript.StopGrowing();
                float destroyTime = currentBubbleScript.size * 2f;  // 基于泡泡的大小决定销毁时间
                currentBubbleScript.StartDestroyCountdown();  // 启动销毁倒计时
            }
            // 鼠标松开时恢复鼠标可见
            Cursor.visible = true;
        }

        // 如果鼠标正在按下，增加按压时间
        if (isPressing && currentBubble != null)
        {
            mousePressTime += Time.deltaTime;

            //逐渐增大泡泡大小
            float sizeIncrease = 0.01f;
            currentBubbleScript.Grow(sizeIncrease);

            // 泡泡位置跟随鼠标
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentBubble.transform.position = mousePosition;
        }
    }

    void GenerateBubble()
    {
        // 获取鼠标当前位置
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 在鼠标位置生成泡泡
        currentBubble = Instantiate(bubbleList[index], mousePosition, Quaternion.identity);
        currentBubbleScript = currentBubble.GetComponent<Bubble>();
        currentBubbleScript.Initialize(0.5f);// 设置泡泡的大小
    }
    private void HandleMovement()
    {
        float moveInput = 0;
        if (Input.GetKey("a") && !isLeftWalled) moveInput = -1; // 向左
        if (Input.GetKey("d") && !isRightWalled) moveInput = 1;  // 向右
        if (moveInput != 0)
        {
            animator.SetBool("Ismoving", true);
            currentSpeed = Mathf.Lerp(currentSpeed, speed * moveInput, Time.deltaTime * 10);
            rb.velocity = new Vector2(currentSpeed * Time.deltaTime, rb.velocity.y);
        }
        else
        {
            animator.SetBool("Ismoving", false);
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * 10);
        }
        //人物反转
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            isInWater = true;
            waterLevel = other.transform.position.y;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            isInWater = false;
            waterLevel = 0.0f;
        }
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
    public void SwitchBubble()
    {
        float scrollValue = Input.mouseScrollDelta.y;

        // 你可以根据滚动值进行各种操作
        if (scrollValue > 0)
        {
            index++;
            index = index % bubbleList.Count;
        }
        else if (scrollValue < 0)
        {
            index--;
            if (index < 0) index += bubbleList.Count;
        }
    }
    private void Check()
    {
        if (!isGrounded && rb.velocity.y < 0)
        {
            animator.SetBool("isFalling", true);
        }
        if (rb.velocity.x == 0)
        {
            animator.SetBool("Ismoving", false);
        }
    }
}


