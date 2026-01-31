using UnityEngine;

public class HitControl: MonoBehaviour
{
     private void Start()
     {
          GameManager.Instance.globalRuleData.OnShowHintTextChanged += OnOnShowHintTextChanged;
     }
     void OnOnShowHintTextChanged(bool flag)
     {
          this.gameObject.SetActive(flag);
     }
}