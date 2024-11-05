namespace MaloProduction.Tween.Core
{
    using MaloProduction.Tween.Delegate;
    using MaloProduction.Tween.Plugin;

    public class TweenerCore<T1, T2> : Tweener
    {
        public T1 startValue;
        public T2 endValue;
        public T2 changeValue;

        public TweenSetter<T1> setter;
        public TweenGetter<T1> getter;

        public ABSPlugin<T1, T2> plugin;

        public override bool AppplyTween()
        {
            if (plugin == null)
                return true;


            plugin.EvalutateAndApply(this, startValue, endValue, changeValue, setter, getter, elapsedTime, duration);

            return false;
        }

        public override void Reset()
        {
            base.Reset();

            setter = null;
            getter = null;
            plugin = null;
        }

        public bool Setup(TweenGetter<T1> getter, TweenSetter<T1> setter, T2 endValue, float duration, ABSPlugin<T1, T2> plugin = null)
        {
            if (plugin != null)
            {
                this.plugin = plugin;
            }
            else
            {
                if (this.plugin == null)
                {
                    this.plugin = PluginsManager.GetDefaultPlugin<T1, T2>();
                }
                if (this.plugin == null)
                {
                    return false;
                }
            }


            this.getter = getter;
            this.setter = setter;
            this.endValue = endValue;
            this.duration = duration;

            this.plugin.SetFrom(this);
            this.plugin.SetChangeValue(this);

            return true;
        }

        public override bool Startup()
        {
            return DoStartup(this);
        }
    }
}
