using System;
using System.Numerics;

namespace CoordinatesHelpers
{
    public class HorizontalCoordinateProjection
    {
        Vector3 horzAxis, vertAxis;

        public Vector3 ViewCenterVector { protected set; get; }

        public void SetViewCenter(HorizontalCoordinate viewCenterCoord)
        {
            ViewCenterVector = viewCenterCoord.ToVector();
            HorizontalCoordinate vertAxisCoord = 
                new HorizontalCoordinate(viewCenterCoord.Azimuth + 90, 0);
            vertAxis = vertAxisCoord.ToVector();
            horzAxis = Vector3.Cross(ViewCenterVector, vertAxis);
        }

        public void GetAngleOffsets(HorizontalCoordinate objectCoord,
                                    out double horzAngle, out double vertAngle)
        {
            Vector3 objectVector = objectCoord.ToVector();
            Vector3 horzObjectCross = Vector3.Cross(objectVector, -horzAxis);
            Vector3 vertObjectCross = Vector3.Cross(objectVector, vertAxis);

            horzObjectCross = Vector3.Normalize(horzObjectCross);
            vertObjectCross = Vector3.Normalize(vertObjectCross);

            double x = Vector3.Dot(horzObjectCross, vertAxis);
            double y = Vector3.Dot(horzObjectCross, ViewCenterVector);
            horzAngle = -180 * Math.Atan2(y, x) / Math.PI;

            x = Vector3.Dot(vertObjectCross, horzAxis);
            y = Vector3.Dot(vertObjectCross, ViewCenterVector);
            vertAngle = -180 * Math.Atan2(y, x) / Math.PI;
        }
    }
}
