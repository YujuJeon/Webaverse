using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    [SerializeField] public GameObject[] _vrHands;
    [SerializeField] private TextMeshProUGUI txtStatus;
    [SerializeField] private TrackedPoseDriver tpd;
    [SerializeField]
    private XRNode controllerNode = XRNode.LeftHand;
    private List<InputDevice> devices = new List<InputDevice>();
    
    public bool isLocked = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //disable all the vr related
        tpd.enabled = false;
        XRSettings.enabled = false;
        GetDeviceInfo();
    }

    void GetDeviceInfo()
    {
        Debug.Log("Device active? : " + XRSettings.isDeviceActive);
        //Debug.Log(XRSettings.loadedDeviceName);
    }

    public void OnClickVRButton()
    {
        isLocked = !isLocked;

        if (isLocked)
        {
            tpd.enabled = true;
            XRSettings.enabled = true;
            GetDeviceInfo();
            for(int i = 0; i < _vrHands.Length; i++)
            {
                _vrHands[i].SetActive(true);
            }
            txtStatus.text = devices.FirstOrDefault() + "conected and VR button activated.";            
        }
        else
        {
            tpd.enabled = false;
            XRSettings.enabled = false;
            GetDeviceInfo();
            for (int i = 0; i < _vrHands.Length; i++)
            {
                _vrHands[i].SetActive(false);
            }
            txtStatus.text = "";
        }
    }
}
