using com.VisionXR.Models;
using Oculus.Platform.Models;
using Oculus.Platform;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{

    [Header("Scriptable objects")]
    public PlayerDataSO playerData;
    private void OnEnable()
    {
        playerData.BuySlideTheBlockLevelsEvent += BuySlideTheBlockLevels;
        playerData.BuyBrainVitaLevelsEvent += BuyBrainVitaLevels;
        playerData.BuyMatchStickLevelsEvent += BuyMatchStickLevels;

    }

    private void OnDisable()
    {
        playerData.BuySlideTheBlockLevelsEvent -= BuySlideTheBlockLevels;
        playerData.BuyBrainVitaLevelsEvent -= BuyBrainVitaLevels;
        playerData.BuyMatchStickLevelsEvent -= BuyMatchStickLevels;

    }

    public void BuySlideTheBlockLevels()
    {

        IAP.LaunchCheckoutFlow("SlideTheBlockLevels").OnComplete(LaunchSlideTheBlockFlowCallback);
        
    }

    public void BuyMatchStickLevels()
    {

        IAP.LaunchCheckoutFlow("MatchStickLevels").OnComplete(LaunchMatchStickFlowCallback);
        
    }

    public void BuyBrainVitaLevels()
    {

        IAP.LaunchCheckoutFlow("BrainvitaLevels").OnComplete(LaunchBrainvitaFlowCallback);
        
    }

    private void LaunchSlideTheBlockFlowCallback(Message<Purchase> msg)
    {
        if (msg.IsError)
        {
            Debug.Log(" Something wrong ");
            return;
        }

        Purchase p = msg.GetPurchase();
        Debug.Log("purchased " + p.Sku);
        playerData.SlideTheBlockPurchaseSuccess();
       
    }

    private void LaunchMatchStickFlowCallback(Message<Purchase> msg)
    {
        if (msg.IsError)
        {
            Debug.Log(" Something wrong ");
            return;
        }

        Purchase p = msg.GetPurchase();
        Debug.Log("purchased " + p.Sku);
        playerData.MatchStickPurchaseSuccess();
    }

    private void LaunchBrainvitaFlowCallback(Message<Purchase> msg)
    {
        if (msg.IsError)
        {
            Debug.Log(" Something wrong ");
            return;
        }

        Purchase p = msg.GetPurchase();
        Debug.Log("purchased " + p.Sku);
        playerData.BrainVitaPurchaseSuccess();

    }

    private void GetIAPData()
    {
        List<string> skuList = new List<string>
        {
            "Premium_Features"
        };

        IAP.GetProductsBySKU(skuList.ToArray()).OnComplete(OnIAPProductsRetrieved);
    }

    private void OnIAPProductsRetrieved(Message<ProductList> msg)
    {
        if (msg.IsError)
        {
            Debug.LogError("Failed to retrieve IAP product data.");
            return;
        }

        foreach (Product product in msg.Data)
        {
            Debug.Log($"SKU: {product.Sku}");
            Debug.Log($"Name: {product.Name}");
            Debug.Log($"Description: {product.Description}");
            // Note: Meta does NOT provide price info via this API
        }
    }
}
