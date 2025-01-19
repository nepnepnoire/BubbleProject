using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // 引用UI组件

public class MainMenuManager : MonoBehaviour
{
    // 用来控制操作说明面板的显示和隐藏
    public GameObject InstructionsPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("level1");
    }
    // Start is called before the first frame update
    public void ExitGame()
    {
        // 如果在编辑器中，退出播放模式
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
               // 如果是构建版本，退出游戏
               Application.Quit();
        #endif
    }
    // 显示操作说明面板
    public void ShowInstructions()
    {
        InstructionsPanel.SetActive(true);  // 激活操作说明面板
    }

    // 关闭操作说明面板
    public void CloseInstructions()
    {
        InstructionsPanel.SetActive(false);  // 隐藏操作说明面板
    }
}
