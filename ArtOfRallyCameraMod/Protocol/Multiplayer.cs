using System;
using UnityEngine;

namespace ArtOfRallyChampionshipMod.Protocol
{
    [Serializable]
    public struct Vec3
    {
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public struct Quat
    {
        public float x;
        public float y;
        public float z;
        public float w;
    }

    public struct NativeMultiplayerCar
    {
        public Vector3 position;
        public Quaternion rotation;
        public float time;
    }
    
    [Serializable]
    public struct MultiplayerCar
    {
        public Vec3 position;
        public Quat rotation;
        public float time;

        public static MultiplayerCar FromBody(Rigidbody body, float time)
        {
            var pos = body.position;
            var rot = body.rotation;
            return new MultiplayerCar
            {
                position = new Vec3
                {
                    x = pos.x,
                    y = pos.y,
                    z = pos.z
                },
                rotation = new Quat
                {
                    x = rot.x,
                    y = rot.y,
                    z = rot.z,
                    w = rot.w,
                },
                time = time,
            };
        }

        public NativeMultiplayerCar ToNative()
        {
            return new NativeMultiplayerCar
            {
                position = new Vector3(position.x, position.y, position.z),
                rotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w),
                time = time,
            };
        }
    }
    
    public struct MultiplayerLiveData
    {
        public float Progress;
        public MultiplayerCar CurrentData;
        public MultiplayerCar LastData;
    }
}