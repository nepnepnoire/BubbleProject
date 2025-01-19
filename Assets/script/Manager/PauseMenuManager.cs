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
        // ȷ����ͣ�˵�һ��ʼ�����ص�
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

    // ���¿�ʼ��ǰ�ؿ�
    public void RestartGame()
    {
        // �ָ�ʱ�䲢���¼��ص�ǰ����
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        // ����ڱ༭���У��˳�����ģʽ
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
               // ����ǹ����汾���˳���Ϸ
               Application.Quit();
#endif
    }
    // ��ʾ����˵�����
    public void ShowInstructions()
    {
        InstructionsPanel.SetActive(true);  // �������˵�����
        PauseMenuPanel.SetActive(false ) ;
    }

    // �رղ���˵�����
    public void CloseInstructions()
    {
        InstructionsPanel.SetActive(false);  // ���ز���˵�����
        PauseMenuPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("GameStart");
    }

}
