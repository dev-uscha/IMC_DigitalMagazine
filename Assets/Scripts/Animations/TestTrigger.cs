using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    public SimpleGrow mover;
    public Pyramid pyramidAnimator;
    public ScaleGrow scaleGrow;
    public Falling faller; 
    public Assamble assamble;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            mover.MoveToEnd(); // Bewegung zum Ende
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            mover.MoveToStart(); // Bewegung zurück
        }
         if (Input.GetKeyDown(KeyCode.P))
        {
            pyramidAnimator.StartPyramidAnimation(); // Aufbauen
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            pyramidAnimator.ReversePyramidAnimation(); // Abbauen
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            scaleGrow.MoveToEnd(); // Bewegung zum Ende
        }if (Input.GetKeyDown(KeyCode.U))
        {
            scaleGrow.MoveToStart(); // Bewegung zurück
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
           faller.DropFromSky(); // Bewegung zurück
        }
        //ASSAMBLE
        if (Input.GetKeyDown(KeyCode.N))
        {
            assamble.MoveToEnd(); // Bewegung zurück
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
           assamble.MoveToStart(); // Bewegung zurück
        }
    }
}
