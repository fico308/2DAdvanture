using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public GameObject startGameButton;

    private void OnEnable() {
        EventSystem.current.SetSelectedGameObject(startGameButton);
    }

    private void Update() {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(startGameButton);
        }
    }

    public void ExitGame()
    {
        Debug.Log("exit");
        Application.Quit();
    }

}
