using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool isControlEnabled; // �����Ƿ�����
    public Rigidbody2D rb;
    public Vector2 inputDirection;
    // ����Animator���
    public Animator animator;
    [Header("�������")]
    public PhysicsMaterial2D defaultMaterial; // Ĭ���������
    public PhysicsMaterial2D slipperyMaterial; // �����������
    [Header("��������")]
    public float speed;
    public float jumpSpeed;
    public float currentSpeed;//��ǰ�ٶ�
    public int maxHealth = 10;
    public int currentHealth = 10;
    [Header("����״̬")]
    public bool isAttacking = false;//�Ƿ����ڹ���
    public bool isGrounded = false;//�Ƿ�Ӵ�����
    public bool isLeftWalled = false;//�Ƿ�������ǽ��
    public bool isRightWalled = false;//�Ƿ������ұ�ǽ��
    private Vector2 checkpointPosition; // �洢����λ��
    private bool isDead = false; // ����Ƿ�����
    [Header("װ������")]
    public float Size;  //���ݴ�С����
    private float mousePressTime = 0f;  // ��갴�µ�ʱ��
    private bool isPressing = false;  // �Ƿ����ڰ������
    [Header("��Ӿ����")]
    public bool isInWater;
    public float buoyancyForce = 10.0f;  // ������С
    public float waterLevel = 0.0f;   // ˮλ�߶�
    //bubble�б�
    [Header("����ʹ�õ������б�")]
    public List<GameObject> bubbleList;
    public int index;
    public GameObject currentBubble;  // ��ǰ���ɵ����ݶ���
    public Bubble currentBubbleScript;  // ��ǰ���ݵĽű�����
    [Header("ʹ�ô���")]
    public int maxTimes = 10;
    public int currentTimes = 0;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //checkpointPosition = transform.position; // ��ʼ������Ϊ��ǰ���λ��
        isControlEnabled = true;
        isGrounded = true;
        isLeftWalled = false;
        isRightWalled = false;
        index = 0;
        // ���ó�ʼ�������
        rb.sharedMaterial = defaultMaterial;
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �����봥����ʱ������������ı�ǩ�� "Ground"����ʾ��ɫ�ڵ�����
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("Isjumping", false);
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
        if (isControlEnabled && !isDead) // ֻ���ڿ���������δ����ʱ�Ŵ�������
        {
        }
        //Ӳֱ״̬�޷�����
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
                // ���㸡��
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
            // ��갴��ʱ��ʼ����ѹʱ��
            mousePressTime = 0f;
            isPressing = true;
            // �������ָ��
            Cursor.visible = false;
            currentTimes = currentTimes + 1;
            GenerateBubble();
        }

        // ����������ɿ�
        if (Input.GetMouseButtonUp(0))
        {
            // ������ѹ
            isPressing = false;
            if (currentBubble != null)
            {
                // ֹͣ���ݵ�����������ʼ���ټ�ʱ
                currentBubbleScript.StopGrowing();
                float destroyTime = currentBubbleScript.size * 2f;  // �������ݵĴ�С��������ʱ��
                currentBubbleScript.StartDestroyCountdown();  // �������ٵ���ʱ
            }
            // ����ɿ�ʱ�ָ����ɼ�
            Cursor.visible = true;
        }

        // ���������ڰ��£����Ӱ�ѹʱ��
        if (isPressing && currentBubble != null)
        {
            mousePressTime += Time.deltaTime;

            //���������ݴ�С
            float sizeIncrease = 0.01f;
            currentBubbleScript.Grow(sizeIncrease);

            // ����λ�ø������
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentBubble.transform.position = mousePosition;
        }
    }

    void GenerateBubble()
    {
        // ��ȡ��굱ǰλ��
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // �����λ����������
        currentBubble = Instantiate(bubbleList[index], mousePosition, Quaternion.identity);
        currentBubbleScript = currentBubble.GetComponent<Bubble>();
        currentBubbleScript.Initialize(0.5f);// �������ݵĴ�С
    }
    private void HandleMovement()
    {
        float moveInput = 0;
        if (Input.GetKey("a") && !isLeftWalled) moveInput = -1; // ����
        if (Input.GetKey("d") && !isRightWalled) moveInput = 1;  // ����
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
            if (isGrounded)//������֮��
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

        // ����Ը��ݹ���ֵ���и��ֲ���
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


