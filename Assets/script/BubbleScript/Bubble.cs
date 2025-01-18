using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public GameObject bubbleObject;
    public Rigidbody2D playerRigidbody;
    public GameObject player;
    [Header("��������")]
    public float size;
    public float destroyTime;  // ���ݵ�����ʱ��
    public float time;
    public float maxSize;
    private bool isGrowing = true;
    private SpriteRenderer spriteRenderer;  // ���ݵ�SpriteRenderer�����ڿ���͸����


    // ���ݵĹ��캯��,���ڲ���
    public virtual void Initialize(float bubbleSize)
    {
        size = bubbleSize;
        transform.localScale = Vector3.one * size;
        // �������ݴ�С����������ʱ�䣨���磺���ݴ�СԽ�󣬳�����ʱ��Խ����
        spriteRenderer = GetComponent<SpriteRenderer>();  // ��ȡSpriteRenderer���
        //��ȡplayer��Ϣ
        player = GameObject.FindGameObjectWithTag("Player");
        playerRigidbody = player.gameObject.GetComponent<Rigidbody2D>();


        
    }
    // �������ݴ�С�����ڰ�ס���ʱ���Ӵ�С��
    public void Grow(float sizeIncrease)
    {
        if (isGrowing)
        {
            size = Mathf.Min(size + sizeIncrease, maxSize);  // ���Ӵ�С�����ΪmaxSize
            transform.localScale = Vector3.one * size;
        }
        //Debug.Log(size);
    }

    // ֹͣ���ݵ�����
    public void StopGrowing()
    {
        isGrowing = false;
    }
    // �����������ٵĶ���
    public void StartDestroyCountdown()
    {
        StartCoroutine(DestroyBubbleWithFade());
    }

    // �������ٵ�Э�̣���͸���Ƚ��䶯����
    private IEnumerator DestroyBubbleWithFade()
    {
        float elapsedTime = 0f;  // �Ѿ�����ʱ��

        // ����ʱ��Ϊ����ʱ���һ�룬�ö�������һ��ʱ��
        float animationTime = destroyTime * 0.5f*size;

        // ��ʼ��͸����
        Color initialColor = spriteRenderer.color;

        // ͸���Ƚ��䶯��
        while (elapsedTime < animationTime)
        {
            float lerpFactor = elapsedTime / animationTime;

            // ͸���Ƚ���
            Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(1f, 0f, lerpFactor));
            spriteRenderer.color = newColor;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // ��������ʱ��������
        Destroy(gameObject);
    }

    public virtual void Start() { }
    public virtual void Update() { }
}
