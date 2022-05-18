using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f;
    
    [SerializeField] float MIN_ANGLE_Y = -60f;
    [SerializeField] float MAX_ANGLE_Y = 60f;

    public Transform target { get; protected set; }
    public Vector3 point { get; protected set; }
    private Camera _cam;
    private bool isFlashlightOn = false;

    void Awake() 
    {
        _cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SwitchFlashlightOnOff();
        }
        
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        Vector3 mousePos = ray.direction.normalized;

        Quaternion targetRotation = Quaternion.LookRotation(ray.direction);
        //targetRotation.y = Mathf.Clamp(targetRotation.y , transform.parent.eulerAngles.y-60, transform.parent.eulerAngles.y+60);
        
        targetRotation = ReturnAngleWithinLocalBounds(ray, targetRotation);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        

        if (isFlashlightOn && Physics.Raycast(ray, out RaycastHit hit)) {
            point = hit.point;
            foreach (Light l in GetComponentsInChildren<Light>())
            {
                if (l.type.Equals(LightType.Spot))
                {
                    float angleInRadians = Mathf.Deg2Rad * l.spotAngle;
                    float coneRadius = Mathf.Tan(angleInRadians) * l.range;
                    FOVServices fov = new FOVServices(coneRadius, 360, 9, 7);
                    if (fov.CheckFOV(hit.point, transform.forward)) {
                        target = fov.GetTarget();
                        Debug.Log("Found the enemy!");
                    }
                }

            }
        }
    }
    

    //Since targetRotation is a number between 0 and 360, we need this function to determine the limits of the flashlight's local rotation
    private Quaternion ReturnAngleWithinLocalBounds(Ray ray, Quaternion targetRotation)
    {
        //minY and maxY values can assume values lower than 0 or bigger than 360
        float minY = transform.parent.eulerAngles.y + MIN_ANGLE_Y;
        float maxY = transform.parent.eulerAngles.y + MAX_ANGLE_Y;
        float targetAngleY = targetRotation.eulerAngles.y;

        //Debug.Log("minY: "+minY+"parentY: "+transform.parent.eulerAngles.y+" maxY: "+maxY+" targetY: "+targetRotation.eulerAngles.y);

        //We then start from the most basic case, when the rotation is completely within bounds
        if (targetRotation.eulerAngles.y < maxY && targetRotation.eulerAngles.y > minY) 
        {
            //Debug.Log("Angle's within both bounds.");
            return targetRotation;
        } 
        else 
        {
            //Now we check if minY has a negative value and add +360 to the boundary to check the rotation again
            if (minY < 0) 
            {
                minY += 360;
                if  (targetRotation.eulerAngles.y > minY) 
                {
                    //Debug.Log("Angle's within negative bounds.");
                    return targetRotation;
                }
                //If it's still out of bounds, 
                if (targetRotation.eulerAngles.y < 180 + transform.parent.eulerAngles.y)
                {
                    //Debug.Log("(-) Angle's out of bounds. Returning maxY");
                    targetAngleY = maxY;
                } else 
                {
                    //Debug.Log("(-) Angle's out of bounds. Returning minY");
                    targetAngleY = minY;
                }
                
            } 
            else if (maxY > 360)
            {
                maxY -= 360;
                if (targetRotation.eulerAngles.y < maxY) 
                {
                    //Debug.Log("Angle's within positive bounds.");
                    return targetRotation;
                }
                //If it's still out of bounds, 
                if (targetRotation.eulerAngles.y < transform.parent.eulerAngles.y - 180) 
                {
                    //Debug.Log("(360) Angle's out of bounds. Returning maxY");
                    targetAngleY = maxY;
                } else 
                {
                    
                    //Debug.Log("(360) Angle's out of bounds. Returning minY");
                    targetAngleY = minY;
                }
            }
            else 
            {
                /*
                    30 - 150, rotação de 160
                    se 160 > 180 - 90, ou seja, 160 > 90
                    ou se 160 > 180 + 90, ou seja, 160 > 270
                    cumpre os dois requisitos e retorna minY...


                    o correto seria:
                    Olhando para 200 como centro, o círculo é divido em 2 quadrantes: 260 à 20 e 20 à 140. Esses dois definirão o lado para o qual a lanterna deve apontar.
                    Se o ângulo é maior que 200 e menor que 20, retorna maxY.
                    Se o ângulo é maior que 300 e menor que 120, retorna minY.
                    Como chegar nessa conta, sabendo que os mínimos sempre serão acima de 0?
                    0
                    200 + 180 = 360 + 20
                */
                float angleOffset = 0, oppositeAngle = 180 + transform.parent.eulerAngles.y;
                if (oppositeAngle > 360) {
                    angleOffset = oppositeAngle - 360;
                    //Debug.Log("(+) Angle Offset Value"+ angleOffset);
                }
                if (targetRotation.eulerAngles.y > oppositeAngle || targetRotation.eulerAngles.y < transform.parent.eulerAngles.y)
                {
                    if (oppositeAngle > 360 && targetRotation.eulerAngles.y < angleOffset) 
                    {
                        //Debug.Log("(+) Angle's out of bounds. Returning maxY");
                        targetAngleY = maxY;
                    }
                    else 
                    {
                        //Debug.Log("(+) Angle's out of bounds. Returning minY");
                        targetAngleY = minY;
                    }
                }
                else
                {
                    //Debug.Log("(+) Angle's out of bounds. Returning maxY");
                    targetAngleY = maxY;
                }
                
            }
            return Quaternion.Euler(
                    targetRotation.x,
                    targetAngleY,
                    targetRotation.z);
        }
    }

    private void SwitchFlashlightOnOff()
    {
        
        isFlashlightOn = !isFlashlightOn;
        foreach (Light l in GetComponentsInChildren<Light>())
        {
            //Debug.Log("Name: "+l.gameObject.name);
            l.enabled = !l.enabled;

        }
    }

    public bool IsOn () {
        return isFlashlightOn;
    }
}
