using System;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Unity.Netcode;

public class WaveGeneratorNetworked : NetworkBehaviour
{
    [Header("Wave Parameters")]
    [SerializeField] public NetworkVariable<float> waveScale = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner);
    [SerializeField] public NetworkVariable<float> waveOffsetSpeed = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner);
    [SerializeField] public NetworkVariable<float> waveHeight = new NetworkVariable<float>(writePerm: NetworkVariableWritePermission.Owner);

    [Header("References and Prefabs")]
    [SerializeField] public MeshFilter waterMeshFilter;
    [SerializeField] private Mesh waterMesh;

    [SerializeField] private static readonly object padlock = new object();

    

    [SerializeField] NativeArray<Vector3> waterVertices;
    [SerializeField] NativeArray<Vector3> waterNormals;

    [SerializeField] JobHandle meshModificationJobHandle; // 1
    [SerializeField] UpdateMeshJob meshModificationJob; // 2

    [SerializeField] public static bool canRun = false;


    public float localWaveScale, localWaveOffsetSpeed, localWaveHeight = 1f;

    
    private struct UpdateMeshJob : IJobParallelFor
    {
        // 1
        public NativeArray<Vector3> vertices;

        // 2
        [ReadOnly]
        public NativeArray<Vector3> normals;

        // 3
        public float offsetSpeed;
        public float scale;
        public float height;

        // 4
        public float time;

        private float Noise(float x, float y)
        {
            float2 pos = math.float2(x, y);
            return noise.snoise(pos);
        }


        public void Execute(int i)
        {
            //lock(padlock)

            //Mutex mut = new Mutex();

            //if (mut.WaitOne(10))

            //{
                //try
                //{
                    // 1
                    if (normals[i].z > 0f)
                    {
                        // 2
                        var vertex = vertices[i];

                        // 3
                        float noiseValue =
                        Noise(vertex.x * scale + offsetSpeed * time, vertex.y * scale +
                        offsetSpeed * time);

                        // 4
                        vertices[i] =
                        new Vector3(vertex.x, vertex.y, noiseValue * height + 0.3f);
                    }
                //}

                //finally
                //{
                //    mut.ReleaseMutex();
                //}
                
            //} else
            //{
             //   print("Mutex has not been released in 1 sec");
            //}
                

        }

    }


    private void Awake()
    { 
        waterMeshFilter = this.gameObject.GetComponent<MeshFilter>();
        
        waterMesh = waterMeshFilter.mesh; 
    }

    private void Start()
    {
       

        waterMesh.MarkDynamic(); // 1

        waterVertices = 
        new NativeArray<Vector3>(waterMesh.vertices, Allocator.Persistent); // 2

        waterNormals = 
        new NativeArray<Vector3>(waterMesh.normals, Allocator.Persistent);

        
       

    }

    
    [BurstCompile]
    private void Update()
    {

        // 1
        meshModificationJob = new UpdateMeshJob()
        {
            vertices = waterVertices,
            normals = waterNormals,
            offsetSpeed = waveOffsetSpeed.Value,
            time = Time.time,
            scale = waveScale.Value,
            height = waveHeight.Value
        };

        // 2
        meshModificationJobHandle =
        meshModificationJob.Schedule(waterVertices.Length, 64);


       
        if (IsOwner)
        {
            
            waveScale.Value = localWaveScale;
            
            waveOffsetSpeed.Value = localWaveOffsetSpeed;
            waveHeight.Value = localWaveScale;

        }
        
        // 1
        meshModificationJobHandle.Complete();

        // 2
        waterMesh.SetVertices(meshModificationJob.vertices);

        // 3
        waterMesh.RecalculateNormals();



    }

    private void OnDestroy()
    {
        waterVertices.Dispose();
        waterNormals.Dispose();
    }

}