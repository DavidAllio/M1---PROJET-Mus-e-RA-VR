using HullDelaunayVoronoi.Hull;
using HullDelaunayVoronoi.Primitives;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class InitPart : MonoBehaviour
{
    public Text DebugT;
    public Material selectionMat;
    public Material invMat;
    public GameObject targ;

    private ConvexHull3 hull;
    private List<Vertex3> vertices;

    public void setVisible(MeshRenderer mre)
    {
        mre.sharedMaterial = selectionMat;
    }

    public void setHide(MeshRenderer mre)
    {
        mre.sharedMaterial = invMat;
    }

    // Start is called before the first frame update
    void Start()
    {

        string[] parts = { "mask_048_0", "mask_048_1", "mask_048_2", "mask_048_3", "mask_048_4", "mask_048_5", "mask_048_6" };
        GameObject statueAsset = Resources.Load<GameObject>("siva/siva");
        GameObject statue = Instantiate(statueAsset, new Vector3(0, 0, 0), Quaternion.identity);
        statue.name = "siva";

        //string[] parts = { "part1", "part2", "part3" };

        for (int i = 0; i < parts.Length; i++)
        {
            //Lecture du fichier de coordonnées dans un tableau de vertices

            TextAsset txtAssets = (TextAsset)Resources.Load("siva/"+parts[i]);
            //TextAsset txtAssets = (TextAsset)Resources.Load("myson/maks3D_Myson_0" + i);
            string fileContent = txtAssets.text;

            readXYZ(fileContent);

            //génération de l'enveloppe convexe
            hull = new ConvexHull3();
            hull.Generate(vertices);

            //Conversion de l'enveloppe convexe en mesh
            Mesh mesh = HullToMesh();


            //Création d'un objet qui va contenir le meshcollider
            GameObject partObject = new GameObject(parts[i]);

            //Création du meshcollider à partir du mesh créé précedemment
            MeshCollider meshCollider = partObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;

            MeshFilter meshFilter = partObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;

            MeshRenderer meshRenderer = partObject.AddComponent<MeshRenderer>();
            Material mat = new Material(invMat);
            meshRenderer.sharedMaterial = mat;
            //meshRenderer.enabled = false;

            partObject.transform.SetParent(statue.transform,true);
            
            
            Debug.Log("fils après2: p=" + partObject.transform.position + " lp=" + partObject.transform.localPosition + " r=" + partObject.transform.rotation + " lr=" + partObject.transform.localRotation);




        }

        //pour debug sans android, décommenter
        //MeshRenderer mrd = transform.gameObject.GetComponent<MeshRenderer>();
        //mrd.enabled = true;
        statue.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        statue.transform.SetParent(targ.transform);
        DebugT.text = "Sélectionnez une zone";

    }

    private Mesh HullToMesh()
    {
        List<Vector3> positions = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> indices = new List<int>();

        for (int i = 0; i < hull.Simplexs.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector3 v = new Vector3();
                
                v.x = hull.Simplexs[i].Vertices[j].X;
                v.y = hull.Simplexs[i].Vertices[j].Y;
                v.z = hull.Simplexs[i].Vertices[j].Z;

                positions.Add(v);
            }

            Vector3 n = new Vector3();
            n.x = hull.Simplexs[i].Normal[0];
            n.y = hull.Simplexs[i].Normal[1];
            n.z = hull.Simplexs[i].Normal[2];

            if (hull.Simplexs[i].IsNormalFlipped)
            {
                indices.Add(i * 3 + 2);
                indices.Add(i * 3 + 1);
                indices.Add(i * 3 + 0);
            }
            else
            {
                indices.Add(i * 3 + 0);
                indices.Add(i * 3 + 1);
                indices.Add(i * 3 + 2);
            }

            normals.Add(n);
            normals.Add(n);
            normals.Add(n);
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(positions);
        mesh.SetNormals(normals);
        mesh.SetTriangles(indices, 0);

        mesh.RecalculateBounds();
        return mesh;
    }

    private void readXYZ(string fileContent)
    {
        string[] linesFromfile = fileContent.Split('\n');
        string[] cutted_str;
        List<Vertex3> tmp = new List<Vertex3>();
        vertices = new List<Vertex3>();

        int ind = 0;


        foreach (string reader in linesFromfile)
        {
            if (reader != "")
            {
                cutted_str = reader.Split(' ');
                float x = float.Parse(cutted_str[0], CultureInfo.InvariantCulture);
                float y = float.Parse(cutted_str[1], CultureInfo.InvariantCulture);
                float z = float.Parse(cutted_str[2], CultureInfo.InvariantCulture);
                tmp.Add(new Vertex3(x, y, z));
            }
            ind++;
        }

        //brassage des points
        while (tmp.Count>0){
            int i = Random.Range(0, tmp.Count);
            vertices.Add(tmp[i]);
            tmp.RemoveAt(i);
        }
    }
}
