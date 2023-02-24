using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using SceneChanger;

public class StartCheckFunction : MonoBehaviour{

    [SerializeField]
    [Tooltip("ボタン")]
    private List<Button> buttonList = new List<Button>((int)BUTTON_ID.MAX);

    [SerializeField]
    [Tooltip("Source")]
    private AudioSource source = null;

    private void Awake() {
        buttonList[(int)BUTTON_ID.YES].OnClickAsObservable()
            .Subscribe(_=> {
                source.Play();
                FadeSceneChanger.Instance.ChangeSceneWithFade("Main", 2f, 2f);
            })
            .AddTo(this);

        buttonList[(int)BUTTON_ID.NO].OnClickAsObservable()
            .Subscribe(_=> this.gameObject.SetActive(false))
            .AddTo(this);
    }

    private enum BUTTON_ID{
        YES,
        NO,
        MAX
    }
}
