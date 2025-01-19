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
            // 获取当前场景的索引
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            // 计算下一个场景的索引
            int nextSceneIndex = currentSceneIndex + 1;
            // 检查是否还有下一个场景
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                // 加载下一个场景
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("You have completed all levels!");
            }
        }
    }
}
