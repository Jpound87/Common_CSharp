using Common.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common.Timers
{
    #region Structs

    #region BlinkerColors
    /// <summary>
    /// Friend of TickTocker_Blinker
    /// </summary>
    internal struct BlinkerColors
    {
        #region Identity
        public const String StructName = nameof(BlinkerColors);
        public String Identity
        {
            get
            {
                return StructName;
            }
        }
        #endregion

        #region Accessors
        public Color ForeColor_On { get; private set; } //= Constant.AM_Color.BlinkActivatedFore;
        public Color ForeColor_Off { get; private set; } //= Constant.AM_Color.BlinkActivatedBack;

        public Color BackColor_On { get; private set; } //= SystemColors.Control;
        public Color BackColor_Off { get; private set; } //= SystemColors.ControlText;
        #endregion

        #region Constructor
        internal BlinkerColors(Color onForeColor, Color onBackColor, Color offForeColor, Color offBackColor)
        {
            ForeColor_On = onForeColor;
            BackColor_On = onBackColor;
            ForeColor_Off = offForeColor;
            BackColor_Off = offBackColor;
        }
        #endregion
    }
    #endregion

    #region BlinkerControlColors
    /// <summary>
    /// Friend of TickTocker_Blinker
    /// </summary>
    public struct BlinkerControlColors
    {
        #region Identity
        public const String StructName = nameof(BlinkerControlColors);
        public String Identity
        {
            get
            {
                return StructName;
            }
        }
        #endregion

        #region Accessors
        public Control Control { get; private set;}

        internal BlinkerColors BlinkerColors { get; private set; }
        public Color ForeColor_On => BlinkerColors.ForeColor_On;
        public Color ForeColor_Off => BlinkerColors.ForeColor_Off;
        public Color BackColor_On => BlinkerColors.BackColor_On;
        public Color BackColor_Off => BlinkerColors.BackColor_Off;
        #endregion

        #region Constructor
        public BlinkerControlColors(Control control, Color onForeColor, Color onBackColor, Color offForeColor, Color offBackColor)
        {
            Control = control;
            BlinkerColors = new BlinkerColors(onForeColor, onBackColor, offForeColor, offBackColor);
        }
        #endregion

        #region Methods
        internal KeyValuePair<Control, BlinkerColors> AsKeyValuePair()
        {
            return new KeyValuePair<Control, BlinkerColors>(Control, BlinkerColors);
        }
        #endregion
    }
    #endregion

    #endregion

    public class TickTocker_Blinker : TickTocker
    {
        #region Identity
        new public const String ClassName = nameof(TickTocker_Blinker);
        public override String Identity
        {
            get
            {
                return ClassName;
            }
        }
        #endregion

        #region Readonly
        private readonly Action blink_Action;
        private readonly Action onBlinkOn_Action;
        private readonly Action onBlinkOff_Action;
        private readonly Dictionary<Control, BlinkerColors> dict_Control_BlinkerColors = new Dictionary<Control, BlinkerColors>();
        private readonly HashSet<Control> blinkingControls = new HashSet<Control>();
        #endregion /Readonly

        #region Events
        public event Action PreBlink;
        public event Action BlinkOn;
        public event Action BlinkOff;
        #endregion

        #region Accessors
        private readonly HashSet<Control> allControls = new HashSet<Control>();
        public HashSet<Control> AllControls
        {
            get
            {
                return allControls;
            }
        }

        public Control[] BlinkingControls
        {
            get
            {
                return blinkingControls.ToArray();
            }
        }
        #endregion

        #region Globals
        private bool blink_Flag = false;
        #endregion

        #region Constructor
        public TickTocker_Blinker(int interval_ms) : base(interval_ms)
        {
            blink_Action = new Action(BlinkIndicator);
            Tick += blink_Action;
            onBlinkOn_Action = new Action(OnBlinkOn);
            BlinkOn += onBlinkOn_Action;
            onBlinkOff_Action = new Action(OnBlinkOff);
            BlinkOff += onBlinkOff_Action;
        }

        public TickTocker_Blinker(int interval_ms, params BlinkerControlColors[] blinkerControlColorsArray) : base(interval_ms)
        {
            blink_Action = new Action(BlinkIndicator);
            Tick += blink_Action;
            onBlinkOn_Action = new Action(OnBlinkOn);
            BlinkOn += onBlinkOn_Action;
            onBlinkOff_Action = new Action(OnBlinkOff);
            BlinkOff += onBlinkOff_Action;
            
            allControls = new HashSet<Control>();
            for (int bcc = 0; bcc<blinkerControlColorsArray.Length; bcc++) 
            {
                BlinkerControlColors bcc_at = blinkerControlColorsArray[bcc];
                allControls.Add(bcc_at.Control);
                dict_Control_BlinkerColors.TryAddOrUpdate(bcc_at.Control, bcc_at.BlinkerColors);
            }
        }
        #endregion /Constructor

        #region Control
        /// <summary>
        /// Starts blinking all contained controls
        /// </summary>
        public void Start()
        {
            lock (blinkingControls)
            {
                blinkingControls.UnionWith(allControls);
                if (blinkingControls.Any())
                {// In the upsetting situation where someone provided an empty list.
                    base.Start();
                }
            }
        }

        /// <summary>
        /// Starts blinking the given controls.
        /// </summary>
        /// <param name="controls">Controls to blink.</param>
        public void Start(params Control[] controls)
        {
            lock (blinkingControls)
            {
                blinkingControls.UnionWith(controls);
                if (blinkingControls.Any())
                {
                    base.Start();
                }
            }
        }

        /// <summary>
        /// Stop the blinking of all contained controls.
        /// </summary>
        new public void Stop()
        {
            lock (blinkingControls)
            {
                base.Stop();
                if (!blink_Flag)
                {// We stopped in the blink state.
                    OnBlinkOff();
                }
                blinkingControls.Clear();
            }
        }

        /// <summary>
        /// Stops blinking the given controls.
        /// </summary>
        /// <param name="controls">Controls to stop blinking.</param>
        public void Stop(params Control[] controls)
        {
            lock (blinkingControls)
            {
                blinkingControls.ExceptWith(controls);
                if (!blinkingControls.Any())
                {// If there are none left no need to run the timer.
                    base.Stop();
                }
                if (!blink_Flag)
                {// We stopped in the blink state.
                    TurnOffList(controls);
                }
            }
        }
        #endregion /Control

        #region Add
        public bool TryAddControls(params BlinkerControlColors[] blinkerControlColorsArray)
        {
            ConcurrentBag<int> removeIndicies = new ConcurrentBag<int>();
            try
            {
                Parallel.For(0, blinkerControlColorsArray.Length, (bcc) =>
                {
                    BlinkerControlColors bcc_at = blinkerControlColorsArray[bcc];// Blind carbon copy.
                    if (allControls.Contains(bcc_at.Control))
                    {
                        removeIndicies.Add(bcc);
                    }
                });
                if (removeIndicies.Any())
                {
                    List<int> sortedIndicies = removeIndicies.ToList();
                    sortedIndicies.Sort();
                    foreach (int si in sortedIndicies)
                    {// We want this to go in decending order, remove largest first!
                        blinkerControlColorsArray.ArrayRemoveResize(si);
                    }
                }
                if (blinkerControlColorsArray.Any())
                {// We have some to add still...
                    int at = allControls.Count;
                    for (int bcc = at; bcc < blinkerControlColorsArray.Length; bcc++)
                    {
                        BlinkerControlColors bcc_at = blinkerControlColorsArray[bcc];
                        allControls.Add(bcc_at.Control);
                        dict_Control_BlinkerColors.TryAddOrUpdate(bcc_at.Control, bcc_at.BlinkerColors);
                    }
                    return true;// If any added...
                }
            }
            catch
            {

            }
            return false;
        }
        #endregion /Add

        #region Update
        public bool TryUpdateControlColor(params BlinkerControlColors[] newContolColorsArray)
        {
            try
            {
                foreach (BlinkerControlColors newControlColors in newContolColorsArray)
                {
                    if (allControls.Contains(newControlColors.Control))
                    {
                        return dict_Control_BlinkerColors.TryAddOrUpdate(newControlColors.Control, newControlColors.BlinkerColors);
                    }
                    else
                    {
                        return TryAddControls(newContolColorsArray);
                    }
                }
            }
            catch
            {

            }
            return false;
        }
        #endregion /Update 

        #region Blink
        private void BlinkIndicator()
        {
            try
            {
                PreBlink?.Invoke();
                if (blink_Flag)
                {
                    BlinkOn?.Invoke();
                    blink_Flag = false;
                }
                else
                {// They return to normal
                    BlinkOff?.Invoke();
                    blink_Flag = true;
                }
            }
            catch
            {
#if DEBUG
                throw;
#endif
            }
        }

        /// <summary>
        /// This helper method will turn on all the 'blinking' boxes.
        /// </summary>
        private void OnBlinkOn()
        {
            lock (blinkingControls)
            {
                foreach (Control control in blinkingControls)
                {
                    control.BackColor = dict_Control_BlinkerColors[control].BackColor_On; 
                    control.ForeColor = dict_Control_BlinkerColors[control].ForeColor_On;
                }
            }
        }

        /// <summary>
        /// This helper method will turn off all the 'blinking' boxes.
        /// </summary>
        private void OnBlinkOff()
        {
            lock (blinkingControls)
            {
                TurnOffList(blinkingControls);
            }
        }

        /// <summary>
        /// Helper for turning 'off' controls (to off state colors).
        /// </summary>
        /// <param name="controls"></param>
        private void TurnOffList(IEnumerable<Control> controls)// Mouth-breathers are on this list.
        {
            foreach (Control control in controls)
            {
                control.BackColor = dict_Control_BlinkerColors[control].BackColor_Off;
                control.ForeColor = dict_Control_BlinkerColors[control].ForeColor_Off;
            }
        }
        #endregion
    }
}
