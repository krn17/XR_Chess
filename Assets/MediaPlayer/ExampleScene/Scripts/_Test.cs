using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HeadersModel headersModel1 = new HeadersModel();
        headersModel1.key = "Provider1";
        headersModel1.value = "JioCinema1";

        HeadersModel headersModel2 = new HeadersModel();
        headersModel2.key = "Provider2";
        headersModel2.value = "JioCinema2";

        HeadersModel[] hmArray = new HeadersModel[2];
        hmArray[0] = headersModel1;
        hmArray[1] = headersModel2;

        HeadersJsonModel model = new HeadersJsonModel();
        model.hmArray = hmArray;

        string json = JsonUtility.ToJson(model);
        Debug.Log("json : " + json);
    }
}
