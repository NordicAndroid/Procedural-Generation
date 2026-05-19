using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject player;
    public Canvas startMenu;
    public void SpawnPlayer()
    {
        player.SetActive(true);
        startMenu.enabled = false;
    }

}
