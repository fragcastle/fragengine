namespace FragEngine.Collisions
{
    public enum CollisionType
    {
        A,      // collision is detected only against "B" types
        B,      // collision is detected only against "A" types
        NONE    // no collision detection
    }
}
