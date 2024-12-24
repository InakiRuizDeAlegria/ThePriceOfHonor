using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Scrollbar scrollbar;
    private MusicManager musicManager;

    void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();

        float volumenGuardado = PlayerPrefs.GetFloat("VolumenMusica", 1f);

        if (musicManager != null && scrollbar != null)
        {
            scrollbar.value = volumenGuardado;
            scrollbar.onValueChanged.AddListener(CambiarVolumen);
        }
    }

    public void CambiarVolumen(float valor)
    {
        if (musicManager != null)
        {
            musicManager.CambiarVolumen(valor);
        }
    }
}
