using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE;

    [Header("References")]
    public AnimationCurve healthBarFade;

    void Start()
    {
        INSTANCE = this;
    }

    public void FADE_ReturnToMainMenu(bool save)
    {
        if (save)
        {
            Fader.INSTANCE.FadeTo(() => ReturnToMainMenu(true), () => SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu") && FileManager.DoesSaveExist(Universe.universeName), 0.5f);
        }

        else
        {
            Fader.INSTANCE.FadeTo(() => ReturnToMainMenu(false), () => SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"), 0.5f);
        }
        
    }

    private void ReturnToMainMenu(bool save)
    {
        if(save) Universe.INSTANCE.SaveUniverse();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
    }
}
