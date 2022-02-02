using System;
using System.Reflection;
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
    public struct BrakeData
    {
        public float temperatureFront;
        public float temperatureBack;

        [NonSerialized]
        public static FieldInfo? BrakeTemperatureFrontFieldInfo =
            typeof(BrakeEffects).GetField("BrakeTemperatureFront", BindingFlags.NonPublic);
        [NonSerialized]
        public static FieldInfo? BrakeTemperatureBackFieldInfo =
            typeof(BrakeEffects).GetField("BrakeTemperatureBack", BindingFlags.NonPublic);

        public static BrakeData FromBrakeEffects(BrakeEffects brakeEffects)
        {
            return new BrakeData
            {
                temperatureBack = (float)BrakeTemperatureBackFieldInfo?.GetValue(brakeEffects)!,
                temperatureFront = (float)BrakeTemperatureFrontFieldInfo?.GetValue(brakeEffects)!
            };
        }
    }

    [Serializable]
    public struct WingData
    {
        public float downForce;
        public float dragForce;

        public static WingData FromWing(Wing wing)
        {
            return new WingData
            {
                downForce = wing.downForce,
                dragForce = wing.dragForce
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
        public BrakeData brakeData;
        public DrivetrainData drivetrain;

        public static CarData FromCarController(CarDynamics data, Drivetrain drivetrain, Rigidbody body)
        {
            var position1 = body.position;
            var rotation1 = body.rotation.eulerAngles;
            var velocity1 = body.velocity;
            return new CarData
            {
                position = new[] { position1.x, position1.y, position1.z },
                rotation = new[] { rotation1.x, rotation1.y, rotation1.z },
                velocity = new[] { velocity1.x, velocity1.y, velocity1.z },
                throttleInput = data.carController.throttleInput,
                steeringInput = data.carController.steerInput,
                brakeInput = data.carController.brakeInput,
                handbrakeInput = data.carController.handbrakeInput,
                clutchInput = data.carController.clutchInput,
                absTriggered = data.carController.ABSTriggered,
                tcsTriggered = data.carController.TCSTriggered,
                espTriggered = data.carController.ESPTriggered,
                brakeData = BrakeData.FromBrakeEffects(data.brakeEffects),
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