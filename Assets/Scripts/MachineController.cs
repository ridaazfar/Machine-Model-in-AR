using UnityEngine;

public class MachineController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator animation;
    public void PlayReverse()
    {
        animation.SetTrigger("playReverse");
    }

    public void PlayForward()
    {
        animation.SetTrigger("playForward");
    }
}
