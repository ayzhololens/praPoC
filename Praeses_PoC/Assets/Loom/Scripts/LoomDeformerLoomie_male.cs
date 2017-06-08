using UnityEngine;

public class LoomDeformerLoomie_male : LoomAvatar
{
    public string avatarName = "loomie_male";

    [Header("Expressions")]
    [Range(0, 1)] public float Anger = 0;
    [Range(0, 1)] public float Disgust = 0;
    [Range(0, 1)] public float Fear = 0;
    [Range(0, 1)] public float Sad = 0;
    [Range(0, 1)] public float Smile = 0;
    [Range(0, 1)] public float Surprised = 0;
    [Range(0, 1)] public float Wink = 0;
    [Range(0, 1)] public float BrowLookUp = 0;
    [Range(0, 1)] public float BrowLookRight = 0;
    [Range(0, 1)] public float BrowLookLeft = 0;

    [Header("Geometry Filters")]
    public bool hair = true;
    public bool eyelash = true;

    [Header("Animations")]
    public bool howAreYou = false;
    public AudioSource howAreYouSound;
    public bool browA = false;
    public bool browB = false;
    public bool browC = false;
    public bool mouthA = false;
    public bool mouthB = false;
    public bool mouthC = false;
    public bool blink = false;

    [Header("Constraints")]
    [Range(0, 1)] public float headTracking = 0;
    [Range(0, 1)] public float eyesTracking = 0;
    public Vector3 targetPosition = new Vector3(50, 0, 100);

    [Header("Facial Controls")]
    [Range(0, 1)] [Tooltip("Head Turn Left")] public float HeadTurnLeft = 0;
    [Range(0, 1)] [Tooltip("Head Turn Right")] public float HeadTurnRight = 0;
    [Range(0, 1)] [Tooltip("Head Up")] public float HeadUp = 0;
    [Range(0, 1)] [Tooltip("Head Down")] public float HeadDown = 0;
    [Range(0, 1)] [Tooltip("Chin Raiser")] public float ChinRaiser = 0;
    [Range(0, 1)] [Tooltip("Jaw Clench")] public float JawClench = 0;
    [Range(0, 1)] [Tooltip("Jaw Drop")] public float JawDrop = 0;
    [Range(0, 1)] [Tooltip("Jaw Left")] public float JawLeft = 0;
    [Range(0, 1)] [Tooltip("Jaw Right")] public float JawRight = 0;
    [Range(0, 1)] [Tooltip("Jaw Thrust")] public float JawThrust = 0;
    [Range(0, 1)] [Tooltip("Swallow")] public float Swallow = 0;
    [Range(0, 1)] [Tooltip("Tongue Flick")] public float TongueFlick = 0;
    [Range(0, 1)] [Tooltip("Left Brow Lowered")] public float LeftBrowLowered = 0;
    [Range(0, 1)] [Tooltip("Left Cheek Puff")] public float LeftCheekPuff = 0;
    [Range(0, 1)] [Tooltip("Left Cheek Raiser")] public float LeftCheekRaiser = 0;
    [Range(0, 1)] [Tooltip("Left Cheek Suck")] public float LeftCheekSuck = 0;
    [Range(0, 1)] [Tooltip("Left Chin Raiser Upper Lip")] public float LeftChinRaiserUpperLip = 0;
    [Range(0, 1)] [Tooltip("Left Dimpler")] public float LeftDimpler = 0;
    [Range(0, 1)] [Tooltip("Left Eye Closed")] public float LeftEyeClosed = 0;
    [Range(0, 1)] [Tooltip("Left Eye Look Down")] public float LeftEyeLookDown = 0;
    [Range(0, 1)] [Tooltip("Left Eye Look Left")] public float LeftEyeLookLeft = 0;
    [Range(0, 1)] [Tooltip("Left Eye Look Right")] public float LeftEyeLookRight = 0;
    [Range(0, 1)] [Tooltip("Left Eye Look Up")] public float LeftEyeLookUp = 0;
    [Range(0, 1)] [Tooltip("Left Eye Upper Lid Raiser")] public float LeftEyeUpperLidRaiser = 0;
    [Range(0, 1)] [Tooltip("Left Funneler")] public float LeftFunneler = 0;
    [Range(0, 1)] [Tooltip("Left Flat Pucker")] public float LeftFlatPucker = 0;
    [Range(0, 1)] [Tooltip("Left Inner Brow Raiser")] public float LeftInnerBrowRaiser = 0;
    [Range(0, 1)] [Tooltip("Left Lip Corner Close")] public float LeftLipCornerClose = 0;
    [Range(0, 1)] [Tooltip("Left Lip Corner Depressor")] public float LeftLipCornerDepressor = 0;
    [Range(0, 1)] [Tooltip("Left Lip Corner Puller")] public float LeftLipCornerPuller = 0;
    [Range(0, 1)] [Tooltip("Left Lip Flattener")] public float LeftLipFlattener = 0;
    [Range(0, 1)] [Tooltip("Left Lower Lip Depressor")] public float LeftLowerLipDepressor = 0;
    [Range(0, 1)] [Tooltip("Left Lower Lip Suck")] public float LeftLowerLipSuck = 0;
    [Range(0, 1)] [Tooltip("Left Lip Pressor")] public float LeftLipPressor = 0;
    [Range(0, 1)] [Tooltip("Left Lips Together")] public float LeftLipsTogether = 0;
    [Range(0, 1)] [Tooltip("Left Lip Stretcher")] public float LeftLipStretcher = 0;
    [Range(0, 1)] [Tooltip("Left Lid Tightener")] public float LeftLidTightener = 0;
    [Range(0, 1)] [Tooltip("Left Nose Down")] public float LeftNoseDown = 0;
    [Range(0, 1)] [Tooltip("Left Nose Flare")] public float LeftNoseFlare = 0;
    [Range(0, 1)] [Tooltip("Left Nasal Labial Fold")] public float LeftNasalLabialFold = 0;
    [Range(0, 1)] [Tooltip("Left Nose Suck")] public float LeftNoseSuck = 0;
    [Range(0, 1)] [Tooltip("Left Nose Wrinkler")] public float LeftNoseWrinkler = 0;
    [Range(0, 1)] [Tooltip("Left Outer Brow Raiser")] public float LeftOuterBrowRaiser = 0;
    [Range(0, 1)] [Tooltip("Left Pucker")] public float LeftPucker = 0;
    [Range(0, 1)] [Tooltip("Left Platysma")] public float LeftPlatysma = 0;
    [Range(0, 1)] [Tooltip("Left Shark Corner Puller")] public float LeftSharkCornerPuller = 0;
    [Range(0, 1)] [Tooltip("Left Upper Lip Raiser")] public float LeftUpperLipRaiser = 0;
    [Range(0, 1)] [Tooltip("Left Upper Lip Suck")] public float LeftUpperLipSuck = 0;
    [Range(0, 1)] [Tooltip("Right Brow Lowered")] public float RightBrowLowered = 0;
    [Range(0, 1)] [Tooltip("Right Cheek Puff")] public float RightCheekPuff = 0;
    [Range(0, 1)] [Tooltip("Right Cheek Raiser")] public float RightCheekRaiser = 0;
    [Range(0, 1)] [Tooltip("Right Cheek Suck")] public float RightCheekSuck = 0;
    [Range(0, 1)] [Tooltip("Right Chin Raiser Upper Lip")] public float RightChinRaiserUpperLip = 0;
    [Range(0, 1)] [Tooltip("Right Dimpler")] public float RightDimpler = 0;
    [Range(0, 1)] [Tooltip("Right Eye Closed")] public float RightEyeClosed = 0;
    [Range(0, 1)] [Tooltip("Right Eye Look Down")] public float RightEyeLookDown = 0;
    [Range(0, 1)] [Tooltip("Right Eye Look Left")] public float RightEyeLookLeft = 0;
    [Range(0, 1)] [Tooltip("Right Eye Look Right")] public float RightEyeLookRight = 0;
    [Range(0, 1)] [Tooltip("Right Eye Look Up")] public float RightEyeLookUp = 0;
    [Range(0, 1)] [Tooltip("Right Eye Upper Lid Raiser")] public float RightEyeUpperLidRaiser = 0;
    [Range(0, 1)] [Tooltip("Right Funneler")] public float RightFunneler = 0;
    [Range(0, 1)] [Tooltip("Right Flat Pucker")] public float RightFlatPucker = 0;
    [Range(0, 1)] [Tooltip("Right Inner Brow Raiser")] public float RightInnerBrowRaiser = 0;
    [Range(0, 1)] [Tooltip("Right Lip Corner Close")] public float RightLipCornerClose = 0;
    [Range(0, 1)] [Tooltip("Right Lip Corner Depressor")] public float RightLipCornerDepressor = 0;
    [Range(0, 1)] [Tooltip("Right Lip Corner Puller")] public float RightLipCornerPuller = 0;
    [Range(0, 1)] [Tooltip("Right Lip Flattener")] public float RightLipFlattener = 0;
    [Range(0, 1)] [Tooltip("Right Lower Lip Depressor")] public float RightLowerLipDepressor = 0;
    [Range(0, 1)] [Tooltip("Right Lower Lip Suck")] public float RightLowerLipSuck = 0;
    [Range(0, 1)] [Tooltip("Right Lip Pressor")] public float RightLipPressor = 0;
    [Range(0, 1)] [Tooltip("Right Lips Together")] public float RightLipsTogether = 0;
    [Range(0, 1)] [Tooltip("Right Lip Stretcher")] public float RightLipStretcher = 0;
    [Range(0, 1)] [Tooltip("Right Lid Tightener")] public float RightLidTightener = 0;
    [Range(0, 1)] [Tooltip("Right Nose Down")] public float RightNoseDown = 0;
    [Range(0, 1)] [Tooltip("Right Nose Flare")] public float RightNoseFlare = 0;
    [Range(0, 1)] [Tooltip("Right Nasal Labial Fold")] public float RightNasalLabialFold = 0;
    [Range(0, 1)] [Tooltip("Right Nose Suck")] public float RightNoseSuck = 0;
    [Range(0, 1)] [Tooltip("Right Nose Wrinkler")] public float RightNoseWrinkler = 0;
    [Range(0, 1)] [Tooltip("Right Outer Brow Raiser")] public float RightOuterBrowRaiser = 0;
    [Range(0, 1)] [Tooltip("Right Pucker")] public float RightPucker = 0;
    [Range(0, 1)] [Tooltip("Right Platysma")] public float RightPlatysma = 0;
    [Range(0, 1)] [Tooltip("Right Shark Corner Puller")] public float RightSharkCornerPuller = 0;
    [Range(0, 1)] [Tooltip("Right Upper Lip Raiser")] public float RightUpperLipRaiser = 0;
    [Range(0, 1)] [Tooltip("Right Upper Lip Suck")] public float RightUpperLipSuck = 0;
    [Range(0, 1)] [Tooltip("Left Iris Scale")] public float LeftIrisScale = 0;
    [Range(0, 1)] [Tooltip("Right Iris Scale")] public float RightIrisScale = 0;

    LoomAvatar loom;

    void Start ()
    {
        loom = LoomLoadAvatar (avatarName + "_GRP");
    }

    void Update ()
    {
        loom.UpdateStart ();

        Transform loomGeos = transform.Find("loom");
        if (loomGeos != null) {
            for (int loomItem = 0; loomItem < loomGeos.childCount; loomItem++) {
                Transform loomChild = loomGeos.GetChild (loomItem);
                loomChild.gameObject.SetActive (true);
                if (!hair && loomChild.name.Contains ("hair"))
                    loomChild.gameObject.SetActive (false);
                if (!eyelash && loomChild.name.Contains ("eyelash"))
                    loomChild.gameObject.SetActive (false);
                loomChild.transform.localScale = Vector3.one;
            }
        }

        loom.SetHeadTracking (headTracking);
        loom.SetEyesTracking (eyesTracking);
        loom.SetHeadTrackingTargetPosition (targetPosition);

        loom.SetControl ("Anger", Anger);
        loom.SetControl ("Disgust", Disgust);
        loom.SetControl ("Fear", Fear);
        loom.SetControl ("Sad", Sad);
        loom.SetControl ("Smile", Smile);
        loom.SetControl ("Surprised", Surprised);
        loom.SetControl ("Wink", Wink);
        loom.SetControl ("BrowLookUp", BrowLookUp);
        loom.SetControl ("BrowLookRight", BrowLookRight);
        loom.SetControl ("BrowLookLeft", BrowLookLeft);
        if (howAreYou) {
            loom.SetBehaviorPlayOnce ("howAreYou");
            howAreYou = false;
            if (howAreYouSound != null) howAreYouSound.Play ();
        }
        loom.SetBehaviorActive ("browA", browA);
        loom.SetBehaviorActive ("browB", browB);
        loom.SetBehaviorActive ("browC", browC);
        loom.SetBehaviorActive ("mouthA", mouthA);
        loom.SetBehaviorActive ("mouthB", mouthB);
        loom.SetBehaviorActive ("mouthC", mouthC);
        loom.SetBehaviorActive ("blink", blink);
        loom.SetControl ("c_HTL", HeadTurnLeft);
        loom.SetControl ("c_HTR", HeadTurnRight);
        loom.SetControl ("c_HU", HeadUp);
        loom.SetControl ("c_HD", HeadDown);
        loom.SetControl ("c_CR", ChinRaiser);
        loom.SetControl ("c_JC", JawClench);
        loom.SetControl ("c_JD", JawDrop);
        loom.SetControl ("c_JL", JawLeft);
        loom.SetControl ("c_JR", JawRight);
        loom.SetControl ("c_JT", JawThrust);
        loom.SetControl ("c_SW", Swallow);
        loom.SetControl ("c_TF", TongueFlick);
        loom.SetControl ("l_BL", LeftBrowLowered);
        loom.SetControl ("l_CHP", LeftCheekPuff);
        loom.SetControl ("l_CHR", LeftCheekRaiser);
        loom.SetControl ("l_CHS", LeftCheekSuck);
        loom.SetControl ("l_CRUL", LeftChinRaiserUpperLip);
        loom.SetControl ("l_DM", LeftDimpler);
        loom.SetControl ("l_EC", LeftEyeClosed);
        loom.SetControl ("l_ELD", LeftEyeLookDown);
        loom.SetControl ("l_ELL", LeftEyeLookLeft);
        loom.SetControl ("l_ELR", LeftEyeLookRight);
        loom.SetControl ("l_ELU", LeftEyeLookUp);
        loom.SetControl ("l_EULR", LeftEyeUpperLidRaiser);
        loom.SetControl ("l_FN", LeftFunneler);
        loom.SetControl ("l_FP", LeftFlatPucker);
        loom.SetControl ("l_IBR", LeftInnerBrowRaiser);
        loom.SetControl ("l_LCC", LeftLipCornerClose);
        loom.SetControl ("l_LCD", LeftLipCornerDepressor);
        loom.SetControl ("l_LCP", LeftLipCornerPuller);
        loom.SetControl ("l_LF", LeftLipFlattener);
        loom.SetControl ("l_LLD", LeftLowerLipDepressor);
        loom.SetControl ("l_LLS", LeftLowerLipSuck);
        loom.SetControl ("l_LP", LeftLipPressor);
        loom.SetControl ("l_LPT", LeftLipsTogether);
        loom.SetControl ("l_LS", LeftLipStretcher);
        loom.SetControl ("l_LT", LeftLidTightener);
        loom.SetControl ("l_ND", LeftNoseDown);
        loom.SetControl ("l_NF", LeftNoseFlare);
        loom.SetControl ("l_NLF", LeftNasalLabialFold);
        loom.SetControl ("l_NS", LeftNoseSuck);
        loom.SetControl ("l_NW", LeftNoseWrinkler);
        loom.SetControl ("l_OBR", LeftOuterBrowRaiser);
        loom.SetControl ("l_PK", LeftPucker);
        loom.SetControl ("l_PLT", LeftPlatysma);
        loom.SetControl ("l_SCP", LeftSharkCornerPuller);
        loom.SetControl ("l_ULR", LeftUpperLipRaiser);
        loom.SetControl ("l_ULS", LeftUpperLipSuck);
        loom.SetControl ("r_BL", RightBrowLowered);
        loom.SetControl ("r_CHP", RightCheekPuff);
        loom.SetControl ("r_CHR", RightCheekRaiser);
        loom.SetControl ("r_CHS", RightCheekSuck);
        loom.SetControl ("r_CRUL", RightChinRaiserUpperLip);
        loom.SetControl ("r_DM", RightDimpler);
        loom.SetControl ("r_EC", RightEyeClosed);
        loom.SetControl ("r_ELD", RightEyeLookDown);
        loom.SetControl ("r_ELL", RightEyeLookLeft);
        loom.SetControl ("r_ELR", RightEyeLookRight);
        loom.SetControl ("r_ELU", RightEyeLookUp);
        loom.SetControl ("r_EULR", RightEyeUpperLidRaiser);
        loom.SetControl ("r_FN", RightFunneler);
        loom.SetControl ("r_FP", RightFlatPucker);
        loom.SetControl ("r_IBR", RightInnerBrowRaiser);
        loom.SetControl ("r_LCC", RightLipCornerClose);
        loom.SetControl ("r_LCD", RightLipCornerDepressor);
        loom.SetControl ("r_LCP", RightLipCornerPuller);
        loom.SetControl ("r_LF", RightLipFlattener);
        loom.SetControl ("r_LLD", RightLowerLipDepressor);
        loom.SetControl ("r_LLS", RightLowerLipSuck);
        loom.SetControl ("r_LP", RightLipPressor);
        loom.SetControl ("r_LPT", RightLipsTogether);
        loom.SetControl ("r_LS", RightLipStretcher);
        loom.SetControl ("r_LT", RightLidTightener);
        loom.SetControl ("r_ND", RightNoseDown);
        loom.SetControl ("r_NF", RightNoseFlare);
        loom.SetControl ("r_NLF", RightNasalLabialFold);
        loom.SetControl ("r_NS", RightNoseSuck);
        loom.SetControl ("r_NW", RightNoseWrinkler);
        loom.SetControl ("r_OBR", RightOuterBrowRaiser);
        loom.SetControl ("r_PK", RightPucker);
        loom.SetControl ("r_PLT", RightPlatysma);
        loom.SetControl ("r_SCP", RightSharkCornerPuller);
        loom.SetControl ("r_ULR", RightUpperLipRaiser);
        loom.SetControl ("r_ULS", RightUpperLipSuck);
        loom.SetControl ("l_iris_Scale", LeftIrisScale);
        loom.SetControl ("r_iris_Scale", RightIrisScale);

        loom.UpdateEnd ();
    }
}
