using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class OptionManager : MonoBehaviour{

    [SerializeField]
    [Tooltip("オプションボタン")]
    private Button optionButton = null;

    [SerializeField]
    [Tooltip("閉じるボタン")]
    private Button closeButton = null;

    [SerializeField]
    [Tooltip("オプション画面")]
    private GameObject optionWindow = null;

    private void Awake() {
        
        optionWindow.SetActive(false);

        // オプション開く
        optionButton.OnClickAsObservable()
            .Where(_=> !optionWindow.activeSelf)
            .Subscribe(_=> {
                optionWindow.SetActive(true);
                MainManager.instance.gameUpdatePermit = false;
                }).AddTo(this);

        // オプション閉じる
        closeButton.OnClickAsObservable()
            .Where(_=> optionWindow.activeSelf)
            .Subscribe(_=> {
                optionWindow.SetActive(false);
                MainManager.instance.gameUpdatePermit = true;
                }).AddTo(this);
    }
}
