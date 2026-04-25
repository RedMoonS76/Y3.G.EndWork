using UnityEngine;

public class StopWall : MonoBehaviour
{
    private void Update()
    {
        if (GameObject.FindWithTag("Enemy") == null)
        {
            this.gameObject.SetActive(false);
        }
    }
}
