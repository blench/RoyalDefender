using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tower3 : MonoBehaviour {

    //主菜单的音频播放器
    private AudioSource soundAudioSource;
    //主逻辑脚本
    private Controller1 controller1;
    //升级UI
    public GameObject escalate;
    //建立点
    public GameObject foundPoint;
    //攻击范围
    public GameObject range;
    //图片数组
    public Sprite[] sprites;
    //音效数组
    public AudioClip[] audioClips;
    //等级
    private int grade = 1;
    //升级中
    private bool isGrade = false;
    [HideInInspector]
    public int SwitchAudio = 1;
    //法师塔的伤害
    public float attackPower { set; get; }
    void Start()
    {
        soundAudioSource = GameObject.Find("MainMenuCanvas").transform.Find("Sound").GetComponent<AudioSource>();

        controller1 = GameObject.Find("Controller1").GetComponent<Controller1>();
        //初始伤害
        attackPower = 15;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!transform.Find("Tower").gameObject.GetComponent<Button>().interactable)
            {
                if (escalate.GetComponent<Animator>().GetInteger("Switch") == 1)
                {
                    range.GetComponent<Image>().enabled = false;
                    escalate.GetComponent<Animator>().SetInteger("Switch", 2);
                    Invoke("Delay", 0.1f);
                }
            }
        }
    }
    private void Delay()
    {
        transform.Find("Tower").gameObject.GetComponent<Button>().interactable = true;
    }
    //------------------------
    public void OnPointerTower()
    {
        soundAudioSource.clip = controller1.audios[7];
        soundAudioSource.Play();
        transform.Find("Tower").gameObject.GetComponent<Button>().interactable = false;
        Invoke("TowerDelay", 0.1f);
    }
    private void TowerDelay()
    {
        range.GetComponent<Image>().enabled = true;
        escalate.GetComponent<Animator>().SetInteger("Switch", 1);
    }
    //-------------------------
    public void OnPointerEnterTowerEscalate()
    {
        isGrade = false;
        RectTransform rectTransform = range.GetComponent<RectTransform>();
        if (grade == 1)
        {
            rectTransform.localScale = new Vector3(1.1f, 1.1f, 1);
        }
        else if (grade == 2)
        {
            rectTransform.localScale = new Vector3(1.2f, 1.2f, 1);
        }
        else if (grade == 3)
        {
            rectTransform.localScale = new Vector3(1.3f, 1.3f, 1);
        }
    }
    public void OnPointerExitTowerEscalate()
    {
        if (!isGrade)
        {
            RectTransform rectTransform = range.GetComponent<RectTransform>();
            if (grade == 1)
            {
                rectTransform.localScale = new Vector3(1, 1, 1);
            }
            else if (grade == 2)
            {
                rectTransform.localScale = new Vector3(1.1f, 1.1f, 1);
            }
            else if (grade == 3)
            {
                rectTransform.localScale = new Vector3(1.2f, 1.2f, 1);
            }
        }
    }
    //升级
    public void OnPointerTowerEscalate()
    {
        if (grade != 3)
        {
            if (grade == 1)
            {
                grade = 2;
                isGrade = true;
                attackPower = 30;
                transform.Find("Tower").gameObject.GetComponent<Image>().sprite = sprites[1];
                transform.Find("Mage").gameObject.transform.localPosition += new Vector3(0, 0.5f, 0);
            }
            else if (grade == 2)
            {
                grade = 3;
                isGrade = true;
                attackPower = 50;
                transform.Find("Tower").gameObject.GetComponent<Image>().sprite = sprites[2];
                transform.Find("Mage").gameObject.transform.localPosition += new Vector3(0, 3, 0);
            }
            foundPoint.GetComponent<AudioSource>().clip = audioClips[SwitchAudio % 3];
            foundPoint.GetComponent<AudioSource>().Play();
            range.GetComponent<Range3>().isStart = false;
            SwitchAudio++;
            Invoke("EscalateDelay", 1.5f);
        }
    }
    public void EscalateDelay()
    {
        range.GetComponent<Range3>().isStart = true;
    }
    //-------------------------
    public void OnPointerSell()
    {
        transform.Find("Mage").gameObject.transform.localPosition = new Vector3(0.5f, 26.3f, 0);
        transform.Find("Mage").gameObject.SetActive(false);
        isGrade = false;
        grade = 1;
        attackPower = 15;
        soundAudioSource.clip = controller1.audios[9];
        soundAudioSource.Play();
        range.GetComponent<PolygonCollider2D>().enabled = false;
        range.GetComponent<Range3>().isStart = false;
        range.GetComponent<Range3>().isFix = false;
        range.GetComponent<Range3>().Enemys.Clear();
        range.GetComponent<Range3>().attackTarget = null;
        range.GetComponent<Range3>().patrolTime = 0;
        range.GetComponent<Range3>().nextTime = 0;
        escalate.GetComponent<Animator>().SetInteger("Switch", 2);
        range.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        transform.Find("Tower").gameObject.GetComponent<Image>().sprite = sprites[0];
        transform.Find("Tower").gameObject.SetActive(false);
        foundPoint.transform.Find("Soil").GetComponent<Button>().interactable = true;
        transform.Find("Tower").gameObject.GetComponent<Button>().interactable = true;
        Invoke("SellDelay", 0.2f);
    }
    private void SellDelay()
    {
        transform.gameObject.SetActive(false);
        range.GetComponent<Range3>().enabled = false;
    }
}
