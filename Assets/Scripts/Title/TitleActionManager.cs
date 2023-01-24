using UnityEngine;
using SceneChanger;
using DG.Tweening;

public class TitleActionManager : MonoBehaviour{

    [Tooltip("Input")]
    private PlayerInputScript m_input = null;

    [SerializeField]
    [Tooltip("点滅するテキスト")]
    private CanvasGroup flashText = null;

    private void OnEnable() { m_input.Enable(); }

    private void OnDisable() { m_input.Disable(); }

    private void Awake() {
        m_input = new PlayerInputScript();

        // Click Event
        m_input.UI.Click.performed += _ => {
            FadeSceneChanger.Instance.ChangeSceneWithFade("Preparation", 2f, 2f);
        };

        flashText.DOFade(1f, 0.8f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear)
            .SetLink(this.gameObject)
            .Play();
    }
}
