using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class Grid : MonoBehaviour {

    public int xSize, ySize;

    public float scaleX = 1f;

    private float scaleY;

    private Vector3[] vertices;

    private Mesh  mesh;

    // Use this for initialization
    void Start()
    {
        // StartCoroutine (Generate());

        Generate();
    }


    private void Awake()
    {
        scaleY = Mathf.Sqrt(Mathf.Pow(scaleX, 2) - Mathf.Pow(scaleX / 2, 2));
    }


    private void   Generate()
    {
        WaitForSeconds Wait = new WaitForSeconds(0.05f);
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                var v1= new Vector3(x * scaleX,  y * scaleY);
                var v2= new Vector3(x * scaleX+scaleX /2,  y * scaleY);
                var uv1= new Vector2((float)x*scaleX / xSize, (float)y *scaleY/ ySize);
                var uv2 = new Vector2((float)x * scaleX / xSize, (float)(y * scaleY +scaleY /2)/ ySize);

                if (y % 2 == 0)
                {
                    //if (x < xSize)
                    {
                        vertices[i] = v2;
                        uv[i] = uv2;
                    }
                }
                else
                {
                    
                    vertices[i] = v1;
                    uv[i] = uv1;
                }

                tangents[i] = tangent;
            }
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;


        int[] triangles = new int[xSize *ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
         {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                if (y % 2 != 0)
                {
                    //if (x < xSize - 1)
                    {
                        triangles[ti] = vi;
                        triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                        triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                        triangles[ti + 5] = vi + xSize + 2;
                    }
                }
                else
                {
                    triangles[ti] = triangles[ti + 3] = vi;
                    triangles[ti + 1] = vi + xSize + 1;
                    triangles[ti + 2] = triangles[ti + 4] = vi + xSize + 2;
                    triangles[ti + 5] = vi + 1;
                }
            }
         }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        
        //yield return Wait;
        /*int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[3] = triangles[2] = 1;
        triangles[4] = triangles[1] = xSize + 1;
        triangles[5] = xSize + 2;*/
        /* mesh.triangles = triangles;
         yield return Wait;*/
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
           
        }
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }


	// Update is called once per frame
	void Update () {
		
	}
}
