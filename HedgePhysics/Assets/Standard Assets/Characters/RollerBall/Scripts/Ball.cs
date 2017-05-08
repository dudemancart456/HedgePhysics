using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Ball
{
    public class Ball : MonoBehaviour
    {
        public float m_MovePower = 5;
        public float m_JumpPower = 2; 

        private const float k_GroundRayLength = 1f; 
        private Rigidbody m_Rigidbody;

        public Vector3 Gravity;

        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }


        public void Move(Vector3 moveDirection, bool jump)
        {
      
            m_Rigidbody.AddForce(moveDirection*m_MovePower);

            if (Physics.Raycast(transform.position, -Vector3.up, k_GroundRayLength) && jump)
            {
                m_Rigidbody.AddForce(Vector3.up*m_JumpPower, ForceMode.Acceleration);
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawRay(DownRay);
        }
    }
}
