using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent(typeof(Button))]
public class RewardedAdsButton : MonoBehaviour, IUnityAdsListener
{

#if UNITY_IOS
    private string gameId = "4090124";
#else
    private string gameId = "4090125";
#endif

    public Button rewardButton;
    public string myPlacementId = "rewardedVideo";
    public bool unityAdsReady = false;

    public GameManager gameManager;

    void Start()
    {
        // rewardButton = GetComponent<Button>();

        // Set interactivity to be dependent on the Placement’s status:
        unityAdsReady = Advertisement.IsReady(myPlacementId);
        // rewardButton.interactable = unityAdsReady;

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (rewardButton) rewardButton.onClick.AddListener(ShowRewardedVideo);

        // Initialize the Ads listener and service:
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);
    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo()
    {
        Advertisement.Show(myPlacementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementId)
        {
            unityAdsReady = true;
            rewardButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId != myPlacementId) return;

        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            print("continuing the game");
            gameManager.Continue();
            // Reward the user for watching the ad to completion.
            print("Time to reward");
        }
        else if (showResult == ShowResult.Skipped)
        {
            gameManager.StopPlaying();
            // Do not reward the user for skipping the ad.
            print("Time to not get a reward i guess because you skipped");
        }
        else if (showResult == ShowResult.Failed)
        {
            gameManager.StopPlaying();
            print("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}
