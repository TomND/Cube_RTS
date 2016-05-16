using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerEconomy : MonoBehaviour {

    private Text txtGoldVar;
    private Text txtMunitionVar;
    private Text txtFuelVar;

    public float goldVarSpeed = 1f;
    public float munitionVarSpeed = 1f;
    public float fuelVarSpeed = 1f;

    public int goldVar;
    public int munitionVar;
    public int fuelVar;

    public int goldVarBoost = 1;
    public int munitionVarBoost = 1;
    public int fuelVarBoost = 1;

    public int DrillCount;
    public int FactoryCount;
    public int OilRefineryCount;

    public Drill[] Drill;
    public Factory[] Factory;
    public OilRefinery[] OilRefinery;


    void Start() {
        txtGoldVar = UI.root.txtGoldVar;
        txtMunitionVar = UI.root.txtMunitionVar;
        txtFuelVar = UI.root.txtFuelVar;
        StartCoroutine(GoldProcess());
        StartCoroutine(MunitionProcess());
        StartCoroutine(FuelProcess());
    }

    IEnumerator GoldProcess() {
        while (true) {
            goldVar += goldVarBoost;
            txtGoldVar.text = goldVar.ToString() + " (" + goldVarBoost + "/" + goldVarSpeed + "s)";
            yield return new WaitForSeconds(goldVarSpeed);
        }
    }

    IEnumerator MunitionProcess() {
        while (true) {
            munitionVar += munitionVarBoost;
            txtMunitionVar.text = munitionVar.ToString() + " (" + munitionVarBoost + "/" + munitionVarSpeed + "s)";
            yield return new WaitForSeconds(munitionVarSpeed);
        }
    }

    IEnumerator FuelProcess() {
        while (true) {
            fuelVar += fuelVarBoost;
            txtFuelVar.text = fuelVar.ToString() + " (" + fuelVarBoost + "/" + fuelVarSpeed + "s)";
            yield return new WaitForSeconds(fuelVarSpeed);
        }
    }



    

}
