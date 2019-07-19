using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class FoundPoint : MonoBehaviour {

    //主菜单的音频播放器
    private AudioSource soundAudioSource;
    //主逻辑脚本
    private Controller1 Controller1;
    //选择建塔UI
    public GameObject tower;
    //四种塔的游戏物体
    public GameObject tower1;
    public GameObject tower2;
    public GameObject tower3;
    public GameObject tower4;
    //进度条
    public GameObject progressBar;
    //是否建立建筑
    private bool isBuild = false;
    public Sprite[] sprites;
    //自身的音频播放器
    private AudioSource selfAudioSource;
    public GameObject range1;
    public GameObject range2;
    public GameObject range3;
    public GameObject range4;

    public GameObject image1;
    public GameObject image2;
    public GameObject image3;
    public GameObject image4;

    public GameObject towerImage1;
    public GameObject towerImage2;
    public GameObject towerImage3;
    public GameObject towerImage4;
    public EventTrigger[] eventTriggers;
    void Start () {
        selfAudioSource = GetComponent<AudioSource>();

        soundAudioSource = GameObject.Find("Sound").GetComponent<AudioSource>();

        Controller1 = GameObject.Find("Controller1").GetComponent<Controller1>();

        progressBar.transform.Find("Image").GetComponent<Image>().fillAmount = 0;

        foreach (EventTrigger trigger in eventTriggers)
            trigger.enabled = false;
    }
	
	void Update() {
        if (isBuild)
        {
            progressBar.transform.Find("Image").GetComponent<Image>().fillAmount += 0.02f;
            if (progressBar.transform.Find("Image").GetComponent<Image>().fillAmount == 1)
            {
                StartCoroutine(FireDelay());
            }
;       }
        if (Input.GetMouseButtonDown(0))
        {
            if (tower.transform.Find("Image").gameObject.activeInHierarchy)
            {
                if (tower.gameObject.GetComponent<Animator>().GetInteger("Switch") == 1)
                {
                    tower.gameObject.GetComponent<Animator>().SetInteger("Switch", 2);
                    transform.Find("Soil").GetComponent<Button>().interactable = true;
                    tower.transform.Find("Image").gameObject.SetActive(false);
                    foreach (EventTrigger trigger in eventTriggers)
                        trigger.enabled = false;
                    if (!isBuild)
                    {
                        tower1.SetActive(false);
                        towerImage1.SetActive(false);
                        tower2.SetActive(false);
                        towerImage2.SetActive(false);
                        tower3.SetActive(false);
                        towerImage3.SetActive(false);
                        tower4.SetActive(false);
                        towerImage4.SetActive(false);
                    }
                }
            }
        }
    }
    IEnumerator FireDelay()
    {
        isBuild = false;
        progressBar.SetActive(false);
        transform.Find("Soil").GetComponent<Button>().interactable = false;
        transform.Find("Soil").GetComponent<EventTrigger>().enabled = false;
        progressBar.transform.Find("Image").GetComponent<Image>().fillAmount = 0;
        if (tower1.activeInHierarchy)
        {
            Tower1 code = tower1.GetComponent<Tower1>();
            image1.SetActive(false);
            towerImage1.SetActive(true);
            range1.GetComponent<PolygonCollider2D>().enabled = true;
            range1.GetComponent<Range1>().enabled = true;
            selfAudioSource.clip = code.audioClips[code.SwitchAudio % 3];
            selfAudioSource.Play();
            tower1.transform.Find("Archer1").gameObject.SetActive(true);
            tower1.transform.Find("Archer2").gameObject.SetActive(true);
            code.SwitchAudio++;
            yield return new WaitForSeconds(1.5f);
            range1.GetComponent<Range1>().isStart = true;
        }
        else if (tower2.activeInHierarchy)
        {
            Tower2 code = tower2.GetComponent<Tower2>();
            image2.SetActive(false);
            towerImage2.SetActive(true);
            range2.GetComponent<Range2>().isFirst = 1;
            range2.GetComponent<Range2>().enabled = true;
            selfAudioSource.clip = code.audioClips[code.SwitchAudio % 4];
            selfAudioSource.Play();
            code.SwitchAudio++;
        }
        else if (tower3.activeInHierarchy)
        {
            Tower3 code = tower3.GetComponent<Tower3>();
            image3.SetActive(false);
            towerImage3.SetActive(true);
            range3.GetComponent<PolygonCollider2D>().enabled = true;
            range3.GetComponent<Range3>().enabled = true;
            selfAudioSource.clip = code.audioClips[code.SwitchAudio % 3];
            selfAudioSource.Play();
            tower3.transform.Find("Mage").gameObject.SetActive(true);
            code.SwitchAudio++;
            yield return new WaitForSeconds(1.5f);
            range3.GetComponent<Range3>().isStart = true;
        }
        else if (tower4.activeInHierarchy)
        {
            Tower4 code = tower4.GetComponent<Tower4>();
            image4.SetActive(false);
            towerImage4.SetActive(true);
            range4.GetComponent<PolygonCollider2D>().enabled = true;
            range4.GetComponent<Range4>().enabled = true;
            selfAudioSource.clip = code.audioClips[code.SwitchAudio % 3];
            selfAudioSource.Play();
            code.SwitchAudio++;
            yield return new WaitForSeconds(1.5f);
            range4.GetComponent<Range4>().isStart = true;
        }
    }

    //当指针移入UI控件上时
    public void OnPointerEnter()
    {
        soundAudioSource.clip = Controller1.audios[0];
        soundAudioSource.Play();
    }
    //当指针在UI控件上按下时
    public void OnPointerDown()
    {
        soundAudioSource.clip = Controller1.audios[1];
        soundAudioSource.Play();
    }
    //------------------------------
    public void OnPointerSoil()
    {
        soundAudioSource.clip = Controller1.audios[7];
        soundAudioSource.Play();
        transform.Find("Soil").GetComponent<Button>().interactable = false;
        StartCoroutine(SoilDelay());
    }
    private IEnumerator SoilDelay()
    {
        yield return new WaitForSeconds(0.1f);
        tower.gameObject.GetComponent<Animator>().SetInteger("Switch", 1);
        yield return new WaitForSeconds(0.1f);
        foreach (EventTrigger trigger in eventTriggers)
            trigger.enabled = true;
    }
    //----------------------------------
    public void OnPointerEnterTower1()
    {
        towerImage1.SetActive(true);
        tower1.SetActive(true);
    }
    public void OnPointerExitTower1()
    {
        if (!isBuild)
        {
            tower1.SetActive(false);
            towerImage1.SetActive(false);
        }
    }
    //---------------------------------
    public void OnPointerEnterTower2()
    {
        towerImage2.SetActive(true);
        tower2.SetActive(true);
    }
    public void OnPointerExitTower2()
    {
        if (!isBuild)
        {
            tower2.SetActive(false);
            towerImage2.SetActive(false);
        }
    }
    //-----------------------------------
    public void OnPointerEnterTower3()
    {
        towerImage3.SetActive(true);
        tower3.SetActive(true);
    }
    public void OnPointerExitTower3()
    {
        if (!isBuild)
        {
            tower3.SetActive(false);
            towerImage3.SetActive(false);
        }
    }
    //--------------------------------
    public void OnPointerEnterTower4()
    {
        towerImage4.SetActive(true);
        tower4.SetActive(true);
    }
    public void OnPointerExitTower4()
    {
        if (!isBuild)
        {
            tower4.SetActive(false);
            towerImage4.SetActive(false);
        }
    }
    //-----------------------------
    public void OnPointerTower1()
    {
        isBuild = true;
        range1.GetComponent<Image>().enabled = false;
        towerImage1.SetActive(false);
        image1.SetActive(true);
        progressBar.SetActive(true);
        selfAudioSource.clip = Controller1.audios[8];
        selfAudioSource.Play();
    }
    public void OnPointerTower2()
    {
        isBuild = true;
        range2.GetComponent<Image>().enabled = false;
        towerImage2.SetActive(false);
        image2.SetActive(true);
        progressBar.SetActive(true);
        selfAudioSource.clip = Controller1.audios[8];
        selfAudioSource.Play();
    }
    public void OnPointerTower3()
    {
        isBuild = true;
        range3.GetComponent<Image>().enabled = false;
        towerImage3.SetActive(false);
        image3.SetActive(true);
        progressBar.SetActive(true);
        selfAudioSource.clip = Controller1.audios[8];
        selfAudioSource.Play();
    }
    public void OnPointerTower4()
    {
        isBuild = true;
        range4.GetComponent<Image>().enabled = false;
        towerImage4.SetActive(false);
        image4.SetActive(true);
        progressBar.SetActive(true);
        selfAudioSource.clip = Controller1.audios[8];
        selfAudioSource.Play();
    }
}
