// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GenerateCube : MonoBehaviour
{
    [SerializeField] private Shader shader;
    [SerializeField] private PointLight pointLight;

    private MeshRenderer _renderer;

    private void Start()
    {
        // Generate the mesh and assign to the mesh filter.
        GetComponent<MeshFilter>().mesh = CreateMesh();
        
        // Store renderer reference
        this._renderer = gameObject.GetComponent<MeshRenderer>();

        // Assign custom shader
        this._renderer.material.shader = this.shader;
    }
    
    private void Update()
    {
        // Pass updated light colour to shader
        this._renderer.material.SetColor("_PointLightColor", 
            this.pointLight.GetColor());
        
        // Pass updated light position to shader
        this._renderer.material.SetVector("_PointLightPosition",
            this.pointLight.GetWorldPosition());
    }

    private Mesh CreateMesh()
    {
        // Our beloved cube is back...
        var mesh = new Mesh
        {
            name = "Cube"
        };
       

        // Define the vertex positions (same as workshop 2).
        mesh.SetVertices(new[]
        {
            // Top face
            new Vector3(-1.0f, 1.0f, -1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),

            new Vector3(-1.0f, 1.0f, -1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, -1.0f),

            // Bottom face
            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3(1.0f, -1.0f, 1.0f),
            new Vector3(-1.0f, -1.0f, 1.0f),

            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3(1.0f, -1.0f, -1.0f),
            new Vector3(1.0f, -1.0f, 1.0f),

            // Left face
            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3(-1.0f, -1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f),

            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, -1.0f),

            // Right face
            new Vector3(1.0f, -1.0f, -1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, -1.0f, 1.0f),

            new Vector3(1.0f, -1.0f, -1.0f),
            new Vector3(1.0f, 1.0f, -1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),

            // Front face
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, -1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),

            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, -1.0f, 1.0f),
            new Vector3(1.0f, -1.0f, 1.0f),

            // Back face
            new Vector3(-1.0f, 1.0f, -1.0f),
            new Vector3(1.0f, 1.0f, -1.0f),
            new Vector3(1.0f, -1.0f, -1.0f),

            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3(-1.0f, 1.0f, -1.0f),
            new Vector3(1.0f, -1.0f, -1.0f)
        });

        // Define the vertex colours (same as workshop 2).
        mesh.SetColors(new[]
        {
            // Top face
            Color.red,
            Color.red,
            Color.red,

            Color.red,
            Color.red,
            Color.red,

            // Bottom face
            Color.red,
            Color.red,
            Color.red,

            Color.red,
            Color.red,
            Color.red,

            // Left face
            Color.yellow,
            Color.yellow,
            Color.yellow,

            Color.yellow,
            Color.yellow,
            Color.yellow,

            // Right face
            Color.yellow,
            Color.yellow,
            Color.yellow,

            Color.yellow,
            Color.yellow,
            Color.yellow,

            // Front face
            Color.blue,
            Color.blue,
            Color.blue,

            Color.blue,
            Color.blue,
            Color.blue,

            // Back face
            Color.blue,
            Color.blue,
            Color.blue,

            Color.blue,
            Color.blue,
            Color.blue
        });

        // Task 1: Define the correct normals (as unit vectors; currently they're all "zero")
        List<Vector3> normals = calculateNormals(mesh);
        var topNormal = normals[0];
        var bottomNormal = normals[1];
        var leftNormal = normals[2];
        var rightNormal = normals[3];
        var frontNormal = normals[4];
        var backNormal = normals[5];

        mesh.SetNormals(new[]
        {
            topNormal, // Top
            topNormal,
            topNormal,
            topNormal,
            topNormal,
            topNormal,

            bottomNormal, // Bottom
            bottomNormal,
            bottomNormal,
            bottomNormal,
            bottomNormal,
            bottomNormal,

            leftNormal, // Left
            leftNormal,
            leftNormal,
            leftNormal,
            leftNormal,
            leftNormal,

            rightNormal, // Right
            rightNormal,
            rightNormal,
            rightNormal,
            rightNormal,
            rightNormal,

            frontNormal, // Front
            frontNormal,
            frontNormal,
            frontNormal,
            frontNormal,
            frontNormal,

            backNormal, // Back
            backNormal,
            backNormal,
            backNormal,
            backNormal,
            backNormal
        });

        // Define the indices (same as workshop 2).
        var indices = Enumerable.Range(0, mesh.vertices.Length).ToArray();
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);

        return mesh;
    }
    private List<Vector3>calculateNormals(Mesh mesh)
    {    
        List<Vector3> normals = new List<Vector3>();
        
        for ( int i =0; i<mesh.vertices.Length; i += 6)
        {
            normals.Add(GetNormal(mesh.vertices[i], mesh.vertices[i + 1], mesh.vertices[i + 2]));
        }

        return normals;        
    }
    private Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        // Find vectors corresponding to two of the sides of the triangle.
        Vector3 side1 = b - a;
        Vector3 side2 = c - a;

        // Cross the vectors to get a perpendicular vector, then normalize it.
        return Vector3.Cross(side1, side2).normalized;
    }
}
