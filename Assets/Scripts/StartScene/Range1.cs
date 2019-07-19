using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range1 : MonoBehaviour {
    //主逻辑脚本
    private Controller2 Controller2;
    //弓箭手1号的动画状态机
    public Animator Archer1;
    //弓箭手2号的动画状态机
    public Animator Archer2;
    //父物体
    public Tower1 tower1;
    //弓箭游戏物体
    public GameObject arrow;
    //进入我方塔巡逻的范围的敌人
    //[HideInInspector]
    public List<GameObject> Enemys = new List<GameObject>();
    //攻击目标
    public GameObject attackTarget;
    //下次的射击时间
    public float nextTime { set; get; }
    //巡逻切换时间
    public float patrolTime { set; get; }
    //切换射手
    private int switchArcher = 1;
    //确定目标
    public bool isFix { set; get; }

    public bool isStart { set; get; }

    private void Start()
    {
        Controller2 = GameObject.Find("Controller2").GetComponent<Controller2>();
        isFix = false;
        attackTarget = null;
        patrolTime = 0;
        nextTime = 0;
        isStart = false;
    }

    void Update() {
        if (!isStart) return;
        if (!isFix)
        {
            if (Enemys.Count != 0)
            {
                if(Enemys[0]!=null) SelectEnemy();
            }
            else
            {
                if (patrolTime < Time.time)
                {
                    patrolTime = Time.time + 2.0f;
                    int a = Random.Range(0, 2);
                    if (a == 0) Archer1.SetInteger("Switch", 0);
                    else Archer1.SetInteger("Switch", 1);
                    int b = Random.Range(0, 2);
                    if (b == 0) Archer2.SetInteger("Switch", 0);
                    else Archer2.SetInteger("Switch", 1);
                }
            }
        }
        if (attackTarget != null)
        {
            if (!attackTarget.GetComponent<Enemy>().isDead)
            {
                StartCoroutine(AttackEnemy());
            }
            else
            {
                isFix = false;
                foreach (GameObject enemy in Enemys)
                {
                    if (attackTarget == enemy)
                    {
                        Enemys.Remove(enemy);
                        break;
                    }
                }
            }
        }
        else isFix = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            foreach (GameObject enemy in Enemys)
            {
                if (collision.gameObject == enemy)
                {
                    return;
                }
            }
            Enemys.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            foreach (GameObject enemy in Enemys)
            {
                if (collision.gameObject == enemy)
                {
                    Enemys.Remove(enemy);
                    break;
                }
            }
            if (collision.gameObject == attackTarget)
            {
                attackTarget = null;
                isFix = false;
            }
        }
    }
    //选择
    private void SelectEnemy()
    {
        GameObject game = Enemys[Enemys.Count-1];
        Vector3 pos = Controller2.battlePoint.GetComponent<BattlePoint>().points2[Controller2.battlePoint.GetComponent<BattlePoint>().points2.Length - 1];
        if (Enemys.Count != 1)
        {
            for (int i = Enemys.Count - 2; i >= 0; i--)
            {
                if (Enemys[i] != null)
                {
                    if (!isStart) return;
                    if ((game.transform.position - pos).magnitude > (Enemys[i].transform.position - pos).magnitude)
                        game = Enemys[i];
                }
                else
                {
                    return;
                }
            }
        }
        attackTarget = game;
        isFix = true;
    }
    //攻击
    IEnumerator AttackEnemy()
    {
        if (nextTime < Time.time)
        {
            nextTime = Time.time + 0.5f;
            if (switchArcher == 1)
            {
                if (transform.position.y > attackTarget.transform.position.y)
                {
                    Archer1.SetInteger("Switch", 2);
                    yield return new WaitForSeconds(0.1f);
                    Archer1.SetInteger("Switch", 0);
                }
                else
                {
                    Archer1.SetInteger("Switch", 3);
                    yield return new WaitForSeconds(0.1f);
                    Archer1.SetInteger("Switch", 1);
                }
                switchArcher = 2;
                Invoke("AttackDelay1", 0.25f);
            }
            else
            {
                if (transform.position.y > attackTarget.transform.position.y)
                {
                    Archer2.SetInteger("Switch", 2);
                    yield return new WaitForSeconds(0.1f);
                    Archer2.SetInteger("Switch", 0);
                }
                else
                {
                    Archer2.SetInteger("Switch", 3);
                    yield return new WaitForSeconds(0.1f);
                    Archer2.SetInteger("Switch", 1);
                }
                switchArcher = 1;
                Invoke("AttackDelay2", 0.25f);
            }
            int a = Random.Range(0, 2);
            if (a == 0) tower1.foundPoint.GetComponent<AudioSource>().clip = tower1.audioClips[3];
            else tower1.foundPoint.GetComponent<AudioSource>().clip = tower1.audioClips[4];
            tower1.foundPoint.GetComponent<AudioSource>().Play();
        }
    }
    private void AttackDelay1()
    {
        GameObject game = Instantiate(arrow, Archer1.transform.position, Quaternion.identity);
        game.GetComponent<Arrow>().enemy = attackTarget;
        game.GetComponent<Arrow>().attackPower = tower1.attackPower;
    }
    private void AttackDelay2()
    {
        GameObject game = Instantiate(arrow, Archer2.transform.position, Quaternion.identity);
        game.GetComponent<Arrow>().enemy = attackTarget;
        game.GetComponent<Arrow>().attackPower = tower1.attackPower;
    }
}
