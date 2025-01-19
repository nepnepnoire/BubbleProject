using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornDoor : MonoBehaviour
{

    public GameObject playerPrefab;  // ��ҵ�Ԥ�Ƽ�
    private GameObject playerInstance;  // ���ʵ��
    public Camera mainCamera;       // �������
    void Start()
    {
        // ��ҳ�ʼ����
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (playerPrefab != null && gameObject.transform != null)
        {
            // �ڳ������������ʵ��
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
            // ���ٵ�ǰ���ʵ��
            Destroy(playerInstance);
        }
        // �����������
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
            // ����������������
            mainCamera.transform.SetParent(playerInstance.transform);
            mainCamera.transform.localPosition = new Vector3(0, 4, -10); // ����������������ҵ�λ��
        }
        else
        {
            Debug.LogError("Player instance is null, cannot set camera follow.");
        }
    }
}
