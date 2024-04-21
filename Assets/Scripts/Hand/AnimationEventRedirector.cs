using UnityEngine;

public class AnimationEventRedirector : MonoBehaviour
{
    public HandBehavior parentScript;

    public void StartFalling()
    {
        parentScript.StartFalling();
    }

    public void EnableCollider()
    {
        parentScript.EnableCollider();
    }
}
