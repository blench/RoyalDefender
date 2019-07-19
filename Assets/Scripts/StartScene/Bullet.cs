using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    //音效
    public AudioClip[] audioClips;
    //爆炸效果
    public GameObject explosion;
    //图片数组
    public Sprite[] sprites;
    //已选择的敌人
    public GameObject enemy { set; get; }
    //炮弹的攻击力
    public float attackPower { set; get; }
    [HideInInspector]
    public List<GameObject> enemys = new List<GameObject>();
    public float t { set; get; }

    private Vector3 pos1;
    private Vector3 pos2;

    public int grade { set; get; }

    private bool isDead = false;
    void Start () {
        if (enemy == null)
        {
            Destroy(transform.gameObject);
            return;
        }
        pos1 = transform.position;
        pos2 = (enemy.transform.position - transform.position) / 3 + transform.position;

        if (enemy.transform.position.x > pos1.x)
        {
            if (enemy.transform.position.y > pos1.y)
            {
                pos2.x -= 1;
                pos2.y += 3;
            }
            else
            {
                pos2.x += 1;
                pos2.y += 3;
            }
        }
        else
        {
            if (enemy.transform.position.y > pos1.y)
            {
                pos2.y += 2;
            }
            else
            {
                pos2.x -= 1;
                pos2.y += 2;
            }
        }
        t = 0;
    }
    private void FixedUpdate()
    {
        if (transform.GetComponent<CircleCollider2D>().enabled) return;
        if (enemy == null)
        {
            Destroy(transform.gameObject);
            return;
        }
        else
        {
            Vector3 pos = Mathf.Pow((1 - t), 2) * pos1 + 2 * t * (1 - t) * pos2 + Mathf.Pow(t, 2) * enemy.transform.position;
            if (grade == 1||grade == 2)
            {
                transform.Rotate(new Vector3(0, 0, 10), Space.Self);
            }
            else if (grade == 3)
            {
                transform.up = pos - transform.position;
            }
            transform.position = pos;
            if (t <= 1)
            {
                t += Time.fixedDeltaTime;
            }
            if (t>=1)
            {
                transform.GetComponent<CircleCollider2D>().enabled = true;
                transform.GetComponent<AudioSource>().clip = audioClips[0];
                transform.GetComponent<AudioSource>().Play();
                Invoke("Delay", 0.1f);
            }
        }
    }
    private void Delay()
    {
        isDead = true;
        foreach (GameObject enemy in enemys)
        {
            if (enemy != null)
                enemy.GetComponent<Enemy>().Injured(attackPower);
        }
        Destroy(Instantiate(explosion, transform.position+new Vector3(0,0.6f,0), Quaternion.identity), 1.0f);
        Destroy(transform.gameObject, 1.0f);
        transform.GetComponent<SpriteRenderer>().enabled = false;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isDead) return;
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
}
