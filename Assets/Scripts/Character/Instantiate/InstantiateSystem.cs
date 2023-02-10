using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class InstantiateSystem : MonoBehaviour{

    [Tooltip("インスタンス")]
    public static InstantiateSystem instance = null;

    [Header("トラッキング許可")]
    public bool trackingFlag = true;

    [SerializeField]
    [Header("エリア判定")]
    private CircleCollider2D m_areaColl = null;

    [Header("スポーン先の親")]
    public GameObject characterSpawnParent;

    Vector3 finalPosition = Vector3.zero;

    GameObject characterInstance = null;

    GameObject m_chara = null;


    private void Awake() {
        if(!instance){
            instance = this;
        }
    }

    /// <summary>
    /// キャラクターの生成 + トラッキング
    /// </summary>
    /// <param name="chara">キャラクターのインスタンス</param>
    public void CharacterInstantiate(GameObject chara){

        // 初期位置を設定
        Vector3 mouse = Input.mousePosition;
        Vector3 pos = (Vector2)Camera.main.ScreenToWorldPoint(mouse);
        m_chara = Instantiate(chara, pos, Quaternion.identity);

        characterInstance = chara;

        // トラッキング開始
        trackingFlag = true;
        this.UpdateAsObservable()
            .Where(_=> trackingFlag)
            .Subscribe(_=>{
                Vector3 mouse = Input.mousePosition;
                m_chara.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(mouse);
                finalPosition = m_chara.transform.position;
            }).AddTo(this);
    }

    public void DestroyCharacterCash(){
        Destroy(m_chara);
    }

    /// <summary>
    /// キャラクターをマウスポインタの位置から生成
    /// </summary>
    public void InstantiateNewPosition(){
        trackingFlag = false;

        Instantiate(characterInstance, finalPosition, Quaternion.identity, characterSpawnParent.transform);
    }

    // TODO:キャラクターがすべて同じ場所に生成されるバグを修正する
}
