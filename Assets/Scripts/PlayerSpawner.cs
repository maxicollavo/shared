using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {

        if (player == Runner.LocalPlayer)
        {
            Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
    }

}
