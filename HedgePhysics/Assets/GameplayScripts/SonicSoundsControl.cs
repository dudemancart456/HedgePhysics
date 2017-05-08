using UnityEngine;
using System.Collections;

public class SonicSoundsControl : MonoBehaviour {

    public PlayerBhysics Player;

    AudioSource Source;
    public AudioSource Source2;
    public AudioSource Source3;
    public AudioClip[] FootSteps;
    public AudioClip Jumping;
    public AudioClip AirDash;
    public AudioClip HomingAttack;
    public AudioClip Skidding;
    public AudioClip Spin;
    public AudioClip SpinDash;
    public AudioClip SpinDashRelease;
    public AudioClip RingLoss;
    public AudioClip Die;
    public AudioClip Spiked;

    public float pitchBendingRate = 1;

    public void Test(string i)
    {

    }

    void Start()
    {
        Source = GetComponent<AudioSource>();
    }

    public void FootStepSoundPlay()
    {
        int rand = Random.Range(0, FootSteps.Length);
        Source.clip = FootSteps[rand];
        Source.Play();
    }
    public void JumpSound()
    {
        Source2.clip = Jumping;
        Source2.Play();
    }
    public void SkiddingSound()
    {
        Source2.clip = Skidding;
        Source2.Play();
    }
    public void HomingAttackSound()
    {
        Source2.clip = HomingAttack;
        Source2.Play();
    }
    public void AirDashSound()
    {
        Source2.clip = AirDash;
        Source2.Play();
    }
    public void SpinningSound()
    {
        Source2.clip = Spin;
        Source2.Play();
    }
    public void SpinDashSound()
    {
        Source2.clip = SpinDash;
        Source2.Play();
    }
    public void SpinDashReleaseSound()
    {
        Source2.clip = SpinDashRelease;
        Source2.Play();
    }
    public void RingLossSound()
    {
        Source3.clip = RingLoss;
        Source3.Play();
    }
    public void DieSound()
    {
        Source3.clip = Die;
        Source3.Play();
    }
    public void SpikedSound()
    {
        Source3.clip = Spiked;
        Source3.Play();
    }

}
