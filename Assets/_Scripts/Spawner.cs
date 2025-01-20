using Photon.Pun;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoint;

    void Start()
    {
        Transform spawn;

        if (PhotonNetwork.IsMasterClient)
        {
            spawn = spawnPoint[0];

        }
        else
        {

            spawn = spawnPoint[1];
        }

        PhotonNetwork.Instantiate("Player", spawn.position, Quaternion.identity);
    }

    
}
