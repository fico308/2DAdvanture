public abstract class BaseState
{
    protected Enemy enemy;
    public abstract void OnEnter(Enemy enemy);
    public abstract void OnExit();
    public abstract void LoginUpdate();
    public abstract void PhysicsUpdate();

}
