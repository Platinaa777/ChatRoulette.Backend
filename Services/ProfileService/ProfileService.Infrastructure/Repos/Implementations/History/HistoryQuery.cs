namespace ProfileService.Infrastructure.Repos.Implementations.History;

public static class HistoryQuery
{
    public const string SqlFindByHistoryId = $@"
            SELECT history_id as Id, user_id as UserId, doom_points as DoomSlayerPoints, avatar_points as AvatarPoints
            FROM user_histories
            WHERE history_id = @Id;
         ";

    public const string SqlFindByUserId = $@"
            SELECT history_id as Id, user_id as UserId, doom_points as DoomSlayerPoints, avatar_points as AvatarPoints
            FROM user_histories
            WHERE user_id = @UserId;
        ";

    public const string SqlAddHistory = $@"
        INSERT INTO user_histories (history_id, user_id, doom_points, avatar_points)
        VALUES 
            (@Id, @UserId, @DoomSlayerPoints, @AvatarPoints);
        ";
    
    public const string SqlUpdateUpdate = $@"
        UPDATE user_histories
        SET
            doom_points = @DoomSlayerPoints,
            avatar_points = @AvatarPoints
        WHERE history_id = @Id
        ";    
}