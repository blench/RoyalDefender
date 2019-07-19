using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tower2 : MonoBehaviour {

    //主菜单的音频播放器
    private AudioSource soundAudioSource;
    //主逻辑脚本
    private Controller1 Controller1;
    private Controller2 Controller2;
    //升级UI
    public GameObject escalate;
    //建立点
    public GameObject foundPoint;
    //攻击范围
    public GameObject range;
    //集结点
    public GameObject assemblyPoint;
    //旗
    public GameObject flag;
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
    //小兵的伤害
    public float attackPower { set; get; }
    //小兵的Hp
    public float hp { set; get; }
    private bool isAssembly = false;
    void Start()
    {
        soundAudioSource = GameObject.Find("MainMenuCanvas").transform.Find("Sound").GetComponent<AudioSource>();

        Controller1 = GameObject.Find("Controller1").GetComponent<Controller1>();
        Controller2 = GameObject.Find("Controller2").GetComponent<Controller2>();
        //初始攻击力
        attackPower = 2;
        hp = 40;
        grade = 1;
    }
    void Update()
    {
        if (Controller2.isStart == 4)
        {
            foreach (GameObject soldier in range.GetComponent<Range2>().soldiers)
                Destroy(soldier);
        }
        if (isAssembly)
        {
            assemblyPoint.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            assemblyPoint.transform.position = new Vector3(assemblyPoint.transform.position.x, assemblyPoint.transform.position.y, 0);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!transform.Find("Tower").gameObject.GetComponent<Button>().interactable)
            {
                if (escalate.GetComponent<Animator>().GetInteger("Switch") == 1)
                {
                    if (!isAssembly) range.GetComponent<Image>().enabled = false;
                    escalate.GetComponent<Animator>().SetInteger("Switch", 2);
                    Invoke("Delay", 0.1f);
                }
            }
            if (isAssembly && assemblyPoint.activeInHierarchy)
            {
                StartCoroutine(Assembly());
            }
        }
    }
    private void Delay()
    {
        transform.Find("Tower").gameObject.GetComponent<Button>().interactable = true;
    }
    //----------------------
    private IEnumerator Assembly()
    {
        Vector2 pos1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //发射射线，记录碰撞信息和碰撞结果
        if (Physics2D.Raycast(pos1, pos1, 1000))
        {
            isAssembly = false;
            range.GetComponent<Image>().enabled = false;
            flag.GetComponent<Image>().enabled = true;
            flag.transform.position = assemblyPoint.transform.position;
            assemblyPoint.SetActive(false);
            range.GetComponent<Range2>().MovePoint(flag.transform.position);
            foundPoint.GetComponent<AudioSource>().clip = audioClips[4];
            foundPoint.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(1.5f);
            flag.GetComponent<Image>().enabled = false;
        }
        else
        {
            Sprite sprite = assemblyPoint.GetComponent<Image>().sprite;
            Color color = assemblyPoint.GetComponent<Image>().color;
            assemblyPoint.GetComponent<Image>().sprite = sprites[3];
            assemblyPoint.GetComponent<Image>().color = Color.white;
            yield return new WaitForSeconds(0.5f);
            assemblyPoint.GetComponent<Image>().sprite = sprite;
            assemblyPoint.GetComponent<Image>().color = color;
        }
    }
    //------------------------
    public void OnPointerTower()
    {
        soundAudioSource.clip = Controller1.audios[7];
        soundAudioSource.Play();
        transform.Find("Tower").gameObject.GetComponent<Button>().interactable = false;
        StartCoroutine(TowerDelay());
    }
    private IEnumerator TowerDelay()
    {
        yield return new WaitForSeconds(0.1f);
        escalate.GetComponent<Animator>().SetInteger("Switch", 1);
        yield return new WaitForSeconds(0.1f);
        range.GetComponent<Image>().enabled = true;
    }
    //-------------------------
    public void OnPointerEnterTowerEscalate()
    {
        isGrade = false;
        if (grade == 1)
        {

        }
        else if (grade == 2)
        {

        }
        else if (grade == 3)
        {

        }
    }
    public void OnPointerExitTowerEscalate()
    {
        if (!isGrade)
        {
            if (grade == 1)
            {

            }
            else if (grade == 2)
            {

            }
            else if (grade == 3)
            {

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
                attackPower = 8;
                hp = 100;
                if (range.GetComponent<Range2>().soldiers.Length != 0)
                {
                    foreach (GameObject soldier in range.GetComponent<Range2>().soldiers)
                    {
                        if(soldier!=null)
                            soldier.transform.Find("Image").GetComponent<Animator>().SetLayerWeight(1, 1);
                    }    
                }
                transform.Find("Tower").gameObject.GetComponent<Animator>().SetLayerWeight(1, 1);
            }
            else if (grade == 2)
            {
                grade = 3;
                isGrade = true;
                attackPower = 14;
                hp = 150;
                transform.Find("Tower").gameObject.GetComponent<Animator>().SetLayerWeight(2, 1);
            }
            foundPoint.GetComponent<AudioSource>().clip = audioClips[SwitchAudio % 4];
            foundPoint.GetComponent<AudioSource>().Play();
            SwitchAudio++;
        }
    }
    public void OnPointerMuster()
    {
        isAssembly = true;
        Invoke("MusterDelay", 0.2f);
    }
    private void MusterDelay()
    {
        assemblyPoint.SetActive(true);
    }
    //-------------------------
    public void OnPointerSell()
    {
        foreach (GameObject soldier in range.GetComponent<Range2>().soldiers)
        {
            Destroy(soldier);
        }
        range.GetComponent<Range2>().soldiers = new GameObject[3];
        range.GetComponent<Range2>().isFirst = 2;
        hp = 40;
        isGrade = false;
        attackPower = 2;
        grade = 1;
        soundAudioSource.clip = Controller1.audios[9];
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
        range.GetComponent<Range2>().enabled = false;
    }
}
