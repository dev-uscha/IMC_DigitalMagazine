using UnityEngine;

public class PageFlipTrigger : MonoBehaviour
{
    public PageManager pageManager;
    public bool flipForward = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (flipForward){
            pageManager.FlipForward();
                    Debug.Log("Flip Foreard");
        }
            
        else{
             pageManager.FlipBackward();
             Debug.Log("Flip Backward");
        }
           
    }
}
