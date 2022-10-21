using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    private List<Tween> _activeTweens = new();

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < _activeTweens.Count; i++)
        {
            var activeTween = _activeTweens[i];

            float dist = Vector3.Distance(activeTween.Target.position, activeTween.EndPos);

            if (dist > 0.1f)
            {
                var elapsedTime = Time.time - activeTween.StartTime;
                var interpolationRatio = elapsedTime / activeTween.Duration;
                activeTween.Target.position =
                    Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, interpolationRatio);
            }
            else
            {
                activeTween.Target.position = activeTween.EndPos;
                _activeTweens.Remove(activeTween);
            }
        }
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        /*if (activeTween == null)
        {
            activeTween = new Tween(targetObject, startPos, endPos, Time.time, duration);
        }*/

        if (!TweenExists(targetObject))
        {
            var tween = new Tween(targetObject, startPos, endPos, Time.time, duration);
            _activeTweens.Add(tween);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveTween(Transform target)
    {
        _activeTweens.RemoveAll(x => x.Target == target);
    }

    public bool TweenExists(Transform target)
    {
        return _activeTweens.Exists(x => x.Target == target);
    }
}
