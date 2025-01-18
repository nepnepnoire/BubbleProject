using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public GameObject bubbleObject;
    public Rigidbody2D playerRigidbody;
    public GameObject player;
    [Header("参数倍率")]
    public float size;
    public float destroyTime;  // 泡泡的销毁时间
    public float time;
    public float maxSize;
    private bool isGrowing = true;
    private SpriteRenderer spriteRenderer;  // 泡泡的SpriteRenderer，用于控制透明度


    // 泡泡的构造函数,用于测试
    public virtual void Initialize(float bubbleSize)
    {
        size = bubbleSize;
        transform.localScale = Vector3.one * size;
        // 根据泡泡大小来设置销毁时间（例如：泡泡大小越大，持续的时间越长）
        spriteRenderer = GetComponent<SpriteRenderer>();  // 获取SpriteRenderer组件
        //获取player信息
        player = GameObject.FindGameObjectWithTag("Player");
        playerRigidbody = player.gameObject.GetComponent<Rigidbody2D>();


        
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
    // 启动泡泡销毁的动画
    public void StartDestroyCountdown()
    {
        StartCoroutine(DestroyBubbleWithFade());
    }

    // 泡泡销毁的协程（带透明度渐变动画）
    private IEnumerator DestroyBubbleWithFade()
    {
        float elapsedTime = 0f;  // 已经过的时间

        // 动画时间为销毁时间的一半，让动画持续一定时间
        float animationTime = destroyTime * 0.5f*size;

        // 初始的透明度
        Color initialColor = spriteRenderer.color;

        // 透明度渐变动画
        while (elapsedTime < animationTime)
        {
            float lerpFactor = elapsedTime / animationTime;

            // 透明度渐变
            Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(1f, 0f, lerpFactor));
            spriteRenderer.color = newColor;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 动画结束时销毁泡泡
        Destroy(gameObject);
    }

    public virtual void Start() { }
    public virtual void Update() { }
}
