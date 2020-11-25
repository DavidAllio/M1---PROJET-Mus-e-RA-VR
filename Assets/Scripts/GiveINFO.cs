using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Video;

public class GiveINFO : MonoBehaviour
{

    public GameObject back;
    public Text txt;
    public GameObject videoBack;
    public VideoPlayer video;
    public GameObject quad;
    private Image quadImg;
    private Texture2D tex;

    public AudioSource Audio;

    // Start is called before the first frame update
    void Start()
    {
        //le fond
        back.SetActive(false);

        //le texte
        txt.text = "";

        //la vidéo
        VideoClip clip = Resources.Load<VideoClip>("siva/manuscritsShiva");
        video.clip = clip;
        video.Stop();
        videoBack.SetActive(false);

        //l'image
        quadImg = quad.GetComponent<Image>();
        quad.SetActive(false);

        tex = Resources.Load<Texture2D>("siva/DN0048");
        quadImg.material.mainTexture = tex;

        //le son
        Audio.Stop();
    }

    void show()
    {
        back.SetActive(true);
    }

    public void PlayVid()
    {
        show();
        videoBack.SetActive(true);
        back.GetComponent<Image>().color = new Color(1,1,1,1);
        video.Play();
    }
    public void PlaySound()
    {
        show();
        txt.text = "Fichier audio";
        Audio.Play();
    }

    public void PlayImg()
    {
        show();
        quad.SetActive(true);
        

    }

    public void showText(string text)
    {
        txt.text = text;
        show();
    }

    public void StopAll()
    {
        video.Stop();
        quad.SetActive(false);
        back.SetActive(false);
        videoBack.SetActive(false);
        back.GetComponent<Image>().color = new Color(1, 1, 1, 0.59f);
        Audio.Stop();
        txt.text = "";
    }
}
