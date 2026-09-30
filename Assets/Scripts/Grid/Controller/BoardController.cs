using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BeastLinkBattle.Grid.Data;
using BeastLinkBattle.Grid.Manager;
using BeastLinkBattle.Grid.Service;
using System;
using BeastLinkBattle.Grid.Data.Policies;

namespace BeastLinkBattle.Grid.Controller
{
    public class BoardController : MonoBehaviour
    {
        private IGridService _gridService;
        private IMatchService _matchService;
        private IRespawnService _respawnService;
        private IGridManager _gridManager;
        private IShuffleService _shuffleService;
        private RespawnPolicy _respawnPolicy;

        private bool _hasData = false;
        private List<Vector2Int> _lastMatchPath;

        public IGridService GetGridService() => _gridService;
        public List<Vector2Int> GetLastMatchPath() => _lastMatchPath;
        public bool IsInitialized() => _hasData;

        public event Action OnBoardDataReady;
        public event Action<Vector2Int> OnTileUpdated;
        public event Action<ITileContent, Vector2Int, Vector2Int> OnMatchCompleted;
        public event Action<List<Vector2Int>> OnDrawMatchPath;

        //DEBUG
        public Vector2Int? DebugSelectedPos { get; private set; }

        //Function DEBUG
        public void SetDebugSelectedPos(Vector2Int pos)
        {
            DebugSelectedPos = pos;
        }

        public void ClearDebugSelectedPos()
        {
            DebugSelectedPos = null;
        }

        public void Initialize(IGridService gridService, IMatchService matchService, IRespawnService respawnService, IGridManager gridManager, RespawnPolicy policy, IShuffleService shuffleService)
        {
            _gridService = gridService;
            _matchService = matchService;
            _respawnService = respawnService;
            _gridManager = gridManager;
            _respawnPolicy = policy;
            _shuffleService = shuffleService;
            _gridManager.FillGrid();
            _hasData = true;

            OnBoardDataReady?.Invoke();
        }


        public void ProcessMatch(Vector2Int posA, Vector2Int posB, ITileContent specificRespawnContent = null)
        {
            if (!_hasData) return;

            _lastMatchPath = _matchService.CheckMatch(posA, posB);

            if (_lastMatchPath != null && _lastMatchPath.Count > 0)
            {
                StartCoroutine(MatchSequenceRoutine(posA, posB, _lastMatchPath, specificRespawnContent));
            }
        }

        private IEnumerator MatchSequenceRoutine(Vector2Int posA, Vector2Int posB, List<Vector2Int> path, ITileContent specificContent)
        {

            ITileContent matchedContent = _gridService.GetTile(posA).Content;

            // --- BU?C 1: CH?Y VFX N?I ---
            OnDrawMatchPath?.Invoke(path); 

            yield return new WaitForSeconds(0.3f); 

            if (matchedContent != null)
            {
                OnMatchCompleted?.Invoke(matchedContent, posA, posB);
            }
            _gridService.ClearTileContent(posA);
            _gridService.ClearTileContent(posB);

            OnTileUpdated?.Invoke(posA); 
            OnTileUpdated?.Invoke(posB);

            yield return new WaitForSeconds(0.2f);
        }

        private IEnumerator CheckAndExecuteAutoRespawn()
        {
            if (_respawnPolicy == null) yield break;

            int totalPlayable = _gridService.GetTotalPlayableTiles();
            List<Vector2Int> emptyPositions = _gridService.GetEmptyPositions();

            if (_respawnPolicy.ShouldTriggerRespawn(totalPlayable, emptyPositions.Count))
            {
                float delayBetweenSpawns = _respawnPolicy.GetSequentialDelay();
                RespawnRequest request = new RespawnRequest(emptyPositions);
                _respawnService.ExecuteRespawn(request);
                foreach (var pos in emptyPositions)
                {
                    OnTileUpdated?.Invoke(pos); 

                    if (delayBetweenSpawns > 0)
                    {
                        yield return new WaitForSeconds(delayBetweenSpawns);
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
            CheckAndAutoShuffle();
        }

        public void TriggerComboRespawn()
        {
            if (!_hasData) return;
            StartCoroutine(ExecuteRespawnRoutine());
        }

        private IEnumerator ExecuteRespawnRoutine()
        {
            List<Vector2Int> emptyPositions = _gridService.GetEmptyPositions();
            if (emptyPositions.Count == 0) yield break;
            RespawnRequest request = new RespawnRequest(emptyPositions);
            _respawnService.ExecuteRespawn(request);

            foreach (var pos in emptyPositions)
            {
                OnTileUpdated?.Invoke(pos);
            }

            yield return new WaitForSeconds(0.1f);
        }

        private void CheckAndAutoShuffle()
        {
            if (!_matchService.HasAnyMatch() && _gridService.GetOccupiedPositions().Count > 0)
            {
                Debug.Log("H?t du?ng di! T? d?ng Shuffle...");
                PerformShuffle();
            }
        }
        public void TriggerSkillShuffle()
        {
            Debug.Log(">>> Kích ho?t Skill Shuffle <<<");
            PerformShuffle();
        }
        private void PerformShuffle()
        {
            if (!_hasData) return;

            var updatedPositions = _shuffleService.ExecuteShuffle();

            foreach (var pos in updatedPositions)
            {
                OnTileUpdated?.Invoke(pos);
            }
        }
        public void ClearSpecificContent(bool clearBeast)
        {
            if (!_hasData) return;

            List<Vector2Int> occupied = _gridService.GetOccupiedPositions();
            foreach (var pos in occupied)
            {
                var tile = _gridService.GetTile(pos);
                if (tile.Content == null) continue;

                bool isTarget = clearBeast ? (tile.Content is BeastContent) : (tile.Content is EnergyContent);

                if (isTarget)
                {
                    _gridService.ClearTileContent(pos);
                    OnTileUpdated?.Invoke(pos); 
                }
            }

            TriggerComboRespawn();
        }
    }
}
