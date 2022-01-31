using System;
using UnityEngine;

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
        public float[] position;
        public float[] rotation;
        public float[] velocity;
        public float throttleInput;
        public float steeringInput;
        public float brakeInput;
        public float handbrakeInput;
        public float clutchInput;
        public bool absTriggered;
        public bool tcsTriggered;
        public bool espTriggered;
        public DrivetrainData drivetrain;

        public static CarData FromCarController(CarController data, Drivetrain drivetrain, Rigidbody body)
        {
            var position1 = body.position;
            var rotation1 = body.rotation.eulerAngles;
            var velocity1 = body.velocity;
            return new CarData
            {
                position = new[] { position1.x, position1.y, position1.z },
                rotation = new[] { rotation1.x, rotation1.y, rotation1.z },
                velocity = new[] { velocity1.x, velocity1.y, velocity1.z },
                throttleInput = data.throttleInput,
                steeringInput = data.steerInput,
                brakeInput = data.brakeInput,
                handbrakeInput = data.handbrakeInput,
                clutchInput = data.clutchInput,
                absTriggered = data.ABSTriggered,
                tcsTriggered = data.TCSTriggered,
                espTriggered = data.ESPTriggered,
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