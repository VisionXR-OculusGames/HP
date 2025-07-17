using System;
using UnityEngine;

namespace com.VisionXR.Models
{
    [CreateAssetMenu(fileName = "UIDataSO", menuName = "ScriptableObjects/UIDataSO", order = 1)]
    public class UIDataSO : ScriptableObject
    {
        // variables

        public Games currentGame;
        public MatchSticklevelsType currentMatchStickLevelsType;
        public SlideTheBlocklevelsType currentBlocksLevelsType;
        public BrainVitalevelsType currentBrainVitalevelsType;
        public TangramlevelsType currentTangramLevelsType;


        // Actions
        public Action GameChangedEvent;
        public Action<int> MatchStickLevelChangedEvent;
        public Action<int> BlockLevelChangedEvent;
        public Action<int> HenoiLevelChangedEvent;
        public Action<int> StartBrainVitaEvent;
        public Action<int> BrainVitaLevelChangedEvent;
        public Action<int> TangramLevelChangedEvent;
        public Action HomeButtonClickedEvent;
        public Action ExitGameEvent;
        public Action AllLevelsCompleteEvent;


        // Methods

    
        public void HomeClicked()
        {
            HomeButtonClickedEvent?.Invoke();
        }

        public void SetGame(Games games)
        {
            currentGame = games;
            GameChangedEvent?.Invoke();
        }
       
        public void MatchStickLevelChanged(int levelNo)
        {
            MatchStickLevelChangedEvent?.Invoke(levelNo);
        }

        public void BlockLevelChanged(int levelNo)
        {
            BlockLevelChangedEvent?.Invoke(levelNo);
        }

        public void TangramLevelChanged(int levelNo)
        {
            TangramLevelChangedEvent?.Invoke(levelNo);
        }
        public void HenoiLevelChanged(int levelNo)
        {
            HenoiLevelChangedEvent?.Invoke(levelNo);
        }

        public void BrainvitaLevelChanged(int levelNo)
        {
            BrainVitaLevelChangedEvent?.Invoke(levelNo);
        }
        public void StartBrainVita(int id)
        {
            StartBrainVitaEvent?.Invoke(id);
        }

        public void ExitGame()
        {
            ExitGameEvent?.Invoke();
        }

        public void AllLevelsComplete()
        {
            AllLevelsCompleteEvent?.Invoke();
        }
    }
}


