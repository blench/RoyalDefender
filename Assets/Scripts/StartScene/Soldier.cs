using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Soldier : MonoBehaviour {

    public Vector3 point { set; get; }
    //小兵的攻击力
    public float attackPower { set; get; }
    //自身的Animator
    public Animator selfAnimator;
    //Hp游戏物体
    public Image hp;
    //实际hp
    public float RealHp { set; get; }
    //初始hp
    public float initHp { set; get; }
    //是否死亡
    public bool isDead { set; get; }
    //是否攻击
    public bool isAttack { set; get; }
    //已选择的敌人
    public GameObject enemy { set; get; }
    //巡逻切换时间
    private float patrolTime = 0;
    //攻击间隔
    private float attackTime = 0;

    public int status { set; get; }
    void Start () {
        isDead = false;
        RealHp = initHp;
        isAttack = false;
        enemy = null;
        status = 1;
    }
	void Update () {
        if (!isDead)
        {
            if (enemy != null)
            {
                if (enemy.GetComponent<Enemy>().isDead)
                {
                    isAttack = false;
                    enemy = null;
                }
            }
            else
            {
                isAttack = false;
            }
            if (isAttack)
            {
                if (enemy == null) return;
                if (enemy.GetComponent<Enemy>().soldier == null)
                {
                    enemy.GetComponent<Enemy>().soldier = transform.gameObject;
                }
                if (attackTime < Time.time) StartCoroutine(Move(enemy.transform.position + new Vector3(0.35f, 0, 0)));
            }
            else
            {
                if(attackTime < Time.time) StartCoroutine(Move(point));
            }
        }
    }
    private IEnumerator Move(Vector3 pos)
    {
        Vector3 p = pos - transform.position;
        if (p.magnitude > 0.02f)
        {
            if (transform.position.x < pos.x) selfAnimator.SetInteger("Status", 3);
            else selfAnimator.SetInteger("Status", 2);
            transform.Translate(p.normalized * Time.deltaTime * 1, Space.World);
        }
        else
        {
            if (selfAnimator.GetInteger("Status") == 2)
            {
                status = 1;
                selfAnimator.SetInteger("Status", 0);
            }
            else if (selfAnimator.GetInteger("Status") == 3)
            {
                status = 1;
                selfAnimator.SetInteger("Status", 1);
                yield return new WaitForSeconds(0.01f);
                selfAnimator.SetInteger("Status", 0);
            }
            else
            {
                if (isAttack)
                {
                    if (attackTime < Time.time)
                    {
                        status = 2;
                        attackTime = Time.time + 1.2f;
                        selfAnimator.SetInteger("Status", 4);
                        yield return new WaitForSeconds(0.2f);
                        selfAnimator.SetInteger("Status", 0);
                        if (enemy != null) enemy.GetComponent<Enemy>().Injured(attackPower);
                    }
                }
                else
                {
                    if (patrolTime < Time.time)
                    {
                        patrolTime = Time.time + 2.0f;
                        int a = Random.Range(0, 2);
                        if (a == 0) selfAnimator.SetInteger("Status", 0);
                        else selfAnimator.SetInteger("Status", 1);
                    }
                }
            }
        }
    }
    //----------------------
    public void Injured(float hurt)
    {
        RealHp -= hurt;
        hp.GetComponent<Image>().fillAmount = RealHp / initHp;
        if (RealHp <= 0)
        {
            RealHp = 0;
            selfAnimator.SetInteger("Status", 5);
            transform.Find("Hp").gameObject.SetActive(false);
            isDead = true;
        }
    }
}
