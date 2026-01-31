using UnityEngine;

public class HitControl: MonoBehaviour
{
     private void Start()
     {
          GameManager.Instance.Data.OnShowHintTextChanged += OnOnShowHintTextChanged;
     }
     void OnOnShowHintTextChanged(bool flag)
     {
          this.gameObject.SetActive(flag);
     }
}