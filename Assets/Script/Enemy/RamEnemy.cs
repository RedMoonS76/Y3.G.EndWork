using UnityEngine;


public class RamEnemy : MonoBehaviour
{
    private void OnEnable()
    {
        if (Random.Range(0, 10) >= 5)
        {
            this.gameObject.SetActive(false);
        }
    }
}
