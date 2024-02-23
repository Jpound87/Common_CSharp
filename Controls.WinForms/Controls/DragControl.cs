using Common;
using Datam.WinForms.Interface;
using Parameters.Interface;
using System;

namespace Datam.WinForms.Controls
{
    public class DragControl: IEquatable<IDrag>
    {
        private IDrag wrappedControl;
        public IDrag WrappedControl
        {
            get
            {
                return wrappedControl;// TODO: IGauge
            }
        }
        public IParameter Parameter
        {
            get
            {
                return wrappedControl.Parameter;
            }
            set
            {
                wrappedControl.Parameter = value;
            }
        }

        private readonly FAST_Semaphore draggingWaitHandle = new FAST_Semaphore(1, 1);
        private bool inDrag;
        public bool InDrag
        {
            get
            {
                return inDrag;
            }
            set
            {
                inDrag = value;
                wrappedControl.InDrag = inDrag;
                if (inDrag)
                {
                    draggingWaitHandle.Wait(0);
                }
                else
                {
                    draggingWaitHandle.TryRelease();
                }
            }
        }


        public DragControl(IDrag _gauge)
        {
            wrappedControl = _gauge;
        }
        public void SetControl(IDrag control)
        {
            wrappedControl.InDrag = false;
            wrappedControl = control;
        }

        public void SetParameter(IParameter parameter)
        {
            wrappedControl.Parameter = parameter;
        }

        public void Select()
        {
            wrappedControl.SelectControl();
        }

        public bool Equals(IDrag other)
        {
            if (other == null) return false;
            return wrappedControl == other;
        }

    }
}
