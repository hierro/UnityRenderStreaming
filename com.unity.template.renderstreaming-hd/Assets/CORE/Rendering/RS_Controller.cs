
using UnityEngine;

using UnityEngine.InputSystem;
using Unity.RenderStreaming;
using UnityEngine.UI;

using Cinemachine;

using Sirenix.OdinInspector;

using CORE.UTILS;


namespace CORE.Rendering {

    public class RS_Controller : MonoBehaviour
    {

        #region PUBLIC

        [FoldoutGroup("CAMERA", Expanded = true)]


        [FoldoutGroup("CAMERA/Movement_In")]
        [SerializeField, MinMaxSlider(-1, 1)]
        public Vector2 CameraMoveDeltaIn;
        [FoldoutGroup("CAMERA/Movement_In")]
        [SerializeField, MinMaxSlider(-1, 1)]
        public Vector2 CameraMoveDeltaVectorIn;
        [FoldoutGroup("CAMERA/Movement_In")]
        [SerializeField]
        public AnimationCurve CameraDeltaAnimationCurve = new AnimationCurve();


        [FoldoutGroup("CAMERA/Movement_Out")]
        [ShowInInspector]
        [DisableInEditorMode, DisableInPlayMode]
        private Vector2 PointerDelta;

        [FoldoutGroup("CAMERA/Movement_Out")]
        [SerializeField] [DisableInEditorMode, DisableInPlayMode]
        public float CameraMoveDelta;
        [FoldoutGroup("CAMERA/Movement_Out")]
        [SerializeField]
        [DisableInEditorMode, DisableInPlayMode]
        public Vector3 CameraMoveDeltaVector;


        [FoldoutGroup("DEBUG")]
        public Text DebugText;

        #endregion

        #region PRIVATE



        private bool Lockon;

        #endregion

        #region SETUP


        private void Awake()
        {
            CinemachineCore.GetInputAxis = GetAxisCustom;
        }


        void Start()
        {

        }

        virtual public void Init()
        {




        }


        #endregion

        #region METHODS

        static bool IsMouseDragged(Mouse m, bool useLeftButton)
        {
            if (null == m)
                return false;

            if (Screen.safeArea.Contains(m.position.ReadValue()))
            {
                //check left/right click
                if ((useLeftButton && m.leftButton.isPressed) || (!useLeftButton && m.rightButton.isPressed))
                {
                    return true;
                }
            }

            return false;
        }
        public float GetAxisCustom(string axisName)
        {

            if ( IsMouseDragged(Mouse.current, true))

            {

                PointerDelta = Mouse.current.delta.ReadValue();
                PointerDelta.Normalize();

                if (axisName == "Mouse X")
                {
                    return PointerDelta.x;
                }
                else if (axisName == "Mouse Y")
                {
                    return PointerDelta.y;
                }

            }
            return 0;
        }


        #endregion

        #region DEBUG

        virtual public void PrintDebug(string message)
        {

            if (DebugText != null)
            {
                DebugText.text = message;
            }
        }

        #endregion

        #region UPDATE

        virtual public void Update()
        {

        }

        #endregion

    }
}

