using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBall : MonoBehaviour {

    public GameObject enemy { set; get; }

    //箭的攻击力
    public float attackPower { set; get; }

    void Update()
    {
        if (enemy == null) Destroy(transform.gameObject);
        else
        {
            Vector3 p = enemy.transform.position - transform.position;
            transform.Translate(p.normalized * Time.deltaTime * 5, Space.World);
            transform.right = -p.normalized;
            transform.Rotate(new Vector3(0, 0, -50), Space.World);
            if ((transform.position - enemy.transform.position).magnitude < 0.1)
            {
                Destroy(transform.gameObject);
                enemy.GetComponent<Enemy>().Injured(attackPower);
            }
        }
        
    }
}
