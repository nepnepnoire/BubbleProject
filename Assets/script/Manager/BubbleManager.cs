using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // 引用 TextMeshPro

public class BubbleManager : MonoBehaviour
{
    

    [Header("UI 显示")]
    public TextMeshProUGUI bubbleTypeText;  // 当前泡泡类型的文本显示
    public TextMeshProUGUI bubbleCountText;  // 剩余泡泡数量的文本显示
    public Image bubbleImage;  // 显示泡泡图片的 UI Image 组件



    private PlayerController playerController;  // 引用玩家控制器以便获取泡泡信息
    private Sprite currentBubbleSprite;  // 当前泡泡的图片

    // Start is called before the first frame update
    void Start()
    {
       
    }



    // Update is called once per frame
    void Update()
    {
        playerController = FindObjectOfType<PlayerController>();
        UpdateBubbleUI();
    }

    private void UpdateBubbleUI()
    {
        if (bubbleTypeText != null) {
            //显示当前泡泡类型
            bubbleTypeText.text =playerController.bubbleList[playerController.index].name;
            //Debug.Log("当前泡泡：" + playerController.bubbleList[playerController.index].name);
        }


        if(bubbleCountText != null)
        {
            //显示当前泡泡数量
            bubbleCountText.text = "left:"+(playerController.maxTimes - playerController.currentTimes);
            //Debug.Log("剩余数量：" + (playerController.maxTimes - playerController.currentTimes));
        }

        // 获取当前泡泡类型的图片并显示
        if (bubbleImage != null)
        {
            SetBubbleImage();
        }
    }

    private void SetBubbleImage()
    {
        GameObject currentBubbleObject = playerController.bubbleList[playerController.index];
        if (currentBubbleObject != null)
        {
            string bubbleName = currentBubbleObject.name;

            switch(bubbleName)
            {
                case "RiseExampleBubble":
                    currentBubbleSprite = Resources.Load<Sprite>(bubbleName);
                    break;

                default:
                    Debug.LogWarning("未找到对应名称的泡泡图片: " + bubbleName);
                    break;
            }

            // 更新 Image 组件的 sprite
            if (bubbleImage != null && currentBubbleSprite != null)
            {
                bubbleImage.sprite = currentBubbleSprite;
            }
        }
    }
}
