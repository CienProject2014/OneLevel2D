using System.Drawing;

namespace OneLevel2D
{
    static class CoordinateConverter
    {
        public static Point ToOrigin(Point point)
        {
            Point translated = point - Blackboard.LeftTopOffset;
            return translated;
        }

        public static Point BoardToGame(Point point, Size size)
        {
            return BoardToGame(point, size.Width, size.Height);
        }

        public static Point BoardToGame(Point point, int width, int height)
        {
            Point translatedPoint = point - Blackboard.LeftTopOffset;
            int newX = translatedPoint.X;
            int newY = State.Document.Height - (translatedPoint.Y + height);
            
            Point flippedPoint = new Point(newX, newY);

            return flippedPoint;
        }

        public static Point GameToBoard(Point point, int width, int height)
        {
            int newX = point.X;
            int newY = State.Document.Height - (point.Y + height);

            Point translatedPoint = new Point(newX, newY) + Blackboard.LeftTopOffset;

            return translatedPoint;
        }

        public static Point BoardToComposite(Point compositeLocation, Point imageLocation, int width, int height)
        {
            return Point.Empty;
        }

        // composite에 상대적인 image좌표를 뒤집는다.
        public static Point CompositeToBoard(Point imageLocation, Size compositeSize)
        {
            Point flipped = new Point(imageLocation.X, compositeSize.Height - imageLocation.Y);
            return flipped;
        }

    }
}
