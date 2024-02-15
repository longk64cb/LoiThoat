using Everest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] Button startBtn;
    [SerializeField] Button quitBtn;
    [SerializeField] GameObject startMenuFx;
    [SerializeField] GameObject gameIntroGO;
    public void OnStartGame()
    {
        AudioManager.I.Stop(SoundID.day);
        startBtn.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);
        LoadingController.I.Loading(() =>
        {
            startMenuFx.SetActive(false);
        }, () =>
        {
            gameIntroGO.SetActive(true);
            this.Delay(6f, () =>
            {
                AudioManager.I.Play(SoundID.night);
                gameIntroGO.SetActive(false);
                GameController.I.OnGameFirstDialog();
            });
        });
    }
}
