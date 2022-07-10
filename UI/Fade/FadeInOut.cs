using System.Collections;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    private StageManager _stageManager;
    private UnityEngine.UI.Image _fadeImg;
    
    [SerializeField] private float fadeTime = 2f;

    private void Awake()
    {
        _stageManager = GameObject.Find(nameof(StageManager)).GetComponent<StageManager>();
        _fadeImg = GetComponent<UnityEngine.UI.Image>();

        _stageManager.OnStageStart += StageStart;
        _stageManager.OnStageEnd += StageEnd;
    }
    
    private void StageStart(StageManager owner)
    {
        StartCoroutine(FadeIn());
    }

    private void StageEnd(StageManager owner)
    {
        StartCoroutine(FadeOut());
    }

    
    private IEnumerator FadeIn()              
    {
        var color = _fadeImg.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / (fadeTime);
            _fadeImg.color = color;
            yield return null;
        }

        _stageManager.isStart = true;
    }
    
    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2.5f);
        
        _stageManager.isStart = false;
        var color = _fadeImg.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / (fadeTime);
            _fadeImg.color = color;
            yield return null;
        }

        _stageManager.isNext = false;
    }
}