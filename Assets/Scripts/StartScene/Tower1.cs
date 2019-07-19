using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tower1 : MonoBehaviour {

    //主菜单的音频播放器
    private AudioSource soundAudioSource;
    //主逻辑脚本
    private Controller1 codeController1;
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
    //弓箭手的攻击力
    public float attackPower { set; get; }

    void Start () {
        soundAudioSource = GameObject.Find("MainMenuCanvas").transform.Find("Sound").GetComponent<AudioSource>();

        codeController1 = GameObject.Find("Controller1").GetComponent<Controller1>();
        //初始攻击力
        attackPower = 2;
    }
	void Update() {
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
        soundAudioSource.clip = codeController1.audios[7];
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
            rectTransform.localScale = new Vector3(1.4f, 1.4f, 1);
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
                attackPower = 15;
                transform.Find("Tower").gameObject.GetComponent<Image>().sprite = sprites[1];
                transform.Find("Archer1").gameObject.GetComponent<Animator>().SetLayerWeight(1, 1);
                transform.Find("Archer2").gameObject.GetComponent<Animator>().SetLayerWeight(1, 1);
            }
            else if (grade == 2)
            {
                grade = 3;
                isGrade = true;
                attackPower = 30;
                transform.Find("Tower").gameObject.GetComponent<Image>().sprite = sprites[2];
                transform.Find("Archer1").gameObject.GetComponent<Animator>().SetLayerWeight(2, 1);
                transform.Find("Archer2").gameObject.GetComponent<Animator>().SetLayerWeight(2, 1);
            }
            foundPoint.GetComponent<AudioSource>().clip = audioClips[SwitchAudio % 3];
            foundPoint.GetComponent<AudioSource>().Play();
            transform.Find("Archer1").gameObject.transform.localPosition += new Vector3(0, 2, 0);
            transform.Find("Archer2").gameObject.transform.localPosition += new Vector3(0, 2, 0);
            range.GetComponent<Range1>().isStart = false;
            SwitchAudio++;
            Invoke("EscalateDelay", 1.5f);
        }
    }
    public void EscalateDelay()
    {
        range.GetComponent<Range1>().isStart = true;
    }
    //-------------------------
    public void OnPointerSell()
    {
        transform.Find("Archer1").gameObject.transform.localPosition = new Vector3(-5.3f, 27.5f, 0);
        transform.Find("Archer2").gameObject.transform.localPosition = new Vector3(7.4f, 27.5f, 0);
        transform.Find("Archer1").gameObject.GetComponent<Animator>().SetLayerWeight(1, 0);
        transform.Find("Archer2").gameObject.GetComponent<Animator>().SetLayerWeight(1, 0);
        transform.Find("Archer1").gameObject.GetComponent<Animator>().SetLayerWeight(2, 0);
        transform.Find("Archer2").gameObject.GetComponent<Animator>().SetLayerWeight(2, 0);
        transform.Find("Archer1").gameObject.SetActive(false);
        transform.Find("Archer2").gameObject.SetActive(false);

        grade = 1;
        isGrade = false;
        attackPower = 2;
        soundAudioSource.clip = codeController1.audios[9];
        soundAudioSource.Play();
        range.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        range.GetComponent<PolygonCollider2D>().enabled = false;
        range.GetComponent<Range1>().isStart = false;
        range.GetComponent<Range1>().isFix = false;
        range.GetComponent<Range1>().Enemys.Clear();
        range.GetComponent<Range1>().attackTarget = null;
        range.GetComponent<Range1>().patrolTime = 0;
        range.GetComponent<Range1>().nextTime = 0;
        escalate.GetComponent<Animator>().SetInteger("Switch", 2);
        transform.Find("Tower").gameObject.GetComponent<Image>().sprite = sprites[0];
        transform.Find("Tower").gameObject.SetActive(false);
        foundPoint.transform.Find("Soil").GetComponent<Button>().interactable = true;
        transform.Find("Tower").gameObject.GetComponent<Button>().interactable = true;
        Invoke("SellDelay", 0.2f);
    }
    private void SellDelay()
    {
        transform.gameObject.SetActive(false);
        range.GetComponent<Range1>().enabled = false;
    }
}
