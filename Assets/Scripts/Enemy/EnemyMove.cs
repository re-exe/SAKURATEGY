using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EnemyMove : MonoBehaviour{

    [SerializeField]
    [Header("移動速度")]
    private float moveSpeed = 0f;

    [SerializeField]
    [Header("攻撃力")]
    private float attackPower = 0f;

    [Tooltip("Rigidbody")]
    private Rigidbody2D m_rigidbody = null;

    private void Awake() {
        m_rigidbody = GetComponent<Rigidbody2D>();

        moveSpeed = moveSpeed / 150f;

        this.FixedUpdateAsObservable()
            .Where(_=> m_rigidbody)
            .Subscribe(_=>{
                m_rigidbody.AddForce(this.transform.right * moveSpeed);

                if(this.transform.position.x > 20f){
                    Destroy(this.gameObject);
                }
            }).AddTo(this);
    }
}
