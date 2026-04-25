using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject SettingPanel;
    public GameObject ExitPanel;

    public Slider VoiceSlider;
    public Slider SfxSlider;

    public Button BtnSetting;
    public Button BtnExit;
    public Button Btnback;
    public Button Btnyes;
    public Button Btnno;

    private void Start()
    {
        // 初始化界面状态
        MainPanel.SetActive(true);
        SettingPanel.SetActive(false);
        ExitPanel.SetActive(false);

        // 绑定按钮事件
        BtnSetting.onClick.AddListener(ShowSettings);
        BtnExit.onClick.AddListener(ShowExit);
        Btnback.onClick.AddListener(ReturnToMainMenu);
        Btnyes.onClick.AddListener(ExitGame);
        Btnno.onClick.AddListener(ReturnToMainMenu);

    }

    private void ShowSettings()
    {
        MainPanel.SetActive(false);
        SettingPanel.SetActive(true);
    }

    private void ShowExit()
    {
        MainPanel.SetActive(false);
        ExitPanel.SetActive(true);
    }

    private void ReturnToMainMenu()
    {
        SettingPanel.SetActive(false);
        ExitPanel.SetActive(false);
        MainPanel.SetActive(true);

    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    private void ExitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Exiting game (simulated in editor).");
#else
    // 在构建的游戏中，真正退出应用程序
    Application.Quit();
#endif

    }

    public void ResetInterfaceStates()
    {
        MainPanel.SetActive(true);
        SettingPanel.SetActive(false);
        ExitPanel.SetActive(false);
    }
}