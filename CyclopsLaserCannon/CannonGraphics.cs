using UnityEngine;
using UnityEngine.UI;
using Common;
using static Common.GameHelper;

namespace CyclopsLaserCannonModule
{
    public partial class CannonControl
    {
        public GameObject This_Cyclops_Root;
        public GameObject CamPosition_Keel;
        public GameObject CannonCamPosition;
        public GameObject PowerText;
        public GameObject DepthText;
        
        public GameObject Button_Cannon;
        public GameObject Cannon_Camera;        

        private GameObject cannon_base_right;
        private GameObject cannon_right_tube_right;
        private GameObject cannon_right_tube_left;
        private GameObject cannon_right_rotation_point;

        private GameObject cannon_base_left;
        private GameObject cannon_left_tube_right;
        private GameObject cannon_left_tube_left;
        private GameObject cannon_left_rotation_point;

        private LineRenderer right_right, right_left, left_right, left_left;

        private Color beamcolor = Color.green;

        private void CreateCannonButton()
        {
            GameObject Abilities = This_Cyclops_Root.FindChild("HelmHUD").FindChild("Canvas_RightHUD").FindChild("Abilities");
            GameObject Button_Camera = Abilities.FindChild("Button_Camera");

            Button_Cannon = Instantiate(Button_Camera, Abilities.transform, false);

            Destroy(Button_Cannon.GetComponent<CyclopsExternalCamsButton>());

            Button_Cannon.name = "Button_Cannon";
            Button_Cannon.transform.localPosition = new Vector3(-300f, 432f, 0);

            CannonButton button_instance = Button_Cannon.GetOrAddComponent<CannonButton>();
            button_instance.control_instance = this;            
        }

        public void CreateCannonCamera()
        {
            CamPosition_Keel = This_Cyclops_Root.FindChild("ExternalCams").FindChild("CamPosition_Keel");

            CannonCamPosition = new GameObject("CannonCamPosition");
            CannonCamPosition.transform.SetParent(transform, false);
            CannonCamPosition.transform.localPosition = CamPosition_Keel.transform.localPosition;
            CannonCamPosition.transform.localRotation = Quaternion.Euler(25, 180, 0);

            GameObject Content = uGUI_CameraCyclops.main.gameObject.FindChild("Content");
            GameObject CameraCyclops = Content.FindChild("CameraCyclops");

            Cannon_Camera = Instantiate(CameraCyclops, Content.transform, false);

            Destroy(Cannon_Camera.FindChild("FrameLeft"));
            Destroy(Cannon_Camera.FindChild("FrameRight"));
            Destroy(Cannon_Camera.FindChild("Fader"));

            Cannon_Camera.name = "Cannon_Camera";

            GameObject Canvas_CenterHUD = This_Cyclops_Root.FindChild("HelmHUD").FindChild("Canvas_CenterHUD");
            GameObject PowerStatus = Canvas_CenterHUD.FindChild("PowerStatus");            
            GameObject PowerIcon = PowerStatus.FindChild("PowerIcon");

            PowerText = PowerStatus.FindChild("PowerText");
            DepthText = Canvas_CenterHUD.FindChild("DepthStatus").FindChild("DepthText");

            GameObject PowerIconCopy = Instantiate(PowerIcon, Cannon_Camera.transform, false);
            GameObject PowerTextCopy = Instantiate(PowerText, Cannon_Camera.transform, false);
            GameObject DepthTextCopy = Instantiate(DepthText, Cannon_Camera.transform, false);
            GameObject LowPowerText = Instantiate(DepthText, Cannon_Camera.transform, false);

            PowerIconCopy.transform.localPosition = new Vector3(60f, 400f, 0f);
            PowerIconCopy.transform.localScale = new Vector3(0.60f, 0.60f, 0.60f);

            PowerTextCopy.transform.localPosition = new Vector3(122f, 400f, 0.00f);
            PowerTextCopy.transform.localScale = new Vector3(0.30f, 0.30f, 0.30f);

            DepthTextCopy.transform.localPosition = new Vector3(-40f, 400f, 0.00f);
            DepthTextCopy.transform.localScale = new Vector3(0.30f, 0.30f, 0.30f);

            LowPowerText.transform.localPosition = new Vector3(2f, 220f, 0.00f);
            LowPowerText.transform.localScale = new Vector3(0.40f, 0.40f, 0.40f);

            LowPowerText.name = "LowPowerText";
            camera_instance = Cannon_Camera.GetOrAddComponent<CannonCamera>();
            camera_instance.control_instance = this;

            camera_instance.PowerText = PowerTextCopy.GetComponent<Text>();
            camera_instance.DepthText = DepthTextCopy.GetComponent<Text>();
            camera_instance.LowPowerText = LowPowerText.GetComponent<Text>();
        }


        private void CreateCannonRight()
        {
            cannon_base_right = new GameObject("cannon_base_right");
            cannon_base_right.transform.SetParent(gameObject.transform, false);
            cannon_base_right.transform.localPosition = new Vector3(-3.55f, -7.19f, 0.81f);
            cannon_base_right.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            //GameObject cannon_right_circuit_box = Instantiate(CraftData.GetPrefabForTechType(TechType.StarshipCircuitBox), Vector3.zero, Quaternion.identity, cannon_base_right.transform);
            GameObject cannon_right_circuit_box = CraftData.InstantiateFromPrefab(TechType.StarshipCircuitBox);
            cannon_right_circuit_box.transform.SetParent(cannon_base_right.transform, false);

            cannon_right_circuit_box.CleanObject();
            cannon_right_circuit_box.name = "cannon_right_circuit_box";
            cannon_right_circuit_box.transform.localPosition = new Vector3(-0.56f, 0.43f, -0.66f);
            cannon_right_circuit_box.transform.localScale = new Vector3(0.72f, -0.72f, 0.72f);
            cannon_right_circuit_box.transform.localRotation = Quaternion.Euler(78f, 270f, 180f);

            //GameObject cannon_pylon_right = Instantiate(CraftData.GetPrefabForTechType(TechType.PowerTransmitter), Vector3.zero, Quaternion.identity, cannon_base_right.transform);
            GameObject cannon_pylon_right = CraftData.InstantiateFromPrefab(TechType.PowerTransmitter);
            cannon_pylon_right.transform.SetParent(cannon_base_right.transform, false);
            Utils.ZeroTransform(cannon_pylon_right.transform);

            GameObject laserBeam = Instantiate(cannon_pylon_right.GetComponent<PowerFX>().vfxPrefab, null, false);
            laserBeam.SetActive(false);

            LineRenderer lineRenderer = laserBeam.GetComponent<LineRenderer>();
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
            lineRenderer.positionCount = 2;
            lineRenderer.material.color = beamcolor;

            Destroy(cannon_pylon_right.FindChild("powerFX_AttachPoint"));
            Destroy(cannon_pylon_right.FindChild("builder trigger"));
            cannon_pylon_right.CleanObject();
            cannon_pylon_right.name = "cannon_pylon_right";
            cannon_pylon_right.transform.localPosition = new Vector3(0.00f, 0.44f, 0f);
            cannon_pylon_right.transform.localRotation = Quaternion.Euler(0f, 180f, 180f);            

            cannon_right_rotation_point = new GameObject("cannon_right_rotation_point");
            cannon_right_rotation_point.transform.SetParent(cannon_pylon_right.transform, false);
            cannon_right_rotation_point.transform.localPosition = new Vector3(0.00f, 0.98f, 0.00f);
            cannon_right_rotation_point.transform.localRotation = Quaternion.Euler(25f, 180f, 0f);

            GameObject cannon_right = Instantiate(CraftData.GetPrefabForTechType(TechType.ExosuitTorpedoArmModule), Vector3.zero, Quaternion.identity, cannon_right_rotation_point.transform);
            Destroy(cannon_right.FindChild("GameObject"));
            cannon_right.FindChild("model").FindChild("exosuit_rig_armLeft:exosuit_torpedoLauncher_geo").name = "cannon_model";

            cannon_right.CleanObject();
            cannon_right.name = "cannon_right";
            cannon_right.transform.localPosition = new Vector3(0f, 0.14f, -0.66f);
            cannon_right.transform.localRotation = Quaternion.Euler(0f, 270f, 180f);
                        
            cannon_right.FindChild("model").FindChild("cannon_model").SetMeshMaterial(Main.cannon_material, 1);

            cannon_right_tube_right = Instantiate(laserBeam, Vector3.zero, Quaternion.identity, cannon_right.transform);
            cannon_right_tube_right.name = "cannon_right_tube_right";
            cannon_right_tube_right.transform.localPosition = new Vector3(0.68f, 0.0f, -0.17f);
            cannon_right_tube_right.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            right_right = cannon_right_tube_right.GetComponent<LineRenderer>();

            cannon_right_tube_left = Instantiate(laserBeam, Vector3.zero, Quaternion.identity, cannon_right.transform);
            cannon_right_tube_left.name = "cannon_right_tube_left";
            cannon_right_tube_left.transform.localPosition = new Vector3(0.68f, 0.0f, 0.17f);
            cannon_right_tube_left.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            right_left = cannon_right_tube_left.GetComponent<LineRenderer>();

            Destroy(laserBeam);
        }

        public void CreateCannonLeft()
        {
            cannon_base_left = new GameObject("cannon_base_left");
            cannon_base_left.transform.SetParent(gameObject.transform);
            cannon_base_left.transform.localPosition = new Vector3(3.55f, -7.19f, 0.81f);
            cannon_base_left.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            //GameObject cannon_left_circuit_box = Instantiate(CraftData.GetPrefabForTechType(TechType.StarshipCircuitBox), Vector3.zero, Quaternion.identity, cannon_base_left.transform);
            GameObject cannon_left_circuit_box = CraftData.InstantiateFromPrefab(TechType.StarshipCircuitBox);
            cannon_left_circuit_box.transform.SetParent(cannon_base_left.transform, false);

            cannon_left_circuit_box.CleanObject();
            cannon_left_circuit_box.name = "cannon_left_circuit_box";
            cannon_left_circuit_box.transform.localPosition = new Vector3(0.56f, 0.48f, -0.66f);
            cannon_left_circuit_box.transform.localScale = new Vector3(0.72f, 0.72f, 0.72f);
            cannon_left_circuit_box.transform.localRotation = Quaternion.Euler(72f, 90f, 0f);

            //GameObject cannon_pylon_left = Instantiate(CraftData.GetPrefabForTechType(TechType.PowerTransmitter), Vector3.zero, Quaternion.identity, cannon_base_left.transform);

            GameObject cannon_pylon_left = CraftData.InstantiateFromPrefab(TechType.PowerTransmitter);
            cannon_pylon_left.transform.SetParent(cannon_base_left.transform, false);
            Utils.ZeroTransform(cannon_pylon_left.transform);

            GameObject laserBeam = Instantiate(cannon_pylon_left.GetComponent<PowerFX>().vfxPrefab, cannon_base_left.transform, false);
            laserBeam.SetActive(false);

            LineRenderer lineRenderer = laserBeam.GetComponent<LineRenderer>();
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
            lineRenderer.positionCount = 2;
            lineRenderer.material.color = beamcolor;

            Destroy(cannon_pylon_left.FindChild("powerFX_AttachPoint"));
            Destroy(cannon_pylon_left.FindChild("builder trigger"));
            cannon_pylon_left.CleanObject();
            cannon_pylon_left.name = "cannon_pylon_left";
            cannon_pylon_left.transform.localPosition = new Vector3(0.00f, 0.44f, 0f);
            cannon_pylon_left.transform.localRotation = Quaternion.Euler(0f, 180f, 180f);            

            cannon_left_rotation_point = new GameObject("cannon_left_rotation_point");
            cannon_left_rotation_point.transform.SetParent(cannon_pylon_left.transform, false);
            cannon_left_rotation_point.transform.localPosition = new Vector3(0.00f, 0.98f, 0.00f);
            cannon_left_rotation_point.transform.localRotation = Quaternion.Euler(25f, 180f, 0f);

            GameObject cannon_left = Instantiate(CraftData.GetPrefabForTechType(TechType.ExosuitTorpedoArmModule), Vector3.zero, Quaternion.identity, cannon_left_rotation_point.transform);
            Destroy(cannon_left.FindChild("GameObject"));
            cannon_left.FindChild("model").FindChild("exosuit_rig_armLeft:exosuit_torpedoLauncher_geo").name = "cannon_model";

            cannon_left.CleanObject();
            cannon_left.name = "cannon_left";
            cannon_left.transform.localPosition = new Vector3(0.00f, 0.14f, -0.66f);
            cannon_left.transform.localRotation = Quaternion.Euler(0f, 270f, 180f);

            cannon_left.FindChild("model").FindChild("cannon_model").SetMeshMaterial(Main.cannon_material, 1);

            cannon_left_tube_right = Instantiate(laserBeam, Vector3.zero, Quaternion.identity, cannon_left.transform);
            cannon_left_tube_right.name = "cannon_left_tube_right";
            cannon_left_tube_right.transform.localPosition = new Vector3(0.68f, 0.0f, -0.17f);
            cannon_left_tube_right.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            left_right = cannon_left_tube_right.GetComponent<LineRenderer>();

            cannon_left_tube_left = Instantiate(laserBeam, Vector3.zero, Quaternion.identity, cannon_left.transform);
            cannon_left_tube_left.name = "cannon_left_tube_left";
            cannon_left_tube_left.transform.localPosition = new Vector3(0.68f, 0.0f, 0.17f);
            cannon_left_tube_left.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
            left_left = cannon_left_tube_left.GetComponent<LineRenderer>();

            Destroy(laserBeam);
        }

    }
}