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

    [SerializeField]
    [Header("攻撃スパン")]
    private float attackSpan = 0f;

    [SerializeField]
    [Tooltip("HP")]
    private float hp = 0f;

    [Tooltip("HP(代入用)")]
    public float enemyHP {get; set;}

    [Tooltip("Rigidbody")]
    private Rigidbody2D m_rigidbody = null;

    [Tooltip("現在の挙動")]
    private ENEMY_STATE m_state = ENEMY_STATE.DUMMY;

    [Tooltip("接触している相手の情報")]
    private CharacterMove m_charaMove = null;

    [SerializeField]
    [Tooltip("攻撃音")]
    private AudioClip attackSound = null;

    [Tooltip("ソース")]
    private AudioSource source = null;

    [Tooltip("敵の挙動")]
    private enum ENEMY_STATE{
        DUMMY,
        MOVE,
        ATTACK,
        WAIT
    }

    private void Awake() {
        m_rigidbody = GetComponent<Rigidbody2D>();

        enemyHP = hp;

        moveSpeed = moveSpeed / 150f;

        m_state = ENEMY_STATE.MOVE;
        EnemyBehavior();

        source = GetComponent<AudioSource>();

    }

    private void EnemyBehavior(){

        this.FixedUpdateAsObservable()
            .Where(_=> m_state != ENEMY_STATE.DUMMY)
            .Subscribe(_=>{

                switch(m_state){
                    case ENEMY_STATE.MOVE:

                        m_rigidbody.AddForce(this.transform.right * moveSpeed);
                        
                        break;

                    case ENEMY_STATE.ATTACK:

                        m_rigidbody.velocity = Vector2.zero;

                        break;

                    case ENEMY_STATE.WAIT:
                        break;
                }

                // 自動殺戮処理
                if(this.transform.position.x > 20f){
                    Destroy(this.gameObject);
                }

                if(enemyHP <= 0f){
                    Destroy(this.gameObject);
                    MainManager.instance.enemyGauge.fillAmount -= 0.04f;
                }

                hp = enemyHP;

            }).AddTo(this);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Character")){
            m_state = ENEMY_STATE.ATTACK;

            m_charaMove = other.gameObject.GetComponent<CharacterMove>();
            StartCoroutine(AttackCol(attackSpan));
            StopCoroutine(TreeAttack(attackSpan));
        }
        
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.CompareTag("Character")){
            m_state = ENEMY_STATE.MOVE;
            StopCoroutine(AttackCol(attackSpan));
            StopCoroutine(TreeAttack(attackSpan));
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Tree")){
            m_state = ENEMY_STATE.ATTACK;
            StartCoroutine(TreeAttack(attackSpan));
        }
    }

    private IEnumerator AttackCol(float waittime){

        source.clip = attackSound;

        while(true){
            m_charaMove.charaHP = m_charaMove.charaHP - attackPower;
            source.Play();

            yield return new WaitForSeconds(waittime);
        }
    }

    private IEnumerator TreeAttack(float time){

        var p = attackPower / 1000f;

        while(true){
            MainManager.instance.treeGauge.fillAmount -= p;

            yield return new WaitForSeconds(time);
        }
    }
}
