using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float size;
    public float destroyTime;  // ���ݵ�����ʱ��
    public GameObject bubbleObject;
    public Rigidbody2D playerRigidbody;
    public GameObject player;
    public float time;
    public float maxSize;
    private bool isGrowing = true;

    // ���ݵĹ��캯��,���ڲ���
    public virtual void Initialize(float bubbleSize)
    {
        size = bubbleSize;
        transform.localScale = Vector3.one * size;
        // �������ݴ�С����������ʱ�䣨���磺���ݴ�СԽ�󣬳�����ʱ��Խ����

        //��ȡplayer��Ϣ
        player = GameObject.FindGameObjectWithTag("Player");
        playerRigidbody = player.gameObject.GetComponent<Rigidbody2D>();


        // �����������ݵ�Э��
        StartCoroutine(DestroyBubbleAfterTime());
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
    // �������ٵ�Э��
    private IEnumerator DestroyBubbleAfterTime()
    {
        // �ȴ�ָ��������ʱ��
        yield return new WaitForSeconds(destroyTime);

        // ��������
        Destroy(gameObject);
    }

    public virtual void Start() { }
    public virtual void Update() { }
}
