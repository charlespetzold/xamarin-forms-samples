using System;
using System.Numerics;

namespace AstronomyLib
{
    public class HorizontalCoordinateProjection
    {
        Vector3 horzAxis, vertAxis;

        public Vector3 ViewCenterVector
        {
            protected set;
            get;
        }

        public void SetViewCenter(HorizontalCoordinate viewCenterCoord)
        {
            this.ViewCenterVector = viewCenterCoord.ToVector();
            HorizontalCoordinate vertAxisCoord = new HorizontalCoordinate(viewCenterCoord.Azimuth + Angle.Right, Angle.Zero);
            vertAxis = vertAxisCoord.ToVector();
            horzAxis = Vector3.Cross(this.ViewCenterVector, vertAxis);
        }

        public void GetAngleOffsets(HorizontalCoordinate objectCoord, out Angle horzAngle, out Angle vertAngle)
        {
            Vector3 objectVector = objectCoord.ToVector();
            Vector3 horzObjectCross = Vector3.Cross(objectVector, -horzAxis);
            Vector3 vertObjectCross = Vector3.Cross(objectVector, vertAxis);

            horzObjectCross = Vector3.Normalize(horzObjectCross);
            vertObjectCross = Vector3.Normalize(vertObjectCross);

            double x = Vector3.Dot(horzObjectCross, vertAxis);
            double y = Vector3.Dot(horzObjectCross, ViewCenterVector);

            horzAngle = -Angle.ArcTangent(y, x);

            x = Vector3.Dot(vertObjectCross, horzAxis);
            y = Vector3.Dot(vertObjectCross, ViewCenterVector);

            vertAngle = -Angle.ArcTangent(y, x);
        }
    }
}
