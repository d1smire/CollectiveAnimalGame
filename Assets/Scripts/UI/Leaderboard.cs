using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.IO;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject ScoreColection;

    public void GetLeaderBoard() 
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "ScoreByWin",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLederboardGet, OnError);
    }

    private void OnLederboardGet(GetLeaderboardResult result)
    {
        foreach(var item in result.Leaderboard) 
        {
            GameObject ScoreCard = Resources.Load<GameObject>("PlayerScoreCard");

            var createdCard = Instantiate(ScoreCard, ScoreColection.transform);

            TextMeshProUGUI text1 = createdCard.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text1.text = (item.Position + 1).ToString();

            TextMeshProUGUI text2 = createdCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            text2.text = item.DisplayName.ToString();

            TextMeshProUGUI text3 = createdCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            text3.text = item.StatValue.ToString();
        }
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

}
