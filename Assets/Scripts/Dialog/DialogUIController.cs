using DG.Tweening;
using Everest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogUIController : MonoBehaviour
{
    public const float ANIM_TIME = 0.5f;

    public Dialog currentDialog;

    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Transform rightCharacterPos;
    [SerializeField] private Transform leftCharacterPos;

    [SerializeField] private Text dialogText;
    [SerializeField] private Text characterName;
    [SerializeField] private Text characterDescription;

    [SerializeField] private GameObject nextBtn;

    [SerializeField] EnumResource<CharacterEnum, CharacterData> characterDict = new();
    [SerializeField] List<DialogChoiceController> dialogChoiceList = new();

    private CharacterEnum currentCharacter = CharacterEnum.None;
    private DialogCharacterController characterTF;

    public Action onFinishDialog;
    private bool isDead;

    public void Awake()
    {
        characterName.text = string.Empty;
        characterDescription.text = string.Empty;
        dialogText.text = string.Empty;
    }

    public void OnEnable()
    {
        currentDialog = null;
        currentCharacter = CharacterEnum.None;
        onFinishDialog = null;
        characterName.text = string.Empty;
        characterDescription.text = string.Empty;
        dialogText.text = string.Empty;
         
        foreach (var characterData in characterDict)
        {
            if (characterData.Key == CharacterEnum.None) continue;
            characterData.Value.characterTF.gameObject.SetActive(false);
        }

        foreach (var choice in  dialogChoiceList)
        {
            choice.gameObject.SetActive(false);
        }
    }

    public void StartDialog(Dialog dialog, Action onFinishDialog = null)
    {
        gameObject.SetActive(true);
        this.onFinishDialog = onFinishDialog;
        canvasGroup.DOFade(1f, ANIM_TIME).From(0f);
        SetCurrentDialog(dialog);
    }

    public void SetCurrentDialog(Dialog currentDialog)
    {
        nextBtn.SetActive(false);

        if (currentDialog == null)
        {
            EndDialog();
            return;
        }

        var sequence = RunPreAnimation(currentDialog);

        sequence.OnComplete(() =>
        {
            this.currentDialog = currentDialog;
            dialogText.text = currentDialog.dialogText;

            currentCharacter = currentDialog.character;
            characterName.text = characterDict[currentCharacter].characterName;
            characterDescription.text = $"({characterDict[currentCharacter].characterDescription})";

            //foreach (var characterData in characterDict)
            //{
            //    if (characterData.Key == CharacterEnum.None) continue;
            //    characterData.Value.characterTF.gameObject.SetActive(characterData.Key == currentCharacter);
            //}

            characterTF = characterDict[currentCharacter].characterTF;
            Sequence showSequence = DOTween.Sequence();
            if (characterTF != null)
            {
                characterTF.transform.SetParent(currentDialog.dialogPos == CharacterDialogPos.Left ? leftCharacterPos : rightCharacterPos);
                characterTF.transform.localPosition = Vector3.zero;
                showSequence.Join(characterTF.Show(currentDialog.dialogPos));
            }

            showSequence.Join(characterName.DOFade(1f, ANIM_TIME));
            showSequence.Join(characterDescription.DOFade(1f, ANIM_TIME));
            showSequence.Join(dialogText.DOFade(1f, ANIM_TIME));

            for (int i = 0; i < dialogChoiceList.Count; i++)
            {
                var choice = dialogChoiceList[i];
                if (i < currentDialog.dialogChoices.Length)
                {
                    var dialogChoice = currentDialog.dialogChoices[i];
                    choice.Setup(dialogChoice.text, dialogChoice.nextDialog);
                }
                else
                {
                    choice.gameObject.SetActive(false);
                }
            }

            nextBtn.SetActive(currentDialog.dialogChoices.Length <= 0);
        });
    }

    public void NextDialog()
    {
        SetCurrentDialog(currentDialog.nextDialog);
    }

    public void OnSelectChoice(int id)
    {
        if (dialogChoiceList[id].choiceStr == "Kiểm tra") isDead = true;

        SetCurrentDialog(dialogChoiceList[id].targetDialog);
    }

    public void EndDialog()
    {
        canvasGroup.DOFade(0f, ANIM_TIME).OnComplete(() =>
        {
            gameObject.SetActive(false);
            onFinishDialog?.Invoke();
            if (isDead) GameController.I.EndGame();
        });
    }

    public Sequence RunPreAnimation(Dialog nextDialog)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Join(dialogText.DOFade(0f, ANIM_TIME));

        if (nextDialog.character != currentCharacter)
        {
            if (characterTF != null) sequence.Join(characterTF.Hide());
            sequence.Join(characterName.DOFade(0f, ANIM_TIME));
            sequence.Join(characterDescription.DOFade(0f, ANIM_TIME));
        }
        return sequence;
    }
}

[Serializable]
public struct CharacterData
{
    public DialogCharacterController characterTF;
    public string characterName;
    public string characterDescription;
}