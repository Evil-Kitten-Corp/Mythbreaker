namespace not_this_again.Enums
{
    public enum CombatType
    {
        None,
        Attack,
        Dodge,
        HitReaction
    }
    
    public enum CharacterState
    {
        Idle,
        Walk,
        Run,
        Sprint,
        Patrol,
        Trace
    }
    
    public enum AttackType
    {
        None,
        LightAttack,
        StrongAttack
    }
    
    public enum AttackDirection
    {
        Front = 0,
        Back = 1,
        Right = 2,
        Left = 4,
        Up = 5,
        Down = 6
    }
    
    public enum ComboState
    {
        Stop,
        Playing
    }
    
    public enum Keystroke
    {
        None,
        LightAttack,
        StrongAttack
    }
}