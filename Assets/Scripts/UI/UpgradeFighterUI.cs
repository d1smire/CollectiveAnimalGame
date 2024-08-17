using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeFighterUI : MonoBehaviour
{
    [SerializeField] private string fighterName;
    
    private Button fighterButton;
    private Image fighterimage;

    [SerializeField] private TextMeshProUGUI[] _characteristicNaming;
    [SerializeField] private Button[] _upgradebuttons;

    [SerializeField] private PlayerParams currentAnimalParams;

    private CanvasSwitcher _canvasSwitcher;
    private CardManager _cardManager;
    private Notification _notification;
    private TextBtnGameObjData _textBtnGameObjData;
    private FighterData _fighterData;


    private void Start()
    {
        _textBtnGameObjData = GameObject.Find("Text&ButtonGameObj").GetComponent<TextBtnGameObjData>();
        _notification = GameObject.Find("Notification(Clone)").GetComponent<Notification>();
        _canvasSwitcher = GameObject.Find("CanvasSwitcher").GetComponent<CanvasSwitcher>();
        _cardManager = GameObject.Find("PlayfabManager(Clone)").GetComponent<CardManager>();
        _fighterData = GameObject.Find("SaveFighterData(Clone)").GetComponent<FighterData>();

        fighterButton = GetComponent<Button>();
        fighterimage = GameObject.Find("ImageAnimal").GetComponent<Image>();

        fighterButton.onClick.AddListener(CreateAnimalCharacteristics);

        currentAnimalParams = Resources.Load<PlayerParams>("CurrentCharCharacteristics");
    }

    private void OnDestroy()
    {
        fighterButton.onClick.RemoveAllListeners();
    }

    public void CreateAnimalCharacteristics()
    {
        SetCurrentParameters();
        SetTextMassive();
        SetText();
        SetUpgradeAbleBtnMassive();
        SetUpgradeBtn();
        GameObject.Find("ImageIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>("AnimalsIcon/" + "Animal" + currentAnimalParams.AnimalId + "Icon");
        _canvasSwitcher.OpenAnimalParams();
        ChooseFighter();
    }

    private void SetTextMassive() 
    {
        var text = _textBtnGameObjData.SendText();

        _characteristicNaming = new TextMeshProUGUI[text.Length];

        for (int i = 0; i < _characteristicNaming.Length; i++)
        {
            _characteristicNaming[i] = text[i].GetComponent<TextMeshProUGUI>();
        }
    }

    private void SetText() 
    {
        _characteristicNaming[0].text = "Name: " + currentAnimalParams.Name;
        _characteristicNaming[1].text = "Char XP: " + currentAnimalParams.CharXP + " / " + currentAnimalParams.NeedXPToLvlUp;
        _characteristicNaming[2].text = "Char Level: " + currentAnimalParams.CharLevel;
        _characteristicNaming[3].text = "Points for upgrade: " + currentAnimalParams.PointsForUpgrade;

        _characteristicNaming[4].text = "Health: " + currentAnimalParams.Health;
        _characteristicNaming[5].text = "ATK: " + currentAnimalParams.ATK;
        _characteristicNaming[6].text = "DEF: " + currentAnimalParams.DEF;
        _characteristicNaming[7].text = "Speed: " + currentAnimalParams.Speed;
        _characteristicNaming[8].text = "Crit chance: " + currentAnimalParams.CritChance;
        _characteristicNaming[9].text = "Energy recovery: " + currentAnimalParams.EnergyRecovery;
        _characteristicNaming[10].text = "Energy for ultimates: " + currentAnimalParams.EnergyForUltimates;
        _characteristicNaming[11].text = "Need energy to use ultimates: " + currentAnimalParams.NeedEnergyToUseUltimates;
    }

    private void SetUpgradeAbleBtnMassive() 
    {
        var btn = _textBtnGameObjData.SendButtons();

        _upgradebuttons = new Button[btn.Length];

        for (int i = 0; i < _upgradebuttons.Length; i++)
        {
            _upgradebuttons[i] = btn[i].GetComponent<Button>();
        }
    }

    private void SetUpgradeBtn() 
    {
        _upgradebuttons[0].onClick.AddListener(upgradeCharXp);
        
        for (int i = 1; i < _upgradebuttons.Length; i++)
        {
            int index = i;
            _upgradebuttons[i].onClick.AddListener(() => upgradeParams(index));
        }

    }

    public void SetCurrentParameters() 
    {
        currentAnimalParams.SetParams(_cardManager.GetCharParams(fighterName));
    }

    private void upgradeCharXp()
    {
        currentAnimalParams.AddExperience(5);
        SetText();
    }

    private void upgradeParams(int paramsNumber) 
    {
        if (IsEnoughPoints())
        {
            switch (paramsNumber)
            {
                case 1:
                    currentAnimalParams.UpgradeHealth();
                    break;
                case 2:
                    currentAnimalParams.UpgradeATK();
                    break;
                case 3:
                    currentAnimalParams.UpgradeDEF();
                    break;
                case 4:
                    currentAnimalParams.UpgradeSpeed();
                    break;
                case 5:
                    currentAnimalParams.UpgradeCritChance();
                    break;
                case 6:
                    currentAnimalParams.UpgradeEnergyRecovery();
                    break;
            }
            SetText();
        }
        else 
        {
            _notification.SomeInformation(InfoStatus.Error, "Недостатньо очків для покращення");
        }
    }
    private bool IsEnoughPoints() 
    {
        if (currentAnimalParams.PointsForUpgrade > 0) 
        {
            return true;
        }
        return false;
    }

    public void ChooseFighter() 
    {
        fighterimage.sprite = Resources.Load<Sprite>("AnimalsIcon/" + "Animal" + currentAnimalParams.AnimalId + "Icon");
        fighterimage.color = Color.white;

        _fighterData.SetFighterInfo(fighterName, currentAnimalParams);
    }

}
