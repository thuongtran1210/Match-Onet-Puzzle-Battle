using BeastLinkBattle.Grid.Data;
using BeastLinkBattle.Gameplay.Battle.Models; // Add thêm
using System;
using System.Collections.Generic;
using BeastLinkBattle.Gameplay.Data;
using BeastLinkBattle.Gameplay.Data.Skills;

namespace BeastLinkBattle.Gameplay
{

    // Interface cho BattleService, nơi sẽ xử lý logic chính của trận đấu, bao gồm:
    // - Quản lý danh sách thú cưng của người chơi và kẻ địch
    // - Xử lý khi có một match thành công và cống năng lượng
    // - Giải quyết các pha chiến đấu, bao gồm cả việc thực thi kỹ năng và tấn công
    public interface IBattleService
    {
        event Action<PlayerBeastModel> OnPlayerBeastCreated;
        event Action<EnemyModel> OnEnemyCreated;

        void ProcessMatchedContent(ITileContent content);
        void InitializeBasePets();
        void ResolveBattlePhase();
        void ResolveSingleBeast(BeastDefinition def);
        void ExecuteSkill(SkillDefinition skill);
        event Action<Dictionary<BeastDefinition, int>> OnQueueUpdated;
        bool IsQueueEmpty();
        bool HasBeastInQueue(BeastDefinition def);
    }
}