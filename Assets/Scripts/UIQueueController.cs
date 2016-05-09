using UnityEngine;
using System.Collections;

public class UIQueueController : MonoBehaviour {

    UnityEngine.UI.Text textNameQueue;
    UnityEngine.UI.Slider sliderProgress;

    // Use this for initialization
    void Start () {
        textNameQueue = GameObject.Find("TextObjectInQueue").GetComponent<UnityEngine.UI.Text>();
        sliderProgress = GameObject.Find("SliderProgressBuilding").GetComponent<UnityEngine.UI.Slider>();

        textNameQueue.gameObject.SetActive(false);
        sliderProgress.gameObject.SetActive(false);

        BuildController buildController = GetComponent<BuildController>();
        buildController.OnStartBuild += BuildController_OnStartBuild;
        buildController.OnUpdateProgressBuild += BuildController_OnUpdateProgressBuild;
        buildController.OnFinishBuild += BuildController_OnFinishBuild;
    }

    private void BuildController_OnFinishBuild()
    {
        textNameQueue.gameObject.SetActive(false);
        sliderProgress.gameObject.SetActive(false);
    }

    void BuildController_OnUpdateProgressBuild(double progress)
    {
        sliderProgress.value = (float)progress;
    }

    void BuildController_OnStartBuild(string name)
    {
        textNameQueue.gameObject.SetActive(true);
        sliderProgress.gameObject.SetActive(true);

        sliderProgress.value = 0;
        textNameQueue.text = name;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
