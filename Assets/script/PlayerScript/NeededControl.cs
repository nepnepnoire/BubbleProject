using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;
using UnityEngine.InputSystem;

public class NeededControl : MonoBehaviour
{
    private bool isControlEnabled; // �����Ƿ�����
    public Rigidbody2D rb;
    public Vector2 inputDirection;
    [Header("�������")]
    public PhysicsMaterial2D defaultMaterial; // Ĭ���������
    public PhysicsMaterial2D slipperyMaterial; // �����������
    [Header("��������")]
    public float speed;
    public float jumpSpeed;
    public float currentSpeed;//��ǰ�ٶ�
    public int maxHealth = 10;
    public int currentHealth = 10;
    public float speedMultiplier = 1.0f; // �ٶȱ�����������������
    public float timeSinceLastPress = 0f; // ��¼��һ�ΰ�����ʱ��
    public float timeThreshold = 0.5f; // ʱ����ֵ���ڸ�ʱ�������������
    public int pressCount = 0; // ��������
    [Header("����״̬")]
    public bool isAttacking = false;//�Ƿ����ڹ���
    public bool isGrounded = false;//�Ƿ�Ӵ�����
    public bool isLeftWalled = false;//�Ƿ�������ǽ��
    public bool isRightWalled = false;//�Ƿ������ұ�ǽ��

    private Vector2 checkpointPosition; // �洢����λ��
    private bool isDead = false; // ����Ƿ�����


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
        if (isControlEnabled && !isDead) // ֻ���ڿ���������δ����ʱ�Ŵ�������
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
        if (Input.GetKey("a") && !isLeftWalled) moveInput = -1; // ����
        if (Input.GetKey("d") && !isRightWalled) moveInput = 1;  // ����

        // ��ⰴ������˲��
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

        // �����������������ٶȱ���
        if (pressCount >= 2)
        {
            speedMultiplier = 2.0f; // �������μ����ϣ��ٶȼӱ����ɸ�����Ҫ��������
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

        // ���ﷴת
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
            if (isGrounded)//������֮��
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                //dashingCondition = true;
                isGrounded = false;
            }
        }
    }
}


