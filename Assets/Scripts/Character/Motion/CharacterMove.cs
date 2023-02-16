using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// キャラの動きを管理する
/// </summary>
public class CharacterMove : MonoBehaviour{

    [SerializeField]
    [Tooltip("ID")]
    private TREE_ID id = TREE_ID.MAX;

    [Tooltip("現在の挙動")]
    private CHARA_STATE m_state = CHARA_STATE.DUMMY;

    [Tooltip("移動速度")]
    private float moveSpeed = 0f;

    [Tooltip("攻撃力")]
    private float attackPower = 0f;

    [SerializeField]
    [Tooltip("HP")]
    private float hp = 0f;

    [Tooltip("HP(代入用)")]
    public float charaHP {get; set;}

    [Tooltip("Rigidbody")]
    private Rigidbody2D m_rigidbody = null;

    [Tooltip("情報リスト")]
    private List<CharacterParamAsset.CharacterParam> m_paramList = new List<CharacterParamAsset.CharacterParam>();

    [Tooltip("接触している相手の情報")]
    private EnemyMove m_enemyMove = null;

    [Tooltip("キャラクターの挙動")]
    private enum CHARA_STATE{
        DUMMY,
        MOVE,
        ATTACK,
        WAIT
    }

    [Tooltip("キャラクターの種類ID")]
    public enum TREE_ID{
        SOMEIYOSHINO,
        KANHIZAKURA,
        KAWAZUSAKURA,
        HIGANZAKURA,
        OYAMAZAKURA,
        MAX
    }

    private void Awake() {

        CharacterParamAsset characterParam = Resources.Load("CharacterParamAsset") as CharacterParamAsset;
        m_paramList = characterParam.characterParamList;

        // init
        moveSpeed = m_paramList[(int)id].moveSpeed;
        attackPower = m_paramList[(int)id].attackPower;
        charaHP = m_paramList[(int)id].hp;
        hp = m_paramList[(int)id].hp;
        m_rigidbody = GetComponent<Rigidbody2D>();

        moveSpeed = moveSpeed / 150f;

        m_state = CHARA_STATE.MOVE;

        CharacterBehavior(id);
    }

    private void CharacterBehavior(TREE_ID id){

        this.FixedUpdateAsObservable()
            .Where(_=> m_state != CHARA_STATE.DUMMY)
            .Subscribe(_=>{

                switch(m_state){
                    case CHARA_STATE.MOVE:

                        m_rigidbody.AddForce(this.transform.right * -moveSpeed);
                        break;

                    case CHARA_STATE.ATTACK:

                        m_rigidbody.velocity = Vector2.zero;
                        

                        break;

                    case CHARA_STATE.WAIT:
                        m_rigidbody.velocity = Vector2.zero;

                        break;
                }

                // 自動殺戮処理
                if(this.transform.position.x < -20f){
                    Destroy(this.gameObject);
                }

                if(charaHP <= 0f){
                    Destroy(this.gameObject);
                }

                hp = charaHP;

            }).AddTo(this);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy")){
            m_state = CHARA_STATE.ATTACK;

            m_enemyMove = other.gameObject.GetComponent<EnemyMove>();
            StartCoroutine(AttackCol(m_paramList[(int)id].frequency));
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy")){
            m_state = CHARA_STATE.WAIT;
            StopCoroutine(AttackCol(m_paramList[(int)id].frequency));
        }
    }

    private IEnumerator AttackCol(float waittime){

        while(true){

            m_enemyMove.enemyHP = m_enemyMove.enemyHP - attackPower;

            yield return new WaitForSeconds(waittime);
        }
    }
}
