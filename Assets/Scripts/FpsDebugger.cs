using Tayx.Graphy;
using Tayx.Graphy.Utils.NumString;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class FpsDebugger : MonoBehaviour
{
    [FormerlySerializedAs("m_fpsText")] [SerializeField] private TMP_Text mFPSText;
        
    private GraphyManager _mGraphyManager;
        
    private int _mUpdateRate = 4;
    private int _mFrameCount;
    private float _mDeltaTime;
    private float _mFPS;

    private void Awake()
    {
        G_IntString.Init( 0, 2000 ); // Max fps expected
        G_FloatString.Init( 0, 100 ); // Max ms expected per frame

        _mGraphyManager = transform.root.GetComponentInChildren<GraphyManager>();

        UpdateParameters();
    }

    private void UpdateParameters()
    {
        _mUpdateRate = _mGraphyManager.FpsTextUpdateRate;
    }
        
    private void Update()
    {
        _mDeltaTime += Time.unscaledDeltaTime;

        _mFrameCount++;

        if (_mDeltaTime > 1f / _mUpdateRate)
        {
            _mFPS = _mFrameCount / _mDeltaTime;

            // Update fps
            mFPSText.text = Mathf.RoundToInt( _mFPS ).ToStringNonAlloc();
            SetFpsRelatedTextColor( mFPSText, _mFPS );

            // Reset variables
            _mDeltaTime = 0f;
            _mFrameCount = 0;
        }
    }
        
    private void SetFpsRelatedTextColor(TMP_Text text, float fps)
    {
        int roundedFps = Mathf.RoundToInt( fps );

        if( roundedFps >= _mGraphyManager.GoodFPSThreshold )
        {
            text.color = _mGraphyManager.GoodFPSColor;
        }
        else if( roundedFps >= _mGraphyManager.CautionFPSThreshold )
        {
            text.color = _mGraphyManager.CautionFPSColor;
        }
        else
        {
            text.color = _mGraphyManager.CriticalFPSColor;
        }
    }
}