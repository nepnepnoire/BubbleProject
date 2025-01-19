using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // ����UI���

public class MainMenuManager : MonoBehaviour
{
    // �������Ʋ���˵��������ʾ������
    public GameObject InstructionsPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("level1");
    }
    // Start is called before the first frame update
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
    }

    // �رղ���˵�����
    public void CloseInstructions()
    {
        InstructionsPanel.SetActive(false);  // ���ز���˵�����
    }
}
