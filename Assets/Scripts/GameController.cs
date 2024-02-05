using Everest;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    public IngameData gameData;
    public StartMenuController startMenuController;
    public MainUI mainUI;
    public AchivementUIController achivement;
    public PlayerController playerController;

    [SerializeField] Sprite deadAchiveSprite;

    private void Start()
    {
        startMenuController.gameObject.SetActive(true);
        AudioManager.I.Play(SoundID.day);
    }

    #region Map
    [SerializeField] MapController[] arrMap;
    #endregion

    public GameTimeEnum gameTime;

    public void SetGameTime(GameTimeEnum gameTime)
    {
        this.gameTime = gameTime;

        LoadingController.I.Loading(() =>
        {
            foreach (var map in arrMap)
            {
                map.OnChangeGameTime(gameTime);
            }

            mainUI.SetTaskText("Điều tra ao cá vào ban đêm");

            if (gameTime == GameTimeEnum.Night)
            {
                AudioManager.I.Stop(SoundID.day);
                AudioManager.I.Stop(SoundID.yard_BG_1);
                AudioManager.I.Stop(SoundID.yard_BG_2);
                AudioManager.I.Play(SoundID.night);
                if (playerController.CurrentMap == MapEnum.Yard)
                    AudioManager.I.Play(SoundID.yard_BG_night);
            }
        });
    }
    #region Game Story
    public DialogUIController dialogController;
    public TutorialPopup tutorialPopup;
    public EndGameController endGameController;

    [SerializeField] Dialog firstDialog;
    [SerializeField] Dialog lakeDialog;

    public bool isEndGame;


    public void OnGameFirstDialog()
    {
        dialogController.StartDialog(firstDialog, () =>
        {
            LoadingController.I.Loading(() =>
            {
                startMenuController.gameObject.SetActive(false);
                AudioManager.I.Stop(SoundID.night);
            }, () =>
            {
                AudioManager.I.Play(SoundID.day);
                tutorialPopup.Show();
            });
        });
    }

    public void StartDialog(Dialog dialog)
    {
        if (isEndGame)
            return;

        dialogController.StartDialog(dialog);
    }

    public void OnArriveLake()
    {
        StartDialog(lakeDialog);
    }

    public void EndGame()
    {
        isEndGame = true;
        endGameController.Show();
        achivement.Show(deadAchiveSprite);
        AudioManager.I.StopAllSound();
    }
    #endregion

    public void OnTryAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }

    public void UpdateMeetEveryoneTask(int character)
    {
        gameData.characterTalked[(CharacterEnum)character] = true;
        int count = gameData.characterTalked.Count(x => x.Value);
        mainUI.SetTaskText($"Gặp mọi người trong dinh thự <color=orange>{count}/3</color>");

        if (count >= 3)
            mainUI.SetTaskText("Điều tra ao cá vào ban đêm");
    }

    public void SetNextTask()
    {
        if (gameData.characterTalked.Count(x => x.Value) >= 3)
            mainUI.SetTaskText("Điều tra ao cá vào ban đêm");
    }
}

public enum GameTimeEnum
{
    Day,
    Night
}

public enum MapEnum
{
    Bedroom,
    LivingRoom,
    Yard
}
