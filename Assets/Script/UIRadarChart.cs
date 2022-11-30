using UnityEngine;


public class UIRadarChart : MonoBehaviour {

    public int[] stats; // The values of the stats that we want to display.
    public float unitsPerStat = 2f; // Size of the graph. Increase to make mesh bigger.

    public Color color = new Color(1, 0.8f, 0.34f, 0.7f); // Colour of the mesh.
    CanvasRenderer canvasRenderer; // Dynamically-generated mesh renderer.
    Material material; // Dynamically-generated material for the mesh.

    void GenerateMesh() {
        Mesh mesh = new Mesh();

        // Don't draw mesh if there are less than 3 stats.
        int len = stats.Length;
        if(len < 3) {
            Debug.LogWarning("Unable to draw mesh as there are only 2 stats.");
            return;
        }

        // Create the vertices.
        Vector3[] vertices = new Vector3[len];

        // Count the number of triangles we need, and generate the array.
        int triangleCount = len - 2;
        int[] triangles = new int[triangleCount * 3];

        // Calculate the angle between each stat.
        float angleIncrement = 360f / len;
        
        // Set the verticles of each stat.
        for(int i = 0; i < len; i++) {
            vertices[i] = Quaternion.Euler(0, 0, angleIncrement * i) * Vector3.up * unitsPerStat * stats[i];
        }

        // Draws the triangles.
        for(int i = 0; i < triangleCount; i++) {
            int start = i * 3;
            triangles[start] = 0;
            triangles[start+1] = 1+i;
            triangles[start+2] = 2+i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        canvasRenderer.SetMesh(mesh);
        canvasRenderer.SetMaterial(material, null);
        canvasRenderer.SetColor(color);

    }

    void Start() {
        // Generates the material for the mesh.
        material = new Material(Shader.Find("UI/Default"));

        // Create the GameObject containing the mesh renderer.
        GenerateCanvasRenderer();

    }

    private void Update()
    {
        // Create the mesh.
        GenerateMesh();
    }

    // Dynamically generates a child GameObject on where this script is attached to,
    // then create a CanvasRenderer component for it (which we will use to draw the mesh).
    void GenerateCanvasRenderer() {
        GameObject go = new GameObject("PolygonMesh");
        RectTransform r = go.AddComponent<RectTransform>();
        canvasRenderer = go.AddComponent<CanvasRenderer>();
        r.SetParent(transform);
        r.localPosition = Vector3.zero;
    }
}