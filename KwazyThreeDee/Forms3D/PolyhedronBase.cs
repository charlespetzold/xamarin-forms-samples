using System;
using System.Collections.Generic;

namespace Forms3D
{
    public abstract class PolyhedronBase : SharedLineMesh
    {
        protected abstract Point3D[,] Faces
        {
            get;
        }

        protected override IList<SharedLine> Generate()
        {
            List<SharedLine> sharedLines = new List<SharedLine>();

            for (int face = 0; face < Faces.GetLength(0); face++)
            {
                Vector3D normal = Vector3D.CrossProduct(Faces[face, 2] - Faces[face, 0],
                                                        Faces[face, 1] - Faces[face, 0]);

                // For faces that are triangles.
                if (Faces.GetLength(1) == 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Point3D point1 = Faces[face, i];
                        Point3D point2 = Faces[face, (i + 1) % 3];
                        InsertIntoCollection(sharedLines, point1, point2, normal);
                    }
                }

                // For faces that are not triangles.
                else
                {
                    int num = Faces.GetLength(1) - 1;

                    for (int i = 0; i < num; i++)
                    {
                        Point3D point1 = Faces[face, i + 1];
                        Point3D point2 = Faces[face, (i + 1) % num + 1];
                        InsertIntoCollection(sharedLines, point1, point2, normal);
                    }
                }
            }

            sharedLines.TrimExcess();
            return sharedLines;
        }

        void InsertIntoCollection(List<SharedLine> sharedLines, Point3D pt1, Point3D pt2, Vector3D normal)
        {
            bool foundMatch = false;

            for (int i = 0; i < sharedLines.Count; i++)
            {
                if ((sharedLines[i].Point1 == pt1 && sharedLines[i].Point2 == pt2) ||
                    (sharedLines[i].Point1 == pt2 && sharedLines[i].Point2 == pt1))
                {
                    SharedLine sharedLine = sharedLines[i];
                    sharedLine.Normal2 = normal;
                    sharedLines[i] = sharedLine;
                    foundMatch = true;
                    break;
                }
            }

            if (!foundMatch)
            {
                SharedLine sharedLine = new SharedLine(pt1, pt2, normal, new Vector3D());
                sharedLines.Add(sharedLine);
            }
        }
    }
}

