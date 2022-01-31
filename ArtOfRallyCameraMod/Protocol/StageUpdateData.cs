using System;

// ReSharper disable InconsistentNaming

namespace ArtOfRallyChampionshipMod.Protocol
{
    [Serializable]
    public struct DrivetrainData
    {
        public float clutch;
        public float rpm;
        public float torque;
        public int gear;
        public float wheelTireVelocity;
        public bool canStall;
        public float throttle;
        public bool shiftTriggered;
        public bool canShiftAgain;
        public float currentPower;
        public float currentGearRatio;
        public bool isChangingGear;
        public float velocity;
        public bool isStalling;

        public static DrivetrainData FromDrivetrain(Drivetrain drivetrain)
        {
            return new DrivetrainData
            {
                wheelTireVelocity = drivetrain.wheelTireVelo,
                velocity = drivetrain.velo,
                isStalling = drivetrain.StallNow,
                currentPower = drivetrain.currentPower,
                isChangingGear = drivetrain.changingGear,
                currentGearRatio = drivetrain.ratio,
                shiftTriggered = drivetrain.shiftTriggered,
                canShiftAgain = drivetrain.CanShiftAgain,
                throttle = drivetrain.throttle,
                canStall = drivetrain.canStall,
                rpm = drivetrain.rpm,
                torque = drivetrain.torque,
                gear = drivetrain.gear,
                clutch = drivetrain.clutch.GetClutchPosition(),
            };
        }
    }

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
        public DrivetrainData drivetrain;

        public static CarData FromReplayKey(
            ReplayKey_Car data,
            Drivetrain drivetrain
        )
        {
            return new CarData
            {
                frame = data.frame,
                position = new[] { data.position.x, data.position.y, data.position.z },
                rotation = new[] { data.rotation.x, data.rotation.y, data.rotation.z },
                velocity = new[] { data.velocity.x, data.velocity.y, data.velocity.z },
                throttleInput = data.throttleInput,
                steeringInput = data.steeringInput,
                brakeInput = data.brakeInput,
                handbrakeInput = data.handbrakeInput,
                gear = data.gear,
                resetCarThisFrame = data.resetCarThisFrame,
                engineCondition = data.engineCondition,
                dirtiness = data.dirtyness,
                drivetrain = DrivetrainData.FromDrivetrain(drivetrain),
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