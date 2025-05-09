using UnityEngine;

public class DataDirector : MonoBehaviour
{
    public enum Setting
    {
        MusicVolume = 0,
        SfxVolume = 1,
        AmbienceVolume = 2,
        MicDevice = 3,
        ProximityVoice = 4,
        Resolution = 5,
        Fullscreen = 6,
        MicVolume = 7,
        TextToSpeechVolume = 8,
        CameraShake = 9,
        CameraAnimation = 10,
        Tips = 11,
        Vsync = 12,
        MasterVolume = 13,
        CameraSmoothing = 14,
        LightDistance = 15,
        Bloom = 16,
        LensEffect = 17,
        MotionBlur = 18,
        MaxFPS = 19,
        ShadowQuality = 20,
        ShadowDistance = 21,
        ChromaticAberration = 22,
        Grain = 23,
        WindowMode = 24,
        RenderSize = 25,
        GlitchLoop = 26,
        AimSensitivity = 27,
        CameraNoise = 28,
        Gamma = 29,
        PlayerNames = 30,
        RunsPlayed = 31,
        PushToTalk = 32,
        TutorialPlayed = 33,
        TutorialJumping = 34,
        TutorialSprinting = 35,
        TutorialSneaking = 36,
        TutorialHiding = 37,
        TutorialTumbling = 38,
        TutorialPushingAndPulling = 39,
        TutorialRotating = 40,
        TutorialReviving = 41,
        TutorialHealing = 42,
        TutorialCartHandling = 43,
        TutorialItemToggling = 44,
        TutorialInventoryFill = 45,
        TutorialMap = 46,
        TutorialChargingStation = 47,
        TutorialOnlyOneExtraction = 48,
        TutorialChat = 49,
        TutorialFinalExtraction = 50,
        TutorialMultipleExtractions = 51,
        TutorialShop = 52
    }

    public enum SettingType
    {
        Audio = 0,
        Gameplay = 1,
        Graphics = 2,
        None = 3
    }
}