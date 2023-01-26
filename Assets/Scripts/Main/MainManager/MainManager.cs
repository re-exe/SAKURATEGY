using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ゲームメインを管理する
/// </summary>
public class MainManager : MonoBehaviour{

    [Header("ゲーム更新許可")]
    public bool gameUpdatePermit = true;

    [SerializeField]
    [Tooltip("出現時間リスト")]
    private List<EnemySpawnTime> EnemySpawnTimeList = new List<EnemySpawnTime>();

    [Tooltip("現在の状態")]
    private MAIN_STATE m_nowState = MAIN_STATE.DUMMY;

    [Tooltip("直前の状態")]
    private MAIN_STATE m_preState = MAIN_STATE.DUMMY;

    [Tooltip("初期化フラグ")]
    private bool initFlag = false;

    

    public enum MAIN_STATE{
        DUMMY,
        COUNTDOWN,
        START,
        MAIN,
        RESULT
    }

    [System.Serializable]
    public class EnemySpawnTime{
        [Header("名前")]
        public string name = "";

        [Header("スポーンまでの経過時間")]
        public float time = 0f;
    }

    private void Awake() {
        
        // ゲームメインの更新
        this.UpdateAsObservable()
            .Where(_=> gameUpdatePermit)
            .Subscribe(_=>UpdateState())
            .AddTo(this);
    }

    // 更新処理
    private void UpdateState(){
        switch(m_nowState){
            case MAIN_STATE.COUNTDOWN: CountDown(); break;
            case MAIN_STATE.START: GameStart(); break;
            case MAIN_STATE.MAIN: MainUpdate(); break;
            case MAIN_STATE.RESULT: Result(); break;
        }
    }

    // 状態遷移
    private void ChangeState(MAIN_STATE state){
        m_preState = m_nowState;
        m_nowState = state;
        initFlag = true;
    }

    private void CountDown(){
        if(initFlag){
            initFlag = false;
        }
    }

    private void GameStart(){
        if(initFlag){
            initFlag = false;
        }
    }

    private void MainUpdate(){
        if(initFlag){
            initFlag = false;
        }
    }

    private void Result(){
        if(initFlag){
            initFlag = false;
        }
    }
}