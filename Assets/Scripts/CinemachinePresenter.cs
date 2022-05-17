using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachinePresenter : MonoBehaviour
{

    [SerializeField] ThirdPersonController player;

    [SerializeField] float[] radiusRunning = {5, 8, 5};
    [SerializeField] float smoothSpeed = 5f;
    private float[] _radiusWalking = new float[3];
    private CinemachineFreeLook _freeCam;
    // Start is called before the first frame update
    void Awake()
    {
        CinemachineCore.GetInputAxis = GetAxisMouseKey1;
        _freeCam = gameObject.GetComponent<CinemachineFreeLook>();
        for (int i = 0; i < _freeCam.m_Orbits.Length; i++) {
            _radiusWalking[i] = _freeCam.m_Orbits[i].m_Radius;
        }
    }

    
    private void OnEnable() 
    {
        player.onPlayerRunningAction += UpdateCameraOrbit;
    }
    private void OnDisable()
    {
        player.onPlayerRunningAction -= UpdateCameraOrbit;
    }

        //returns normal call if Mouse 1 (right button) is pressed
    private float GetAxisMouseKey1(string axisName) 
    {
        if (axisName == "Mouse X" || axisName == "Mouse Y") 
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                //returns normal call if the button is pressed
                return UnityEngine.Input.GetAxis(axisName);
            } 
            else
            {
                return 0;
            }
        }
        //returns normal call if axisName is not either "Mouse X" or "Mouse Y"
        return UnityEngine.Input.GetAxis(axisName);
    }

    private void UpdateCameraOrbit(bool isRunning) 
    {
        if (isRunning)
        {
            for (int i = 0; i < _freeCam.m_Orbits.Length; i++) {
                _freeCam.m_Orbits[i].m_Radius = Mathf.Lerp(_freeCam.m_Orbits[i].m_Radius, 
                radiusRunning[i], 
                smoothSpeed * Time.deltaTime);
            }
            //Debug.Log("RUNNING");
        }
        else
        {
            for (int i = 0; i < _freeCam.m_Orbits.Length; i++) {
                _freeCam.m_Orbits[i].m_Radius = Mathf.Lerp(_freeCam.m_Orbits[i].m_Radius, 
                _radiusWalking[i], 
                smoothSpeed * Time.deltaTime);
            }
            //Debug.Log("RUNNING");
            //Debug.Log("WALKING");

        }

    }

}
