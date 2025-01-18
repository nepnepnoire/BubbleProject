using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [Header("参数倍率")]
    public float time;
    public float maxSize;
    public float size;
    private float destroyTime;  // 泡泡的销毁时间
    private bool isGrowing = true;

    // 泡泡的构造函数,用于测试
    public void Initialize(float bubbleSize)
    {
        size = bubbleSize;
        transform.localScale = Vector3.one * size;
        // 根据泡泡大小来设置销毁时间（例如：泡泡大小越大，持续的时间越长）
        //destroyTime = size * time;  // 假设每单位大小，持续时间为 2 秒

    }
    // 更新泡泡大小（用于按住鼠标时增加大小）
    public void Grow(float sizeIncrease)
    {
        if (isGrowing)
        {
            size = Mathf.Min(size + sizeIncrease, maxSize);  // 增加大小，最大为maxSize
            transform.localScale = Vector3.one * size;
        }
        //Debug.Log(size);
    }

    // 停止泡泡的增长
    public void StopGrowing()
    {
        isGrowing = false;
    }

    // 泡泡销毁的协程
    private IEnumerator DestroyBubbleAfterTime()
    {
        // 等待指定的销毁时间
        yield return new WaitForSeconds(destroyTime);

        // 销毁泡泡
        Destroy(gameObject);
    }

    // 启动销毁倒计时
    public void StartDestroyCountdown(float destroyTime)
    {
        StartCoroutine(DestroyBubbleAfterTime(destroyTime));
    }

    // 泡泡销毁的协程
    private IEnumerator DestroyBubbleAfterTime(float destroyTime)
    {
        // 等待指定的销毁时间
        yield return new WaitForSeconds(destroyTime);

        // 销毁泡泡
        Destroy(gameObject);
    }
}
