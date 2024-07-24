namespace ShootGame
{
    public enum BuffState
    {
        InActive,
        Active,
        ActiveReady,
    }

    public class BuffParams
    {
        #region 流血数据
        public static float BleedDamage { get; } = 1.0f;
        public static float BleedDuration { get;} = 3.0f;
        public static float BleedTickInterval { get; } = 0.5f;
        public static int MaxBleedStack { get; } = 5;
        #endregion

        #region 血怒数据
        public static float MightDuration { get;} = 3.0f;
        public static float AttackValueBonus { get; } = 10.0f;
        #endregion
    }
}
