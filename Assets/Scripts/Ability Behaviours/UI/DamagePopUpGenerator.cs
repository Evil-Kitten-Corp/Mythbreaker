using BrunoMikoski.ServicesLocation;
using TMPro;
using UnityEngine;

namespace Ability_Behaviours.UI
{
    public class DamagePopUpGenerator : MonoBehaviour
    {
        public GameObject prefab;

        private void Awake()
        {
            ServiceLocator.Instance.RegisterInstance(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                CreatePopUp(Vector3.one, Random.Range(0, 1000).ToString(), Color.yellow);
            }
        }

        public void CreatePopUp(Vector3 position, string text, Color color)
        {
            var popup = Instantiate(prefab, position, Quaternion.identity);
            var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            temp.text = text;
            temp.faceColor = color;

            //Destroy Timer
            Destroy(popup, 1f);
        }
    }
}