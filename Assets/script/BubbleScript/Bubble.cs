using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("��������")]
    public float time;
    public float maxSize;
    public float size;
    private float destroyTime;  // ���ݵ�����ʱ��
    private bool isGrowing = true;

    // ���ݵĹ��캯��,���ڲ���
    public void Initialize(float bubbleSize)
    {
        size = bubbleSize;
        transform.localScale = Vector3.one * size;
        // �������ݴ�С����������ʱ�䣨���磺���ݴ�СԽ�󣬳�����ʱ��Խ����
        //destroyTime = size * time;  // ����ÿ��λ��С������ʱ��Ϊ 2 ��

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

    // �������ٵ���ʱ
    public void StartDestroyCountdown(float destroyTime)
    {
        StartCoroutine(DestroyBubbleAfterTime(destroyTime));
    }

    // �������ٵ�Э��
    private IEnumerator DestroyBubbleAfterTime(float destroyTime)
    {
        // �ȴ�ָ��������ʱ��
        yield return new WaitForSeconds(destroyTime);

        // ��������
        Destroy(gameObject);
    }
}
