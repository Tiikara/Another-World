using UnityEngine;
using System.Collections;

public class TeamInfo : MonoBehaviour {
    public enum Status
    {
        Neutral, War, Ally
    }

    Status[,] matrixStatus = new Status[5,5];
    Color[] colorStatus = new Color[3];
	// Use this for initialization
	void Start () {
	    for(int i=0;i<5;i++)
        {
            for(int j=0;j<5;j++)
            {
                matrixStatus[i, j] = Status.War;
            }
        }
        SetStatus(0, 4, Status.Neutral);
        SetStatus(1, 4, Status.Neutral);
        SetStatus(2, 4, Status.Neutral);
        SetStatus(3, 4, Status.Neutral);

        colorStatus[(int)Status.War] = Color.red;
        colorStatus[(int)Status.War] = Color.red;
        colorStatus[(int)Status.War] = Color.red;
    }

    public void SetStatus(int player1, int player2, Status status)
    {
        matrixStatus[player1, player2] = status;
        matrixStatus[player2, player1] = status;
    }

    public Status GetStatus(int player1, int player2)
    {
        return matrixStatus[player1, player2];
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
