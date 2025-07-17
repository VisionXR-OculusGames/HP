using System;
using UnityEngine;

namespace com.VisionXR.Models
{
    [CreateAssetMenu(fileName = "LevelDataSO", menuName = "ScriptableObjects/LevelDataSO", order = 1)]
    public class LevelDataSO : ScriptableObject
    {
     
        [Header(" Level No Free")]
        public int MatchStickLevelNo;
        public int BlockLevelNo;
        public int HenoiLevelNo;
        public int BrainVitaLevelNo;
        public int TangramLevelNo;

        // moves
        [Header(" Hints")]
        public int NoOfHints = 3;

        // Actions

        // Level Events
        public Action MatchStickLevelSetEvent;
        public Action<int> MatchStickLevelSuccesEvent;


        public Action HenoiLevelSetEvent;
        public Action<int> HenoiLevelSuccesEvent;


        public Action BlockLevelSetEvent;
        public Action<int> BlockLevelSuccesEvent;

        public Action BrainvitaLevelSetEvent;
        public Action<int> BraivitaLevelSuccesEvent;


        public Action TangramLevelSetEvent;
        public Action<int> TangramLevelSuccesEvent;

        public Action<int> LoadNextLevelEvent;

        // Score and Moves
        public Action<int> SetMovesEvent;
        public Action<int,int> SetMovesAndTotalMovesEvent;
        public Action<int> SetScoreEvent;
        public Action<int> SetLevelEvent;
        public Action<int> SetMinMarblesEvent;

        public Action<int, int,string> SetMatchStickMovesEvent;
        public Action<int, int> SetTangramMovesEvent;
        // Methods

        private void OnEnable()
        {
            MatchStickLevelNo = 0;
            BlockLevelNo = 0;
            HenoiLevelNo = 0;
            BrainVitaLevelNo = 0;
            TangramLevelNo = 0;
            NoOfHints = 3;

        }

        public void SetMatchStickLevel(int level)
        {
            MatchStickLevelNo = level;
            MatchStickLevelSetEvent?.Invoke();
        }
        public void MatchStickLevelSuccess()
        {
            MatchStickLevelSuccesEvent?.Invoke(MatchStickLevelNo);
        }

        public void SetBlockLevel(int level)
        {
            BlockLevelNo = level;
            BlockLevelSetEvent?.Invoke();
        }
        public void BlockLevelSuccess()
        {
            BlockLevelSuccesEvent?.Invoke(BlockLevelNo);
        }

        public void SetBrainVitaLevel(int level)
        {
            BrainVitaLevelNo = level;
            BrainvitaLevelSetEvent?.Invoke();
        }
        public void BrainvitaLevelSuccess()
        {
            BraivitaLevelSuccesEvent?.Invoke(BrainVitaLevelNo);
        }
        public void SetHenoiLevel(int level)
        {
            HenoiLevelNo = level;
            HenoiLevelSetEvent?.Invoke();
        }
        public void HenoiLevelSuccess()
        {
            HenoiLevelSuccesEvent?.Invoke(MatchStickLevelNo);
        }

        public void SetTangramLevel(int level)
        {
            TangramLevelNo = level;
            TangramLevelSetEvent?.Invoke();
        }
        public void TangramLevelSuccess()
        {
            TangramLevelSuccesEvent?.Invoke(TangramLevelNo);
        }
        public void SetMoves(int moves)
        {
            SetMovesEvent?.Invoke(moves);
        }

        public void SetMovesAndTotalMoves(int moves,int minimumMoves)
        {
            SetMovesAndTotalMovesEvent?.Invoke(moves, minimumMoves);
        }

        public void SetLevel(int level)
        {
            SetLevelEvent?.Invoke(level);
        }

        public void SetScore(int score)
        {
            SetScoreEvent?.Invoke(score);
        }

        public void SetMinMarbles(int marbles)
        {
            SetMinMarblesEvent?.Invoke(marbles);
        }

        public void SetMatchStickMoves(int currentMove,int TotalMoves,string hint)
        {
            SetMatchStickMovesEvent?.Invoke(currentMove, TotalMoves,hint);
        }

        public void LoadNextLevel(int levelNo)
        {
            LoadNextLevelEvent?.Invoke(levelNo);
        }

        public void SetTangramMoves(int currentMove, int totalMoves)
        {
            SetTangramMovesEvent?.Invoke(currentMove, totalMoves);
        }

    }
}


