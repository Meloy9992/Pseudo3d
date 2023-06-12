using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public Image slider; // �������� �������
    public GameObject screen;
    //public GameObject player;
    //public int idScene = 0;
    public void LoadLevel()
    {
        DataGame data = new FileDataHandler(Application.persistentDataPath, "Save.json").Load(); // TODO - ��������� �������

        screen.SetActive(true);
        StartCoroutine(LoadSceneGame(data.SceneNumber));
    }

    IEnumerator LoadSceneGame(int idScene)
    {
        Debug.LogError("ID ����� ������� �������� " + idScene);
        SceneManager.LoadScene(1);
        AsyncOperation LoadAsync = SceneManager.LoadSceneAsync(idScene); // ������ ��������� ����� �� id

        while (!LoadAsync.isDone) // ���� ����� �� ����������
        {
            float progress = LoadAsync.progress; // �������� �������� ��������
            Debug.LogError("�������� �������� ����� " + idScene + " " + progress);
            slider.fillAmount = progress; // ��������� ������� ����������
            yield return null; // ��� ��� ��� ��������(�����������) - ���� ������� ���-��
        }
       
    }
}
