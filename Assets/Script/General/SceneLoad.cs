using UnityEngine;

public class SceneLoad : MonoBehaviour
{
    public PlayerController playerController;
    private void Start()
    {
        playerController.clearItem();
        playerController.clearGold();
    }
}
