using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ゲームメインを管理する
/// </summary>
public class MainManager : MonoBehaviour{

    [Tooltip("Instance")]
    public static MainManager instance = null;

    [Header("ゲーム更新許可")]
    public bool gameUpdatePermit = false;

    [Tooltip("現在の状態")]
    private MAIN_STATE m_nowState = MAIN_STATE.DUMMY;

    [Tooltip("直前の状態")]
    private MAIN_STATE m_preState = MAIN_STATE.DUMMY;

    [Tooltip("初期化フラグ")]
    private bool initFlag = false;


    /*-------------------------------------------------------------------------------*/
    

    public enum MAIN_STATE{
        DUMMY,
        COUNTDOWN,
        START,
        MAIN,
        RESULT
    }

    private void Awake() {

        // インスタンス生成
        if(!instance)
            instance = this;
        
        // ゲームメインの更新
        this.UpdateAsObservable()
            .Where(_=> gameUpdatePermit)
            .Subscribe(_=>UpdateState())
            .AddTo(this);

        ChangeState(MAIN_STATE.COUNTDOWN);
        gameUpdatePermit = true;
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
        Debug.Log("NowState : " + m_nowState.ToString());
    }

    private void CountDown(){
        if(initFlag){
            initFlag = false;
            ChangeState(MAIN_STATE.START);
        }
    }

    private void GameStart(){
        if(initFlag){
            initFlag = false;

            EnemyInstantiateSystem.instance.EnemyInstantiateStart();
            ChangeState(MAIN_STATE.MAIN);
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
