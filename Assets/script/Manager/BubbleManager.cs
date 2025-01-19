using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // ���� TextMeshPro

public class BubbleManager : MonoBehaviour
{
    

    [Header("UI ��ʾ")]
    public TextMeshProUGUI bubbleTypeText;  // ��ǰ�������͵��ı���ʾ
    public TextMeshProUGUI bubbleCountText;  // ʣ�������������ı���ʾ
    public Image bubbleImage;  // ��ʾ����ͼƬ�� UI Image ���



    private PlayerController playerController;  // ������ҿ������Ա��ȡ������Ϣ
    private Sprite currentBubbleSprite;  // ��ǰ���ݵ�ͼƬ

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
            //��ʾ��ǰ��������
            bubbleTypeText.text =playerController.bubbleList[playerController.index].name;
            //Debug.Log("��ǰ���ݣ�" + playerController.bubbleList[playerController.index].name);
        }


        if(bubbleCountText != null)
        {
            //��ʾ��ǰ��������
            bubbleCountText.text = "left:"+(playerController.maxTimes - playerController.currentTimes);
            //Debug.Log("ʣ��������" + (playerController.maxTimes - playerController.currentTimes));
        }

        // ��ȡ��ǰ�������͵�ͼƬ����ʾ
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
                    Debug.LogWarning("δ�ҵ���Ӧ���Ƶ�����ͼƬ: " + bubbleName);
                    break;
            }

            // ���� Image ����� sprite
            if (bubbleImage != null && currentBubbleSprite != null)
            {
                bubbleImage.sprite = currentBubbleSprite;
            }
        }
    }
}
