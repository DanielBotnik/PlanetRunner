using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlanet : MonoBehaviour
{

    public Material planetMaterial;

    private GameObject planet;
    private Mesh planetMesh;
    private Vector3[] planetVertices;
    private int[] planetTriangles;
    private MeshRenderer planetMeshRenderer;
    private MeshFilter planetMeshFilter;
    private MeshCollider planetMeshCollider;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material = planetMaterial;
        CreatePlanet();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePlanet()
    {
        CreatePlanetGameObject();
        RecalculateMesh();
    }

    private void CreatePlanetGameObject()
    {
        planet = new GameObject();
        planetMeshFilter = planet.AddComponent<MeshFilter>();
        planetMesh = planetMeshFilter.mesh;
        planetMeshRenderer = planet.AddComponent<MeshRenderer>();
        planetMeshRenderer.material = planetMaterial;
        IcoSphere.CreateIcoSphere(planet, 3, 1f);
    }

    private void RecalculateMesh()
    {
        planetMesh.RecalculateBounds();
        planetMesh.RecalculateTangents();
        planetMesh.RecalculateNormals();
    }
}
