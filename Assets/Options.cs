using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class Options : MonoBehaviour
{
    public void ToggleBackground(bool active)
    {
        Camera.main.GetComponent<CameraAI>().background.gameObject.SetActive(active);
    }

    public AudioMixer musicMixer;
    public AudioMixer soundMixer;
    public void SetMusicLevel(float slidervalue)
    {
        musicMixer.SetFloat("Music volume", Mathf.Log10(slidervalue)*20);
    }
    public void SetSoundLevel(float slidervalue)
    {
        soundMixer.SetFloat("Sounds volume", Mathf.Log10(slidervalue) * 20);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print(transform.localPosition == Vector3.zero);
            if(transform.localPosition == Vector3.zero)
            {
                transform.DOMove(MainMeniAI.MM.outside.position, 1);
            }
            else transform.DOLocalMove(Vector3.zero, 1);
        }
    }
}
