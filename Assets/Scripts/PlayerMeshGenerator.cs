using System.Collections.Generic;
using UnityEngine;

public class PlayerMeshGenerator : MonoBehaviour
{
    //public PolygonCollider2D polyCollider;

    [Range(3, 10)] [SerializeField]
    public int sides = 4;

    [Range(.1f, 10f)] [SerializeField]
    private float radius = 5;
    
    void Update()
    {
        //polyCollider = GetComponent<PolygonCollider2D>();
        PolyMesh(radius, sides);
    }

    void PolyMesh(float rad, int n)
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        //verticies
        List<Vector3> verticiesList = new List<Vector3> { };
        List<Vector2> uvList = new List<Vector2>() { };
        float x;
        float y;
        for (int i = 0; i < n; i ++)
        {
            x = rad * Mathf.Sin((2 * Mathf.PI * i) / n);
            y = rad * Mathf.Cos((2 * Mathf.PI * i) / n);
            verticiesList.Add(new Vector3(x, y, 0f));
            uvList.Add(new Vector2(x, y));
        }
        Vector3[] verticies = verticiesList.ToArray();

        //triangles
        List<int> trianglesList = new List<int> { };
        for(int i = 0; i < (n-2); i++)
        {
            trianglesList.Add(0);
            trianglesList.Add(i+1);
            trianglesList.Add(i+2);
        }
        int[] triangles = trianglesList.ToArray();

        //normals
        List<Vector3> normalsList = new List<Vector3> { };
        for (int i = 0; i < verticies.Length; i++)
        {
            normalsList.Add(-Vector3.forward);
        }
        Vector3[] normals = normalsList.ToArray();

        var uv = uvList.ToArray();

        // uv[0] = new Vector2(0, 1);
        // uv[1] = new Vector2(1, 1);
        // uv[2] = new Vector2(0, 0);
        // uv[3] = new Vector2(1, 0);
        
        //initialise
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        for (var i = 0; i < uv.Length; i++)
        {
            //Go through the array
            //uv[i] += Vector2(.05, .09); //i can move them okay...
            var rot = Quaternion.Euler(0, 0, 45f);
            uv[i] = rot * uv[i];
        }
        mesh.uv = uv;
        //polyCollider

        List<Vector2> pathList = new List<Vector2> { };
        for (int i = 0; i < n; i++)
        {
            pathList.Add(new Vector2(verticies[i].x, verticies[i].y));
        }
        Vector2[] path = pathList.ToArray();

    }
}
