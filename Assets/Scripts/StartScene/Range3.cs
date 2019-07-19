using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range3 : MonoBehaviour{
    //主逻辑脚本
    private Controller2 Controller2;
    //法师
    public Animator mage;
    //父物体
    public Tower3 tower3;
    //法术球
    public GameObject spellBall;
    //进入我方塔巡逻的范围的敌人
    [HideInInspector]
    public List<GameObject> Enemys = new List<GameObject>();
    //攻击目标
    public GameObject attackTarget { set; get; }
    //下次的射击时间
    public float nextTime { set; get; }
    //巡逻切换时间
    public float patrolTime { set; get; }
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
    void Update()
    {
        if (!isStart) return;
        if (!isFix)
        {
            if (Enemys.Count != 0)
            {
                if(Enemys[0]!=null)SelectEnemy();
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
                if (Enemys[i]!=null)
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
            nextTime = Time.time + 1.0f;
            if (transform.position.y > attackTarget.transform.position.y)
            {
                mage.SetInteger("Switch", 2);
                yield return new WaitForSeconds(0.1f);
                mage.SetInteger("Switch", 0);
            }
            else
            {
                mage.SetInteger("Switch", 3);
                yield return new WaitForSeconds(0.1f);
                mage.SetInteger("Switch", 1);
            }
            tower3.foundPoint.GetComponent<AudioSource>().clip = tower3.audioClips[3];
            tower3.foundPoint.GetComponent<AudioSource>().Play();
            Invoke("AttackDelay", 0.35f);
        }
    }
    private void AttackDelay()
    {
        GameObject game = Instantiate(spellBall, mage.transform.Find("GameObject").transform.position, Quaternion.identity);
        game.GetComponent<SpellBall>().enemy = attackTarget;
        game.GetComponent<SpellBall>().attackPower = tower3.attackPower;
    }
}
