using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Controller1 : MonoBehaviour {
    //Play UI的游戏对象
    public GameObject playCanvas;
    //MainMenuCanvas UI的游戏对象
    public GameObject mainMenuCanvas;
    //MainMenu游戏物体
    public GameObject mainMenu;
    //Cutover游戏物体
    public GameObject cutover;
    //Background游戏物体
    public GameObject background;
    //WorldMap游戏物体
    public GameObject worldMap;
    //TipBoard游戏物体
    public GameObject tipBoard;
    //BattleScene游戏物体
    public GameObject battleScene;
    //SitePanle游戏物体
    public GameObject sitePanle;
    public GameObject pause;
    public GameObject site;
    public GameObject hpGold;
    public GameObject startFighting;
    //Victory游戏物体
    public GameObject victory;
    //Controller2游戏物体
    public Controller2 controller2;
    //游戏音效数组
    public AudioClip[] audios;
    //游戏图片数组
    public Sprite[] sprites;
    //挂上此代码的游戏物体的音频播放器
    private AudioSource selfAudioSource;
    //主菜单的音频播放器
    public AudioSource soundAudioSource;
    //音频控制按钮1
    public GameObject audio1;
    //音频控制值1，bool
    private bool isAudio1 = true;
    //音频控制按钮2
    public GameObject audio2;
    //音频控制值2，bool
    private bool isAudio2 = true;

    public GameObject battlePoint;
    public void Start()
    {
        selfAudioSource = transform.GetComponent<AudioSource>();
        controller2.gameObject.GetComponent<AudioSource>().Stop();
    }

    private void Update()
    {
        if (mainMenu.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mainMenu.GetComponent<Animator>().SetInteger("IntClick", 3);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (mainMenu.activeInHierarchy)
            {
                if (mainMenu.GetComponent<Animator>().GetInteger("IntClick") == 1)
                {
                    mainMenu.GetComponent<Animator>().SetInteger("IntClick", 2);
                }
            }
        }
        if (selfAudioSource.volume == 0)
        {
            isAudio2 = false;
            audio2.GetComponent<Image>().color = Color.gray;
            sitePanle.transform.Find("Music").Find("Toggle").GetComponent<Toggle>().isOn = false;
        }
        else
        {
            isAudio2 = true;
            audio2.GetComponent<Image>().color = Color.white;
            sitePanle.transform.Find("Music").Find("Toggle").GetComponent<Toggle>().isOn = true;
        }
        AudioSource[] audioSources = GameObject.FindObjectsOfType<AudioSource>();
        if (soundAudioSource.volume == 0)
        {
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource.gameObject.name != "Controller1" && audioSource.gameObject.name != "Sound")
                {
                    audioSource.volume = 0;
                }   
            }
            isAudio1 = false;
            audio1.GetComponent<Image>().color = Color.gray;
            sitePanle.transform.Find("SoundEffect").Find("Toggle").GetComponent<Toggle>().isOn = false;
        }
        else
        {
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource.gameObject.name != "Controller1" && audioSource.gameObject.name != "Sound") 
                {
                    audioSource.volume = 1;
                }
                if(audioSource.gameObject.name.Substring(0,5)== "Arrow")
                {
                    audioSource.volume = 0.1f;
                }
            }
            isAudio1 = true;
            audio1.GetComponent<Image>().color = Color.white;
            sitePanle.transform.Find("SoundEffect").Find("Toggle").GetComponent<Toggle>().isOn = true;
        }
        if (controller2.deadNum == 0 && controller2.isStart==3)
        {
            controller2.isStart = 2;
            StartCoroutine(Delay());
        }
    }
    private IEnumerator Delay()
    {
        pause.GetComponent<Button>().interactable = false;
        pause.GetComponent<EventTrigger>().enabled = false;
        site.GetComponent<Button>().interactable = false;
        site.GetComponent<EventTrigger>().enabled = false;
        yield return new WaitForSeconds(1.0f);
        selfAudioSource.Stop();
        selfAudioSource.loop = false;
        selfAudioSource.clip = audios[11];
        selfAudioSource.Play();
        victory.SetActive(true);
    }

    //第一个界面上那个Play按钮的方法
    public void OnPointerPlay()
    {
        playCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        selfAudioSource.clip = audios[2];
        selfAudioSource.Play();
    }
    //当指针移入UI控件上时
    public void OnPointerEnter()
    {
        soundAudioSource.clip = audios[0];
        soundAudioSource.Play();
    }
    //当指针在UI控件上按下时
    public void OnPointerDown()
    {
        soundAudioSource.clip = audios[1];
        soundAudioSource.Play();
    }
    //---------------------
    //下面2个是控制音频的方法
    //---------------------
    public void OnPointerAudio1()
    {
        //音频控制，关
        if (isAudio1)
        {
            audio1.GetComponent<Image>().color = Color.gray;
            soundAudioSource.volume = 0;
            isAudio1 = false;
        }
        //开
        else
        {
            audio1.GetComponent<Image>().color = Color.white;
            soundAudioSource.volume = 1;
            isAudio1 = true;
        }
    }
    public void OnPointerAudio2()
    {
        //音频控制，关
        if (isAudio2)
        {
            audio2.GetComponent<Image>().color = Color.gray;
            selfAudioSource.volume = 0;
            isAudio2 = false;
        }
        //开
        else
        {
            audio2.GetComponent<Image>().color = Color.white;
            selfAudioSource.volume = 1;
            isAudio2 = true;
        }
    }
    //-----------------------------
    //开始游戏
    //-----------------------------
    public void OnPointerStart()
    {
        Invoke("StartDelay", 0.2f);
    }
    private void StartDelay()
    {
        mainMenu.GetComponent<Animator>().SetInteger("IntClick", 1);
    }
    //-----------------------
    //关于游戏
    //-------------------------
    public void OnPointerPertain()
    {

    }
    //------------------------
    //下面3个方法是创建新游戏或者继续游戏
    //--------------------------
    public void OnPointerGame1()
    {
        soundAudioSource.clip = audios[4];
        soundAudioSource.Play();
        selfAudioSource.Stop();
        mainMenu.SetActive(false);
        cutover.SetActive(true);
        Invoke("Game123Delay", 4.0f);
        background.GetComponent<SpriteRenderer>().sprite = sprites[1];
    }
    public void OnPointerGame2()
    {
        soundAudioSource.clip = audios[4];
        soundAudioSource.Play();
        selfAudioSource.Stop();
        mainMenu.SetActive(false);
        cutover.SetActive(true);
        Invoke("Game123Delay", 4.0f);
        background.GetComponent<SpriteRenderer>().sprite = sprites[1];
    }
    public void OnPointerGame3()
    {
        soundAudioSource.clip = audios[4];
        soundAudioSource.Play();
        selfAudioSource.Stop();
        mainMenu.SetActive(false);
        cutover.SetActive(true);
        Invoke("Game123Delay", 4.0f);
        background.GetComponent<SpriteRenderer>().sprite = sprites[1];
    }
    private void Game123Delay()
    {
        cutover.SetActive(false);
        worldMap.SetActive(true);
        selfAudioSource.clip = audios[3];
        selfAudioSource.Play();
        soundAudioSource.clip = audios[5];
        soundAudioSource.Play();
    }
    //-------------------------
    //退回主界面的按钮的方法
    //--------------------------
    public void OnPointQuit()
    {
        soundAudioSource.clip = audios[4];
        soundAudioSource.Play();
        selfAudioSource.Stop();
        worldMap.SetActive(false);
        cutover.SetActive(true);
        Invoke("QuitDelay", 4.0f);
        background.GetComponent<SpriteRenderer>().sprite = sprites[0];
    }
    private void QuitDelay()
    {
        cutover.SetActive(false);
        mainMenu.SetActive(true);
        selfAudioSource.clip = audios[2];
        selfAudioSource.Play();
    }
    //---------------------
    //关闭世界地图上面板的方法
    //-----------------------
    public void OnPointerClose()
    {
        tipBoard.GetComponent<Animator>().SetBool("Switch", true);
        Invoke("CloseDelay", 1.0f);

        foreach (Transform game in worldMap.transform)
        {
            if (game.GetComponent<Button>() != null)
            {
                game.GetComponent<EventTrigger>().enabled = true;
                game.GetComponent<Button>().interactable = true;
            }
        }
    }
    private void CloseDelay()
    {
        tipBoard.SetActive(false);
    }
    //------------------------
    //战斗点的方法
    //-------------------------
    public void OnPointerBattlePoint()
    {
        battlePoint = GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject;
        string str = battlePoint.name.Substring(11, 1);
        if (str == "1")
        {
            tipBoard.transform.Find("Map").GetComponent<Image>().sprite = sprites[2];
            tipBoard.transform.Find("Title").transform.Find("Image").transform.Find("Text").GetComponent<Text>().text = "南部港口";
        }
        else if(str == "2")
        {
            tipBoard.transform.Find("Map").GetComponent<Image>().sprite = sprites[3];
            tipBoard.transform.Find("Title").transform.Find("Image").transform.Find("Text").GetComponent<Text>().text = "农场";
        }
        tipBoard.SetActive(true);
        tipBoard.GetComponent<Animator>().SetBool("Switch", false);
        foreach (Transform game in worldMap.transform)
        {
            if (game.GetComponent<Button>() != null)
            {
                game.GetComponent<EventTrigger>().enabled = false;
                game.GetComponent<Button>().interactable = false;
            }
        }
    }
    //---------------------
    //战斗吧按钮的方法
    //-------------------------
    public void OnPointerBattle()
    {
        soundAudioSource.clip = audios[4];
        soundAudioSource.Play();
        selfAudioSource.Stop();
        controller2.battlePoint = battlePoint;
        cutover.SetActive(true);
        tipBoard.SetActive(false);
        worldMap.SetActive(false);
        foreach(Transform game in soundAudioSource.transform)
        {
            game.gameObject.SetActive(false);
        }
        foreach (Transform game in worldMap.transform)
        {
            if (game.GetComponent<Button>() != null)
            {
                game.GetComponent<EventTrigger>().enabled = true;
                game.GetComponent<Button>().interactable = true;
            }
        }
        controller2.GetComponent<Controller2>().enabled = true;
        controller2.GetComponent<Controller2>().isStart = 1;
        Invoke("BattleDelay", 4.0f);
        background.GetComponent<SpriteRenderer>().sprite = tipBoard.transform.Find("Map").GetComponent<Image>().sprite;
    }
    private void BattleDelay()
    {
        cutover.SetActive(false);
        battleScene.SetActive(true);
        selfAudioSource.clip = audios[6];
        selfAudioSource.Play();
    }
    //----------------------------------
    //游戏战斗时的设置面板的方法
    //------------------------------
    public void OnPointerSite()
    {
        sitePanle.SetActive(true);
        pause.GetComponent<Button>().interactable = false;
        pause.GetComponent<EventTrigger>().enabled = false;
        site.GetComponent<Button>().interactable = false;
        site.GetComponent<EventTrigger>().enabled = false;
    }
    //上面设置面板上的调节音效toggle的方法
    public void OnPointerSoundEffect()
    {
        bool isOn = sitePanle.transform.Find("SoundEffect").Find("Toggle").GetComponent<Toggle>().isOn;
        if (isOn) soundAudioSource.volume = 1;
        else soundAudioSource.volume = 0;
    }
    //上面设置面板上的调节音乐toggle的方法
    public void OnPointerMusic()
    {
        bool isOn = sitePanle.transform.Find("Music").Find("Toggle").GetComponent<Toggle>().isOn;
        if (isOn) selfAudioSource.volume = 1;
        else selfAudioSource.volume = 0;
    }
    //上面设置面板上的继续按钮的方法
    public void OnPointerContinue()
    {
        sitePanle.GetComponent<Animator>().SetBool("Switch", true);
        Invoke("ContinueDelay", 0.5f);
        pause.GetComponent<Button>().interactable = true;
        pause.GetComponent<EventTrigger>().enabled = true;
        site.GetComponent<Button>().interactable = true;
        site.GetComponent<EventTrigger>().enabled = true;
    }
    public void ContinueDelay()
    {
        sitePanle.SetActive(false);
    }
    //------------------------------
    //上面设置面板上的退出按钮的方法
    public void OnPointerBattleSceneQuit()
    {
        Controller2 code = controller2.GetComponent<Controller2>();
        code.gameObject.GetComponent<AudioSource>().Stop();
        code.enabled = false;
        code.isStart = 4;
        code.number = 0;
        code.deadNum = 100;
        foreach (GameObject enemy in code.enemys)
            Destroy(enemy);
        controller2.GetComponent<Controller2>().enemys = new List<GameObject>();
        foreach (GameObject tower in code.towers)
            Destroy(tower);
        controller2.GetComponent<Controller2>().towers = new List<GameObject>();

        selfAudioSource.Stop();
        cutover.SetActive(true);
        soundAudioSource.clip = audios[4];
        soundAudioSource.Play();
        sitePanle.SetActive(false);
        battleScene.SetActive(false);
        startFighting.SetActive(true);
        pause.GetComponent<Button>().interactable = true;
        pause.GetComponent<EventTrigger>().enabled = true;
        site.GetComponent<Button>().interactable = true;
        site.GetComponent<EventTrigger>().enabled = true;
        Invoke("BattleSceneQuitDelay", 4.0f);
        background.GetComponent<SpriteRenderer>().sprite = sprites[1];
    }
    public void BattleSceneQuitDelay()
    {
        cutover.SetActive(false);
        worldMap.SetActive(true);
        selfAudioSource.clip = audios[3];
        selfAudioSource.Play();
        soundAudioSource.clip = audios[5];
        soundAudioSource.Play();
        foreach (Transform game in soundAudioSource.transform)
            game.gameObject.SetActive(true);
    }
    //-------------------------------
    //上面设置面板上的重来按钮的方法
    public void OnPointerAgain()
    {
        sitePanle.GetComponent<Animator>().SetBool("Switch", true);
        Invoke("ContinueDelay", 0.5f);
        pause.GetComponent<Button>().interactable = true;
        pause.GetComponent<EventTrigger>().enabled = true;
        site.GetComponent<Button>().interactable = true;
        site.GetComponent<EventTrigger>().enabled = true;
    }
    public void AgainDelay()
    {
        sitePanle.SetActive(false);
    }
    //-------------------------
    public void OnPointerStartFighting()
    {
        controller2.GetComponent<Controller2>().isStart = 3;
        startFighting.SetActive(false);
        selfAudioSource.clip = audios[10];
        selfAudioSource.Play();
    }
    //胜利或输了之后退出
    public void OnPointerVictoryQuit()
    {
        Controller2 code = controller2.GetComponent<Controller2>();
        code.gameObject.GetComponent<AudioSource>().Stop();
        code.enabled = false;
        code.isStart = 4;
        code.number = 0;
        code.deadNum = 100;
        controller2.GetComponent<Controller2>().enemys = new List<GameObject>();
        foreach (GameObject tower in code.towers)
            Destroy(tower);
        controller2.GetComponent<Controller2>().towers = new List<GameObject>();

        selfAudioSource.loop = true;
        battleScene.SetActive(false);
        pause.GetComponent<Button>().interactable = true;
        pause.GetComponent<EventTrigger>().enabled = true;
        site.GetComponent<Button>().interactable = true;
        site.GetComponent<EventTrigger>().enabled = true;
        victory.SetActive(false);
        selfAudioSource.Stop();
        cutover.SetActive(true);
        soundAudioSource.clip = audios[4];
        soundAudioSource.Play();
        Invoke("BattleSceneQuitDelay", 4.0f);
        background.GetComponent<SpriteRenderer>().sprite = sprites[1];
    }
    public void OnPointerQuitFix()
    {
        Application.Quit();
    }
    public void OnPointerQuitCancel()
    {
        mainMenu.GetComponent<Animator>().SetInteger("IntClick", 4);
    }

}
