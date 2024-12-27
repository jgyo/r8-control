using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace R8LocoCtrl.Controls
{
    public class ReverserSlider : Slider
    {
        private ToolTip? _autoToolTip;

        private ToolTip? AutoToolTip
        {
            get
            {
                if(_autoToolTip == null)
                {
                    FieldInfo? @field = typeof(Slider).GetField(
                        "_autoToolTip",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    Debug.Assert(@field != null);
                    _autoToolTip = @field.GetValue(this) as ToolTip;
                }

                return _autoToolTip;
            }
        }

        private void UpdateToolTip()
        {
            Debug.Assert(this.AutoToolTip != null);
            var content = this.AutoToolTip.Content;
            switch(content)
            {
                case "0":
                    this.AutoToolTip.Content = "R";
                    break;
                case "1":
                    this.AutoToolTip.Content = "N";
                    break;
                case "2":
                    this.AutoToolTip.Content = "F";
                    break;
            }
        }

        protected override void OnThumbDragDelta(DragDeltaEventArgs e)
        {
            base.OnThumbDragDelta(e);
            this.UpdateToolTip();
        }

        protected override void OnThumbDragStarted(DragStartedEventArgs e)
        {
            base.OnThumbDragStarted(e);
            this.UpdateToolTip();
        }
    }
}
