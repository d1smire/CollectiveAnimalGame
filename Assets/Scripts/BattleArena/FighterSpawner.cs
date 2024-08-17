using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _player1Side; 
    [SerializeField] private GameObject _player2Side;

    public Fighter SpawnPlayer1(Fighter player1Template, Character characterParams)
    {
        return SpawnFighters(player1Template, _player1Side, characterParams);
    }

    public Fighter SpawnPlayer2(Fighter player2Template, Character characterParams)
    {
        return SpawnFighters(player2Template, _player2Side, characterParams);
    }

    private Fighter SpawnFighters(Fighter fighterTemplate, GameObject spawnPoints, Character characterParams)
    {
        Fighter newFighter = Instantiate(fighterTemplate, spawnPoints.transform.position, Quaternion.identity, spawnPoints.transform);
        newFighter.playerParams = characterParams;
        
        if (spawnPoints == _player2Side)
        {
            newFighter.transform.Rotate(0f, 180f, 0f);
        }

        newFighter.CurrentStayPoint(spawnPoints);

        return newFighter;
    }

}
