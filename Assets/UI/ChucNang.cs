using UnityEngine;
using UnityEngine.SceneManagement;

public class ChucNang : MonoBehaviour
{
    // AudioSource dùng để phát nhạc
    private AudioSource musicAudioSource;

    // Biến để theo dõi trạng thái nhạc
    private bool isMusicPlaying = true;

    // Awake được gọi khi script được tải
    private void Awake()
    {
        // Lấy AudioSource từ EventSystem
        GameObject eventSystem = GameObject.Find("EventSystem");
        if (eventSystem != null)
        {
            musicAudioSource = eventSystem.GetComponent<AudioSource>();
        }

        // Nếu không tìm thấy, thử các phương pháp khác
        if (musicAudioSource == null)
        {
            // Lấy component AudioSource từ GameObject chứa script này
            musicAudioSource = GetComponent<AudioSource>();

            // Nếu vẫn không tìm thấy, tìm AudioSource đầu tiên trong scene
            if (musicAudioSource == null)
            {
                AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
                if (allAudioSources.Length > 0)
                {
                    musicAudioSource = allAudioSources[0];
                }
            }
        }
    }

    public void PLAY()
    {
        SceneManager.LoadScene(1);
    }

    public void BACK()
    {
        SceneManager.LoadScene(0);
    }

    public void EXIT()
    {
        Application.Quit();
    }

    public void OnOffMusic()
    {
        if (musicAudioSource == null)
        {
            GameObject eventSystem = GameObject.Find("EventSystem");
            if (eventSystem != null)
            {
                musicAudioSource = eventSystem.GetComponent<AudioSource>();
            }
        }

        if (musicAudioSource != null)
        {
            // Đảo ngược trạng thái mute
            isMusicPlaying = !isMusicPlaying;

            musicAudioSource.mute = !isMusicPlaying;

            Debug.Log(isMusicPlaying ? "Nhạc đang phát (unmute)" : "Nhạc tắt tiếng (mute)");
        }
        else
        {
            Debug.LogWarning("Không tìm thấy AudioSource để phát nhạc!");
        }
    }
}