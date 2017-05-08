using UnityEngine;
using System.Collections;

public class PlayerBinput : MonoBehaviour {

    private PlayerBhysics Player; // Reference to the ball controller.
    CameraControl Cam;
    ActionManager Actions;

    private Vector3 move;
    // the world-relative desired move direction, calculated from the camForward and user input.

    private Transform cam; // A reference to the main camera in the scenes transform
    private Vector3 camForward; // The current forward direction of the camera

    public AnimationCurve InputLerpingRateOverSpeed;
    public bool UtopiaTurning;
    public AnimationCurve UtopiaInputLerpingRateOverSpeed;
    public float InputLerpSpeed { get; set; }
    public Vector3 UtopiaInput { get; set; }
    public float UtopiaIntensity;
    public float UtopiaInitialInputLerpSpeed;
    public float UtopiaLerpingSpeed { get; set; }
    float InitialInputMag;
    float InitialLerpedInput;

    bool LockInput { get; set; }
    float LockedTime;
    Vector3 LockedInput;
    float LockedCounter = 0;
    bool LockCam { get; set; }
    public float prevDecel { get; set; }

    private void Awake()
    {
        // Set up the reference.
        Player = GetComponent<PlayerBhysics>();
        Actions = GetComponent<ActionManager>();
        Cam = GetComponent<CameraControl>();
        prevDecel = Player.MoveDecell;

        // get the transform of the main camera
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
    }

    private void Update()
    {
        // Get curve position

        InputLerpSpeed = InputLerpingRateOverSpeed.Evaluate((Player.rigidbody.velocity.sqrMagnitude / Player.MaxSpeed) / Player.MaxSpeed);
        UtopiaLerpingSpeed = UtopiaInputLerpingRateOverSpeed.Evaluate((Player.rigidbody.velocity.sqrMagnitude / Player.MaxSpeed) / Player.MaxSpeed);

        // Get the axis and jump input.

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate move direction
        if (cam != null)
        {

            Vector3 moveInp = new Vector3(h, 0, v);

            InitialInputMag = moveInp.sqrMagnitude;
            InitialLerpedInput = Mathf.Lerp(InitialLerpedInput, InitialInputMag, Time.deltaTime * UtopiaInitialInputLerpSpeed);

            if (!UtopiaTurning)
            {
                if (moveInp != Vector3.zero)
                {
                    Vector3 transformedInput = Quaternion.FromToRotation(cam.up, Player.GroundNormal) * (cam.rotation * moveInp);
                    transformedInput = transform.InverseTransformDirection(transformedInput);
                    transformedInput.y = 0.0f;
                    Player.RawInput = transformedInput;
                    moveInp = Vector3.Lerp(move, transformedInput, Time.deltaTime * InputLerpSpeed);
                }
                else
                {
                    Vector3 transformedInput = Quaternion.FromToRotation(cam.up, Player.GroundNormal) * (cam.rotation * moveInp);
                    transformedInput = transform.InverseTransformDirection(transformedInput);
                    transformedInput.y = 0.0f;
                    Player.RawInput = transformedInput;
                    moveInp = Vector3.Lerp(move, transformedInput, Time.deltaTime * (InputLerpSpeed * 10));
                }
            }
            else
            {
                if (moveInp != Vector3.zero)
                {
                    Vector3 transformedInput = Quaternion.FromToRotation(cam.up, Player.GroundNormal) * (cam.rotation * moveInp);
                    transformedInput = transform.InverseTransformDirection(transformedInput);
                    transformedInput.y = 0.0f;
                    Player.RawInput = transformedInput;
                    moveInp = Vector3.Lerp(move, transformedInput, Time.deltaTime * UtopiaLerpingSpeed);
                }
                else
                {
                    Vector3 transformedInput = Quaternion.FromToRotation(cam.up, Player.GroundNormal) * (cam.rotation * moveInp);
                    transformedInput = transform.InverseTransformDirection(transformedInput);
                    transformedInput.y = 0.0f;
                    Player.RawInput = transformedInput;
                    moveInp = Vector3.Lerp(move, transformedInput, Time.deltaTime * (UtopiaLerpingSpeed * 10));
                }
            }

            move = moveInp;

            if (UtopiaTurning)
            {
                if (moveInp != Vector3.zero)
                {
                    UtopiaInput = (moveInp * UtopiaIntensity) * InitialLerpedInput;
                    UtopiaInput = Vector3.ClampMagnitude(UtopiaInput, 1);
                }
                else
                {
                    UtopiaInput = Vector3.zero;
                }
                move = UtopiaInput;
            }

        }

        //Lock Input Funcion
        if (LockInput)
        {
            LockedInputFunction();
        }

    }

    void FixedUpdate()
    {

        Debug.DrawRay(transform.position, move, Color.cyan);
        Player.MoveInput = (move);

    }

    void LockedInputFunction()
    {
        move = Vector3.zero;
        LockedCounter += 1;
        Player.MoveDecell = 1;
        Player.b_normalSpeed = 0;

        if (LockCam)
        {
            Cam.Cam.FollowDirection(3, 14, -10,0);
        }

        if (Actions.Action != 0)
        {
            LockedCounter = LockedTime;
        }

        if (LockedCounter > LockedTime)
        {
            Player.MoveDecell = prevDecel;
            LockInput = false;
        }
    }

    public void LockInputForAWhile(float duration, bool lockCam)
    {
        LockedTime = duration;
        LockedCounter = 0;
        LockInput = true;
        LockCam = lockCam;
    }

}
