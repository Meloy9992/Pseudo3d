using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadScene(int idScene)
    {
        SceneManager.LoadScene(idScene, LoadSceneMode.Additive);
    }

    public static Scene GetSceneById(int idScene)
    {
        return SceneManager.GetSceneByBuildIndex(idScene);
    }
}
