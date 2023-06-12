using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public Image slider; // Получить слайдер
    public GameObject screen;
    //public GameObject player;
    //public int idScene = 0;
    public void LoadLevel()
    {
        DataGame data = new FileDataHandler(Application.persistentDataPath, "Save.json").Load(); // TODO - устранить хардкод

        screen.SetActive(true);
        StartCoroutine(LoadSceneGame(data.SceneNumber));
    }

    IEnumerator LoadSceneGame(int idScene)
    {
        Debug.LogError("ID СЦЕНЫ КОТОРУЮ ПЕРЕДАЛИ " + idScene);
        SceneManager.LoadScene(1);
        AsyncOperation LoadAsync = SceneManager.LoadSceneAsync(idScene); // Начать загружать сцену по id

        while (!LoadAsync.isDone) // Пока сцена не загрузится
        {
            float progress = LoadAsync.progress; // Получить прогресс загрузки
            Debug.LogError("Прогресс загрузки сцены " + idScene + " " + progress);
            slider.fillAmount = progress; // Заполнить слайдер прогрессом
            yield return null; // Так как это корутина(Сопрограмма) - надо вернуть что-то
        }
       
    }
}
