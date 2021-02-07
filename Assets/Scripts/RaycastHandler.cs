using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Game
{
    public static class RaycastHandler
    {

        public static void StartHandle()
        {
            Physics2D.SyncTransforms();
            
        }

        public static void Shedule()
        {
            
        }

        public class MyJob : IJobParallelFor
        {
            public void Execute(int index)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}