using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        public GameController GameController;
        
        public Text ScoreLabel;
        public Text HintLabel;

        private void Update()
        {
            ScoreLabel.text = $"Score: {GameController.Score}";
            
            HintLabel.gameObject.SetActive(!GameController.GameStarted);
        }
    }
}