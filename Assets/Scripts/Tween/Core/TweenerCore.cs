namespace MVProduction.Tween.Core
{
    using MVProduction.Tween.Delegate;
    using MVProduction.Tween.Plugin;

    public class TweenerCore<T1, T2> : Tween
    {
        public T1 startValue;
        public T2 endValue;
        public T2 changeValue;

        public TweenSetter<T1> setter;
        public TweenGetter<T1> getter;

        public ABSPlugin<T1, T2> plugin;

        public override bool ApplyTween()
        {
            if (plugin == null)
                return true;

            //plugin is here to make the changement for the tween
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

        /// <summary>
        /// Create all the tween variables that he need to work
        /// </summary>
        /// <param name="getter"></param>
        /// <param name="setter"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <param name="plugin"></param>
        /// <returns>return the congrats of it</returns>
        public bool Setup(TweenGetter<T1> getter, TweenSetter<T1> setter, T2 endValue, float duration, ABSPlugin<T1, T2> plugin = null)
        {
            //if there is a custom plugin
            if (plugin != null)
            {
                this.plugin = plugin;
            }
            else
            {
                if (this.plugin == null)
                {
                    // get the default tween
                    this.plugin = PluginsManager.GetDefaultPlugin<T1, T2>();
                    if (this.plugin == null)
                    {
                        // if there no one return false to kill it
                        return false;
                    }
                }
            }


            this.getter = getter;
            this.setter = setter;
            this.endValue = endValue;
            this.duration = duration;

            this.plugin.SetFrom(this);//to know where do we start
            this.plugin.SetChangeValue(this);//plugin calculate the change value between each frame

            return true;
        }

        public static bool DoStartup<Type1, Type2>(TweenerCore<Type1, Type2> tween)
        {
            tween.startupDone = true;
            tween.active = true;

            return true;
        }

        public override bool Startup()
        {
            return DoStartup(this);
        }
    }
}
