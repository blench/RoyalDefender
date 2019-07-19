using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller2 : MonoBehaviour {

    //主逻辑脚本
    public GameObject Controller1;
    //黄金数量
	public int goldNum{ set; get;}
	public Text GoldText;
    //生命值大小
    public int lifeValue { set; get;}
	public Text LifeValueText;
    //敌人1
    public GameObject Enemy1;
    [HideInInspector]
    public List<GameObject> enemys = new List<GameObject>();
    //塔
    public GameObject tower;
    [HideInInspector]
    public List<GameObject> towers = new List<GameObject>();
    //敌人出生点
    private Vector3 pos;
    //战斗点
    public GameObject battlePoint { set; get; }
    //战斗点脚本
    private BattlePoint battlePointCode;
    //下次的时间
    private float nextTime = 0;
    //出生数
    public int number { set; get; }
    
    public int deadNum { set; get; }

    public int isStart { set; get; }

    void Start () {
        deadNum = 100;
        isStart = 1;
        number = 0;
        goldNum = 1000;
        lifeValue = 100;
    }

    void Update() {
        if (isStart==1)
        {
            battlePointCode = battlePoint.GetComponent<BattlePoint>();
            pos = battlePointCode.points2[0];
            NewTower();
            isStart = 2; 
        }
        else if (isStart == 3)
        {
            NewEnemy();
            FindBattle();
        }
        GoldLifeChange();
    }

    private void GoldLifeChange() 
    {
        GoldText.text = goldNum + "";
        LifeValueText.text = lifeValue + "";
    }
    private void NewTower()
    {
        foreach(Vector3 pos in battlePointCode.TowerPoints)
        {
            towers.Add(Instantiate(tower, pos, Quaternion.identity));
        }
    }
    //生成敌人
    private void NewEnemy()
    {
        if (nextTime <= Time.time && number<100)
        {
            nextTime = Time.time + 1.0f;
            GameObject game = Instantiate(Enemy1, pos, Quaternion.identity);
            enemys.Add(game);
            game.name = "Enemy" + number;
            game.GetComponent<Enemy>().initHp = number * 5 + 30;
            game.GetComponent<Enemy>().attackPower = number * 1;
            int a = Random.Range(0, 3);
            if (a == 0) game.GetComponent<Enemy>().points = battlePointCode.points1;
            else if (a == 1) game.GetComponent<Enemy>().points = battlePointCode.points2;
            else if (a == 2) game.GetComponent<Enemy>().points = battlePointCode.points3;
            number++;
        }
    }
    //寻找小兵打架
    private void FindBattle()
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            if (enemys[i] != null)
            {
                if (enemys[i].GetComponent<Enemy>().isDead)
                {
                    enemys.Remove(enemys[i]);
                    goldNum += 100;
                    deadNum--;
                }
            }
            else
            {
                enemys.Remove(enemys[i]);
                deadNum--;
            }
        }
        
        bool b = false;
        for (int i = 0; i < enemys.Count; i++)
        {
            if(enemys[i] != null)
            {
                if (!enemys[i].GetComponent<Enemy>().isDead)
                {
                    if (enemys[i].GetComponent<Enemy>().soldier != null)
                    {
                        Soldier _soldier = enemys[i].GetComponent<Enemy>().soldier.GetComponent<Soldier>();
                        if (_soldier.isAttack)
                        {
                            if(_soldier.selfAnimator.GetInteger("Status")==4 || _soldier.selfAnimator.GetInteger("Status") == 0)
                            {
                                b = true;
                            }
 
                        } 

                        if (_soldier.isDead)
                        {
                            lifeValue -= 1;
                        }   
                    }
                }
            }
        }
        if (b)
        {
            if (!transform.GetComponent<AudioSource>().isPlaying)
                transform.GetComponent<AudioSource>().Play();
        }
        else
        {
            if (transform.GetComponent<AudioSource>().isPlaying)
                transform.GetComponent<AudioSource>().Stop();
        }
    }
}
