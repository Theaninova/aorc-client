using System;
using System.Reflection;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace ArtOfRallyChampionshipMod.Protocol
{
    [Serializable]
    public struct AssistanceData
    {
        public float absTriggered;
        public float tcsTriggered;
        public float espTriggered;

        public static AssistanceData? FromController(CarController? controller, AssistanceData? last)
        {
            if (controller == null) return null;
            const float decay = 0.1f;
            var decayDelta = Time.deltaTime / decay;

            return new AssistanceData
            {
                absTriggered = Math.Max(controller.ABSTriggered ? 1f : 0f, (last?.absTriggered ?? 0f) - decayDelta),
                tcsTriggered = Math.Max(controller.TCSTriggered ? 1f : 0f, (last?.tcsTriggered ?? 0f) - decayDelta),
                espTriggered = Math.Max(controller.ESPTriggered ? 1f : 0f, (last?.espTriggered ?? 0f) - decayDelta)
            };
        }
    }

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

        public static DrivetrainData? FromDrivetrain(Drivetrain? drivetrain)
        {
            return drivetrain == null
                ? (DrivetrainData?)null
                : new DrivetrainData
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
    public struct InputData
    {
        public float throttleInput;
        public float steeringInput;
        public float brakeInput;
        public float handbrakeInput;
        public float clutchInput;

        public static InputData? FromCarController(CarController? controller)
        {
            return controller == null
                ? (InputData?)null
                : new InputData
                {
                    throttleInput = controller.throttleInput,
                    steeringInput = controller.steerInput,
                    brakeInput = controller.brakeInput,
                    handbrakeInput = controller.handbrakeInput,
                    clutchInput = controller.clutchInput,
                };
        }
    }

    [Serializable]
    public struct BrakeData
    {
        public float temperatureFront;
        public float temperatureBack;

        [NonSerialized] public static FieldInfo? BrakeTemperatureFrontFieldInfo =
            typeof(BrakeEffects).GetField("BrakeTemperatureFront",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        [NonSerialized] public static FieldInfo? BrakeTemperatureBackFieldInfo =
            typeof(BrakeEffects).GetField("BrakeTemperatureRear",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        public static BrakeData? FromBrakeEffects(BrakeEffects? brakeEffects)
        {
            return brakeEffects == null
                ? (BrakeData?)null
                : new BrakeData
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
    public struct PositionData
    {
        public float[] position;
        public float[] rotation;
        public float[] velocity;

        public static PositionData? FromRigidBody(Rigidbody? body)
        {
            if (body == null) return null;
            var position1 = body.position;
            var rotation1 = body.rotation.eulerAngles;
            var velocity1 = body.velocity;

            return new PositionData
            {
                position = new[] { position1.x, position1.y, position1.z },
                rotation = new[] { rotation1.x, rotation1.y, rotation1.z },
                velocity = new[] { velocity1.x, velocity1.y, velocity1.z },
            };
        }
    }

    [Serializable]
    public struct CarData
    {
        public PositionData? positionData;
        public InputData? inputData;
        public BrakeData? brakeData;
        public DrivetrainData? drivetrain;
        public AssistanceData? assistance;

        public static CarData FromCarController(CarController? data, Drivetrain? drivetrain, Rigidbody? body,
            CarDynamics? dynamics, CarData? last)
        {
            return new CarData
            {
                positionData = PositionData.FromRigidBody(body),
                inputData = data == null ? null : InputData.FromCarController(data),
                brakeData = dynamics == null ? null : BrakeData.FromBrakeEffects(dynamics.brakeEffects),
                drivetrain = DrivetrainData.FromDrivetrain(drivetrain),
                assistance = AssistanceData.FromController(data, last?.assistance)
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