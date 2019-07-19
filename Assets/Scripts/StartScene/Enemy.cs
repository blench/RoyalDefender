using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour {
    public AudioClip[] audioClips;
    //路点数组
    public Vector2[] points { set; get; }
    //自身的Animator
    private Animator selfAnimator;
    //Hp游戏物体
    public GameObject hp;
    //实际hp
    public float RealHp { set; get; }
    //初始hp
    public float initHp { set; get; }
    //攻击力
    public float attackPower { set; get; }
    //路点数
    public int index { set; get; }
    //是否死亡
    public bool isDead { set; get; }
    //是否攻击
    public bool isAttack { set; get; }
    //已选择的我方小兵
    public GameObject soldier { set; get; }
    //攻击间隔
    private float attackTime = 0;
    void Start () {
        selfAnimator = transform.GetComponent<Animator>();
        RealHp = initHp;
        index = 0;
        isDead = false;
        isAttack = false;
        soldier = null;
    }

	void Update() {
        if (!isDead)
        {
            if (soldier == null) isAttack = false;
            if (isAttack)
            {
                StartCoroutine(AttackSoldier());
            }
            else
            {
                if (!isDead) Move();
                attackTime = Time.time + 1.0f;
            }
        }
    }

    private void Move()
    {
        Vector2 selfPos = transform.position;
        Vector2 p = points[index] - selfPos;
        if (Mathf.Abs(p.x) > Mathf.Abs(p.y))
        {
            if (p.x > 0) selfAnimator.SetInteger("Switch", 4);
            else selfAnimator.SetInteger("Switch", 3);
        }
        else
        {
            if (p.y < 0) selfAnimator.SetInteger("Switch", 1);
            else selfAnimator.SetInteger("Switch", 2);
        }
        transform.Translate(p.normalized * Time.deltaTime * 0.5f, Space.World);
        if ((new Vector2(transform.position.x, transform.position.y) - points[index]).magnitude < 0.1)
        {
            index++;
            if (index == points.Length)
            {
                Destroy(transform.gameObject);
            }
        }
    }

    private IEnumerator AttackSoldier()
    {
        selfAnimator.SetInteger("Switch", 0);
        if (attackTime < Time.time)
        {
            attackTime = Time.time + 1.0f;
            if ((soldier.transform.position - transform.position).magnitude < 0.4f)
            {
                selfAnimator.SetInteger("Switch", 6);
                yield return new WaitForSeconds(0.3f);
                if (soldier != null) soldier.GetComponent<Soldier>().Injured(attackPower);
                else yield return new WaitForEndOfFrame();
            }
        }
        if(soldier != null)
        {
            if (soldier.GetComponent<Soldier>().isDead)
            {
                soldier = null;
                isAttack = false;
            }
            else
            {
                if (!soldier.GetComponent<Soldier>().isAttack)
                {
                    soldier = null;
                    isAttack = false;
                }
            }
        }
    }
    public void Injured(float hurt)
    {
        if (isDead) return;
        RealHp -= hurt;
        hp.GetComponent<Image>().fillAmount = RealHp / initHp;
        if (RealHp <= 0)
        {
            RealHp = 0;
            isDead = true;
            selfAnimator.SetInteger("Switch", 5);
            transform.GetComponent<CapsuleCollider2D>().enabled = false;
            transform.Find("Hp").gameObject.SetActive(false);
            transform.GetComponent<AudioSource>().clip = audioClips[0];
            transform.GetComponent<AudioSource>().Play();
            Invoke("InjuredDelay", 3.0f);
        }
    }
    private void InjuredDelay()
    {
        Destroy(transform.gameObject);
    }
}
