using Tayx.Graphy;
using Tayx.Graphy.Utils.NumString;
using TMPro;
using UnityEngine;

public class FpsDebugger : MonoBehaviour
{
    [SerializeField] private TMP_Text m_fpsText = null;
        
    private GraphyManager m_graphyManager = null;
        
    private int m_updateRate = 4;
    private int m_frameCount = 0;
    private float m_deltaTime = 0f;
    private float m_fps = 0f;

    private void Awake()
    {
        G_IntString.Init( 0, 2000 ); // Max fps expected
        G_FloatString.Init( 0, 100 ); // Max ms expected per frame

        m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();

        UpdateParameters();
    }
        
    public void UpdateParameters()
    {
        m_updateRate = m_graphyManager.FpsTextUpdateRate;
    }
        
    private void Update()
    {
        m_deltaTime += Time.unscaledDeltaTime;

        m_frameCount++;

        if( m_deltaTime > 1f / m_updateRate )
        {
            m_fps = m_frameCount / m_deltaTime;

            // Update fps
            m_fpsText.text = Mathf.RoundToInt( m_fps ).ToStringNonAlloc();
            SetFpsRelatedTextColor( m_fpsText, m_fps );

            // Reset variables
            m_deltaTime = 0f;
            m_frameCount = 0;
        }
    }
        
    private void SetFpsRelatedTextColor(TMP_Text text, float fps)
    {
        int roundedFps = Mathf.RoundToInt( fps );

        if( roundedFps >= m_graphyManager.GoodFPSThreshold )
        {
            text.color = m_graphyManager.GoodFPSColor;
        }
        else if( roundedFps >= m_graphyManager.CautionFPSThreshold )
        {
            text.color = m_graphyManager.CautionFPSColor;
        }
        else
        {
            text.color = m_graphyManager.CriticalFPSColor;
        }
    }
}