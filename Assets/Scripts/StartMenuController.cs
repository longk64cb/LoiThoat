using Everest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] Button startBtn;
    [SerializeField] GameObject startMenuFx;
    [SerializeField] GameObject gameIntroGO;
    public void OnStartGame()
    {
        startBtn.gameObject.SetActive(false);
        LoadingController.I.Loading(() =>
        {
            startMenuFx.SetActive(false);
        }, () =>
        {
            gameIntroGO.SetActive(true);
            this.Delay(6f, () =>
            {
                gameIntroGO.SetActive(false);
                GameController.I.OnGameFirstDialog();
            });
        });
    }
}
