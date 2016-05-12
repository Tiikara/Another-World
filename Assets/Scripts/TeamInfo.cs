using UnityEngine;
using System.Collections;

public class TeamInfo : MonoBehaviour {
    public enum Status
    {
        Neutral, War, Ally
    }

    Status[,] matrixStatus = new Status[3,3];
    Color[] colorStatus = new Color[3];
	// Use this for initialization
	void Start () {
	    for(int i=0;i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                matrixStatus[i, j] = Status.Neutral;
            }
        }
        SetStatus(0, 1, Status.War);

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
