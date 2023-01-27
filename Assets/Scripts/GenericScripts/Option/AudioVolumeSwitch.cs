using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class AudioVolumeSwitch : MonoBehaviour{

    [Tooltip("ボタン")]
    private Button button = null;

    [Tooltip("画像")]
    private Image image = null;

    [Tooltip("テキスト")]
    private TextMeshProUGUI text = null;

    [Tooltip("ミュート中ならtrue")]
    private bool muteFlag = false;

    [SerializeField]
    [Tooltip("ボタンイメージ")]
    private Sprite[] sprites = new Sprite[2];
    

    private void Awake() {
        GameObject child = transform.GetChild(0).gameObject;
        text = child.GetComponent<TextMeshProUGUI>();
        button = GetComponent<Button>();
        image = GetComponent<Image>();


        button.OnClickAsObservable()
            .Subscribe(_=>{
                muteFlag = !muteFlag;

                if(!muteFlag){
                    // 再生中
                    image.sprite = sprites[0];
                    text.SetText("プレイ中");
                    AudioListener.volume = 1f;
                } else {
                    // ミュート中
                    image.sprite = sprites[1];
                    text.SetText("ミュート中");
                    AudioListener.volume = 0f;
                }
            }).AddTo(this);
        
    }
}
