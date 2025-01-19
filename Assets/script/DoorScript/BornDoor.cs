using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornDoor : MonoBehaviour
{

    public GameObject playerPrefab;  // 玩家的预制件
    private GameObject playerInstance;  // 玩家实例
    public Camera mainCamera;       // 主摄像机
    void Start()
    {
        // 玩家初始出生
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (playerPrefab != null && gameObject.transform != null)
        {
            // 在出生点生成玩家实例
            playerInstance = Instantiate(playerPrefab, gameObject.transform.position, gameObject.transform.rotation);
            if (mainCamera != null)
            {
                SetCameraFollow();
            }
        }
        else
        {
            Debug.LogError("Player prefab or spawn point not set!");
        }
    }
    public void RespawnPlayer()
    {
        if (playerInstance != null)
        {
            // 销毁当前玩家实例
            Destroy(playerInstance);
        }
        // 重新生成玩家
        SpawnPlayer();
        if (mainCamera != null)
        {
            SetCameraFollow();
        }
    }

    void SetCameraFollow()
    {
        if (playerInstance != null)
        {
            // 设置摄像机跟随玩家
            mainCamera.transform.SetParent(playerInstance.transform);
            mainCamera.transform.localPosition = new Vector3(0, 4, -10); // 调整摄像机相对于玩家的位置
        }
        else
        {
            Debug.LogError("Player instance is null, cannot set camera follow.");
        }
    }
}
