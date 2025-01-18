using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool isControlEnabled; // 控制是否启用
    public Rigidbody2D rb;
    public Vector2 inputDirection;
    //EnemyManager enemyManager;

    [Header("物理材质")]
    public PhysicsMaterial2D defaultMaterial; // 默认物理材质
    public PhysicsMaterial2D slipperyMaterial; // 滑溜物理材质
    [Header("基本参数")]
    public float speed;
    public float jumpSpeed;
    public float dashSpeed;//冲刺力
    public float dashDis;
    public float dashDuration;//冲刺持续时间
    public float currentSpeed;//当前速度
    public int maxHealth = 10;
    public int currentHealth = 10;


    [Header("人物状态")]
    public bool isAttacking = false;//是否正在攻击
    public bool isGrounded = false;//是否接触地面
    public bool isLeftWalled = false;//是否贴在左墙壁
    public bool isRightWalled = false;//是否贴在右边墙壁
    //public bool dashingCondition = false;//是否可以冲刺
    //public bool isDashing = false;//
    //public float DashStartTimer;//冲刺计时器
    //public float DashCDStartTimer;//冲刺冷却
    //public float DashCD;

    private Vector2 checkpointPosition; // 存储检查点位置
    private bool isDead = false; // 玩家是否死亡

    [Header("装备泡泡")]
    public GameObject bubblePrefab;  // 泡泡预设
    public float Size;  //泡泡大小倍率
    private float mousePressTime = 0f;  // 鼠标按下的时间
    private bool isPressing = false;  // 是否正在按下鼠标
    /*
    [Header("攻击参数")]
    public GameObject attackTriggerPrefab; // 用于设置攻击触发器的预制体
    public float attackRange = 5f; // 攻击范围
    private float lastAttackTime;
    [Header("受伤无敌")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;
    public int FixedScale;
    public float knock_backSpeed;//击退水平速度
    public float knock_upSpeed;//击退竖直速度
    */



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
        //if (isDead)
        //{
        //    Die();
        //    currentHealth = maxHealth;
             //enemyManager.ReloadEnemies();
        //}
        if (isControlEnabled && !isDead) // 只有在控制启用且未死亡时才处理输入
        {


            // 示例：检测玩家死亡
            if (Input.GetKeyDown(KeyCode.R)) // 假设 R 键用于死亡和防卡死
            {
                Die();
            }
        }
        //硬直状态无法操作
            if (isControlEnabled)
            {
            //DashContinue();
            //HandleDash();
            //HandleAttack();
            HandleJump();
            HandleBlow();
            if (currentHealth <= 0)
                {

                    Debug.Log("Game Over!");
                    isDead = true;
                }
        }




    }

    //public void knock_back()
    //{
    //    rb.velocity = new Vector2(FixedScale * knock_backSpeed, 0);
    //}
    /*
    private void InvulnerableCount()
    {
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }
    */

    private void FixedUpdate()
    {
        if (isControlEnabled)
        {
            HandleMovement();
            //UpdateMaterial(); // 添加更改物理材质的方法
        }

    }
    private void HandleBlow()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 鼠标按下时初始化按压时间
            mousePressTime = 0f;
            isPressing = true;
        }

        // 检测鼠标左键松开
        if (Input.GetMouseButtonUp(0))
        {
            // 结束按压
            isPressing = false;
            GenerateBubble();
        }

        // 如果鼠标正在按下，增加按压时间
        if (isPressing)
        {
            mousePressTime += Time.deltaTime;
        }
    }

    void GenerateBubble()
    {
        // 获取鼠标当前位置
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 根据按下时间生成不同大小的泡泡
        float bubbleSize = 1f*Size;  // 默认泡泡大小为1

        if (mousePressTime >= 2f)
        {
            bubbleSize = 3f*Size;  // 长按2秒生成大泡泡
        }
        else if (mousePressTime >= 1f)
        {
            bubbleSize = 2f*Size;  // 长按1秒生成中等大小泡泡
        }
        else
        {
            bubbleSize = 0.5f*Size;  // 快速点击生成小泡泡
        }

        // 在鼠标位置生成泡泡
        GameObject bubble = Instantiate(bubblePrefab, mousePosition, Quaternion.identity);
        Bubble bubbleScript = bubble.GetComponent<Bubble>();
        bubbleScript.Initialize(bubbleSize);  // 设置泡泡的大小
    }
    private void HandleMovement()
    {
        float moveInput = 0;
        if (Input.GetKey("a") && !isLeftWalled) moveInput = -1; // 向左
        if (Input.GetKey("d") && !isRightWalled) moveInput = 1;  // 向右
        /*
        if (isDashing && transform.localScale.x * moveInput > 0)
        {
            return;
        };
        */
        if (moveInput != 0)
        {
             currentSpeed = Mathf.Lerp(currentSpeed, speed * moveInput, Time.deltaTime * 10);
             rb.velocity = new Vector2(currentSpeed * Time.deltaTime, rb.velocity.y);
        }
        else
        {
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

        //冲突处理
        //&dash

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
    /*
    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dashingCondition)
            {
                rb.velocity = new Vector2((int)transform.localScale.x * dashSpeed, 0);
                dashingCondition = false;
                isDashing = true;
                DashStartTimer = dashDuration;
                DashCDStartTimer = DashCD;
            }
        }
    }
    public void DashContinue()
    {
        if (isDashing)
        {
            DashStartTimer -= Time.deltaTime;
            if (DashStartTimer <= 0)
            {
                isDashing = false;
                rb.velocity = new Vector2((int)transform.localScale.x, 0);
            }
            else
            {
                rb.velocity = new Vector2((int)transform.localScale.x * dashSpeed, 0);
            }
        }
    }  
    private void HandleAttack()
    {
        if (Time.time - lastAttackTime > 0.35)
        {
            isAttacking = false;
        }
        if (Input.GetMouseButtonDown(0))
        {

            if (isAttacking == true)
            {
                //B动作
                if (Time.time - lastAttackTime < 0.35)
                {
                    Debug.Log("Attack B");

                }
                isAttacking = false;//重置攻击状态

            }
            else if (isAttacking == false)
            {
                //播放A动作
                Debug.Log("Attack A");
                isAttacking = true;
                attackA();
                lastAttackTime = Time.time;
            }
        }
    }
    public void attackA()
    {
        Vector3 Scalex = new Vector3((int)transform.localScale.x, 0, 0);
        // 计算攻击触发器的位置
        Vector3 spawnPosition = transform.position + Scalex * attackRange; // 根据角色方向计算生成位置

        // 生成攻击触发器
        GameObject attackTrigger = Instantiate(attackTriggerPrefab, spawnPosition, Quaternion.identity);
        attackTrigger.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
        // 设置攻击触发器的标签
        attackTrigger.tag = "Pattack";
        Destroy(attackTrigger, 0.2f); // 1秒后销毁

    }
    public void Interact(Interact interactor)
    {
        Debug.Log("Press E to interact");

    }

    private void HandleAttach()
    {
        // 检查是否在冷却时间内
        if (!isAttaching&& Time.time < lastAttachTime + attachCD)
        {
            isAttaching = false; // 在冷却期间禁止附着
            return;
        }

        attachCondition = (!physicsCheck.isGround) && (physicsCheck.isleftWall || physicsCheck.isrightWall);//离地且有墙


        if (attachCondition == true)
        {
            //附着
            if ((Input.GetKey(KeyCode.A) && physicsCheck.isleftWall) || (Input.GetKey(KeyCode.D) && physicsCheck.isrightWall))
            {
                //Debug.Log("Attach");
                isAttaching = true;
                rb.velocity = new Vector2(0, slideDownSpeed);
                //lastAttachTime = Time.time;
            }
            else
            {
                // 离开附着状态
                if (isAttaching)
                {
                    isAttaching = false; // 更新附着状态
                    //lastAttachTime = Time.time; // 更新离开附着时间
                }
            }

        }
        else
        {
            // 离开附着状态
            if (isAttaching)
            {
                isAttaching = false; // 更新附着状态
                //lastAttachTime = Time.time; // 更新离开附着时间
            }
        }
    }
    private void HandleGlide()
    {
        glideCondition = (!physicsCheck.isGround) && (rb.velocity.y < 0) && (!isAttaching);
        if (glideCondition == true)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                //Debug.Log("Glide");
                isGliding = true;
                rb.velocity = new Vector2(rb.velocity.x, glidingSpeed);
            }
        }
        else isGliding = false;
    }

    private void HandleHook()
    {
        if (physicsCheck.isGrapplePoint)
        {
            //Debug.Log("Find hookpoint");
            if (Input.GetKey(KeyCode.F) || isGrappling) // 按“E”键抓钩
            {
                canInput = false;
                Grapple();
                dashingCondition = true;
            }
        }

    }
    private void Grapple()
    {
        // 获取抓钩点的碰撞体
        Collider2D grappleCollider = Physics2D.OverlapCircle((Vector2)transform.position + physicsCheck.HookOffset, physicsCheck.checkHookRaduis, physicsCheck.HookLayer);

        if (grappleCollider != null) // 确保抓取点存在
        {
            grapplePoint = grappleCollider.transform.position;
            isGrappling = true;
            rb.gravityScale = 0;
            grappleing(grapplePoint);
            if (Vector2.Distance(transform.position, grappleCollider.transform.position) < 1f)
            {
                grappleCollider.gameObject.SetActive(false);
                StartCoroutine(ReactivateAfterDelay(grappleCollider.gameObject, 3f));
                ReleaseGrapple(); // 释放抓取
            }
            //StartCoroutine(Grappleing(grapplePoint));
        }
        //    // 只在抓取点有效时进行后续处理
        //}
    }
    private IEnumerator ReactivateAfterDelay(GameObject obj, float delay)
    {
        // 等待指定的时间
        yield return new WaitForSeconds(delay);
        // 重新激活物体
        obj.SetActive(true);
    }
    public void grappleing(Vector3 grapplepoint)
    {
        float speed = 250f;
        if (isGrappling)
        {
            // 每帧移动一定的距离
            transform.position = Vector2.MoveTowards(transform.position, grapplepoint, speed * Time.deltaTime);
            totaltime += Time.deltaTime;
        }
    }




    private void ReleaseGrapple()
    {
        isGrappling = false;
        rb.gravityScale = 15;
        rb.velocity = new Vector2(0, 0);
        canInput = true;
    }

    public void SetCheckpoint(Vector2 position)
    {
        checkpointPosition = position; // 更新检查点位置
    }
*/
    public void Die()
    {
        isDead = true; // 设置死亡状态
        transform.position = checkpointPosition; // 复活到检查点位置
        isDead = false; // 恢复状态
    }

    /*private void UpdateMaterial()
    {
        if (!isGrounded) // 如果不在地面
        {
            rb.sharedMaterial = slipperyMaterial; // 设置为滑溜物理材质
        }
        else
        {
            rb.sharedMaterial = defaultMaterial; // 恢复为默认物理材质
        }
    }*/

}


