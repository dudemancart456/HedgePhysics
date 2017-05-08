using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HurtControl : MonoBehaviour {

    ActionManager Actions;
    LevelProgressControl Level;
    PlayerBhysics Player;
    Objects_Interaction Objects;
    CameraControl Cam;

    public int InvencibilityTime;
    int counter;
    public bool IsHurt { get; set; }
    public bool IsInvencible { get; set; }
    float flickerCount;
    public float FlickerSpeed;

    public SkinnedMeshRenderer[] SonicSkins;

    public GameObject MovingRing;
    GameObject releaseDirection;
    public int MaxRingLoss;
    public float RingReleaseSpeed;
    public float RingArcSpeed;
    bool releasingRings = false;
    int RingsToRelease;

    public bool isDead { get; set; }
    int deadCounter = 0;

    public Image FadeOutImage;

	void Awake () {

        Player = GetComponent<PlayerBhysics>();
        Level = GetComponent<LevelProgressControl>();
        counter = InvencibilityTime;
        Actions = GetComponent<ActionManager>();
        Objects = GetComponent<Objects_Interaction>();
        releaseDirection = new GameObject();
        Cam = GetComponent<CameraControl>();

    }
	
	void FixedUpdate () {

        counter += 1;
        if(counter < InvencibilityTime)
        {
            IsInvencible = true;
            SkinFlicker();
        }
        else
        {
            IsInvencible = false;
            IsHurt = false;
            ToggleSkin(true);
        }

        if (releasingRings)
        {
            if(RingsToRelease > 30) { RingsToRelease = 30; }
            RingLoss();
        }

        //IsDead things
        if(isDead == true)
        {
            Death();
        }
        else
        {
            Color alpha = Color.black;
            alpha.a = 0;
            FadeOutImage.color = Color.Lerp(FadeOutImage.color, alpha, Time.fixedTime * 3);
        }
	}

    void Death()
    {
        Actions.ChangeAction(4);
        Player.MoveInput = Vector3.zero;
        deadCounter += 1;

        if(deadCounter > 80)
        {
            Color alpha = Color.black;
            FadeOutImage.color = Color.Lerp(FadeOutImage.color, alpha, Time.fixedTime * 1);
        }
        if (deadCounter > 170)
        {
            Actions.Action04.CharacterAnimator.SetBool("Dead", false);
            Level.ResetToCheckPoint();
            isDead = false;
            deadCounter = 0;
        }
    }

    void SkinFlicker()
    {
        flickerCount += FlickerSpeed;
        if(flickerCount < 0)
        {
            ToggleSkin(false);
        }
        else
        {
            ToggleSkin(true);
        }
        if(flickerCount > 10)
        {
            flickerCount = -10;
        }
    }

    void RingLoss()
    {
        Objects_Interaction.RingAmmount = 0;

        if(RingsToRelease > 0)
        {
            Vector3 pos = transform.position;
            pos.y += 1;
            GameObject movingRing = (GameObject)Instantiate(MovingRing, pos, Quaternion.identity);
            movingRing.GetComponent<Rigidbody>().velocity = Vector3.zero;
            movingRing.GetComponent<Rigidbody>().AddForce((releaseDirection.transform.forward * RingReleaseSpeed), ForceMode.Acceleration);
            releaseDirection.transform.Rotate(0, RingArcSpeed, 0);
            RingsToRelease -= 1;
        }
        else
        {
            releasingRings = false;
        }
    }

    public void ToggleSkin(bool on)
    {
        for (int i = 0; i < SonicSkins.Length; i++)
        {
            SonicSkins[i].enabled = on;
        }
    }

    public void GetHurt()
    {
        IsHurt = true;
        counter = 0;

        if(Objects_Interaction.RingAmmount > 0 && !releasingRings)
        {
            RingsToRelease = Objects_Interaction.RingAmmount;
            releasingRings = true;
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.tag == "Pit")
        {
            Cam.Cam.LagCamera(0, 0);
        }
    }
    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Pit")
        {
            Objects.Sounds.DieSound();
            isDead = true;
        }
    }
}
