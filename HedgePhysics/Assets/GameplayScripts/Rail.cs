using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Rail : MonoBehaviour {

    public List<Vector3> RailPath;
    Vector3[] RailArray;
    public MeshFilter RailPathMesh;

    public PlayerBhysics Player;
    public float RailSpeed;


	void Start () {

        for (int i = 0; i < RailPathMesh.mesh.vertexCount; i++)
        {
            if (RailPathMesh.mesh.colors[i].r > 0.4f)
            {
                RailPath.Add(RailPathMesh.mesh.vertices[i]);
            }
        }
        RailArray = RailPath.ToArray();

    }

    void FixedUpdate()
    {

        RailSet();

    }
	
    public void RailSet()
    {

        Vector3 posToGo = transform.TransformDirection(GetClosestTarget(RailPath.ToArray(), 50));
        Player.rigidbody.velocity = (posToGo - Player.transform.position) * RailSpeed;

    }

    Vector3 GetClosestTarget(Vector3[] tgts, float maxDistance)
    {
        Vector3[] gos = tgts;
        Vector3 closest = Vector3.zero;
        float distance = maxDistance;
        Vector3 position = transform.position;
        foreach (Vector3 go in gos)
        {
            Vector3 diff = go - position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance)
            {
                float facingAmmount = Vector3.Dot(Player.rigidbody.velocity.normalized, go.normalized);
                if (facingAmmount > 0)
                {
                    closest = go;
                    distance = curDistance;
                }

            }
        }
        Debug.Log(closest);
        return closest;
    }

}
