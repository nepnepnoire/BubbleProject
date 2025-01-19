using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject InstructionsPanel;
    public GameObject PauseMenuPanel;

    public bool isPaused = false;

    void Start()
    {
        // 确保暂停菜单一开始是隐藏的
        PauseMenuPanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (isPaused) { ContinueGame(); }
            else { PauseGame(); }

        }
    }


    public void ContinueGame()
    {
        isPaused = false;
        PauseMenuPanel.SetActive(false );
        Time.timeScale = 1.0f;
    }
    private void PauseGame()
    {
        isPaused = true;
        PauseMenuPanel.SetActive(true );
        Time.timeScale = 0f;
    }

    // 重新开始当前关卡
    public void RestartGame()
    {
        // 恢复时间并重新加载当前场景
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

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
        PauseMenuPanel.SetActive(false ) ;
    }

    // 关闭操作说明面板
    public void CloseInstructions()
    {
        InstructionsPanel.SetActive(false);  // 隐藏操作说明面板
        PauseMenuPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("GameStart");
    }

}
