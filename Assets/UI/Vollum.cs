using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundToggleAnimated : MonoBehaviour
{
    public Button toggleButton;
    public Image buttonImage;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public float animationTime = 0.2f;
    private bool isMuted = false;

    // Reference to AudioSource
    private AudioSource musicAudioSource;

    void Start()
    {
        // Kiểm tra các tham chiếu trước khi sử dụng
        if (toggleButton == null)
        {
            toggleButton = GetComponent<Button>();
            Debug.LogWarning("toggleButton chưa được gán, đang tự động tìm trên GameObject hiện tại");
        }

        if (buttonImage == null)
        {
            buttonImage = GetComponent<Image>();
            Debug.LogWarning("buttonImage chưa được gán, đang tự động tìm trên GameObject hiện tại");
        }

        // Kiểm tra null một lần nữa trước khi sử dụng
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(OnToggleClick);
        }
        else
        {
            Debug.LogError("toggleButton không tìm thấy, hãy gán trong Inspector");
            return; // Thoát khỏi hàm nếu không tìm thấy
        }

        // Tìm AudioSource
        FindAudioSource();

        // Khởi tạo trạng thái âm thanh từ PlayerPrefs
        isMuted = PlayerPrefs.GetInt("SoundMuted", 0) == 1;

        // Áp dụng trạng thái âm thanh
        ApplySoundState();

        // Cập nhật icon
        if (buttonImage != null)
        {
            UpdateButtonVisual();
        }
        else
        {
            Debug.LogError("buttonImage không tìm thấy, hãy gán trong Inspector");
        }
    }

    void OnToggleClick()
    {
        StartCoroutine(ToggleSound());
    }

    IEnumerator ToggleSound()
    {
        // Kiểm tra tham chiếu
        if (buttonImage == null)
        {
            Debug.LogError("buttonImage chưa được gán trong Inspector");
            yield break;
        }

        // Hiệu ứng phóng to
        buttonImage.transform.localScale = Vector3.one * 1.2f;

        yield return new WaitForSeconds(animationTime);

        // Thu nhỏ về kích thước bình thường
        buttonImage.transform.localScale = Vector3.one;

        // Đổi trạng thái âm thanh 
        isMuted = !isMuted;

        // Lưu trạng thái
        PlayerPrefs.SetInt("SoundMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        // Áp dụng trạng thái âm thanh
        ApplySoundState();

        // Cập nhật hình ảnh của nút
        UpdateButtonVisual();
    }

    private void FindAudioSource()
    {
        GameObject eventSystem = GameObject.Find("EventSystem");
        if (eventSystem != null)
        {
            musicAudioSource = eventSystem.GetComponent<AudioSource>();
        }

        if (musicAudioSource == null)
        {
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            if (allAudioSources.Length > 0)
            {
                musicAudioSource = allAudioSources[0];
            }
        }

        // Bắt đầu phát nhạc và cho phép loop nếu tìm thấy
        if (musicAudioSource != null)
        {
            musicAudioSource.loop = true;

            if (!musicAudioSource.isPlaying)
            {
                musicAudioSource.Play();
            }
        }
    }

    private void ApplySoundState()
    {
        if (musicAudioSource != null)
        {
            // KHÔNG gọi Play() lại nếu đã từng phát rồi
            if (!musicAudioSource.isPlaying && !isMuted)
            {
                musicAudioSource.loop = true;
                musicAudioSource.Play(); // Chỉ play nếu chưa chạy và đang bật
            }

            // Mute hoặc unmute
            musicAudioSource.mute = isMuted;

            Debug.Log("Mute bằng button: " + isMuted + " | Object: " + musicAudioSource.gameObject.name);
        }
        else
        {
            AudioListener.volume = isMuted ? 0f : 1f;
            Debug.Log("Âm lượng toàn cục: " + (isMuted ? "0" : "1"));
        }
    }





    void UpdateButtonVisual()
    {
        // Kiểm tra các tham chiếu trước khi sử dụng
        if (buttonImage == null || soundOnSprite == null || soundOffSprite == null)
        {
            Debug.LogError("buttonImage hoặc sprite chưa được gán trong Inspector");
            return;
        }

        // Chỉ cập nhật sprite theo trạng thái
        buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;

        // Thay đổi màu sắc
        buttonImage.color = isMuted ? new Color(0.7f, 0.7f, 0.7f) : Color.white;
    }
}