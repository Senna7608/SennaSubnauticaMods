﻿using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyclopsLaserCannonModule
{
    public class CannonCamera : MonoBehaviour, IInputHandler
    {
        public CannonControl control_instance;
        public CannonCamera cannonCamera;

        public bool usingCamera;
        public float rotationSpeedDamper = 3f;
        private Vector3 startRot;
        private Vector3 rotAngle = new Vector3(0f, 0, 0);
        private Vector2 constraintsX = new Vector2(-22f, 45f);        

        private RectTransform arrow;
        private Image Crosshair;
        private TextMeshProUGUI Title;
        private Transform SNCameraRoot_Parent;
        public TextMeshProUGUI PowerText, DepthText, LowPowerText;       

        private void Awake()
        {
            SNLogger.Debug("CannonCamera: Awake started...");
            cannonCamera = this;
        }

        private void Start()
        {  
            arrow = gameObject.transform.Find("DirectionIndicator/Arrow").gameObject.GetComponent<RectTransform>();

            Crosshair = gameObject.FindChild("Crosshair").GetComponent<Image>();
            Crosshair.color = Color.red;

            Title = gameObject.transform.Find("Title/TitleText").gameObject.GetComponent<TextMeshProUGUI>();
            Title.text = CannonConfig.language_settings["Item_Name"];

            LowPowerText.text = $"{CannonConfig.language_settings["LowPower_Title"]}\n{CannonConfig.language_settings["LowPower_Message"]}";
            LowPowerText.color = Color.red;
            LowPowerText.fontStyle = FontStyles.Bold;
            LowPowerText.overflowMode = TextOverflowModes.Overflow;

            startRot = control_instance.CannonCamPosition.transform.localRotation.eulerAngles;
        }

        public void EnterCamera()
        {
            usingCamera = true;            
            InputHandlerStack.main.Push(this);
            Player main = Player.main;
            MainCameraControl.main.enabled = false;
            SNCameraRoot_Parent = SNCameraRoot.main.transform.parent;
            SNCameraRoot.main.transform.SetParent(control_instance.CannonCamPosition.transform, false);            
            Player.main.SetHeadVisible(true);            
            VRUtil.Recenter();
            gameObject.SetActive(true);
        }

        private void ExitCamera()
        {
            usingCamera = false;
            gameObject.SetActive(false);            
            Player main = Player.main;
            SNCameraRoot.main.transform.SetParent(SNCameraRoot_Parent, false);
            SNCameraRoot.main.transform.localPosition = Vector3.zero;
            SNCameraRoot.main.transform.localRotation = Quaternion.identity;
            MainCameraControl.main.enabled = true;
            Player.main.SetHeadVisible(false);            
        }

        public bool HandleInput()
        {
            if (!usingCamera)
            {
                return false;
            }            
            if (GameInput.GetButtonUp(GameInput.Button.Exit) || Input.GetKeyUp(KeyCode.Escape))
            {
                ExitCamera();
                return false;
            }            
            return true;
        }

        public bool HandleLateInput()
        {
            return true;
        }

        public void OnFocusChanged(InputFocusMode mode)
        {            
        }

        private void LateUpdate()
        {
            if (!usingCamera)
            {
                return;
            }
            Vector2 vector = GameInput.GetLookDelta() / rotationSpeedDamper;
            Vector3 b = new Vector3(-vector.y, vector.x, 0f);
            rotAngle += b;
            float x = Mathf.Clamp(rotAngle.x, constraintsX.x, constraintsX.y);
            float y = rotAngle.y;
            rotAngle = new Vector3(x, y, 0f);
            control_instance.CannonCamPosition.transform.localRotation = Quaternion.Euler(startRot + rotAngle);
            SetDirection(rotAngle.y);
            SetPowerText();
            SetDepthText();
            
            if(control_instance.isLowPower)
            {
                LowPowerText.gameObject.SetActive(true);
            }
            else
            {
                LowPowerText.gameObject.SetActive(false);
            }            
        }       

        public void SetDirection(float angle)
        {
            arrow.localRotation = Quaternion.Euler(0f, 0f, -angle);
        }

        private void SetPowerText()
        {            
            PowerText.text = control_instance.PowerText.GetComponent<TextMeshProUGUI>().text;
        }

        private void SetDepthText()
        {            
            DepthText.text = control_instance.DepthText.GetComponent<TextMeshProUGUI>().text;
        }
    }
}
