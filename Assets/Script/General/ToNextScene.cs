using UnityEngine;
using UnityEngine.SceneManagement;

public class ToNextScene : MonoBehaviour
{
    [Header("场景名称")]
    public string menuScene = "Menu";
    public string startScene = "Start";
    public string finalScene = "5";
    public string[] normalScenes = { "1", "2", "3", "4" };   // 普通关卡场景名
    public int requiredNormalCount = 5;                     // 需要通关的次数

    private const string NORMAL_COUNT_KEY = "NormalLevelCount";  // PlayerPrefs键名

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TransitionToNextScene();
        }
    }

    private void TransitionToNextScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // 1. 菜单 -> 开始场景
        if (currentScene == menuScene)
        {
            SceneManager.LoadScene(startScene);
            return;
        }

        // 2. 开始场景（清空计数器，并随机第一个普通关卡）
        if (currentScene == startScene)
        {
            PlayerPrefs.SetInt(NORMAL_COUNT_KEY, 0);
            PlayerPrefs.Save();
            SceneManager.LoadScene(GetRandomNormalScene());
            return;
        }

        // 3. 普通关卡（1~4）
        if (IsNormalScene(currentScene))
        {
            int count = PlayerPrefs.GetInt(NORMAL_COUNT_KEY, 0) + 1;
            PlayerPrefs.SetInt(NORMAL_COUNT_KEY, count);
            PlayerPrefs.Save();

            // 达到指定次数 -> 最终关卡
            if (count >= requiredNormalCount)
            {
                SceneManager.LoadScene(finalScene);
            }
            else
            {
                SceneManager.LoadScene(GetRandomNormalScene());
            }
            return;
        }

        // 4. 最终关卡 -> 通关，回到菜单并重置计数器
        if (currentScene == finalScene)
        {
            PlayerPrefs.SetInt(NORMAL_COUNT_KEY, 0);
            PlayerPrefs.Save();
            SceneManager.LoadScene(menuScene);
            return;
        }

        // 5. 保底：未知场景跳回开始场景
        Debug.LogWarning($"未识别的场景：{currentScene}，返回 Start");
        SceneManager.LoadScene(startScene);
    }

    private bool IsNormalScene(string sceneName)
    {
        foreach (string normal in normalScenes)
            if (normal == sceneName) return true;
        return false;
    }

    private string GetRandomNormalScene()
    {
        if (normalScenes.Length == 0)
        {
            Debug.LogError("没有配置任何普通关卡场景名！");
            return startScene;
        }
        int index = Random.Range(0, normalScenes.Length);
        return normalScenes[index];
    }
}