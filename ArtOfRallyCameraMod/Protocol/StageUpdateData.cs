using System;
// ReSharper disable InconsistentNaming

namespace ArtOfRallyChampionshipMod.Protocol
{
    [Serializable]
    public struct CarData
    {
        public int frame;
        public float[] position;
        public byte[] rotation;
        public float[] velocity;
        public sbyte throttleInput;
        public sbyte steeringInput;
        public sbyte brakeInput;
        public bool handbrakeInput;
        public byte gear;
        public bool resetCarThisFrame;
        public sbyte engineCondition;
        public sbyte dirtiness;

        public static CarData FromReplayKey(ReplayKey_Car data)
        {
            return new CarData
            {
                frame = data.frame,
                position = new []{ data.position.x, data.position.y, data.position.z },
                rotation = new []{ data.rotation.x, data.rotation.y, data.rotation.z},
                velocity = new []{data.velocity.x, data.velocity.y, data.velocity.z},
                throttleInput = data.throttleInput,
                steeringInput = data.steeringInput,
                brakeInput = data.brakeInput,
                handbrakeInput = data.handbrakeInput,
                gear = data.gear,
                resetCarThisFrame = data.resetCarThisFrame,
                engineCondition = data.engineCondition,
                dirtiness = data.dirtyness,
            };
        }
    }
    
    [Serializable]
    public struct StageUpdateData
    {
        public float time;
        public CarData? carData;
    }
}