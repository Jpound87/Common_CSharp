using System.Drawing;

namespace Common.Interface
{
    public interface ISplitterDragSource : IDragSource
    {
        #region Accessors
        bool IsVertical { get; }
        Rectangle DragLimitBounds { get; }
        #endregion

        #region Methods
        void BeginDrag(Rectangle rectSplitter);
        void EndDrag();
        void MoveSplitter(int offset);
        #endregion
    }
}
