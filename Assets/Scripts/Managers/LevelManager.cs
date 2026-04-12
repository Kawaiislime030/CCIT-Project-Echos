using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("BGM Settings")]
    [Tooltip("在 AudioManager 的 bgm 列表中对应的编号")]
    public int bgmToPlay;

    void Start()
    {
        // 确保 AudioManager 实例存在
        if (AudioManager.instance != null)
        {
            // 调用你 AudioManager 里的 PlayBGM 函数
            AudioManager.instance.PlayBGM(bgmToPlay);
        }
    }
}