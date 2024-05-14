using UnityEngine;public class PinchZoom : MonoBehaviour{    public Transform targetObject; // Assign the GameObject to be scaled
    public float sensitivity = 0.01f; // Sensitivity of the scaling
    public float scaleMin = 0.1f; // Minimum scale
    public float scaleMax = 5f; // Maximum scale


  


    private void Update()    {       
        if (Input.touchCount == 2)        {
            // Get the two touches and store them
            Touch touchZero = Input.GetTouch(0);            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (distance) between the touches in each frame
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in distances between each frame
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Calculate the new scale factor
            float scaleFactor = 1 - (deltaMagnitudeDiff * sensitivity);

            // Update the scale of the target object
            targetObject.localScale *= scaleFactor;

            // Clamp the scale of the target object to prevent it from going below or above limits
            targetObject.localScale = new Vector3(
Mathf.Clamp(targetObject.localScale.x, scaleMin, scaleMax),
Mathf.Clamp(targetObject.localScale.y, scaleMin, scaleMax),
Mathf.Clamp(targetObject.localScale.z, scaleMin, scaleMax)
);                    }        //Debug.Log("Scale: " + targetObject.localScale);    }}