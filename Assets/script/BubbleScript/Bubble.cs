using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float size;
    private float destroyTime;  // ���ݵ�����ʱ��

    // ���ݵĹ��캯��,���ڲ���
    public void Initialize(float bubbleSize)
    {
        size = bubbleSize;
        transform.localScale = Vector3.one * size;
        // �������ݴ�С����������ʱ�䣨���磺���ݴ�СԽ�󣬳�����ʱ��Խ����
        destroyTime = size * 2f;  // ����ÿ��λ��С������ʱ��Ϊ 2 ��

        // �����������ݵ�Э��
        StartCoroutine(DestroyBubbleAfterTime());
    }

    // �������ٵ�Э��
    private IEnumerator DestroyBubbleAfterTime()
    {
        // �ȴ�ָ��������ʱ��
        yield return new WaitForSeconds(destroyTime);

        // ��������
        Destroy(gameObject);
    }
}
