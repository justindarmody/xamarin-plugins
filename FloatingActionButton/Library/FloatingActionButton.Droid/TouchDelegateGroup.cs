using System;
using Android.Views;
using Android.Graphics;
using System.Collections.Generic;

namespace FloatingActionButton.Droid
{
    public class TouchDelegateGroup : TouchDelegate 
    {
        private static readonly Rect USELESS_HACKY_RECT = new Rect();
        private readonly List<TouchDelegate> mTouchDelegates = new List<TouchDelegate>();
        private TouchDelegate mCurrentTouchDelegate;

        public bool Enabled { get; set; }

        public TouchDelegateGroup(View uselessHackyView) : base(USELESS_HACKY_RECT, uselessHackyView) 
        {
        }

        public void addTouchDelegate(TouchDelegate touchDelegate) 
        {
            if (touchDelegate == null)
                throw new ArgumentNullException("touchDelegate cannot be null!");
            
            this.mTouchDelegates.Add(touchDelegate);
        }

        public void removeTouchDelegate(TouchDelegate touchDelegate) {
            mTouchDelegates.Remove(touchDelegate);

            if (mCurrentTouchDelegate == touchDelegate) {
                mCurrentTouchDelegate = null;
            }
        }

        public void clearTouchDelegates() {
            mTouchDelegates.Clear();
            mCurrentTouchDelegate = null;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (!this.Enabled) 
            {
                return false;
            }

            TouchDelegate delegte = null;

            switch (e.Action) 
            {
                case MotionEventActions.Down :
                    for (int i = 0; i < this.mTouchDelegates.Count; i++) {
                        TouchDelegate touchDelegate = mTouchDelegates[i];
                        if (touchDelegate.OnTouchEvent(e)) 
                        {
                            mCurrentTouchDelegate = touchDelegate;
                            return true;
                        }
                    }
                    break;

                case MotionEventActions.Move :
                    delegte = mCurrentTouchDelegate;
                    break;

                case MotionEventActions.Cancel:
                case MotionEventActions.Up:
                    delegte = mCurrentTouchDelegate;
                    mCurrentTouchDelegate = null;
                    break;
            }

            return delegte != null && delegte.OnTouchEvent(e);
        }
    }
}