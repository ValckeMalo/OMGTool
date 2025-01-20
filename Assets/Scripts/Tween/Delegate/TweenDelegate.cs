namespace MVProduction.Tween.Delegate
{
    public delegate void TweenSetter<in T>(T newValue);
    public delegate T TweenGetter<out T>();
    public delegate void TweenCallback();
    public delegate void TweenCallback<in T>(T value);
}