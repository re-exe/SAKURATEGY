using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

public class InstantiateSystem : MonoBehaviour{

    [Tooltip("インスタンス")]
    public static InstantiateSystem instance = null;

    [Header("トラッキング許可")]
    public bool trackingFlag = true;

    [Header("接触判定")]
    public EventTrigger mousePosEvent = null;

    [Header("スポーン先の親")]
    public GameObject characterSpawnParent;

    [Tooltip("最終的なオブジェクトの位置")]
    private Vector3 finalPosition = Vector3.zero;

    [Tooltip("キャラクターのインスタンス")]
    private GameObject characterInstance = null;

    [Tooltip("D&D中のキャラクター保管変数")]
    private GameObject m_chara = null;

    [Tooltip("マウスカーソルがエリア内にいるか")]
    public bool isAreaContactFlag = false;

    [Space(10)]
    [Header("ゲージ関係")]

    [SerializeField]
    [Header("キャラゲージ")]
    private Image charaGauge = null;

    [Space(10)]
    [Header("その他")]
    [SerializeField]
    [Tooltip("コスト不足表示パネル")]
    private GameObject costShortagePanel = null;

    [SerializeField]
    [Tooltip("ソース")]
    private AudioSource source = null;

    [SerializeField]
    [Tooltip("警告音")]
    private AudioClip warningSound = null;

    [SerializeField]
    [Tooltip("スポーン音")]
    private AudioClip spawnSound = null;


    private void Awake() {
        if(!instance){
            instance = this;
        }

        // イベントの登録
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener((e) => isAreaContactFlag = true);
        mousePosEvent.triggers.Add(enter);

        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((e) => isAreaContactFlag = false);
        mousePosEvent.triggers.Add(exit);

        costShortagePanel.SetActive(false);
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

        // コピーするオブジェクトを保存
        characterInstance = chara;

        // トラッキング中は当たり判定を無効にする。
        m_chara.GetComponent<CircleCollider2D>().enabled = false;

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

        if(isAreaContactFlag){
            Instantiate(characterInstance, finalPosition, Quaternion.identity, characterSpawnParent.transform);
        } else {
            return;
        }
    }

    public void CostCount(float cost){

        cost = cost / 100;

        // コスト以下なら
        if(charaGauge.fillAmount <= cost){
            isAreaContactFlag = false;
            StartCoroutine(CostCol());
        } else {

            source.clip = spawnSound;
            source.Play();
            // コスト分減少させる。
            charaGauge.fillAmount = -cost;
        }
    }

    /// <summary>
    /// コスト不足を表示するパネル
    /// </summary>
    /// <returns></returns>
    private IEnumerator CostCol(){
        costShortagePanel.SetActive(true);
        source.clip = warningSound;
        source.Play();

        yield return new WaitForSeconds(1f);

        costShortagePanel.SetActive(false);

        yield return 0;
    }
}
