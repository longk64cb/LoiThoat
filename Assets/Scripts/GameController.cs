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

    [SerializeField] Sprite deadAchiveSprite;

    #region Map
    [SerializeField] MapController[] arrMap;
    #endregion

    public GameTimeEnum gameTime;

    public void SetGameTime(GameTimeEnum gameTime)
    {
        this.gameTime = gameTime;

        foreach (var map in arrMap)
        {
            map.OnChangeGameTime(gameTime);
        }

        mainUI.SetTaskText("Điều tra ao cá vào ban đêm");
    }
    #region Game Story
    public DialogUIController dialogController;
    public TutorialPopup tutorialPopup;
    public EndGameController endGameController;

    [SerializeField] Dialog firstDialog;
    [SerializeField] Dialog lakeDialog;

    public bool isEndGame;

    public void Start()
    {
        //OnGameFirstDialog();
    }

    public void OnGameFirstDialog()
    {
        dialogController.StartDialog(firstDialog, () =>
        {
            LoadingController.I.Loading(() =>
            {
                startMenuController.gameObject.SetActive(false);
            }, () =>
            {
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
