using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

    public Material mat;

    public Triangulator tr;



    void Start() {
        
    }

    List<Mesh> SubdivisionMesh(Mesh mesh)
    {
        List<Mesh> meshes = new List<Mesh>();

        // 厚みのある3角形ポリゴンメッシュへ変換
        int index = 0;
        int polygonCount = mesh.triangles.Length / 3;
        for (int n = 0; n < polygonCount; n++)
        {
            var p1 = mesh.vertices[mesh.triangles[n * 3 + 0]];
            var p2 = mesh.vertices[mesh.triangles[n * 3 + 1]];
            var p3 = mesh.vertices[mesh.triangles[n * 3 + 2]];
            var uv1 = mesh.uv[mesh.triangles[n * 3 + 0]];
            var uv2 = mesh.uv[mesh.triangles[n * 3 + 1]];
            var uv3 = mesh.uv[mesh.triangles[n * 3 + 2]];
            string name = "SubdivisionMesh_" + index.ToString();
            meshes.Add(GenTriangleMesh(name, p1, p2, p3, uv1, uv2, uv3));
            ++index;
        }
        return meshes;
    }

    Mesh GenTriangleMesh(string name, Vector3 p1, Vector3 p2, Vector3 p3, Vector2 uv1, Vector2 uv2, Vector2 uv3)
    {
        float z1 = 10.0f;
        float z2 = 10.1f;

        Vector3[] vtx = new Vector3[3];
        Vector2[] UV = new Vector2[6];

        // 頂点座標の指定.
        vtx[0] = new Vector3(p1.x, p1.z, z1);
        vtx[1] = new Vector3(p2.x, p2.z, z1);
        vtx[2] = new Vector3(p3.x, p3.z, z1);
        //vtx[3] = new Vector3(p1.x, p1.z, z2);
        //vtx[4] = new Vector3(p2.x, p2.z, z2);
        //vtx[5] = new Vector3(p3.x, p3.z, z2);

        // UVの指定
        UV[0] = new Vector2(uv1.x / Screen.width, uv1.y / Screen.height);
        UV[1] = new Vector2(uv2.x / Screen.width, uv2.y / Screen.height);
        UV[2] = new Vector2(uv3.x / Screen.width, uv3.y / Screen.height);
        //UV[3] = new Vector2(uv1.x / Screen.width, uv1.y / Screen.height);
        //UV[4] = new Vector2(uv2.x / Screen.width, uv2.y / Screen.height);
        //UV[5] = new Vector2(uv3.x / Screen.width, uv3.y / Screen.height);

        // 三角形ごとの頂点インデックスを指定.
        int[] idx = new int[8 * 3] {
      0,1,2,
      5,4,3,
      0,3,4,
      0,4,1,
      1,4,5,
      1,5,2,
      2,5,3,
      2,3,0
    };

        //Vector3[] dnml = new Vector3[idx.Length];
        Vector3[] dvtx = new Vector3[idx.Length];
        Vector2[] dUV = new Vector2[idx.Length];
        int[] didx = new int[8 * 3];
        int p = 0;
        for (int n = 0; n < 3; ++n)
        {

            dvtx[p] = vtx[idx[p]];
            dUV[p] = UV[idx[p]];
            didx[p] = p;

            //for (int m = 0; m < 3; m++)
            //{
            //    dvtx[p + m] = vtx[idx[p + m]];
            //    dUV[p + m] = UV[idx[p + m]];
            //    didx[p + m] = p + m;
            //}
            p++;
        }

        var mesh = new Mesh();
        mesh.vertices = dvtx;
        mesh.uv = dUV;
        mesh.triangles = didx;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    List<GameObject> CreateShatteredGameObject(List<Mesh> triangleMeshes)
    {
        List<GameObject> objects = new List<GameObject>();

        int index = 0;
        foreach (var mesh in triangleMeshes)
        {
            GameObject parts = new GameObject("ShatteredParts_" + index.ToString());
            parts.AddComponent<MeshFilter>();
            parts.AddComponent<MeshRenderer>();
            parts.AddComponent<MeshCollider>().convex = true;
            parts.GetComponent<MeshFilter>().sharedMesh = mesh;
            parts.GetComponent<MeshFilter>().sharedMesh.name = name;
            parts.GetComponent<MeshRenderer>().material = this.mat;
            parts.GetComponent<MeshCollider>().sharedMesh = mesh;
            parts.AddComponent<Rigidbody>();


            objects.Add(parts);
            ++index;
        }
        return objects;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButton(1)) {
            Explode();
        }
	
	}

    public void Explode() {
        Mesh mesh = new Mesh();
        //GetComponent<MeshFilter>().mesh = mesh;
        // 頂点の指定
        var vertices = new Vector2[] {
        new Vector2(0, 3),
        new Vector2(0, 2),
        new Vector2(0, 1),
        new Vector2(0, 0),
        new Vector2(3, 3),
        new Vector2(3, 2),
        new Vector2(3, 1),
        new Vector2(3, 0),
        new Vector2(2, 3),
        new Vector2(2, 2),
        new Vector2(2, 1),
        new Vector2(2, 0),
        new Vector2(1, 3),
        new Vector2(1, 2),
        new Vector2(1, 1),
        new Vector2(1, 0),

    };

        // メッシュをすべて分割
        var triangleMeshes = SubdivisionMesh(tr.CreateInfluencePolygon(vertices));

        // メッシュからGameObjectを生成
        var objs = CreateShatteredGameObject(triangleMeshes);

        this.gameObject.GetComponent<MeshRenderer>().enabled = false;

        foreach (var obj in objs)
        {
            var rigid = obj.GetComponent<Rigidbody>();
            var powerx = Random.Range(-100f, 100f);
            var powery = Random.Range(0f, 100f);
            var powerz = Random.Range(-100f, 100f);
            rigid.AddForce(new Vector3(powerx, powery, powerz));
        }
        

        //mesh.RecalculateNormals();
        //mesh.RecalculateBounds();

        //var filter = s.GetComponent<MeshFilter>();
        //filter.sharedMesh = mesh;
    }
}
