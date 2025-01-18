using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool isControlEnabled; // �����Ƿ�����
    public Rigidbody2D rb;
    public Vector2 inputDirection;
    //EnemyManager enemyManager;

    [Header("�������")]
    public PhysicsMaterial2D defaultMaterial; // Ĭ���������
    public PhysicsMaterial2D slipperyMaterial; // �����������
    [Header("��������")]
    public float speed;
    public float jumpSpeed;
    public float dashSpeed;//�����
    public float dashDis;
    public float dashDuration;//��̳���ʱ��
    public float currentSpeed;//��ǰ�ٶ�
    public int maxHealth = 10;
    public int currentHealth = 10;


    [Header("����״̬")]
    public bool isAttacking = false;//�Ƿ����ڹ���
    public bool isGrounded = false;//�Ƿ�Ӵ�����
    public bool isLeftWalled = false;//�Ƿ�������ǽ��
    public bool isRightWalled = false;//�Ƿ������ұ�ǽ��
    //public bool dashingCondition = false;//�Ƿ���Գ��
    //public bool isDashing = false;//
    //public float DashStartTimer;//��̼�ʱ��
    //public float DashCDStartTimer;//�����ȴ
    //public float DashCD;

    private Vector2 checkpointPosition; // �洢����λ��
    private bool isDead = false; // ����Ƿ�����

    [Header("װ������")]
    public GameObject bubblePrefab;  // ����Ԥ��
    public float Size;  //���ݴ�С����
    private float mousePressTime = 0f;  // ��갴�µ�ʱ��
    private bool isPressing = false;  // �Ƿ����ڰ������
    /*
    [Header("��������")]
    public GameObject attackTriggerPrefab; // �������ù�����������Ԥ����
    public float attackRange = 5f; // ������Χ
    private float lastAttackTime;
    [Header("�����޵�")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;
    public int FixedScale;
    public float knock_backSpeed;//����ˮƽ�ٶ�
    public float knock_upSpeed;//������ֱ�ٶ�
    */



    public void Awake()
    {

    }

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //checkpointPosition = transform.position; // ��ʼ������Ϊ��ǰ���λ��
        isControlEnabled = true;
        isGrounded = true;
        isLeftWalled = false;
        isRightWalled = false;
        // ���ó�ʼ�������
        rb.sharedMaterial = defaultMaterial;
    }
   private void OnCollisionEnter2D(Collision2D collision)
    {
        // �����봥����ʱ������������ı�ǩ�� "Ground"����ʾ��ɫ�ڵ�����
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        // �����봥����ʱ������������ı�ǩ�� "LeftWall"����ʾ��ɫ�Ӵ���ǽ��
        if (collision.gameObject.CompareTag("LeftWall"))
        {
            isLeftWalled = true;
        }
        // �����봥����ʱ������������ı�ǩ�� "RightWall"����ʾ��ɫ�Ӵ���ǽ��
        else if (collision.gameObject.CompareTag("RightWall"))
        {
            isRightWalled = true;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // �����봥����ʱ������������ı�ǩ�� "Ground"����ʾ��ɫ�ڵ�����
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // �����봥����ʱ������������ı�ǩ�� "Ground"����ʾ��ɫ�ڵ�����
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        // �����봥����ʱ������������ı�ǩ�� "LeftWall"����ʾ��ɫ�Ӵ���ǽ��
        if (collision.gameObject.CompareTag("LeftWall"))
        {
            isLeftWalled = false;
        }
        // �����봥����ʱ������������ı�ǩ�� "RightWall"����ʾ��ɫ�Ӵ���ǽ��
        else if (collision.gameObject.CompareTag("RightWall"))
        {
            isRightWalled = false;
        }
    }
    public void DisableControls()
    {
        isControlEnabled = false; // ���ÿ���
    }

    public void EnableControls()
    {
        isControlEnabled = true; // ���ÿ���
    }

    void Update()

    {
        //if (isDead)
        //{
        //    Die();
        //    currentHealth = maxHealth;
             //enemyManager.ReloadEnemies();
        //}
        if (isControlEnabled && !isDead) // ֻ���ڿ���������δ����ʱ�Ŵ�������
        {


            // ʾ��������������
            if (Input.GetKeyDown(KeyCode.R)) // ���� R �����������ͷ�����
            {
                Die();
            }
        }
        //Ӳֱ״̬�޷�����
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
            //UpdateMaterial(); // ��Ӹ���������ʵķ���
        }

    }
    private void HandleBlow()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ��갴��ʱ��ʼ����ѹʱ��
            mousePressTime = 0f;
            isPressing = true;
        }

        // ����������ɿ�
        if (Input.GetMouseButtonUp(0))
        {
            // ������ѹ
            isPressing = false;
            GenerateBubble();
        }

        // ���������ڰ��£����Ӱ�ѹʱ��
        if (isPressing)
        {
            mousePressTime += Time.deltaTime;
        }
    }

    void GenerateBubble()
    {
        // ��ȡ��굱ǰλ��
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ���ݰ���ʱ�����ɲ�ͬ��С������
        float bubbleSize = 1f*Size;  // Ĭ�����ݴ�СΪ1

        if (mousePressTime >= 2f)
        {
            bubbleSize = 3f*Size;  // ����2�����ɴ�����
        }
        else if (mousePressTime >= 1f)
        {
            bubbleSize = 2f*Size;  // ����1�������еȴ�С����
        }
        else
        {
            bubbleSize = 0.5f*Size;  // ���ٵ������С����
        }

        // �����λ����������
        GameObject bubble = Instantiate(bubblePrefab, mousePosition, Quaternion.identity);
        Bubble bubbleScript = bubble.GetComponent<Bubble>();
        bubbleScript.Initialize(bubbleSize);  // �������ݵĴ�С
    }
    private void HandleMovement()
    {
        float moveInput = 0;
        if (Input.GetKey("a") && !isLeftWalled) moveInput = -1; // ����
        if (Input.GetKey("d") && !isRightWalled) moveInput = 1;  // ����
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
        //���ﷴת
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

        //��ͻ����
        //&dash

    }

    public void HandleJump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)//������֮��
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
                //B����
                if (Time.time - lastAttackTime < 0.35)
                {
                    Debug.Log("Attack B");

                }
                isAttacking = false;//���ù���״̬

            }
            else if (isAttacking == false)
            {
                //����A����
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
        // ���㹥����������λ��
        Vector3 spawnPosition = transform.position + Scalex * attackRange; // ���ݽ�ɫ�����������λ��

        // ���ɹ���������
        GameObject attackTrigger = Instantiate(attackTriggerPrefab, spawnPosition, Quaternion.identity);
        attackTrigger.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
        // ���ù����������ı�ǩ
        attackTrigger.tag = "Pattack";
        Destroy(attackTrigger, 0.2f); // 1�������

    }
    public void Interact(Interact interactor)
    {
        Debug.Log("Press E to interact");

    }

    private void HandleAttach()
    {
        // ����Ƿ�����ȴʱ����
        if (!isAttaching&& Time.time < lastAttachTime + attachCD)
        {
            isAttaching = false; // ����ȴ�ڼ��ֹ����
            return;
        }

        attachCondition = (!physicsCheck.isGround) && (physicsCheck.isleftWall || physicsCheck.isrightWall);//�������ǽ


        if (attachCondition == true)
        {
            //����
            if ((Input.GetKey(KeyCode.A) && physicsCheck.isleftWall) || (Input.GetKey(KeyCode.D) && physicsCheck.isrightWall))
            {
                //Debug.Log("Attach");
                isAttaching = true;
                rb.velocity = new Vector2(0, slideDownSpeed);
                //lastAttachTime = Time.time;
            }
            else
            {
                // �뿪����״̬
                if (isAttaching)
                {
                    isAttaching = false; // ���¸���״̬
                    //lastAttachTime = Time.time; // �����뿪����ʱ��
                }
            }

        }
        else
        {
            // �뿪����״̬
            if (isAttaching)
            {
                isAttaching = false; // ���¸���״̬
                //lastAttachTime = Time.time; // �����뿪����ʱ��
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
            if (Input.GetKey(KeyCode.F) || isGrappling) // ����E����ץ��
            {
                canInput = false;
                Grapple();
                dashingCondition = true;
            }
        }

    }
    private void Grapple()
    {
        // ��ȡץ�������ײ��
        Collider2D grappleCollider = Physics2D.OverlapCircle((Vector2)transform.position + physicsCheck.HookOffset, physicsCheck.checkHookRaduis, physicsCheck.HookLayer);

        if (grappleCollider != null) // ȷ��ץȡ�����
        {
            grapplePoint = grappleCollider.transform.position;
            isGrappling = true;
            rb.gravityScale = 0;
            grappleing(grapplePoint);
            if (Vector2.Distance(transform.position, grappleCollider.transform.position) < 1f)
            {
                grappleCollider.gameObject.SetActive(false);
                StartCoroutine(ReactivateAfterDelay(grappleCollider.gameObject, 3f));
                ReleaseGrapple(); // �ͷ�ץȡ
            }
            //StartCoroutine(Grappleing(grapplePoint));
        }
        //    // ֻ��ץȡ����Чʱ���к�������
        //}
    }
    private IEnumerator ReactivateAfterDelay(GameObject obj, float delay)
    {
        // �ȴ�ָ����ʱ��
        yield return new WaitForSeconds(delay);
        // ���¼�������
        obj.SetActive(true);
    }
    public void grappleing(Vector3 grapplepoint)
    {
        float speed = 250f;
        if (isGrappling)
        {
            // ÿ֡�ƶ�һ���ľ���
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
        checkpointPosition = position; // ���¼���λ��
    }
*/
    public void Die()
    {
        isDead = true; // ��������״̬
        transform.position = checkpointPosition; // �������λ��
        isDead = false; // �ָ�״̬
    }

    /*private void UpdateMaterial()
    {
        if (!isGrounded) // ������ڵ���
        {
            rb.sharedMaterial = slipperyMaterial; // ����Ϊ�����������
        }
        else
        {
            rb.sharedMaterial = defaultMaterial; // �ָ�ΪĬ���������
        }
    }*/

}


