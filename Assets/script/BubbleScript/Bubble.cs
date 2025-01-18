using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float size;
    private float destroyTime;  // 泡泡的销毁时间

    // 泡泡的构造函数,用于测试
    public void Initialize(float bubbleSize)
    {
        size = bubbleSize;
        transform.localScale = Vector3.one * size;
        // 根据泡泡大小来设置销毁时间（例如：泡泡大小越大，持续的时间越长）
        destroyTime = size * 2f;  // 假设每单位大小，持续时间为 2 秒

        // 启动销毁泡泡的协程
        StartCoroutine(DestroyBubbleAfterTime());
    }

    // 泡泡销毁的协程
    private IEnumerator DestroyBubbleAfterTime()
    {
        // 等待指定的销毁时间
        yield return new WaitForSeconds(destroyTime);

        // 销毁泡泡
        Destroy(gameObject);
    }
}
