using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Configuración de Música")]
    public List<AudioClip> pistasDeAudio;
    public bool reproducirEnOrden = true;
    public float volumen = 0.5f;

    private AudioSource audioSource;
    private int indiceActual = 0;

    private static MusicManager instancia;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        instancia = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = false;
        audioSource.volume = volumen;

        float volumenGuardado = PlayerPrefs.GetFloat("VolumenMusica", 1f);
        CambiarVolumen(volumenGuardado);

        if (pistasDeAudio.Count > 0)
        {
            ReproducirSiguientePista();
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying && pistasDeAudio.Count > 0)
        {
            ReproducirSiguientePista();
        }
    }

    void ReproducirSiguientePista()
    {
        if (pistasDeAudio.Count == 0) return;

        if (reproducirEnOrden)
        {
            audioSource.clip = pistasDeAudio[indiceActual];
            indiceActual = (indiceActual + 1) % pistasDeAudio.Count;
        }
        else
        {
            int nuevoIndice = Random.Range(0, pistasDeAudio.Count);

            while (nuevoIndice == indiceActual && pistasDeAudio.Count > 1)
            {
                nuevoIndice = Random.Range(0, pistasDeAudio.Count);
            }

            indiceActual = nuevoIndice;
            audioSource.clip = pistasDeAudio[indiceActual];
        }

        audioSource.Play();
    }

    public void CambiarVolumen(float nuevoVolumen)
    {
        volumen = Mathf.Clamp01(nuevoVolumen);
        audioSource.volume = volumen;
        PlayerPrefs.SetFloat("VolumenMusica", volumen);
    }

    public void PausarMusica()
    {
        audioSource.Pause();
    }

    public void ReanudarMusica()
    {
        audioSource.UnPause();
    }

    public void DetenerMusica()
    {
        audioSource.Stop();
    }
}
