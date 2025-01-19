using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ��ȡ��ǰ����������
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // ������һ������������
            int nextSceneIndex = currentSceneIndex + 1;
            // ����Ƿ�����һ������
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                // ������һ������
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("You have completed all levels!");
            }
        }
    }
}
