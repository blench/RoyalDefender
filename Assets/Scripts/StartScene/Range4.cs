using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Range4 : MonoBehaviour {
    //主逻辑脚本
    private Controller2 Controller2;
    //炮弹
    public GameObject bullet;
    //父物体
    public Tower4 tower4;
    //进入我方小兵巡逻的范围的敌人
    [HideInInspector]
    public List<GameObject> enemys = new List<GameObject>();
    //是否攻击
    public bool isAttack;
    //已选择的敌人
    public GameObject enemy;
    //攻击间隔
    public float attackTime { set; get; }

    public bool isStart { set; get; }
    void Start () {
        Controller2 = GameObject.Find("Controller2").GetComponent<Controller2>();
        isAttack = false;
        enemy = null;
        attackTime = 0;
        isStart = false;
    }
	
	
	void Update () {
        if (!isStart) return;
        if (!isAttack)
        {
            if (enemys.Count != 0)
            {
                if(enemys[0]!=null)SelectEnemy();
            }
        }
        if (enemy != null)
        {
            if (!enemy.GetComponent<Enemy>().isDead)
            {
                StartCoroutine(AttackEnemy());
            }
            else
            {
                isAttack = false;
                foreach (GameObject game in enemys)
                {
                    if (game == enemy)
                    {
                        enemys.Remove(game);
                        break;
                    }
                }
            }
        }
        else isAttack = false;
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            foreach (GameObject enemy in enemys)
            {
                if (collision.gameObject == enemy)
                {
                    return;
                }
            }
            enemys.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            foreach (GameObject game in enemys)
            {
                if (collision.gameObject == game)
                {
                    enemys.Remove(game);
                    break;
                }
            }
            if (collision.gameObject == enemy)
            {
                enemy = null;
                isAttack = false;
            }
        }
    }
    //选择
    private void SelectEnemy()
    {
        GameObject game = enemys[enemys.Count-1];
        Vector3 pos = Controller2.battlePoint.GetComponent<BattlePoint>().points2[Controller2.battlePoint.GetComponent<BattlePoint>().points2.Length - 1];
        if (enemys.Count != 1)
        {
            for (int i = enemys.Count - 2; i >= 0; i--)
            {
                if (enemys[i] != null)
                {
                    if (!isStart) return;
                    if ((game.transform.position - pos).magnitude > (enemys[i].transform.position - pos).magnitude)
                        game = enemys[i];
                }
                else
                {
                    return;
                }
            }
        }
        enemy = game;
        isAttack = true;
    }
    //攻击
    IEnumerator AttackEnemy()
    {
        if (attackTime < Time.time)
        {
            attackTime = Time.time + 3.0f;
            tower4.gameObject.transform.Find("Tower").GetComponent<Animator>().SetInteger("Switch", 1);
            yield return new WaitForSeconds(0.7f);
            tower4.foundPoint.GetComponent<AudioSource>().clip = tower4.audioClips[3];
            tower4.foundPoint.GetComponent<AudioSource>().Play();
            GameObject game = Instantiate(bullet, transform.Find("GameObject").transform.position, Quaternion.identity);
            game.GetComponent<Bullet>().enemy = enemy;
            game.GetComponent<Bullet>().attackPower = tower4.attackPower;
            game.GetComponent<Bullet>().grade = tower4.grade;
            if (tower4.grade == 1)
            {
                game.GetComponent<SpriteRenderer>().sprite = game.GetComponent<Bullet>().sprites[0];
            }
            else if(tower4.grade == 2)
            {
                game.GetComponent<SpriteRenderer>().sprite = game.GetComponent<Bullet>().sprites[1];
            }
            else if (tower4.grade == 3)
            {
                game.GetComponent<SpriteRenderer>().sprite = game.GetComponent<Bullet>().sprites[2];
            }
            yield return new WaitForSeconds(0.3f);
            tower4.gameObject.transform.Find("Tower").GetComponent<Animator>().SetInteger("Switch", 2);
        }
    }
}
