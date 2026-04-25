using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    private bool isOpen;
    private void Start()
    {
        CloseUI();
    }
    public void OpenUI()
    {
        this.gameObject.SetActive(true);
    }
    public void CloseUI()
    {
        this.gameObject.SetActive(false);
    }
}
