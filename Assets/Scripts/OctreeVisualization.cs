using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeVisualization : MonoBehaviour {
    public GameObject voxelPrefab;
    public float worldSize = 15f;
    public float voxelSize = 1f;
    public int initialVoxels = 4;
    public int maxVoxels = 20;
    public float timeBetweenVoxels = 1.5f;
    public int seed = 0;

    public CameraControl camController;

    PointOctree<GameObject> pointTree;
    float lastVoxelTime;
    int voxelCount;
    float operatingWorldSize = 15f;

    void Awake () {
        voxelCount = 0;
        lastVoxelTime = 0f;

        Random.InitState(seed); 

        // Initial size (metres), initial centre position, minimum node size (metres)
        pointTree = new PointOctree<GameObject>(worldSize, transform.position, voxelSize);

        for (int i = 0; i < initialVoxels; i++)
        {
            AddVoxel();
        }

        operatingWorldSize = worldSize * 7f;
    }

    void AddVoxel()
    {
        float minX = transform.position.x - operatingWorldSize / 2f;
        float maxX = minX + worldSize;
        float minY = transform.position.y - operatingWorldSize / 2f;
        float maxY = minY + worldSize;
        float minZ = transform.position.z - operatingWorldSize / 2f;
        float maxZ = minZ + worldSize;

        Vector3 point = new Vector3(
            Mathf.Lerp(minX, maxX, Mathf.Log(Random.Range(1f, 1000f))/3f),
            Mathf.Lerp(minY, maxY, Mathf.Log(Random.Range(1f, 1000f))/3f),
            Mathf.Lerp(minZ, maxZ, Mathf.Log(Random.Range(1f, 1000f))/3f)
        );

        var obj = Instantiate(voxelPrefab, point, Quaternion.identity, transform);

        pointTree.Add(obj, point);

        voxelCount++;

        camController.AddVisibleTarget(obj.transform);
    }

    // Update is called once per frame
    void Update () {
        if (voxelCount < maxVoxels)
        {
            lastVoxelTime += Time.deltaTime;

            if (lastVoxelTime > timeBetweenVoxels)
            {
                lastVoxelTime = 0f;

                AddVoxel();
            }
        }
	}

    void OnDrawGizmos()
    {
        /*
        boundsTree.DrawAllBounds(); // Draw node boundaries
        boundsTree.DrawAllObjects(); // Draw object boundaries
        boundsTree.DrawCollisionChecks(); // Draw the last *numCollisionsToSave* collision check boundaries
        */

        pointTree.DrawAllBounds(); // Draw node boundaries
        pointTree.DrawAllObjects(); // Mark object positions
    }
}
