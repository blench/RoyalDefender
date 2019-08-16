using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tower4 : MonoBehaviour {

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
    public int grade { set; get; }
    //升级中
    private bool isGrade = false;
    [HideInInspector]
    public int SwitchAudio = 1;
    //炮弹的伤害
    public float attackPower { set; get; }
    private GoldNum goldNum;
    private Text goldNotEnoughText;
    void Start()
    {
        soundAudioSource = GameObject.Find("MainMenuCanvas").transform.Find("Sound").GetComponent<AudioSource>();

        codeController1 = GameObject.Find("Controller1").GetComponent<Controller1>();
        goldNum = GameObject.Find("MainMenuCanvas/BattleScene/HpGold/Gold").GetComponent<GoldNum>();
        goldNotEnoughText = GameObject.Find("MainMenuCanvas/BattleScene/GoldNumNotEnough").GetComponent<Text>();
        grade = 1;
        attackPower = 10;
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
        if (grade != 3 && goldNum.goldNum >= 700)
        {
            if (grade == 1)
            {
                grade = 2;
                isGrade = true;
                attackPower = 25;
                transform.Find("Tower").gameObject.GetComponent<Animator>().SetLayerWeight(1, 1);
            }
            else if (grade == 2)
            {
                grade = 3;
                isGrade = true;
                attackPower = 40;
                transform.Find("Tower").gameObject.GetComponent<Animator>().SetLayerWeight(2, 1);
            }
            foundPoint.GetComponent<AudioSource>().clip = audioClips[SwitchAudio % 3];
            foundPoint.GetComponent<AudioSource>().Play();
            range.GetComponent<Range4>().isStart = false;
            SwitchAudio++;
            Invoke("EscalateDelay", 1.5f);
            goldNum.goldNum -= 700;
        }  else {
           goldNotEnoughText.enabled = true;
            Invoke("HideGoldEnoughText", 2.0f);
        }
    }

    private void HideGoldEnoughText()
    {
        goldNotEnoughText.enabled = false;
    }
    
    public void EscalateDelay()
    {
        range.GetComponent<Range4>().isStart = true;
    }
    //-------------------------
    public void OnPointerSell()
    {
        range.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        range.GetComponent<PolygonCollider2D>().enabled = false;
        range.GetComponent<Range4>().isAttack = false;
        range.GetComponent<Range4>().enemys.Clear();
        range.GetComponent<Range4>().enemy = null;
        range.GetComponent<Range4>().isStart = false;
        range.GetComponent<Range4>().attackTime = 0;
        isGrade = false;
        grade = 1;
        attackPower = 10;
        soundAudioSource.clip = codeController1.audios[9];
        soundAudioSource.Play();
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
        range.GetComponent<Range4>().enabled = false;
    }
}
