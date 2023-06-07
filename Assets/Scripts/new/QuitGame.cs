using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // если нажата q то вменить оружие
        {
            Quit();
        }
    }

    public void Quit()
    {
        Application.Quit(); // Выход из игры
    }
}
