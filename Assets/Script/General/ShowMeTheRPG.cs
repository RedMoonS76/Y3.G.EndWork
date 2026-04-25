using UnityEngine;
using UnityEngine.UI;

public class ShowMeTheRPG : MonoBehaviour
{
    public Text text;
    public PlayerController playerController;
    public RPGattribute rP;
    private void OnEnable()
    {
        text = this.GetComponent<Text>();
    }
    private void FixedUpdate()
    {
        rP.OutputToUIText(text, playerController.rpg_Sum);
    }
}
