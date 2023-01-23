using UnityEngine;
using SceneChanger;

public class TitleActionManager : MonoBehaviour{

    [Tooltip("Input")]
    private PlayerInputScript m_input = null;

    private void OnEnable() { m_input.Enable(); }

    private void OnDisable() { m_input.Disable(); }

    private void Awake() {
        m_input = new PlayerInputScript();

        // Click Event
        m_input.UI.Click.performed += _ => {
            FadeSceneChanger.Instance.ChangeSceneWithFade("Preparation", 2f, 2f);
        };
    }
}
